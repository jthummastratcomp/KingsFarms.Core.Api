using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Results;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class HarvestController : ControllerBase
{
    private readonly IHarvestService _harvestService;

    private readonly ILogger _logger;


    public HarvestController(IHarvestService harvestService, ILogger logger)
    {
        _harvestService = harvestService;
        _logger = logger;
    }

    [HttpGet(CoreApiRoutes.HarvestDataAll)]
    public List<HarvestViewModel> GetAllHarvestData()
    {
        return _harvestService.GetAllHarvestData().ToList();
        
    }

    [HttpGet(CoreApiRoutes.HarvestDataBySeason)]
    public IQueryResult GetHarvestInfo(int season)
    {
        _logger.Information("GetHarvestDataBySeason");
        var list = _harvestService.GetHarvestDataBySeason(season);

        return new QueryResult<List<HarvestViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(CoreApiRoutes.HarvestDataByCalendar)]
    public IQueryResult GetHarvestDataByCalendar(int calendar)
    {
        _logger.Information("HarvestDataByCalendar");
        var list = _harvestService.GetHarvestDataByCalendar(calendar);

        return new QueryResult<List<HarvestViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(CoreApiRoutes.HarvestYearTotalBySeason)]
    public int GetHarvestYearTotal(int season)
    {
        _logger.Information("GetHarvestYearTotalBySeason");
        var total = _harvestService.GetHarvestYearTotalBySeason(season);

        return total;
    }

    [HttpGet(CoreApiRoutes.HarvestYearTotalByCalendar)]
    public int GetHarvestYearTotalCalendar(int calendar)
    {
        _logger.Information("GetHarvestYearTotalByCalendar");
        var total = _harvestService.GetHarvestYearTotalByCalendar(calendar);

        return total;
    }

    [HttpGet(CoreApiRoutes.HarvestStatusTotal)]
    public int GetHarvestStatusTotal(string status)
    {
        _logger.Information("GetHarvestStatusTotal");
        var total = _harvestService.GetHarvestStatusTotal(status.GetEnum<DashboardStatusEnum>());

        return total;
    }

    [HttpGet(CoreApiRoutes.HarvestDataCalendarAll)]
    public HarvestDto GetHarvestDataCalendarAll()
    {
        _logger.Information("GetHarvestDataCalendarAll");
        return _harvestService.GetHarvestDataCalendarAll();
    }

    [HttpGet(CoreApiRoutes.HarvestDataSeasonAll)]
    public HarvestDto GetHarvestDataSeasonAll()
    {
        _logger.Information("GetHarvestDataSeasonAll");
        return _harvestService.GetHarvestDataSeasonAll();
    }

    [HttpGet(CoreApiRoutes.HarvestByYearBySection)]
    public List<SectionHarvestViewModel> GetHarvestByYearBySection()
    {
        return _harvestService.GetHarvestByYearBySection();
    }

    [HttpGet(CoreApiRoutes.HarvestByYearByBed)]
    public List<BedHarvestChartViewModel> GetHarvestByYearByBed()
    {
        return _harvestService.GetHarvestByYearByBed();
    }
}