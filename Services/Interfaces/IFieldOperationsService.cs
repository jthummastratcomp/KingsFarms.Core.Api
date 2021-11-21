using System.Collections.Generic;
using System.Threading.Tasks;
using HotTowel.Web.ViewModels;

namespace HotTowel.Web.Services.Interfaces
{
    public interface IFieldOperationsService
    {
        Task<List<BedHarvestFieldOpsViewModel>> GetBedsFieldOpsAsync();
    }
}