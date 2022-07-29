using KingsFarms.Core.Api.Results;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class FedExLocationsController : ControllerBase
{
    private readonly IFedexLocationService _locationService;

    public FedExLocationsController(IFedexLocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet(ApiRoutes.FedExLocationInfo, Name = "FedExLocationInfo")]
    public IQueryResult GetFedExLocationInfo()
    {
        var data = _locationService.GetLocations();

        return new QueryResult<List<FedexLocationViewModel>> { Data = data, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.ValidatedAddress, Name = "ValidatedAddress")]
    public IQueryResult GetValidatedAddress()
    {
        var data = _locationService.ValidateAddress();

        return new QueryResult<List<FedexLocationViewModel>> { Data = data, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.CreateShipment, Name = "CreateShipment")]
    public IQueryResult CreateShipment()
    {
        var data = _locationService.CreateShipment();

        return new QueryResult<List<FedexLocationViewModel>> { Data = data, Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.CreateShipmentJson, Name = "CreateShipmentJson")]
    public IQueryResult CreateShipmentJson(string json)
    {
        var data = _locationService.CreateShipment(json);

        return new QueryResult<List<FedexLocationViewModel>> { Data = data, Status = new SuccessResult() };
    }
}