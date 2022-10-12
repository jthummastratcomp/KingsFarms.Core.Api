using System.Data;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using OfficeOpenXml;

namespace KingsFarms.Core.Api.Services;

public class UsdaService : IUsdaService
{
    private readonly Serilog.ILogger _logger;
    private readonly string _azStoreConnStr;
    private readonly string _azStoreContName;
    private readonly string _weeklyOrdersUsdaFile;

    public UsdaService(Serilog.ILogger logger, string azStoreConnStr, string azStoreContName, string weeklyOrdersUsdaFile)
    {
        _logger = logger;
        _azStoreConnStr = azStoreConnStr;
        _azStoreContName = azStoreContName;
        _weeklyOrdersUsdaFile = weeklyOrdersUsdaFile;
    }
    
    //[CacheTimeout]
    public List<BedHarvestFieldOpsViewModel> GetBedsInfo()
    {
        var list = new List<BedHarvestFieldOpsViewModel>();

        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_weeklyOrdersUsdaFile);

        using (var memoryStream = new MemoryStream())
        {
            blob.DownloadTo(memoryStream);

            using (var package = new ExcelPackage(memoryStream))
            {
                var harvest20_21 = package.Workbook.Worksheets["2020_2021_HARVEST"];
                var harvest21_22 = package.Workbook.Worksheets["2021_2022_HARVEST"];
                var harvest22_23 = package.Workbook.Worksheets["2022_2023_HARVEST"];

                var dtHarvest20_21 = EpplusUtils.ExcelPackageToDataTable(harvest20_21);
                var dtHarvest21_22 = EpplusUtils.ExcelPackageToDataTable(harvest21_22);
                var dtHarvest22_23 = EpplusUtils.ExcelPackageToDataTable(harvest22_23);

                var dataRow = dtHarvest22_23.Rows[1];
                 
                for (var col = 5; col < 56; col++)
                {
                    var bedNumber = col - 4;
                    var plantsCount = Utils.ParseToInteger(dataRow[col].ToString());
                    list.Add(new BedHarvestFieldOpsViewModel
                    {
                        Id = $"Bed.{bedNumber}",
                        BedNumber = $"Bed {bedNumber}",
                        PlantsCount = plantsCount,
                        Section = GetBedSection(bedNumber),
                        PlantedDate = GetPlantedDate(bedNumber),
                        HarvestQty20_21 = bedNumber > 28 ? 0 : GetHarvestQuantityForBed(dtHarvest20_21, col),
                        HarvestQty21_22 = bedNumber > 51 ? 0 : GetHarvestQuantityForBed(dtHarvest21_22, col),
                        HarvestQty22_23 = bedNumber > 51 ? 0 : GetHarvestQuantityForBed(dtHarvest22_23, col),
                        
                    });
                }

                var total = new BedHarvestFieldOpsViewModel()
                {
                    Id = "Total",
                    BedNumber = "Total",
                    PlantsCount = list.Sum(x=>x.PlantsCount),
                    Section = GetBedSection(0),
                    HarvestQty20_21 = list.Sum(x=>x.HarvestQty20_21),
                    HarvestQty21_22 = list.Sum(x => x.HarvestQty21_22),
                    HarvestQty22_23 = list.Sum(x => x.HarvestQty22_23)

                };
                list.Add(total);
            }
        }
        _logger.Information("GetBedInfo returning {@Count}", list.Count);
        return list;
    }


    //[CacheTimeout]
    public List<BedHarvestFieldOpsViewModel> GetBedsInfoGrouped()
    {
        var groupedList = GetBedsGroupedBySection(GetBedsInfo());

        var list = new List<BedHarvestFieldOpsViewModel>();

        foreach (var byYearGroup in groupedList)
            list.Add(new BedHarvestFieldOpsViewModel
            {
                Section = byYearGroup.Key,
                BedNumber = byYearGroup.Count().ToString(),
                PlantsCount = byYearGroup.Sum(x => x.PlantsCount),
                PlantedDate = byYearGroup.First().PlantedDate,
                HarvestQty20_21 = byYearGroup.Sum(x => x.HarvestQty20_21),
                HarvestQty21_22 = byYearGroup.Sum(x => x.HarvestQty21_22),
                HarvestQty22_23 = byYearGroup.Sum(x => x.HarvestQty22_23)
            });

        _logger.Information("GetBedInfoGrouped returning {@Count}", list.Count);
        return list.OrderBy(x => x.Section).ToList();
    }

    private static IEnumerable<IGrouping<SectionEnum, BedHarvestFieldOpsViewModel>> GetBedsGroupedBySection(IEnumerable<BedHarvestFieldOpsViewModel> list)
    {
        return from bed in list
            group bed by bed.Section
            into listBySection
            orderby listBySection.Key
            select listBySection;
    }

    private static SectionEnum GetBedSection(int bedNumber)
    {
        if (bedNumber >= 1 && bedNumber <= 11) return SectionEnum.MidWest;
        if (bedNumber >= 12 && bedNumber <= 22) return SectionEnum.SouthWest;
        if (bedNumber >= 23 && bedNumber <= 28) return SectionEnum.SouthEastOne;
        if (bedNumber >= 29 && bedNumber <= 37) return SectionEnum.SouthEastTwo;
        if (bedNumber >= 38 && bedNumber <= 45) return SectionEnum.MidEastTwo;
        if (bedNumber >= 46 && bedNumber <= 51) return SectionEnum.MidEastOne;
        return SectionEnum.None;
    }

    private static DateTime GetPlantedDate(int bedNumber)
    {
        if (bedNumber >= 1 && bedNumber <= 11) return new DateTime(2018, 9, 1);
        if (bedNumber >= 12 && bedNumber <= 22) return new DateTime(2018, 9, 1);
        if (bedNumber >= 23 && bedNumber <= 28) return new DateTime(2019, 10, 1);
        if (bedNumber >= 29 && bedNumber <= 37) return new DateTime(2020, 12, 1);
        if (bedNumber >= 38 && bedNumber <= 45) return new DateTime(2021, 1, 1);
        if (bedNumber >= 46 && bedNumber <= 51) return new DateTime(2021, 1, 1);
        return DateTime.MinValue;
    }
        
    private static int GetHarvestQuantityForBed(DataTable table, int bedNumber)
    {
        var qty = 0;
        for (var row = 5; row < 70; row++) qty += Utils.ParseToInteger(table.Rows[row][bedNumber].ToString());

        return qty;
    }

    public string UpdateUsdaInfo(UsdaBedLotInfoViewModel viewModel)
    {
        throw new NotImplementedException();
    }
}