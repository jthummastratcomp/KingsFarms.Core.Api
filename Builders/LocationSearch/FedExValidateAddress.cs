using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public record FedExValidateAddress
{
    [JsonProperty("addressesToValidate")] public List<FedExLocation> Addresses { get; init; }
}