using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExDimension
{
    [JsonProperty("units")] public string Units { get; init; }
    [JsonProperty("length")] public double Length { get; init; }
    [JsonProperty("width")] public double Width { get; init; }
    [JsonProperty("height")] public double Height { get; init; }
}