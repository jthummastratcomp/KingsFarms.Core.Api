using KingsFarms.Core.Api.Helpers;

namespace KingsFarms.Core.Api.ViewModels;

public class HarvestViewModel
{
    public HarvestViewModel(DateTime harvestDate)
    {
        HarvestDate = harvestDate;
    }

    public List<HarvestBedViewModel> BedHarvests { get; set; }
    public int HarvestYear { get; set; }
    private DateTime HarvestDate { get; }
    public string HarvestDateDisplay => HarvestDate.ToString("MM/dd/yyyy");
    public int TotalHarvest => !Utils.HasRows(BedHarvests) ? 0 : BedHarvests.Sum(x => x.HarvestQty);

    public string HarvestsDisplay
    {
        get
        {
            if (!Utils.HasRows(BedHarvests)) return string.Empty;
            var list = BedHarvests.Select(bedHarvest => $"{bedHarvest.BedNumber}: {bedHarvest.HarvestQty} lbs")
                .ToList();

            return string.Join(", ", list);
        }
    }
}