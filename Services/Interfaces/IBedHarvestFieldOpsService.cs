using HotTowel.Core.Api.ViewModels;

namespace HotTowel.Core.Api.Services.Interfaces
{
    public interface IBedHarvestFieldOpsService
    {
        Task<List<BedHarvestFieldOpsViewModel>> GetOrAddBedInfoToCosmosDbAsync();
    }
}