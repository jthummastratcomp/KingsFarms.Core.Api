using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class WeeklyOrdersController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IWeeklyOrdersService _ordersService;
    private readonly IWeeklyOrdersUsdaService _ordersUsdaService;

    public WeeklyOrdersController(IWeeklyOrdersService ordersService, IWeeklyOrdersUsdaService ordersUsdaService, ILogger logger)
    {
        _ordersService = ordersService;
        _ordersUsdaService = ordersUsdaService;
        _logger = logger;
    }

    [HttpGet(CoreApiRoutes.GetInvoiceWeeksListForYear)]
    public List<SearchDto> GetInvoiceWeeksListForYear(int year)
    {
        return _ordersService.GetInvoiceWeeksListForYear(year);
    }

    [HttpGet(CoreApiRoutes.LoadInvoicesForWeek)]
    public List<CustomerInvoicesViewModel> LoadInvoicesForWeek(string week, CompanyEnum company)
    {
        //return _ordersService.LoadInvoicesForWeek(week, company);
        return _ordersUsdaService.LoadInvoicesForWeek(week, company);
    }
    
}