using HotTowel.Web.Results;
using HotTowel.Web.Services.Interfaces;
using HotTowel.Web.ViewModels;
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
    public IQueryResult GetHarvestInfo()
    {
        _logger.Information("GetHarvestInfo");
        var list = _harvestService.GetHarvestInfo();

        return new QueryResult<List<HarvestViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.HarvestWeeks, Name = "GetHarvestWeeks")]
    public IEnumerable<SearchDto> GetHarvestWeeks()
    {
        return _ordersService.GetInvoiceWeeksListForYear();
    }
}