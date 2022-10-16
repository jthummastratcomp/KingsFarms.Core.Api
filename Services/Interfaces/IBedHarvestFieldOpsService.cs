using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IBedHarvestFieldOpsService
{
    Task<List<BedHarvestFieldOpsViewModel>> GetOrAddBedInfoToCosmosDbAsync();
}