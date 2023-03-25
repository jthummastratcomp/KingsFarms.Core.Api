using KingsFarms.Core.Api.ViewModels.Shipment;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IFedexLocationService
{
    List<FedexLocationViewModel> GetLocations();

    List<FedexLocationViewModel> ValidateAddress();
    //CreateShipmentResponse? CreateShipment(string? json = null);
}