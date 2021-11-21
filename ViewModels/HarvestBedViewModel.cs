using System;
using Newtonsoft.Json;

namespace HotTowel.Web.ViewModels
{
    public class HarvestBedViewModel
    {
        public string BedNumber { get; set; }
        public int HarvestQty21 { get; set; }
        public int HarvestQty20 { get; set; }

        public SectionEnum Section { get; set; }
        public string SectionDisplay => Section.GetDescription();
        public int PlantsCount { get; set; }
        public DateTime PlantedDate { get; set; }
        public string PlantedDateDisplay => PlantedDate.ToString("yyyy/MM");

        public decimal Avg20 => PlantsCount > 0 && HarvestQty20 > 0 ? (decimal)HarvestQty20 / PlantsCount : 0;
        public decimal Avg21 => PlantsCount > 0 && HarvestQty21 > 0? (decimal)HarvestQty21 / PlantsCount : 0;


        //[JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        //[JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }

        public string Display
        {
            get
            {
               return $"BedNumber:{BedNumber},Section:{SectionDisplay},PlantedDate:{PlantedDateDisplay},PlantsCount:{PlantsCount}";
            }
        }
    }

    public class HarvestBedCosmosDbModel
    {
        public string BedNumber { get; set; }
        public string Section;
        public int PlantsCount { get; set; }
        public string PlantedDate { get; set; }


        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }

        public string Display
        {
            get
            {
                return $"BedNumber:{BedNumber},Section:{Section},PlantedDate:{PlantedDate},PlantsCount:{PlantsCount}";
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}