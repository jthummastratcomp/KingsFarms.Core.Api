using System.Data;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using OfficeOpenXml;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Services;

public class HorseManureService : IHorseManureService
{
    private readonly string _azStoreConnStr;
    private readonly string _azStoreContName;
    private readonly string _horseManureFile;
    private readonly ILogger _logger;

    public HorseManureService(ILogger logger, string azStoreConnStr, string azStoreContName, string horseManureFile)
    {
        _logger = logger;
        _azStoreConnStr = azStoreConnStr;
        _azStoreContName = azStoreContName;
        _horseManureFile = horseManureFile;
    }

    public List<ManureLoadViewModel> GetManureLoadForMonth(MonthEnum loadMonth)
    {
        var list = new List<ManureLoadViewModel>();

        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_horseManureFile);

        using (var memoryStream = new MemoryStream())
        {
            blob.DownloadTo(memoryStream);

            using var package = new ExcelPackage(memoryStream);
            var dtHarvest = GetManureLoadWorksheetData(package, loadMonth);
            if (dtHarvest != null) list = GetManureLoadViewModels(dtHarvest);
        }

        _logger.Information("GetHarvestInfo returning {@Count}", list.Count);
        return list;
    }

    private static DataTable? GetManureLoadWorksheetData(ExcelPackage package, MonthEnum loadMonth)
    {
        return loadMonth switch
        {
            MonthEnum.January => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["January"]),
            MonthEnum.February => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["February"]),
            MonthEnum.March => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["March"]),
            MonthEnum.April => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["April"]),
            MonthEnum.May => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["May"]),
            MonthEnum.June => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["June"]),
            MonthEnum.July => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["July"]),
            MonthEnum.August => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["August"]),
            MonthEnum.September => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["September"]),
            MonthEnum.October => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["October"]),
            MonthEnum.November => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["November"]),
            MonthEnum.December => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["December"]),
            _ => null
        };
    }


    private static List<ManureLoadViewModel> GetManureLoadViewModels(DataTable table)
    {
        var list = new List<ManureLoadViewModel>();
        
        var farms = GetFarms(table);


        for (var row = 4; row < table.Rows.Count; row++) 
        {
            var dataRow = table.Rows[row];

            var manureLoadDate = Utils.ParseToDateTime(dataRow[0].ToString());
            if (!manureLoadDate.HasValue) continue;

            var manureLoad = new ManureLoadViewModel { ManureLoadDate = manureLoadDate.GetValueOrDefault() };

            for (var index = 0; index < farms.Count; index++)
            {
                var farm = farms[index];
                var farmLoadQty = Utils.ParseToInteger(dataRow[index + 1].ToString());
                if (farmLoadQty > 0) manureLoad.AddFarmLoad(farm, farmLoadQty);
            }

            list.Add(manureLoad);
        }

        return list;
    }

    private static List<string> GetFarms(DataTable table)
    {
        var farms = new List<string>();
        for (var col = 1; col < table.Columns.Count; col++) 
        {
            var farm = GetFarmName(table, col);

            if (!string.IsNullOrEmpty(farm) && !farms.Contains(farm)) farms.Add(farm);
        }

        return farms;
    }

    private static string GetFarmName(DataTable table, int col)
    {
        var farm = table.Rows[0][col].ToString();
        if (string.IsNullOrEmpty(farm) || farm.ToUpper() == "ABC") return string.Empty;


        return farm;
    }
    
}