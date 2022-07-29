using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces
{
    public interface IFieldOperationsService
    {
        Task<List<BedHarvestFieldOpsViewModel>> GetBedsFieldOpsAsync();
    }
}