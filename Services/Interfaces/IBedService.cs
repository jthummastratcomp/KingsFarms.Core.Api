using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IBedService
{
    List<BedHarvestFieldOpsViewModel> GetBedsInfo();
    List<BedHarvestFieldOpsViewModel> GetBedsInfoGrouped();
    List<BedViewModel> GetBedsList();
}