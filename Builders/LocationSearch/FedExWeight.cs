using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExWeight
{
    [JsonProperty("units")] public string? Units { get; init; }
    [JsonProperty("value")] public double Value { get; init; }
}