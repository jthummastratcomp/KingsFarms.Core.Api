using Newtonsoft.Json;

namespace HotTowel.Core.Api.Builders.LocationSearch;

public record FedExLocationSearch
{
    [JsonProperty("location")] public FedExLocation? Location { get; init; }
}