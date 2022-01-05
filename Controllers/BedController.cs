using HotTowel.Web.Results;
using HotTowel.Web.Services.Interfaces;
using HotTowel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotTowel.Core.Api.Controllers;

[ApiController]
public class BedController : ControllerBase
{
    
    private readonly Serilog.ILogger _logger;
    
    private readonly IBedService _bedService;
    


    public BedController(IBedService bedService, Serilog.ILogger logger)
    {
        
        _bedService = bedService;
       
        _logger = logger;
    }

   
    [HttpGet(ApiRoutes.BedInfo, Name = "GetBeds")]
    public IQueryResult GetBedsInfo()
    {
        _logger.Information("GetBedInfo");
        var list = _bedService.GetBedsInfo();

        return new QueryResult<List<BedHarvestFieldOpsViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.BedInfoGrouped, Name = "GetBedsInfoGrouped")]
    public IQueryResult GetBedsInfoGrouped()
    {
        _logger.Information("BedInfoGrouped");
        var list = _bedService.GetBedsInfoGrouped();

        return new QueryResult<List<BedHarvestFieldOpsViewModel>> { Data = list, Status = new SuccessResult() };
    }
    
}