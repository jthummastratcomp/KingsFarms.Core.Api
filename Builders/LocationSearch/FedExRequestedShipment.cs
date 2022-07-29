using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public record FedExRequestedShipment
{
    [JsonProperty("shipper")] public FedExShipperRecipient? Shipper { get; init; }
    [JsonProperty("recipients")] public List<FedExShipperRecipient> Recepients { get; init; }
    [JsonProperty("pickupType")] public string PickupType { get; init; }
    [JsonProperty("serviceType")] public string ServiceType { get; init; }
    [JsonProperty("packagingType")] public string PackagingType { get; init; }
    [JsonProperty("shippingChargesPayment")] public FedExShippingPayment? ShippingPayment { get; init; }
    [JsonProperty("labelSpecification")] public FedExLabelSpecification? LabelSpec { get; init; }
    [JsonProperty("requestedPackageLineItems")] public List<FedExPackageItem> PackageItems { get; init; }
}