using Newtonsoft.Json;

namespace HotTowel.Core.Api.Builders.LocationSearch;

public class FedExShippingPayment
{
    [JsonProperty("paymentType")] public string PaymentType { get; init; }
}