using KingsFarms.Core.Api.Requests;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IFedexShipmentService
{   CreateShipmentResponse? CreateShipment(CreateShipmentRequest request);


}