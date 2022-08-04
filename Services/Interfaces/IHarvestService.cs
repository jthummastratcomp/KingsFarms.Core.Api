using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces
{
    public interface IHarvestService
    {
        List<HarvestViewModel> GetHarvestData(int harvestYear);
        int GetHarvestYearTotal(int year);
    }
}