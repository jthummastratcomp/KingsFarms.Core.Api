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

public class WeeklyOrdersService : IWeeklyOrdersService
{
    private readonly string _azStoreConnStr;
    private readonly string _azStoreContName;
    private readonly IHarvestService _harvestService;
    private readonly IPrepareInvoiceService _prepareInvoiceService;
    private readonly string _weeklyOrdersFile;

    public WeeklyOrdersService(string azStoreConnStr, string azStoreContName, string weeklyOrdersFile, IHarvestService harvestService, IPrepareInvoiceService prepareInvoiceService)
    {
        _azStoreConnStr = azStoreConnStr;
        _azStoreContName = azStoreContName;
        _weeklyOrdersFile = weeklyOrdersFile;
        _harvestService = harvestService;
        _prepareInvoiceService = prepareInvoiceService;
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

        var weekOfYear = GetWeekOfYearForInvoicesFromSheet(weekDate.GetValueOrDefault());
        var currentColumnInDt = weekOfYear + 2;

        var year = weekDate.GetValueOrDefault().Year;

        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_weeklyOrdersFile);

        DataTable dtKings, dtMansi, dtCustomer;

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

        return _prepareInvoiceService.CustomerInvoicesViewModels(company, dtCustomer, dtKings, dtMansi, list, year, weekDate.GetValueOrDefault(), currentColumnInDt);
    }

    public List<CustomerDashboardViewModel> GetCustomersFromOrdersFile()
    {
        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_weeklyOrdersFile);

        using var memoryStream = new MemoryStream();
        blob.DownloadTo(memoryStream);

        using var package = new ExcelPackage(memoryStream);
        var customerTab = package.Workbook.Worksheets["ALL CUSTOMERS"];

        var dtCustomer = EpplusUtils.ExcelPackageToDataTable(customerTab);

        var customersList = Mapper.MapToCustomerDashboardViewModelList(dtCustomer);

        return customersList;
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