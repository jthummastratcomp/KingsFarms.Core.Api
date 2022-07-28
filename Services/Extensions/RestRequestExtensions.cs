using RestSharp;

namespace HotTowel.Core.Api.Services.Extensions;

public static class RestRequestExtensions
{
    public static RestRequest AddHeaderAuthorization(this RestRequest request, string token)
    {
        request.AddHeader("Authorization", "Bearer " + token);
        return request;
    }

    public static RestRequest AddHeaderLocale(this RestRequest request)
    {
        request.AddHeader("X-locale", "en_US");
        return request;
    }

    public static RestRequest AddHeaderContentTypeJson(this RestRequest request)
    {
        request.AddHeader("Content-Type", "application/json");
        return request;
    }
    public static RestRequest AddHeaderContentTypeUrlEncoded(this RestRequest request)
    {
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        return request;
    }
    public static RestRequest AddParameterGrantTypeClientCredentials(this RestRequest request)
    {
        request.AddParameter("grant_type", "client_credentials");
        return request;
    }
    public static RestRequest AddParameterClientId(this RestRequest request, string clientId)
    {
        request.AddParameter("client_id", clientId);
        return request;
    }
    public static RestRequest AddParameterClientSecret(this RestRequest request, string clientSecret)
    {
        request.AddParameter("client_secret", clientSecret);
        return request;
    }
}