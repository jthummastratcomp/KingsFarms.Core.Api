using KingsFarms.Core.Api.Data.Db;
using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class CustomerController : ControllerBase
{
    private readonly KingsFarmsDbContext _context;
    private readonly IWeeklyOrdersUsdaService _ordersService;

    public CustomerController(ILogger<CustomerController> logger, IWeeklyOrdersUsdaService ordersService, KingsFarmsDbContext context)
    {
        _ordersService = ordersService;
        _context = context;
    }

    [HttpGet(CoreApiRoutes.GetCustomersFromOrdersFile)]
    public List<CustomerDashboardViewModel> GetCustomersFromOrdersFile()
    {
        return _ordersService.GetCustomersFromOrdersFile();
    }

    //[HttpGet(CoreApiRoutes.GetCustomersFromDb)]
    //public List<Bed> GetCustomers()
    //{
    //    return _context.Beds.ToList();
    //}
}