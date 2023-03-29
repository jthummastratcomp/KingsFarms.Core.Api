using KingsFarms.Core.Api.ViewModels.Customer;
using KingsFarms.Core.Api.ViewModels.Harvest;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface ISyncService
{
    string SyncBeds();
    string SyncHarvests();
    string SyncCustomers();
    string SyncCustomers(List<CustomerHeaderViewModel> list);
    string SaveHarvestData(HarvestViewModel viewModel);
    string SyncInvoices();
}