using System;
using System.Collections.Generic;
using System.Linq;
using HotTowel.Web.Helpers;

namespace HotTowel.Web.ViewModels
{
    public class HarvestViewModel
    {
        public List<HarvestBedViewModel> BedHarvests { get; set; }
        public DateTime HarvestDate { get; set; }
        public string HarvestDateDisplay => HarvestDate.ToString("MM/dd/yyyy");
        public int TotalHarvest => !Utils.HasRows(BedHarvests) ? 0 : BedHarvests.Sum(x => x.HarvestQty21);

        public string HarvestsDisplay
        {
            get
            {
                if (!Utils.HasRows(BedHarvests)) return string.Empty;
                var list = BedHarvests.Select(bedHarvest => $"{bedHarvest.BedNumber}: {bedHarvest.HarvestQty21} lbs").ToList();

                return string.Join(", ", list);
            }
        }
    }
}