using KingsFarms.Core.Api.Enums;

namespace KingsFarms.Core.Api.ViewModels
{
    public class HarvestBedViewModel
    {
        public string? BedNumber { get; set; }
        public int HarvestQty21 { get; set; }
        public int HarvestQty20 { get; set; }
        public int HarvestQty22 { get; set; }

        private SectionEnum Section { get; set; }
        private string SectionDisplay => Section.GetDescription();
        private int PlantsCount { get; set; }
        private DateTime PlantedDate { get; set; }
        private string PlantedDateDisplay => PlantedDate.ToString("yyyy/MM");

        public decimal Avg20 => PlantsCount > 0 && HarvestQty20 > 0 ? (decimal)HarvestQty20 / PlantsCount : 0;
        public decimal Avg21 => PlantsCount > 0 && HarvestQty21 > 0? (decimal)HarvestQty21 / PlantsCount : 0;
        public decimal Avg22 => PlantsCount > 0 && HarvestQty22 > 0 ? (decimal)HarvestQty22 / PlantsCount : 0;

        //[JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        //[JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }

        public string Display => $"BedNumber:{BedNumber},Section:{SectionDisplay},PlantedDate:{PlantedDateDisplay},PlantsCount:{PlantsCount}";
    }
}