using System.Collections.Generic;
using System.Threading.Tasks;
using HotTowel.Web.ViewModels;

namespace HotTowel.Web.Services
{
    public interface IBedHarvestFieldOpsService
    {
        Task<List<BedHarvestFieldOpsViewModel>> GetOrAddBedInfoToCosmosDbAsync();
    }
}