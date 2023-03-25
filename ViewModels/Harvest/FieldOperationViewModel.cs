using KingsFarms.Core.Api.Enums;

namespace KingsFarms.Core.Api.ViewModels.Harvest;

public class FieldOperationViewModel
{
    public FieldOperationEnum Type { get; set; }
    public DateTime OperationDate { get; set; }
}

public class FieldOperationCosmosDbModel
{
    public string Type { get; set; }
    public string OperationDate { get; set; }
}