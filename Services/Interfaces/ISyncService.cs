using KingsFarms.Core.Api.ViewModels.Customer;
using KingsFarms.Core.Api.ViewModels.Harvest;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface ISyncService
{
    string SyncBedsInfo();
    string SyncHarvestInfo();
    string SyncCustomers(List<CustomerHeaderViewModel> list);
    string SaveHarvestData(HarvestViewModel viewModel);
}