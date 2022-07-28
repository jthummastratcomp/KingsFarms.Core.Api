using Newtonsoft.Json;

namespace HotTowel.Core.Api.Builders.LocationSearch;

public record FedExLocation
{
    [JsonProperty("address")] public FedExAddress? Address { get; init; }
}