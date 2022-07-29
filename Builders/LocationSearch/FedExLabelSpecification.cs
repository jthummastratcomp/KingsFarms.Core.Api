using Newtonsoft.Json;

namespace HotTowel.Core.Api.Builders.LocationSearch;

public class FedExLabelSpecification
{
    [JsonProperty("labelStockType")] public string StockType { get; init; }
    [JsonProperty("imageType")] public string ImageType { get; init; }
    
}