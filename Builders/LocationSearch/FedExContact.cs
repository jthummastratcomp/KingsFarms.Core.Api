using Newtonsoft.Json;

namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExContact
{
    [JsonProperty("phoneNumber")] public string? Phone { get; init; }
    [JsonProperty("personName")] public string? PersonName { get; set; }
}