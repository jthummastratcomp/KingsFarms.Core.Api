using Newtonsoft.Json;

namespace HotTowel.Core.Api.Builders.LocationSearch;

public record FedExAccountNumber
{
    [JsonProperty("value")] public string? Number { get; init; }
}