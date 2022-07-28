using HotTowel.Core.Api.Enums;

namespace HotTowel.Core.Api.ViewModels
{
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
}