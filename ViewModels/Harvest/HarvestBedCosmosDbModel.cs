using Newtonsoft.Json;

namespace KingsFarms.Core.Api.ViewModels.Harvest;

public class HarvestBedCosmosDbModel
{
    public string Section;
    public string BedNumber { get; set; }
    public int PlantsCount { get; set; }
    public string PlantedDate { get; set; }


    [JsonProperty(PropertyName = "id")] public string Id { get; set; }

    [JsonProperty(PropertyName = "partitionKey")]
    public string PartitionKey { get; set; }

    public string Display => $"BedNumber:{BedNumber},Section:{Section},PlantedDate:{PlantedDate},PlantsCount:{PlantsCount}";

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}