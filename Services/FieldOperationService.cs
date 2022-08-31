using System.Data;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using OfficeOpenXml;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Services;

public class FieldOperationService : IFieldOperationService
{
    private readonly string _azStoreConnStr;
    private readonly string _azStoreContName;
    private readonly string _fieldOperationsFile;
    private readonly ILogger _logger;

    public FieldOperationService(ILogger logger, string azStoreConnStr, string azStoreContName,
        string fieldOperationsFile)
    {
        _logger = logger;
        _azStoreConnStr = azStoreConnStr;
        _azStoreContName = azStoreContName;
        _fieldOperationsFile = fieldOperationsFile;
    }

    public List<HarvestViewModel> GetHarvestData(int harvestYear)
    {
        //var harvestYear = Utils.ParseToInteger(year);

        var list = new List<HarvestViewModel>();

        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_fieldOperationsFile);

        using (var memoryStream = new MemoryStream())
        {
            blob.DownloadTo(memoryStream);

            using var package = new ExcelPackage(memoryStream);
            var dtHarvest = GetHarvestWorksheetData(package, harvestYear);
            if (dtHarvest != null) list = GetHarvestViewModels(dtHarvest, harvestYear);
        }

        _logger.Information("GetHarvestDataBySeason returning {@Count}", list.Count);
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
            var model = new HarvestBedViewModel { BedNumber = $"Bed {col - 4}" };
            //if (year == 2020) model.HarvestQty20 = qty;
            //if (year == 2021) model.HarvestQty21 = qty;
            //if (year == 2022) model.HarvestQty22 = qty;
            model.HarvestQty = qty;
            list.Add(model);
        }

        return list;
    }
}