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

    private readonly ILogger<HarvestController> _logger;
    private readonly IWeeklyOrdersService _ordersService;

    public HarvestController(ILogger<HarvestController> logger, IWeeklyOrdersService ordersService)
    {
        _logger = logger;
        _ordersService = ordersService;
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


    [HttpGet]
    [Route(ApiRoutes.HarvestInfo)]
    public List<SearchDto> GetHarvestInfo()
    {
        //return _ordersService.GetInvoiceWeeksListForYear();
        return new List<SearchDto>();
    }
}