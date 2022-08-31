using System.Data;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using LazyCache;
using OfficeOpenXml;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Services;

public class HarvestService : IHarvestService
{
    private readonly string _azStoreConnStr;
    private readonly string _azStoreContName;
    private readonly string _harvestFile;
    private readonly ILogger _logger;

    private IAppCache _appCache;

    public HarvestService(ILogger logger, IAppCache appCache, string azStoreConnStr, string azStoreContName,
        string harvestFile)
    {
        _logger = logger;
        _azStoreConnStr = azStoreConnStr;
        _azStoreContName = azStoreContName;
        _harvestFile = harvestFile;
        _appCache = appCache;
    }

    public List<HarvestViewModel> GetHarvestData(int harvestYear)
    {
        return GetHarvestDataForYear(harvestYear);
    }

    public int GetHarvestYearTotal(int harvestYear)
    {
        var list = GetHarvestDataForYear(harvestYear);
        var total = list.Sum(x => x.TotalHarvest);

        return total;
    }

    public int GetHarvestStatusTotal(DashboardStatusEnum status)
    {
        var list = new List<HarvestViewModel>();
        for (var year = 2020; year <= DateTime.Today.Year; year++) list.AddRange(GetHarvestDataForYear(year));

        return status switch
        {
            DashboardStatusEnum.CurrentYear => list.Where(x => x.HarvestDate.Year == DateTime.Today.Year).Sum(x => x.TotalHarvest),
            DashboardStatusEnum.LastYear => list.Where(x => x.HarvestDate.Year == DateTime.Today.AddYears(-1).Year).Sum(x => x.TotalHarvest),
            _ => list.Sum(x => x.TotalHarvest)
        };
    }


    private List<HarvestViewModel> GetHarvestDataForYear(int harvestYear)
    {
        var list = new List<HarvestViewModel>();

        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_harvestFile);

        using (var memoryStream = new MemoryStream())
        {
            blob.DownloadTo(memoryStream);

            using var package = new ExcelPackage(memoryStream);
            var dtHarvest = GetHarvestWorksheetData(package, harvestYear);
            if (dtHarvest != null) list = GetHarvestViewModels(dtHarvest, harvestYear);
        }

        _logger.Information("GetHarvestData for {@Year} returning {@Count}", harvestYear, list.Count);
        return list;
    }

    private static DataTable? GetHarvestWorksheetData(ExcelPackage package, int harvestYear)
    {
        return harvestYear switch
        {
            2020 => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["2020_2021_HARVEST"]),
            2021 => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["2021_2022_HARVEST"]),
            2022 => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["2022_2023_HARVEST"]),
            _ => null
        };
    }

    private static List<HarvestViewModel> GetHarvestViewModels(DataTable table, int year)
    {
        var list = new List<HarvestViewModel>();

        for (var row = 6; row < 75; row++)
        {
            var dataRow = table.Rows[row];
            var harvestDate = Utils.ParseToDateTime(dataRow[1].ToString());
            if (!harvestDate.HasValue) continue;

            list.Add(new HarvestViewModel(harvestDate.GetValueOrDefault())
                { BedHarvests = GetHarvestsForWeek(dataRow, year) });
        }

        return list;
    }

    private static List<HarvestBedViewModel> GetHarvestsForWeek(DataRow row, int year)
    {
        var list = new List<HarvestBedViewModel>();

        var colMax = year == 2020 ? 33 : 56;
        for (var col = 5; col < colMax; col++)
        {
            var qty = Utils.ParseToInteger(row[col].ToString());
            if (qty <= 0) continue;
            var model = new HarvestBedViewModel
            {
                BedNumber = $"Bed {col - 4}",
                HarvestQty = qty
            };

            list.Add(model);
        }

        return list;
    }
}