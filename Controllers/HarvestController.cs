using HotTowel.Web.Results;
using HotTowel.Web.Services.Interfaces;
using HotTowel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotTowel.Core.Api.Controllers;

[ApiController]
//[Route("[controller]")]
//[Route("harvest")]
public class HarvestController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly Serilog.ILogger _logger;
    private readonly IWeeklyOrdersService _ordersService;
    private readonly IHarvestService _harvestService;

    //public HarvestController(ILogger<HarvestController> logger, IWeeklyOrdersService ordersService)
    //{
    //    _logger = logger;
    //    _ordersService = ordersService;
    //}

    public HarvestController( IWeeklyOrdersService ordersService, IHarvestService harvestService, Serilog.ILogger logger)
    {
        _ordersService = ordersService;
        _harvestService = harvestService;
        _logger = logger;
    }

    //[HttpGet]
    //[Route(ApiRoutes.AllCustomersWeeklyShipmentsForGraph)]
    //public IEnumerable<WeatherForecast> Get()
    //{
    //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //        {
    //            Date = DateTime.Now.AddDays(index),
    //            TemperatureC = Random.Shared.Next(-20, 55),
    //            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //        })
    //        .ToArray();
    //}

    //[HttpGet]
    //[Route(ApiRoutes.HarvestWeeks)]
    //public List<SearchDto> GetInvoiceWeeksListForYear()
    //{
    //    //return _ordersService.GetInvoiceWeeksListForYear();
    //    return new List<SearchDto>();
    //}


    [HttpGet(ApiRoutes.HarvestInfo, Name = "GetHarvestInfo")]
    //[Route(ApiRoutes.HarvestInfo)]
    public List<SearchDto> GetHarvestInfo()
    {
        //return _ordersService.GetInvoiceWeeksListForYear();
        return new List<SearchDto>();
    }

    [HttpGet(ApiRoutes.HarvestWeeks, Name = "GetHarvestWeeks")]
    //[HttpGet]
    //[Route(ApiRoutes.HarvestWeeks)]
    public IEnumerable<SearchDto> GetHarvestWeeks()
    {
        return _ordersService.GetInvoiceWeeksListForYear();
    }

    [HttpGet(ApiRoutes.BedInfo)]
    public IQueryResult GetBedInfo()
    {
        _logger.Information("GetBedInfo");
        var list = _harvestService.GetBedInfo();

        return new QueryResult<List<BedHarvestFieldOpsViewModel>> { Data = list, Status = new SuccessResult() };
    }
}