using HotTowel.Web.Results;
using HotTowel.Web.Services;
using HotTowel.Web.Services.Interfaces;
using HotTowel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotTowel.Core.Api.Controllers;

[ApiController]
public class HarvestController : ControllerBase
{
    
    private readonly Serilog.ILogger _logger;
    private readonly IWeeklyOrdersService _ordersService;
    private readonly IHarvestService _harvestService;
    private readonly IBedHarvestFieldOpsService _bedHarvestFieldOpsService;
    private readonly IFieldOperationsService _fieldOperationsService;


    public HarvestController( IWeeklyOrdersService ordersService, IHarvestService harvestService, 
        IBedHarvestFieldOpsService bedHarvestFieldOpsService, IFieldOperationsService fieldOperationsService, 
        Serilog.ILogger logger)
    {
        _ordersService = ordersService;
        _harvestService = harvestService;
        _bedHarvestFieldOpsService = bedHarvestFieldOpsService;
        _fieldOperationsService = fieldOperationsService;
        _logger = logger;
    }


    //[HttpGet(ApiRoutes.HarvestInfo, Name = "GetHarvestInfo")]
    //public List<SearchDto> GetHarvestInfo()
    //{
    //    return new List<SearchDto>();
    //}

    [HttpGet(ApiRoutes.HarvestInfo)]
    public IQueryResult GetHarvestInfo()
    {
        _logger.Information("GetHarvestInfo");
        var list = _harvestService.GetHarvestInfo();

        return new QueryResult<List<HarvestViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.HarvestWeeks, Name = "GetHarvestWeeks")]
    public IEnumerable<SearchDto> GetHarvestWeeks()
    {
        return _ordersService.GetInvoiceWeeksListForYear();
    }

    [HttpGet(ApiRoutes.BedInfo, Name = "GetBeds")]
    public IQueryResult GetBedInfo()
    {
        _logger.Information("GetBedInfo");
        var list = _harvestService.GetBedInfo();

        return new QueryResult<List<BedHarvestFieldOpsViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.BedInfoGrouped, Name = "GetBedsInfoGrouped")]
    public IQueryResult GetBedInfoGrouped()
    {
        _logger.Information("BedInfoGrouped");
        var list = _harvestService.GetBedInfoGrouped();

        return new QueryResult<List<BedHarvestFieldOpsViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.BedInfoHarvestFieldOps)]
    public async Task<IQueryResult> GetBedInfoWithHarvestAndFieldOps() //todo split to 3 separate methods
    {
        _logger.Information("GetBedInfoWithHarvestAndFieldOps");
        var list = await _bedHarvestFieldOpsService.GetOrAddBedInfoToCosmosDbAsync();

        return new QueryResult<List<BedHarvestFieldOpsViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.FieldOperations)]
    public async Task<IQueryResult> GetFieldOperations()
    {
        _logger.Information("GetFieldOperations");

        var list = await _fieldOperationsService.GetBedsFieldOpsAsync();

        return new QueryResult<List<BedHarvestFieldOpsViewModel>> { Data = list, Status = new SuccessResult() };
    }
}