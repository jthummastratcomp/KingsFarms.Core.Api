using Newtonsoft.Json;

namespace KingsFarms.Core.Api.ViewModels.Harvest;

public class BedFieldOpsCosmosDbModel
{
    public string? OperationDate { get; set; }
    public string WorkType { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}