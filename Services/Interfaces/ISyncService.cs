using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface ISyncService
{
    string SyncBedsInfo();
    string SyncHarvestInfo();
    string SyncCustomers(List<CustomerHeaderViewModel> list);
}