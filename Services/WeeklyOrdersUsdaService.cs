using System.Data;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Mappers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using KingsFarms.Core.Api.ViewModels.Customer;
using KingsFarms.Core.Api.ViewModels.Invoice;
using OfficeOpenXml;

namespace KingsFarms.Core.Api.Services;

public class WeeklyOrdersUsdaService : IWeeklyOrdersUsdaService
{
    private readonly string _azStoreConnStr;
    private readonly string _azStoreContName;
    private readonly IPrepareUsdaInvoiceService _prepareUsdaInvoiceService;
    private readonly string _weeklyOrdersUsdaFile;

    public WeeklyOrdersUsdaService(string azStoreConnStr, string azStoreContName, string weeklyOrdersUsdaFile, IPrepareUsdaInvoiceService prepareUsdaInvoiceService)
    {
        _azStoreConnStr = azStoreConnStr;
        _azStoreContName = azStoreContName;
        _weeklyOrdersUsdaFile = weeklyOrdersUsdaFile;
        _prepareUsdaInvoiceService = prepareUsdaInvoiceService;
    }

    public List<SearchDto> GetInvoiceWeeksListForYear(int year)
    {
        return Utils.GetWeeksOfYear(year);
    }

    public List<CustomerInvoicesViewModel> LoadInvoicesForWeek(string? week, CompanyEnum company)
    {
        var list = new List<CustomerInvoicesViewModel>();

        var weekDate = Utils.ParseToDateTime(week);
        if (!weekDate.HasValue) return list;

        if (weekDate.GetValueOrDefault().DayOfWeek != DayOfWeek.Saturday) return list;

        var year = weekDate.GetValueOrDefault().Year;

        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_weeklyOrdersUsdaFile);

        DataTable dtKings, dtMansi, dtCustomer;
        List<SearchDto> lots;

        using (var memoryStream = new MemoryStream())
        {
            blob.DownloadTo(memoryStream);

            using var package = new ExcelPackage(memoryStream);

            var kingsTab = package.Workbook.Worksheets["KINGS"];
            var mansiTab = package.Workbook.Worksheets["MANSI"];
            var customerTab = package.Workbook.Worksheets["ALL CUSTOMERS"];


            dtKings = EpplusUtils.ExcelPackageToDataTable(kingsTab);
            dtMansi = EpplusUtils.ExcelPackageToDataTable(mansiTab);
            dtCustomer = EpplusUtils.ExcelPackageToDataTable(customerTab);
        }

        var dtSource = company is CompanyEnum.Kings or CompanyEnum.KingsSandbox
            ? dtKings
            : company == CompanyEnum.Mansi
                ? dtMansi
                : null;

        var currentColumnInDt = GetColumn(dtSource, weekDate.GetValueOrDefault());
        lots = GetLots(dtSource, currentColumnInDt, weekDate.GetValueOrDefault());

        return _prepareUsdaInvoiceService.CustomerInvoicesViewModels(company, dtCustomer, dtKings, dtMansi, list, year, weekDate.GetValueOrDefault(), currentColumnInDt, lots);
    }


    public List<CustomerDashboardViewModel> GetCustomersFromOrdersFile()
    {
        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_weeklyOrdersUsdaFile);

        using var memoryStream = new MemoryStream();
        blob.DownloadTo(memoryStream);

        using var package = new ExcelPackage(memoryStream);
        var customerTab = package.Workbook.Worksheets["ALL CUSTOMERS"];

        var dtCustomer = EpplusUtils.ExcelPackageToDataTable(customerTab);

        var customersList = Mapper.MapToCustomerDashboardViewModelList(dtCustomer);

        return customersList;
    }

    private static List<SearchDto> GetLots(DataTable? dtSource, int currentColumnInDt, DateTime weekDate)
    {
        var lots = new List<SearchDto>();

        if (dtSource == null) return lots;

        lots.AddRange(from DataRow row in dtSource.Rows
            let customerKey = row[0].ToString()
            let qty = Utils.ParseToInteger(row[currentColumnInDt].ToString())
            where qty != 0
            let date = Utils.ParseToInteger(row[currentColumnInDt - 3].ToString()).ToString("00/00")
            //let inspectDate = Utils.ParseToDateTime($"{date}/{DateTime.Today.Year}")
            let inspectDate = Utils.ParseToDateTime($"{date}/{weekDate.Year}")
            let bed = Utils.ParseToInteger(row[currentColumnInDt - 2].ToString())
            let lot = row[currentColumnInDt - 1].ToString()
            where inspectDate.HasValue && bed != 0 && !string.IsNullOrEmpty(lot)
            let key = @$"Lot# 2022-065-{inspectDate.Value.ToString("MMddyy")}-{GetBlock(bed)}-{bed.ToString("00")}-{lot}-FL Compliance# CP2223FTP045"
            select new SearchDto { Id = customerKey, Data = key });


        return lots;
    }

    private static string GetBlock(int bedNumber)
    {
        return bedNumber switch
        {
            >= 1 and <= 11 => "74552",
            >= 12 and <= 22 => "74560",
            >= 23 and <= 28 => "74558",
            >= 29 and <= 37 => "74556",
            >= 38 and <= 51 => "74554",
            _ => string.Empty
        };
    }

    private static int GetColumn(DataTable? dtSource, DateTime weekDate)
    {
        if (dtSource == null) return 0;

        //var weekOfYear = DateTime.Parse($"1/1/{DateTime.Today.Year}");
        //var weekOfYear = DateTime.Parse($"1/1/{weekDate.Year}");
        var weekOfYear = DateTime.Parse($"1/7/{weekDate.Year}");
        var columnInDt = PickColumn(weekDate, weekOfYear);

        return columnInDt;
    }

    private static int PickColumn(DateTime weekDate, DateTime weekOfYear)
    {
        var column = 5;
        while (weekDate != weekOfYear)
        {
            weekOfYear = weekOfYear.AddDays(7);
            column += 4;
        }

        return column;
    }
}