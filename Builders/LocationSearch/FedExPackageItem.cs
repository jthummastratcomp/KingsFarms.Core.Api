using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExPackageItem
{
    [JsonProperty("weight")] public FedExWeight Weight { get; init; }
}