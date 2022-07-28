using HotTowel.Core.Api.Results;
using HotTowel.Core.Api.Services.Interfaces;
using HotTowel.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace HotTowel.Core.Api.Controllers;

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

    [HttpGet(ApiRoutes.HarvestInfo)]
    public IQueryResult GetHarvestInfo(int year)
    {
        _logger.Information("GetHarvestInfo");
        var list = _harvestService.GetHarvestInfo(year);

        return new QueryResult<List<HarvestViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.HarvestWeeks, Name = "GetHarvestWeeks")]
    public IEnumerable<SearchDto> GetHarvestWeeks()
    {
        return _ordersService.GetInvoiceWeeksListForYear();
    }
}