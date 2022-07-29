using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExShipperRecipient
{
    [JsonProperty("address")] public FedExAddress Address { get; init; }
    [JsonProperty("contact")] public FedExContact Contact { get; init; }
}