using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces
{
    public interface IHarvestService
    {
        List<HarvestViewModel> GetHarvestInfo(int harvestYear);
    }
}