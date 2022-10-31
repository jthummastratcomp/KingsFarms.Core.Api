using System.Data;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Mappers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
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

        DataTable dtKings, dtMansi, dtCustomer, dtUniques;
        List<SearchDto> lots;

        using (var memoryStream = new MemoryStream())
        {
            blob.DownloadTo(memoryStream);

            using var package = new ExcelPackage(memoryStream);

            var kingsTab = package.Workbook.Worksheets["KINGS"];
            var mansiTab = package.Workbook.Worksheets["MANSI"];
            var customerTab = package.Workbook.Worksheets["ALL CUSTOMERS"];
            var uniquesTab = package.Workbook.Worksheets["Uniques1"];

            dtKings = EpplusUtils.ExcelPackageToDataTable(kingsTab);
            dtMansi = EpplusUtils.ExcelPackageToDataTable(mansiTab);
            dtCustomer = EpplusUtils.ExcelPackageToDataTable(customerTab);
            //dtUniques = EpplusUtils.ExcelPackageToDataTable(uniquesTab);
            //lots = GetLots(uniquesTab);
            //lots = GetLots(uniquesTab);
        }

        var dtSource = company is CompanyEnum.Kings or CompanyEnum.KingsSandbox
            ? dtKings
            : company == CompanyEnum.Mansi
                ? dtMansi
                : null;

        var currentColumnInDt = GetColumn(dtSource, weekDate.GetValueOrDefault());
        lots = GetLots(dtSource, currentColumnInDt);

        return _prepareUsdaInvoiceService.CustomerInvoicesViewModels(company, dtCustomer, dtKings, dtMansi, list, year, weekDate.GetValueOrDefault(), currentColumnInDt, lots);
    }

    private static List<SearchDto> GetLots(DataTable? dtSource, int currentColumnInDt)
    {
        List<SearchDto> lots = new List<SearchDto>();

        if (dtSource == null) return lots;

        foreach (DataRow row in dtSource.Rows)
        {
            var customerKey = row[0].ToString();
            var qty = Utils.ParseToInteger(row[currentColumnInDt].ToString());
            if(qty == 0) continue;

            var date = Utils.ParseToInteger(row[currentColumnInDt - 3].ToString()).ToString("00/00");

            var inspectDate = Utils.ParseToDateTime($"{date}/{DateTime.Today.Year}");
            var bed = Utils.ParseToInteger(row[currentColumnInDt - 2].ToString());
            var lot = row[currentColumnInDt - 1].ToString();


            if (!inspectDate.HasValue || bed == 0 || string.IsNullOrEmpty(lot)) continue;

            var key = $"2022-065-{inspectDate.Value.ToString("MMddyy")}-{GetBlock(bed)}-{bed.ToString("00")}-{lot}-FL";

            lots.Add(new SearchDto { Id = customerKey, Data = key });
        }



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

    //private static List<SearchDto> GetLots(ExcelWorksheet uniquesTab)
    //{
    //    var lots = new List<SearchDto>();
    //    for (var i = 1; i <= 100; i++)
    //    {
    //        var lot = EpplusUtils.GetCellValue(uniquesTab, $"Z{i}");
    //        var customerKey = EpplusUtils.GetCellValue(uniquesTab, $"T{i}");

    //        //var lot = EpplusUtils.GetCellValue(uniquesTab, $"K{i}");
    //        //var customerKey = EpplusUtils.GetCellValue(uniquesTab, $"L{i}");

    //        if (!string.IsNullOrEmpty(lot) && !string.IsNullOrEmpty(customerKey)) lots.Add(new SearchDto { Id = customerKey, Data = lot });
    //    }

    //    return lots;
    //}

    private static int GetColumn(DataTable? dtSource, DateTime weekDate)
    {
        if (dtSource == null) return 0;

        var weekOfYear = DateTime.Parse($"1/1/{DateTime.Today.Year}");

        var columnInDt = PickColumn(weekDate, weekOfYear);

        //if (weekDate.Date == weekOfYear)
        //{
        //    weekOfYear = weekOfYear.AddDays(7);
        //    return 9;
        //}
        //if (weekDate.Date == weekOfYear.AddDays(7)) return 13;
        //if (weekDate.Date == DateTime.Parse($"1/22/{DateTime.Today.Year}")) return 17;
        //if (weekDate.Date == DateTime.Parse($"1/29/{DateTime.Today.Year}")) return 17;
        //if (weekDate.Date == DateTime.Parse($"1/22/{DateTime.Today.Year}")) return 17;
        //if (weekDate.Date == DateTime.Parse($"1/22/{DateTime.Today.Year}")) return 17;
        //if (weekDate.Date == DateTime.Parse($"1/22/{DateTime.Today.Year}")) return 17;

        //var row = dtSource.Rows[0];
        //for (var column = 1; column < dtSource.Columns.Count; column += 4)
        //{
        //    var value = row[column].ToString();

        //    var date = Utils.ParseToDateTime(value);
        //    if (!date.HasValue) continue;

        //    if (date.GetValueOrDefault() == weekDate) return column;
        //}

        return columnInDt;
    }

    private static int PickColumn(DateTime weekDate, DateTime weekOfYear)
    {
        int column = 5;
        while (weekDate != weekOfYear)
        {
            weekOfYear = weekOfYear.AddDays(7);
            column += 4;
        }

        return column;
    }

    //private static int GetColumn(DataTable? dtSource, DateTime weekDate)
    //{
    //    if(dtSource == null) return 0;

    //    var row = dtSource.Rows[0];
    //    for (var column = 2; column < dtSource.Columns.Count; column++)
    //    {
    //        var value = row[column].ToString();

    //        var date = Utils.ParseToDateTime(value);
    //        if (!date.HasValue) continue;

    //        if (date.GetValueOrDefault() == weekDate) return column;
    //    }

    //    return 0;
    //}

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
}