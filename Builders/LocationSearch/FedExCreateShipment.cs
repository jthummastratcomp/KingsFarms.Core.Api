using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public record FedExCreateShipment
{
    [JsonProperty("requestedShipment")] public FedExRequestedShipment? RequestedShipment { get; init; }
    [JsonProperty("accountNumber")] public FedExAccountNumber? AccountNumber { get; init; }
    [JsonProperty("labelResponseOptions")] public string? LabelOptions { get; init; }
}