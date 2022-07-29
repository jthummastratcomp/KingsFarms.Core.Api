using Newtonsoft.Json;

namespace HotTowel.Core.Api.Builders.LocationSearch;

public class FedExPackageItem
{
    [JsonProperty("weight")] public FedExWeight Weight { get; init; }
}