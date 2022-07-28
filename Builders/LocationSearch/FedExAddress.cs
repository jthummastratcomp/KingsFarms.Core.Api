using Newtonsoft.Json;

namespace HotTowel.Core.Api.Builders.LocationSearch;

public record FedExAddress
{
    [JsonProperty("postalCode")] public string ZipCode { get; init; } = "45040";
    [JsonProperty("countryCode")] public string CountryCode { get; init; } = "US";
}