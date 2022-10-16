using KingsFarms.Core.Api.Builders;
using KingsFarms.Core.Api.Requests;
using KingsFarms.Core.Api.Services.Endpoints;
using KingsFarms.Core.Api.Services.Extensions;
using KingsFarms.Core.Api.Services.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Services;

public class FedexShipmentService : IFedexShipmentService
{
    private readonly ILogger _logger;
    private readonly IFedexTokenService _tokenService;
    private readonly string _url;

    public FedexShipmentService(ILogger logger, IFedexTokenService tokenService, string url)
    {
        _logger = logger;
        _tokenService = tokenService;
        _url = url;
    }

    public CreateShipmentResponse? CreateShipment(CreateShipmentRequest request)
    {
        var buildJson = JsonConvert.SerializeObject(FedExShipmentBuilder.BuildShipment(request));

        var client = new RestClient(new RestClientOptions(_url));
        var req = new RestRequest(FedExEndpoints.FedExCreateShipmentEndPoint)
            .AddHeaderAuthorization(_tokenService.GetAccessToken())
            .AddHeaderLocale()
            .AddHeaderContentTypeJson()
            .AddBody(buildJson);

        var resp = client.Post(req);

        if (resp.Content == null) return null;

        var des = JsonConvert.DeserializeObject<CreateShipmentResponse>(resp.Content);
        return des;
    }
}