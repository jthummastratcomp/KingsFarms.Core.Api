using Newtonsoft.Json;

namespace HotTowel.Core.Api.Builders.LocationSearch;

public record FedExCreateShipment
{
    [JsonProperty("requestedShipment")] public FedExRequestedShipment RequestedShipment { get; init; }
    [JsonProperty("accountNumber")] public FedExAccountNumber AccountNumber { get; init; }
    [JsonProperty("labelResponseOptions")] public string LabelOptions { get; init; }
}

public class FedExLabelOptions
{
    public const string Url = "URL_ONLY";
    public const string LABEL = "LABEL";
}