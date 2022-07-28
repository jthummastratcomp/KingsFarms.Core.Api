using HotTowel.Core.Api.ViewModels;

namespace HotTowel.Core.Api.Services.Interfaces
{
    public interface IHarvestService
    {
        List<HarvestViewModel> GetHarvestInfo(int harvestYear);
    }
}