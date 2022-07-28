using System.Text.Json.Serialization;
using HotTowel.Core.Api.Results;
using HotTowel.Core.Api.Services.Interfaces;
using HotTowel.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace HotTowel.Core.Api.Controllers;

[ApiController]
public class FedExPostalController : ControllerBase
{
    private readonly IFedexLocationService _locationService;

    public FedExPostalController(IFedexLocationService locationService)
    {
        _locationService = locationService;
    }
    [HttpGet(ApiRoutes.FedExInfo, Name = "FedExInfo")]
    public IQueryResult GetFedExInfo()
    {
        var data =  _locationService.GetLocations();

        //var token = GetAccessToken();


        //var client = new RestClient(new RestClientOptions("https://apis-sandbox.fedex.com/"));
        //var request = new RestRequest("location/v1/locations")
        //    .AddHeader("Authorization", "Bearer " + token)
        //    .AddHeader("X-locale", "en_US")
        //    .AddHeader("Content-Type", "application/json")
        //    .AddBody(JsonConvert.SerializeObject(new SearchByLocation { loc = new Location { address = new Address() } }));

        //var resp = client.Post(request);

        //var data = new List<FedexLocationViewModel> { new() { BedNumber = resp.Content } };

        return new QueryResult<List<FedexLocationViewModel>> { Data = data, Status = new SuccessResult() };
    }

    //private string GetAccessToken()
    //{
    //    var client = new RestClient(new RestClientOptions("https://apis-sandbox.fedex.com/"));
    //    var request = new RestRequest("oauth/token").AddParameter("grant_type", "client_credentials").AddParameter("client_id", "l74fc21664251c424787e98ae65655e8c8").AddParameter("client_secret", "693a7ae4671e46f58f53f72ea19a85e4")
    //        .AddHeader("Content-Type", "application/x-www-form-urlencoded");
    //    var resp = client.Post<TokenResponse>(request);

    //    return resp.AccessToken;
    //}

    //private record TokenResponse
    //{
    //    [JsonPropertyName("token_type")] public string TokenType { get; init; }

    //    [JsonPropertyName("access_token")] public string AccessToken { get; init; }
    //}


    //private record SearchByLocation
    //{
    //    [JsonProperty("location")] public Location loc { get; init; }
    //}

    //private record Location
    //{
    //    public Address address { get; init; }
    //}

    //private record Address
    //{
    //    public string postalCode { get; init; } = "45040";
    //    public string countryCode { get; init; } = "US";
    //}
}