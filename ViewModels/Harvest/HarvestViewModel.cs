using KingsFarms.Core.Api.Enums;

namespace KingsFarms.Core.Api.ViewModels.Harvest;

public class HarvestViewModel
{
    public int BedNumber { get; set; }
    public int HarvestQty { get; set; }
    public DateTime HarvestDate { get; set; }

    public SectionEnum Section => GetBedSection(BedNumber);

    private SectionEnum GetBedSection(int bedNumber)
    {
        if (bedNumber is >= 1 and <= 11) return SectionEnum.MidWest;
        if (bedNumber is >= 12 and <= 22) return SectionEnum.SouthWest;
        if (bedNumber is >= 23 and <= 28) return SectionEnum.SouthEastOne;
        if (bedNumber is >= 29 and <= 37) return SectionEnum.SouthEastTwo;
        if (bedNumber is >= 38 and <= 45) return SectionEnum.MidEastTwo;
        if (bedNumber is >= 46 and <= 51) return SectionEnum.MidEastOne;
        return SectionEnum.None;
    }
}

//public class HarvestViewModel
//{
//    public HarvestViewModel(DateTime harvestDate)
//    {
//        HarvestDate = harvestDate;
//    }

//    public List<HarvestBedViewModel> BedHarvests { get; set; }
//    //public int HarvestYear { get; set; }
//    public DateTime HarvestDate { get; }
//    public string HarvestDateDisplay => HarvestDate.ToString("MM/dd/yyyy");
//    public int TotalHarvest => !Utils.HasRows(BedHarvests) ? 0 : BedHarvests.Sum(x => x.HarvestQty);

//    public string HarvestsDisplay
//    {
//        get
//        {
//            if (!Utils.HasRows(BedHarvests)) return string.Empty;
//            var list = BedHarvests.Select(bedHarvest => $"{bedHarvest.BedNumber}: {bedHarvest.HarvestQty} lbs")
//                .ToList();

//            return string.Join(", ", list);
//        }
//    }
//}