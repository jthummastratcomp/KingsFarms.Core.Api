using System.Text;
using HotTowel.Web.Helpers;

namespace HotTowel.Web.ViewModels;

public class BedHarvestFieldOpsViewModel
{
    public string BedNumber { get; set; }


    public SectionEnum Section { get; set; }
    public string SectionDisplay => Section.GetDescription();
    public int PlantsCount { get; set; }
    public DateTime PlantedDate { get; set; }
    public string PlantedDateDisplay => PlantedDate.ToString("yyyy/MM");

    public List<BedHarvestViewModel> Harvests { get; set; }
    public List<BedFieldOpsViewModel> FieldOperations { get; set; }

    public string HarvestsDisplay
    {
        get
        {
            if (Harvests == null || Harvests.Count == 0) return string.Empty;
            var list = Harvests.ToList();
            if (!Utils.HasRows(list)) return string.Empty;

            var builder = new StringBuilder();
            foreach (var model in list) builder.Append($"HarvestDate:{model.HarvestDateDisplay},Qty:{model.HarvestQty};");

            return builder.ToString();
        }
    }

    public string FieldOperationsDisplay
    {
        get
        {
            if (FieldOperations == null || FieldOperations.Count == 0) return string.Empty;
            var list = FieldOperations.ToList();
            if (!Utils.HasRows(list)) return string.Empty;

            var builder = new StringBuilder();
            foreach (var model in list) builder.Append($"OperationDate:{model.OperationDateDisplay},WorkType:{model.WorkTypeDisplay};");

            return builder.ToString();
        }
    }

    //todo be refactor
    public int HarvestQty21_22 { get; set; }
    public int HarvestQty20_21 { get; set; }
    public decimal Avg20_21 => PlantsCount > 0 && HarvestQty20_21 > 0 ? (decimal)HarvestQty20_21 / PlantsCount : 0;
    public decimal Avg21_22 => PlantsCount > 0 && HarvestQty21_22 > 0 ? (decimal)HarvestQty21_22 / PlantsCount : 0;
    public decimal GrowthPercent21_22 => PlantsCount > 0 && HarvestQty20_21 > 0 && HarvestQty21_22 > 0 ? (decimal)(HarvestQty21_22 - HarvestQty20_21) / HarvestQty20_21 : 0;

    //todo be refactor

    //[JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    //[JsonProperty(PropertyName = "partitionKey")]
    public string PartitionKey { get; set; }

    public string Display => $"BedNumber:{BedNumber},Section:{Section},PlantedDate:{PlantedDate},PlantsCount:{PlantsCount},{HarvestsDisplay},{FieldOperations}";
}