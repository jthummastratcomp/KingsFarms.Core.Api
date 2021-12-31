using System.Data;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using HotTowel.Web.Helpers;
using HotTowel.Web.Services.Interfaces;
using HotTowel.Web.ViewModels;
using OfficeOpenXml;
using ILogger = Serilog.ILogger;

namespace HotTowel.Web.Services;

public class HarvestService : IHarvestService
{
    private readonly string _azStoreConnStr;
    private readonly string _azStoreContName;
    private readonly string _harvestFile;
    private readonly ILogger _logger;

    public HarvestService(ILogger logger, string azStoreConnStr, string azStoreContName, string harvestFile)
    {
        _logger = logger;
        _azStoreConnStr = azStoreConnStr;
        _azStoreContName = azStoreContName;
        _harvestFile = harvestFile;
    }

    public List<HarvestViewModel> GetHarvestInfo()
    {
        List<HarvestViewModel> list;

        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_harvestFile);

        using (var memoryStream = new MemoryStream())
        {
            blob.DownloadTo(memoryStream);

            using (var package = new ExcelPackage(memoryStream))
            {
                //var harvest20 = package.Workbook.Worksheets["2020_2021_HARVEST"];
                var harvest21 = package.Workbook.Worksheets["2021_2022_HARVEST"];

                //var dtHarvest20 = EpplusUtils.ExcelPackageToDataTable(harvest20);
                var dtHarvest21 = EpplusUtils.ExcelPackageToDataTable(harvest21);

                list = GetHarvestViewModels(dtHarvest21, 2021);
            }
        }

        _logger.Information("GetHarvestInfo returning {@Count}", list.Count);
        return list;
    }


    private static List<HarvestViewModel> GetHarvestViewModels(DataTable table, int year)
    {
        var list = new List<HarvestViewModel>();

        for (var row = 6; row < 75; row++)
        {
            var dataRow = table.Rows[row];
            var harvestDate = Utils.ParseToDateTime(dataRow[1].ToString());
            if (!harvestDate.HasValue) continue;

            list.Add(new HarvestViewModel { HarvestDate = harvestDate.GetValueOrDefault(), BedHarvests = GetHarvestsForWeek(dataRow, year) });
        }

        return list;
    }


    private static List<HarvestBedViewModel> GetHarvestsForWeek(DataRow row, int year)
    {
        var list = new List<HarvestBedViewModel>();

        for (var col = 5; col < 56; col++)
        {
            var qty = Utils.ParseToInteger(row[col].ToString());
            if (qty <= 0) continue;
            var model = new HarvestBedViewModel { BedNumber = $"Bed {col - 4}" };
            if (year == 20201) model.HarvestQty20 = qty;
            if (year == 2021) model.HarvestQty21 = qty;
            list.Add(model);
        }

        return list;
    }
}