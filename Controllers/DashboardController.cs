using KingsFarms.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class DashboardController : ControllerBase
{
    private readonly ILogger _logger;


    public DashboardController(ILogger logger)
    {
        _logger = logger;
    }


    [HttpGet(ApiRoutes.Dashboard)]
    public List<DashboardViewModel> GetDashboard()
    {
        _logger.Information("GetDashboard");
        //var list = _harvestService.GetHarvestData();

        var list = new List<DashboardViewModel>
        {
            new()
            {
                Name = "Customers & Invoices",
                Data1 = new SearchDto { DataType = "Total Billed", Data = "266478" },
                Data2 = new SearchDto { DataType = "Total Paid", Data = "254125" },
                Data3 = new SearchDto { DataType = "Total Balance", Data = "11589" }
            },
            new()
            {
                Name = "Harvests & Field Ops",
                Data1 = new SearchDto { DataType = "Total Harvested", Data = "18453" },
                Data2 = new SearchDto { DataType = "Total Plants", Data = "8515" },
                Data3 = new SearchDto { DataType = "Average per Plant", Data = "2.56" }
            },
            new()
            {
                Name = "Expenses & Labor",
                Data1 = new SearchDto { DataType = "Total Expense", Data = "266478" },
                Data2 = new SearchDto { DataType = "Total Labor", Data = "254125" },
                Data3 = new SearchDto { DataType = "Profit/Loss", Data = "11589" }
            }
        };

        return list;
    }
}