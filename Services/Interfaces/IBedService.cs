using HotTowel.Core.Api.ViewModels;

namespace HotTowel.Core.Api.Services.Interfaces;

public interface IBedService
{
    
    List<BedHarvestFieldOpsViewModel> GetBedsInfo();
    List<BedHarvestFieldOpsViewModel> GetBedsInfoGrouped();
}