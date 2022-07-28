using System.Text.Json.Serialization;
using HotTowel.Core.Api.Services.Endpoints;
using HotTowel.Core.Api.Services.Extensions;
using HotTowel.Core.Api.Services.Interfaces;
using RestSharp;

namespace HotTowel.Core.Api.Services;

public class FedexTokenService : IFedexTokenService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _url;

    public FedexTokenService(string url, string clientId, string clientSecret)
    {
        _url = url;
        _clientId = clientId;
        _clientSecret = clientSecret;
    }


    public string GetAccessToken()
    {
        var client = new RestClient(new RestClientOptions(_url));

        var request = new RestRequest(FedExEndpoints.FedExTokenEndPoint)
            .AddParameterGrantTypeClientCredentials()
            .AddParameterClientId(_clientId)
            .AddParameterClientSecret(_clientSecret)
            .AddHeaderContentTypeUrlEncoded();

        var resp = client.Post<TokenResponse>(request);

        return resp.AccessToken;
    }

    private record TokenResponse
    {
        [JsonPropertyName("token_type")] public string TokenType { get; init; }

        [JsonPropertyName("access_token")] public string AccessToken { get; init; }
    }
}