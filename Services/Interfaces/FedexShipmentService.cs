using KingsFarms.Core.Api.Requests;

namespace KingsFarms.Core.Api.Services.Interfaces;

public class FedexShipmentService : IFedexShipmentService
{
    public CreateShipmentResponse? CreateShipment(CreateShipmentRequest request)
    {
        //var buildJson = string.IsNullOrEmpty(json) ? JsonConvert.SerializeObject(FedExShipmentBuilder.BuildShipment()) : json;

        //var client = new RestClient(new RestClientOptions(_url));
        //var request = new RestRequest(FedExEndpoints.FedExCreateShipmentEndPoint)
        //    .AddHeaderAuthorization(_tokenService.GetAccessToken())
        //    .AddHeaderLocale()
        //    .AddHeaderContentTypeJson()
        //    .AddBody(buildJson);

        //var resp = client.Post(request);

        //if (resp.Content != null)
        //{
        //    var des = JsonConvert.DeserializeObject<CreateShipmentResponse>(resp.Content);
        //    return des;
        //}

        return null;
    }
}