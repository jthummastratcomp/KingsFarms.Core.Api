using KingsFarms.Core.Api.Results;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class BedController : ControllerBase
{
    private readonly IBedService _bedService;

    private readonly ILogger _logger;


    public BedController(IBedService bedService, ILogger logger)
    {
        _bedService = bedService;

        _logger = logger;
    }


    [HttpGet(CoreApiRoutes.BedInfo, Name = "GetBedsInfo")]
    public IQueryResult GetBedsInfo()
    {
        _logger.Information("GetBedInfo");
        var list = _bedService.GetBedsInfo();

        return new QueryResult<List<BedHarvestFieldOpsViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(CoreApiRoutes.BedInfoGrouped, Name = "GetBedsInfoGrouped")]
    public IQueryResult GetBedsInfoGrouped()
    {
        _logger.Information("BedInfoGrouped");
        var list = _bedService.GetBedsInfoGrouped();

        return new QueryResult<List<BedHarvestFieldOpsViewModel>> { Data = list, Status = new SuccessResult() };
    }
}