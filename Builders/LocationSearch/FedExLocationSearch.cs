using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public record FedExLocationSearch
{
    [JsonProperty("location")] public FedExLocation? Location { get; init; }
}