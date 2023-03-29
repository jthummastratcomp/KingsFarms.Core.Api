using KingsFarms.Core.Api.Helpers;
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

    [HttpGet(CoreApiRoutes.CreateShipment, Name = "CreateShipment")]
    public IQueryResult CreateShipment()
    {
        var data = _shipmentService.CreateShipment(null);

        return new QueryResult<CreateShipmentResponse?> { Data = data, Status = new SuccessResult() };
    }

    [HttpPost(CoreApiRoutes.CreateShipmentRequest)]
    public List<SearchDto> CreateShipment(CreateShipmentRequest request)

    {
        var list = new List<SearchDto>();

        var data = _shipmentService.CreateShipment(request);

        var shipments = data?.Output?.Shipments?.ToList();
        if (shipments == null) return list;

        foreach (var shipment in shipments)
        {
            var masterTrackingNumber = shipment?.TrackingNumberMaster;

            if (shipment?.PieceResponses == null) continue;

            foreach (var pieceResponse in shipment?.PieceResponses!)
            {
                //var trackingNumber = $"{pieceResponse?.TrackingNumberMaster}";

                foreach (var packageDocument in pieceResponse.packageDocuments)
                {
                    var labelUrl = packageDocument.url;

                    list.Add(new SearchDto()
                    {
                        //Id = trackingNumber,
                        Data = labelUrl,
                        //DataType = masterTrackingNumber,
                        //Messages = messages
                    });
                }

                

                //var messages = new List<string>();

                //foreach (var customerReference in pieceResponse?.customerReferences!)
                //{
                //    messages.Add($"{customerReference.customerReferenceType} - {customerReference.value}");
                //}

                //list.Add(new SearchDto()
                //{
                //    Id = trackingNumber,
                //    //Data = labelUrl,
                //    DataType = masterTrackingNumber,
                //    //Messages = messages
                //});
            }

            if (shipment.CompletedShipmentDetail?.completedPackageDetails == null) continue;
            for (int i = 0; i < shipment?.CompletedShipmentDetail?.completedPackageDetails.Count; i++)
            {
                var trackingIds = shipment?.CompletedShipmentDetail?.completedPackageDetails[i].trackingIds;
                if (trackingIds != null && Utils.HasRows(trackingIds))
                {
                    var trackingId = trackingIds?.FirstOrDefault();
                    list[i].Id = trackingId?.trackingNumber;
                }
            }

            
        }

        //var trackingNumber = data?.Output?.Shipments?[0]?.TrackingNumberMaster;
        //var labelUrl = data?.Output?.Shipments?[0]?.PieceResponses?[0]?.packageDocuments?[0].url;

        //return new SearchDto { Id = trackingNumber, Data = labelUrl };

        return list;
    }
}