using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExShippingPayment
{
    [JsonProperty("paymentType")] public string? PaymentType { get; init; }
}