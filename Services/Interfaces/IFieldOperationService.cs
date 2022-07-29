using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IFieldOperationService
{
    List<HarvestViewModel> GetHarvestInfo(int harvestYear);
}