using Newtonsoft.Json;

namespace HotTowel.Core.Api.ViewModels;

public class HarvestBedCosmosDbModel
{
    public string BedNumber { get; set; }
    public string Section;
    public int PlantsCount { get; set; }
    public string PlantedDate { get; set; }


    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
    [JsonProperty(PropertyName = "partitionKey")]
    public string PartitionKey { get; set; }

    public string Display
    {
        get
        {
            return $"BedNumber:{BedNumber},Section:{Section},PlantedDate:{PlantedDate},PlantsCount:{PlantsCount}";
        }
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}