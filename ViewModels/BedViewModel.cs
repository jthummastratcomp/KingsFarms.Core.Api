using KingsFarms.Core.Api.Enums;

namespace KingsFarms.Core.Api.ViewModels;

public class BedViewModel
{
    public int BedNumber { get; set; }
    //public SectionEnum Section { get; set; }
    //public string SectionDisplay => Section.GetDescription();
    public int PlantsCount { get; set; }
    public DateTime PlantedDate { get; set; }
    //public string PlantedDateDisplay => PlantedDate.ToString("yyyy/MM");
}