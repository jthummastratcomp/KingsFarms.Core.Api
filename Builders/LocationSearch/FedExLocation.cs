using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public record FedExLocation
{
    [JsonProperty("address")] public FedExAddress? Address { get; init; }
}