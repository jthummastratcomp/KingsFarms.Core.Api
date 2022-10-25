using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IWeeklyOrdersUsdaService _ordersService;

    public CustomerController(ILogger<CustomerController> logger, IWeeklyOrdersUsdaService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpGet(CoreApiRoutes.GetCustomersFromOrdersFile)]
    public List<CustomerDashboardViewModel> GetCustomersFromOrdersFile()
    {
        return _ordersService.GetCustomersFromOrdersFile();
    }
}