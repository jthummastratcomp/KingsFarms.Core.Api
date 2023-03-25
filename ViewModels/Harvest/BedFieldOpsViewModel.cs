using KingsFarms.Core.Api.Enums;
using Newtonsoft.Json;

namespace KingsFarms.Core.Api.ViewModels.Harvest;

public class BedFieldOpsViewModel
{
    public DateTime OperationDate { get; set; }
    public string OperationDateDisplay => OperationDate.ToString("MM/dd/yyyy");
    public FieldOperationEnum WorkType { get; set; }
    public string WorkTypeDisplay => WorkType.GetDescription();

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}