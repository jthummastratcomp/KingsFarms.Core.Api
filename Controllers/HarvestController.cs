using System.ComponentModel;
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
    private readonly IWeeklyOrdersService _ordersService;


    public HarvestController(IWeeklyOrdersService ordersService, IHarvestService harvestService, ILogger logger)
    {
        _ordersService = ordersService;
        _harvestService = harvestService;
        _logger = logger;
    }

    [HttpGet(ApiRoutes.HarvestDataBySeason)]
    public IQueryResult GetHarvestInfo(int season)
    {
        _logger.Information("GetHarvestDataBySeason");
        var list = _harvestService.GetHarvestDataBySeason(season);

        return new QueryResult<List<HarvestViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.HarvestDataByCalendar)]
    public IQueryResult GetHarvestDataByCalendar(int calendar)
    {
        _logger.Information("HarvestDataByCalendar");
        var list = _harvestService.GetHarvestDataByCalendar(calendar);

        return new QueryResult<List<HarvestViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.HarvestYearTotalBySeason)]
    public int GetHarvestYearTotal(int season)
    {
        _logger.Information("GetHarvestYearTotalBySeason");
        var total = _harvestService.GetHarvestYearTotalBySeason(season);

        return total;
    }

    [HttpGet(ApiRoutes.HarvestYearTotalByCalendar)]
    public int GetHarvestYearTotalCalendar(int calendar)
    {
        _logger.Information("GetHarvestYearTotalByCalendar");
        var total = _harvestService.GetHarvestYearTotalByCalendar(calendar);

        return total;
    }

    [HttpGet(ApiRoutes.HarvestStatusTotal)]
    public int GetHarvestStatusTotal(string status)
    {
        _logger.Information("GetHarvestStatusTotal");
        var total = _harvestService.GetHarvestStatusTotal(status.GetEnum<DashboardStatusEnum>());

        return total;
    }

    [HttpGet(ApiRoutes.HarvestWeeks, Name = "GetHarvestWeeks")]
    public IEnumerable<SearchDto> GetHarvestWeeks()
    {
        return _ordersService.GetInvoiceWeeksListForYear();
    }



}