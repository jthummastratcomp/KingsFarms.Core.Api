using HotTowel.Core.Api.ViewModels;

namespace HotTowel.Core.Api.Services.Interfaces;

public interface IFedexLocationService
{
    List<FedexLocationViewModel> GetLocations();
    List<FedexLocationViewModel> ValidateAddress();
    List<FedexLocationViewModel> CreateShipment(string? json = null);
    

}