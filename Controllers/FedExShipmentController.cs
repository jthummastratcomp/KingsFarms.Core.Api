using KingsFarms.Core.Api.Requests;
using KingsFarms.Core.Api.Results;
using KingsFarms.Core.Api.Services;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class FedExShipmentController : ControllerBase
{
    private readonly IFedexLocationService _locationService;

    public FedExShipmentController(IFedexLocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet(ApiRoutes.CreateShipmentRequest, Name = "CreateShipmentJson")]
    public IQueryResult CreateShipment(CreateShipmentRequest request)
    {
        //var data = _locationService.CreateShipment(json);

        return new QueryResult<List<FedexLocationViewModel>> { Data = null, Status = new SuccessResult() };
    }
}