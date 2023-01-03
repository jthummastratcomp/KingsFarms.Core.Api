using Newtonsoft.Json;

namespace KingsFarms.Core.Api.ViewModels;

public class BedsMaintenanceCosmosDbModel
{
    [JsonProperty(PropertyName = "id")] public string Id { get; set; }

    [JsonProperty(PropertyName = "partitionKey")]
    public string PartitionKey { get; set; }

    public string Section { get; set; }
    public int PlantsCount { get; set; }

    public string PlantedDate { get; set; }

    //public Parent[] Parents { get; set; }
    public FieldOperationCosmosDbModel[] FieldOperations { get; set; }

    //public Address1 Address { get; set; }
    //public bool IsRegistered { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}