using System.Data;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using OfficeOpenXml;

namespace KingsFarms.Core.Api.Services;

public class WeeklyOrdersUsdaLotsService : IWeeklyOrdersUsdaLotsService
{
    private readonly string _azStoreConnStr;
    private readonly string _azStoreContName;
    private readonly IHarvestService _harvestService;
    private readonly IPrepareUsdaLotsInvoiceService _prepareUsdaLotsInvoiceService;
    private readonly string _weeklyOrdersFile;

    public WeeklyOrdersUsdaLotsService(string azStoreConnStr, string azStoreContName, string weeklyOrdersFile,
        IPrepareUsdaLotsInvoiceService prepareUsdaLotsInvoiceService)
    {
        _azStoreConnStr = azStoreConnStr;
        _azStoreContName = azStoreContName;
        _weeklyOrdersFile = weeklyOrdersFile;
        _prepareUsdaLotsInvoiceService = prepareUsdaLotsInvoiceService;
    }


    public List<CustomerInvoicesViewModel> LoadInvoicesForWeek(string? week, CompanyEnum company)
    {
        var list = new List<CustomerInvoicesViewModel>();

        var weekDate = Utils.ParseToDateTime(week);
        if (!weekDate.HasValue) return list;

        var weekOfYear = GetWeekOfYearForInvoicesFromSheet(weekDate.GetValueOrDefault());
        var currentColumnInDt = weekOfYear + 2;

        var year = weekDate.GetValueOrDefault().Year;
        var month = weekDate.GetValueOrDefault().Month;
        var day = weekDate.GetValueOrDefault().Day;

        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_weeklyOrdersFile);
        var blobLot = container.GetBlockBlobClient($"{year}-{month}-{day}-lot-assign.xlsx");

        DataTable dtKings, dtMansi, dtCustomer, dtLot;

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

        try
        {
            using (var memoryStream = new MemoryStream())
            {
                blobLot.DownloadTo(memoryStream);

                using var package = new ExcelPackage(memoryStream);

                var lotTab = package.Workbook.Worksheets["LOT"];

                dtLot = EpplusUtils.ExcelPackageToDataTable(lotTab);
            }
        }
        catch (RequestFailedException ex)
        {
            dtLot = new DataTable();
        }


        return _prepareUsdaLotsInvoiceService.CustomerInvoicesViewModels(company, dtCustomer, dtKings, dtMansi, list, year, weekDate.GetValueOrDefault(), currentColumnInDt, dtLot);
    }

    private static int GetWeekOfYearForInvoicesFromSheet(DateTime weekDate)
    {
        var weekOfYear = Utils.GetWeekOfYear(weekDate);
        var firstMondayOfYear = Utils.GetFirstMondayOfYear(DateTime.Today.Year);
        var firstWeekOfYear = Utils.GetWeekOfYear(firstMondayOfYear);
        if (firstWeekOfYear > 1) weekOfYear -= 1;
        return weekOfYear;
    }
}