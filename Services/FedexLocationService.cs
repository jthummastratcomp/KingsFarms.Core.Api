using HotTowel.Core.Api.Builders.LocationSearch;
using HotTowel.Core.Api.Services.Endpoints;
using HotTowel.Core.Api.Services.Extensions;
using HotTowel.Core.Api.Services.Interfaces;
using HotTowel.Core.Api.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using ILogger = Serilog.ILogger;

namespace HotTowel.Core.Api.Services;

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
        var client = new RestClient(new RestClientOptions(_url));
        var request = new RestRequest(FedExEndpoints.FedExLocationsEndPoint)
            .AddHeaderAuthorization(_tokenService.GetAccessToken())
            .AddHeaderLocale()
            .AddHeaderContentTypeJson()
            .AddBody(JsonConvert.SerializeObject(new FedExLocationSearchBuilder()
                .WithLocation(new SearchLocationAddressBuilder()
                    .WithCountryCode("US")
                    .WithPostalCode("45040")
                    .Build())
                .Build()));

        var resp = client.Post(request);

        return new List<FedexLocationViewModel> { new() { BedNumber = resp.Content } };
    }
}