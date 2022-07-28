using HotTowel.Core.Api.Enums;
using Newtonsoft.Json;

namespace HotTowel.Core.Api.ViewModels
{
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
}