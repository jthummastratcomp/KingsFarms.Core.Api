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
    private readonly IAppCache _appCache;
    private readonly string _azStoreConnStr;
    private readonly string _azStoreContName;
    private readonly string _harvestFile;
    private readonly ILogger _logger;

    public HarvestService(ILogger logger, IAppCache appCache, string azStoreConnStr, string azStoreContName,
        string harvestFile)
    {
        _logger = logger;
        _azStoreConnStr = azStoreConnStr;
        _azStoreContName = azStoreContName;
        _harvestFile = harvestFile;
        _appCache = appCache;
    }

    public List<HarvestViewModel> GetHarvestDataBySeason(int harvestYear)
    {
        return GetHarvestDataForYearBySeason(harvestYear);
    }

    public List<HarvestViewModel> GetHarvestDataByCalendar(int calendarYear)
    {
        var list = GetAllHarvestData();
        return list.Where(x => x.HarvestDate.Year == calendarYear).ToList();
    }

    public int GetHarvestYearTotalBySeason(int harvestYear)
    {
        var list = GetHarvestDataForYearBySeason(harvestYear);
        var total = list.Sum(x => x.HarvestQty);

        return total;
    }

    public int GetHarvestYearTotalByCalendar(int calendarYear)
    {
        var list = GetAllHarvestData();
        var total = list.Where(x => x.HarvestDate.Year == calendarYear).Sum(x => x.HarvestQty);

        return total;
    }

    public int GetHarvestStatusTotal(DashboardStatusEnum status)
    {
        var list = GetAllHarvestData();

        return status switch
        {
            DashboardStatusEnum.CurrentYear => list.Where(x => x.HarvestDate.Year == DateTime.Today.Year).Sum(x => x.HarvestQty),
            DashboardStatusEnum.LastYear => list.Where(x => x.HarvestDate.Year == DateTime.Today.AddYears(-1).Year).Sum(x => x.HarvestQty),
            _ => list.Sum(x => x.HarvestQty)
        };
    }

    public HarvestDto GetHarvestDataCalendarAll()
    {
        return new HarvestDto
        {
            ThisYear = GetHarvestYearTotalByCalendar(DateTime.Today.Year),
            LastYear = GetHarvestYearTotalByCalendar(DateTime.Today.AddYears(-1).Year),
            AllYears = GetAllHarvestData().Sum(x => x.HarvestQty)
        };
    }

    public HarvestDto GetHarvestDataSeasonAll()
    {
        throw new NotImplementedException();
    }

    public List<SectionHarvestViewModel> GetHarvestByYearBySection()
    {
        var list = GetAllHarvestData().ToList();
        if (!Utils.HasRows(list)) return new List<SectionHarvestViewModel>();


        var sectionHarvests = new List<SectionHarvestViewModel>();

        var harvestYears = list.Select(y => y.HarvestDate.Year).Distinct();
        foreach (var harvestYear in harvestYears)
        {
            var sectionHarvestForYear = new SectionHarvestViewModel { HarvestYear = harvestYear };

            var harvestsForTheYear = list.Where(x => x.HarvestDate.Year == harvestYear).ToList();

            var groupBySection = harvestsForTheYear.GroupBy(x => x.Section);

            foreach (var groupBy in groupBySection)
            {
                var section = groupBy.Key;
                var sectionHarvest = groupBy.Sum(x => x.HarvestQty);

                if (section == SectionEnum.MidEastOne) sectionHarvestForYear.MidEast1HarvestQty = sectionHarvest;
                if (section == SectionEnum.MidEastTwo) sectionHarvestForYear.MidEast2HarvestQty = sectionHarvest;
                if (section == SectionEnum.MidWest) sectionHarvestForYear.MidWestHarvestQty = sectionHarvest;
                if (section == SectionEnum.SouthEastOne) sectionHarvestForYear.SouthEast1HarvestQty = sectionHarvest;
                if (section == SectionEnum.SouthEastTwo) sectionHarvestForYear.SouthEast2HarvestQty = sectionHarvest;
                if (section == SectionEnum.SouthWest) sectionHarvestForYear.SouthWestHarvestQty = sectionHarvest;
            }

            sectionHarvests.Add(sectionHarvestForYear);
        }

        return sectionHarvests;
    }

    public List<BedHarvestChartViewModel> GetHarvestByYearByBed()
    {
        var list = GetAllHarvestData().ToList();
        if (!Utils.HasRows(list)) return new List<BedHarvestChartViewModel>();

        var bedHarvests = new List<BedHarvestChartViewModel>();

        var beds = list.Select(y => y.BedNumber).Distinct();

        foreach (var bed in beds)
        {
            var bedHarvest = new BedHarvestChartViewModel { BedNumber = bed };

            var harvestsForTheBed = list.Where(x => x.BedNumber == bed).ToList();

            var groupByYearForBed = harvestsForTheBed.GroupBy(x => x.HarvestDate.Year);

            foreach (var groupBy in groupByYearForBed)
            {
                var year = groupBy.Key;
                var yearHarvestForBed = groupBy.Sum(x => x.HarvestQty);

                if (year == 2019) bedHarvest.Year2019 = yearHarvestForBed;
                if (year == 2020) bedHarvest.Year2020 = yearHarvestForBed;
                if (year == 2021) bedHarvest.Year2021 = yearHarvestForBed;
                if (year == 2022) bedHarvest.Year2022 = yearHarvestForBed;
                if (year == 2023) bedHarvest.Year2023 = yearHarvestForBed;
                if (year == 2024) bedHarvest.Year2024 = yearHarvestForBed;
            }

            bedHarvests.Add(bedHarvest);
        }


        return bedHarvests;
    }

    public IEnumerable<HarvestViewModel> GetAllHarvestData()
    {
        return _appCache.GetOrAdd("GetAllHarvestData", () =>
        {
            var list = new List<HarvestViewModel>();
            for (var year = 2020; year <= DateTime.Today.Year; year++) list.AddRange(GetHarvestDataForYearBySeason(year));
            return list;
        }, DateTime.Now.AddHours(2));
    }


    private List<HarvestViewModel> GetHarvestDataForYearBySeason(int season)
    {
        var list = new List<HarvestViewModel>();

        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_harvestFile);

        using (var memoryStream = new MemoryStream())
        {
            blob.DownloadTo(memoryStream);

            using var package = new ExcelPackage(memoryStream);
            var dtHarvest = GetHarvestWorksheetData(package, season);
            if (dtHarvest != null) list = GetHarvestViewModels(dtHarvest, season);
        }

        _logger.Information("GetHarvestDataBySeason for {@Year} returning {@Count}", season, list.Count);
        return list;
    }

    private static DataTable? GetHarvestWorksheetData(ExcelPackage package, int season)
    {
        return season switch
        {
            2020 => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["2020_2021_HARVEST"]),
            2021 => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["2021_2022_HARVEST"]),
            2022 => EpplusUtils.ExcelPackageToDataTable(package.Workbook.Worksheets["2022_2023_HARVEST"]),
            _ => null
        };
    }

    private static List<HarvestViewModel> GetHarvestViewModels(DataTable table, int season)
    {
        var list = new List<HarvestViewModel>();

        var rowsToLoop = 76;
        if (season == 2020) rowsToLoop = 70;

        for (var row = 6; row < rowsToLoop; row++)
        {
            var dataRow = table.Rows[row];
            var harvestDate = Utils.ParseToDateTime(dataRow[1].ToString());
            if (!harvestDate.HasValue) continue;

            //list.Add(new HarvestViewModel(harvestDate.GetValueOrDefault())
            //    { BedHarvests = GetHarvestsForWeek(dataRow, season) });

            list.AddRange(GetHarvestsForWeek(dataRow, season, harvestDate.GetValueOrDefault()));
        }

        return list;
    }

    private static List<HarvestViewModel> GetHarvestsForWeek(DataRow row, int season, DateTime harvestDate)
    {
        var list = new List<HarvestViewModel>();

        var colMax = season == 2020 ? 33 : 56;
        for (var col = 5; col < colMax; col++)
        {
            var qty = Utils.ParseToInteger(row[col].ToString());
            if (qty <= 0) continue;
            var model = new HarvestViewModel
            {
                BedNumber = col - 4,
                HarvestQty = qty,
                HarvestDate = harvestDate
            };

            list.Add(model);
        }

        return list;
    }

    //private static List<HarvestBedViewModel> GetHarvestsForWeek(DataRow row, int season)
    //{
    //    var list = new List<HarvestBedViewModel>();

    //    var colMax = season == 2020 ? 33 : 56;
    //    for (var col = 5; col < colMax; col++)
    //    {
    //        var qty = Utils.ParseToInteger(row[col].ToString());
    //        if (qty <= 0) continue;
    //        var model = new HarvestBedViewModel
    //        {
    //            BedNumber = $"Bed {col - 4}",
    //            HarvestQty = qty,

    //        };

    //        list.Add(model);
    //    }

    //    return list;
    //}
}