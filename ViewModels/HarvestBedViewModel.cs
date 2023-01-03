using KingsFarms.Core.Api.Enums;

namespace KingsFarms.Core.Api.ViewModels;

//public class HarvestBedViewModel
//{
//    public string? BedNumber { get; set; }
//    public int HarvestQty { get; set; }

//    private SectionEnum Section { get; set; }
//    private string SectionDisplay => Section.GetDescription();
//    private int PlantsCount { get; set; }
//    private DateTime PlantedDate { get; set; }
//    private string PlantedDateDisplay => PlantedDate.ToString("yyyy/MM");

//    public decimal Avg => PlantsCount > 0 && HarvestQty > 0 ? (decimal)HarvestQty / PlantsCount : 0;
//    public string Display => $"BedNumber:{BedNumber},Section:{SectionDisplay},PlantedDate:{PlantedDateDisplay},PlantsCount:{PlantsCount}";
//}