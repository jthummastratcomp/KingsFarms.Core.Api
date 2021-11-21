using HotTowel.Core.Api.Controllers;
using HotTowel.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
//[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<CustomerController> _logger;
    private readonly IWeeklyOrdersService _ordersService;

    public CustomerController(ILogger<CustomerController> logger, IWeeklyOrdersService ordersService)
    {
        _logger = logger;
        _ordersService = ordersService;
    }

    //[HttpGet(ApiRoutes.HarvestWeeks, Name = "GetHArvestWeeks")]
    ////[HttpGet]
    ////[Route(ApiRoutes.HarvestWeeks)]
    //public IEnumerable<SearchDto> Get()
    //{
    //    return _ordersService.GetInvoiceWeeksListForYear();
    //}
}