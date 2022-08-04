using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public record FedExAddress
{
    [JsonProperty("postalCode")] public string? ZipCode { get; init; } = "45040";
    [JsonProperty("countryCode")] public string CountryCode { get; init; } = "US";
    [JsonProperty("city")] public string? City { get; init; } = "Cincinnati";
    [JsonProperty("stateOrProvinceCode")] public string? State { get; init; } = "OH";
    [JsonProperty("streetLines")] public List<string?> StreetLines { get; init; } = new List<string?>() { "4029 Melampy Creek", "Apt 103" };
}