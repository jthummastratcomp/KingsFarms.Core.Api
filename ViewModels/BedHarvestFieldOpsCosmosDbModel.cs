using System.Text;
using KingsFarms.Core.Api.Helpers;
using Newtonsoft.Json;

namespace KingsFarms.Core.Api.ViewModels
{
    public class BedHarvestFieldOpsCosmosDbModel
    {
        public string Section;
        public string BedNumber { get; set; }
        public int PlantsCount { get; set; }
        public string? PlantedDate { get; set; }

        public BedHarvestViewModel[] Harvests { get; set; }

        public string HarvestsDisplay
        {
            get
            {
                if (Harvests == null || Harvests.Length == 0) return string.Empty;
                var list = Harvests.ToList();
                if (!Utils.HasRows(list)) return string.Empty;

                var builder = new StringBuilder();
                foreach (var model in list) builder.Append($"HarvestDate:{model.HarvestDateDisplay},Qty:{model.HarvestQty};");

                return builder.ToString();
            }
        }

        public BedFieldOpsViewModel[] FieldOperations { get; set; }

        public string FieldOperationsDisplay
        {
            get
            {
                if (FieldOperations == null || FieldOperations.Length == 0) return string.Empty;
                var list = FieldOperations.ToList();
                if (!Utils.HasRows(list)) return string.Empty;

                var builder = new StringBuilder();
                foreach (var model in list) builder.Append($"OperationDate:{model.OperationDateDisplay},WorkType:{model.WorkTypeDisplay};");

                return builder.ToString();
            }
        }

        [JsonProperty(PropertyName = "id")] public string Id { get; set; }

        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }

        public string Display => $"BedNumber:{BedNumber},Section:{Section},PlantedDate:{PlantedDate},PlantsCount:{PlantsCount},{HarvestsDisplay}";

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}