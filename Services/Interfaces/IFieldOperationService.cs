using HotTowel.Core.Api.ViewModels;

namespace HotTowel.Core.Api.Services.Interfaces;

public interface IFieldOperationService
{
    List<HarvestViewModel> GetHarvestInfo(int harvestYear);
}