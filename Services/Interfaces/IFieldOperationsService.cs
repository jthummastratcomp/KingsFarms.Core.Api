using HotTowel.Core.Api.ViewModels;

namespace HotTowel.Core.Api.Services.Interfaces
{
    public interface IFieldOperationsService
    {
        Task<List<BedHarvestFieldOpsViewModel>> GetBedsFieldOpsAsync();
    }
}