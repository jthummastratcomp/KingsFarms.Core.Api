using Newtonsoft.Json;

namespace HotTowel.Core.Api.ViewModels
{
    public class BedFieldOpsCosmosDbModel
    {
        public string? OperationDate { get; set; }
        public string WorkType { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}