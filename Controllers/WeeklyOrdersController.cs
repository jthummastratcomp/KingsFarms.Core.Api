using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class WeeklyOrdersController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IWeeklyOrdersUsdaService _ordersUsdaService;

    public WeeklyOrdersController(IWeeklyOrdersUsdaService ordersUsdaService, ILogger logger)
    {
        _ordersUsdaService = ordersUsdaService;
        _logger = logger;
    }

    [HttpGet(CoreApiRoutes.GetInvoiceWeeksListForYear)]
    public List<SearchDto> GetInvoiceWeeksListForYear(int year)
    {
        return _ordersUsdaService.GetInvoiceWeeksListForYear(year);
    }

    [HttpGet(CoreApiRoutes.LoadInvoicesForWeek)]
    public List<CustomerInvoicesViewModel> LoadInvoicesForWeek(string week, CompanyEnum company)
    {
        return _ordersUsdaService.LoadInvoicesForWeek(week, company);
    }

    [HttpGet(CoreApiRoutes.FirstMondayOfYear)]
    public DateTime FirstMondayOfYear(int year)
    {
        return Utils.GetFirstMondayOfYear(year);
    }

    [HttpGet(CoreApiRoutes.FirstSaturdayOfYear)]
    public DateTime FirstSaturdayOfYear(int year)
    {
        return Utils.GetFirstSaturdayOfYear(year);
    }
}