using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public record FedExAccountNumber
{
    [JsonProperty("value")] public string? Number { get; init; }
}