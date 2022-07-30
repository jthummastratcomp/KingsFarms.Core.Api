using KingsFarms.Core.Api.Builders.LocationSearch;
using KingsFarms.Core.Api.Services.Endpoints;
using KingsFarms.Core.Api.Services.Extensions;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Services;

public class FedexLocationService : IFedexLocationService
{
    private readonly ILogger _logger;
    private readonly IFedexTokenService _tokenService;
    private readonly string _url;

    public FedexLocationService(ILogger logger, IFedexTokenService tokenService, string url)
    {
        _logger = logger;
        _tokenService = tokenService;
        _url = url;
    }

    public List<FedexLocationViewModel> GetLocations()
    {
        var json = JsonConvert.SerializeObject(new FedExLocationSearchBuilder()
            .WithLocation(new FedExAddressBuilder()
                .WithCountryCode("US")
                .WithPostalCode("45040")
                .Build())
            .Build(), Formatting.Indented);

        var client = new RestClient(new RestClientOptions(_url));
        var request = new RestRequest(FedExEndpoints.FedExLocationsEndPoint)
            .AddHeaderAuthorization(_tokenService.GetAccessToken())
            .AddHeaderLocale()
            .AddHeaderContentTypeJson()
            .AddBody(json);

        var resp = client.Post(request);

        return new List<FedexLocationViewModel> { new() { BedNumber = resp.Content } };
    }

    public List<FedexLocationViewModel> ValidateAddress()
    {
        var client = new RestClient(new RestClientOptions(_url));
        var request = new RestRequest(FedExEndpoints.FedExValidateAddressEndPoint)
            .AddHeaderAuthorization(_tokenService.GetAccessToken())
            .AddHeaderLocale()
            .AddHeaderContentTypeJson()
            .AddBody(JsonConvert.SerializeObject(new FedExAddressesToValidateBuilder()
                .WithLocation(new FedExAddressBuilder()
                    .WithCountryCode("US")
                    .WithPostalCode("45040")
                    .WithCity("Cincinnati")
                    .WithStreetLine1("4029 Melampy Creek Ln")
                    .Build())
                .Build()));

        var resp = client.Post(request);

        return new List<FedexLocationViewModel> { new() { BedNumber = resp.Content } };
    }

    public List<FedexLocationViewModel> CreateShipment(string? json = null)
    {
        var buildJson = string.IsNullOrEmpty(json) ? JsonConvert.SerializeObject(FedExShipmentBuilder.BuildShipment()) : json;

        var client = new RestClient(new RestClientOptions(_url));
        var request = new RestRequest(FedExEndpoints.FedExCreateShipmentEndPoint)
            .AddHeaderAuthorization(_tokenService.GetAccessToken())
            .AddHeaderLocale()
            .AddHeaderContentTypeJson()
            .AddBody(buildJson);

        var resp = client.Post(request);

        if (resp.Content != null)
        {
            var des = JsonConvert.DeserializeObject<CreateShipmentResponse>(resp.Content);
        }

        return new List<FedexLocationViewModel> { new() { BedNumber = resp.Content } };
    }

    
}

public class CreateShipmentResponse
{
    [JsonProperty("transactionId")] public string? TransId { get; set; }
    [JsonProperty("output")] public CreateShipmentResponseOutput? Output { get; set; }
    
}

public class CreateShipmentResponseOutput
{
    [JsonProperty("transactionShipments")] public List<TransactionShipment>? Shipments { get; set; }
    [JsonProperty("alerts")] public List<ShipmentAlert>? Alerts { get; set; }
}

public class ShipmentAlert
{
    [JsonProperty("code")] public string? Code { get; init; }
    [JsonProperty("alertType")] public string? AlertType { get; init; }
    [JsonProperty("message")] public string? Message { get; init; }
}

public class TransactionShipment
{
    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
    [JsonProperty("serviceName")] public string? ServiceName { get; init; }
}