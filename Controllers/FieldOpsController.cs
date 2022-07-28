using HotTowel.Core.Api.Results;
using HotTowel.Core.Api.Services.Interfaces;
using HotTowel.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotTowel.Core.Api.Controllers;

[ApiController]
public class FieldOpsController : ControllerBase
{
    
    private readonly Serilog.ILogger _logger;
   
    private readonly IBedHarvestFieldOpsService _bedHarvestFieldOpsService;
    private readonly IFieldOperationsService _fieldOperationsService;


    public FieldOpsController(IBedHarvestFieldOpsService bedHarvestFieldOpsService, IFieldOperationsService fieldOperationsService, 
        Serilog.ILogger logger)
    {
       
        _bedHarvestFieldOpsService = bedHarvestFieldOpsService;
        _fieldOperationsService = fieldOperationsService;
        _logger = logger;
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