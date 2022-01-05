using HotTowel.Web.ViewModels;

namespace HotTowel.Web.Services.Interfaces;

public interface IBedService
{
    
    List<BedHarvestFieldOpsViewModel> GetBedsInfo();
    List<BedHarvestFieldOpsViewModel> GetBedsInfoGrouped();
}