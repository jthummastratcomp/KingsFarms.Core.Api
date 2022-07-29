using Newtonsoft.Json;

namespace HotTowel.Core.Api.Builders.LocationSearch;

public record FedExValidateAddress
{
    [JsonProperty("addressesToValidate")] public List<FedExLocation> Addresses { get; init; }
}