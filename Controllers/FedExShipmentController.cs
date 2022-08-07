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
    private readonly IFedexShipmentService _shipmentService;


    public FedExShipmentController(IFedexShipmentService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    [HttpGet(ApiRoutes.CreateShipment, Name = "CreateShipment")]
    public IQueryResult CreateShipment()
    {
        var data = _shipmentService.CreateShipment(null);

        return new QueryResult<CreateShipmentResponse?> { Data = data, Status = new SuccessResult() };
    }

    [HttpPost(ApiRoutes.CreateShipmentRequest)]
    public SearchDto CreateShipment(CreateShipmentRequest request)
    //public string? CreateShipment(CreateShipmentRequest request)
    {
        //var json = JsonConvert.SerializeObject(request);

        var data = _shipmentService.CreateShipment(request);
        
        var trackingNumber = data?.Output?.Shipments?[0]?.TrackingNumberMaster;
        var labelUrl = data?.Output?.Shipments?[0]?.PieceResponses?[0]?.packageDocuments?[0].url;

        return new SearchDto() { Id = trackingNumber, Data = labelUrl };
    }
}