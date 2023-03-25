using KingsFarms.Core.Api.ViewModels.Harvest;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IBedService
{
    List<BedHarvestFieldOpsViewModel> GetBedsInfo();
    List<BedHarvestFieldOpsViewModel> GetBedsInfoGrouped();
    List<BedViewModel> GetBedsList();
}