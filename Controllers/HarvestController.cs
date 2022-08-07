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

    [HttpGet(ApiRoutes.HarvestData)]
    public IQueryResult GetHarvestInfo(int year)
    {
        _logger.Information("GetHarvestData");
        var list = _harvestService.GetHarvestData(year);

        return new QueryResult<List<HarvestViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.HarvestYearTotal)]
    public IQueryResult GetHarvestYearTotal(int year)
    {
        _logger.Information("GetHarvestYearTotal");
        var total = _harvestService.GetHarvestYearTotal(year);

        return new QueryResult<int> { Data = total, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.HarvestStatusTotal)]
    public IQueryResult GetHarvestStatusTotal(string status)
    {
        _logger.Information("GetHarvestStatusTotal");
        var total = _harvestService.GetHarvestStatusTotal(status.GetEnum<DashboardStatusEnum>());

        return new QueryResult<int> { Data = total, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.HarvestWeeks, Name = "GetHarvestWeeks")]
    public IEnumerable<SearchDto> GetHarvestWeeks()
    {
        return _ordersService.GetInvoiceWeeksListForYear();
    }



}