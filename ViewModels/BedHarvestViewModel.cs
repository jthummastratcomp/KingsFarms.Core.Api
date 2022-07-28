using Newtonsoft.Json;

namespace HotTowel.Core.Api.ViewModels
{
    public class BedHarvestViewModel
    {
        public DateTime HarvestDate { get; set; }
        public string HarvestDateDisplay => HarvestDate.ToString("MM/dd/yyyy");
        public int HarvestQty { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}