using Newtonsoft.Json;

namespace KingsFarms.Core.Api.ViewModels
{
    public class BedHarvestCosmosDbModel
    {
        public string HarvestDate { get; set; }
        public int HarvestQty { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}