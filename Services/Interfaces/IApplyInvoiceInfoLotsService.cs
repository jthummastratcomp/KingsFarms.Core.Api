using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IApplyInvoiceInfoLotsService
{
    List<CustomerInvoicesViewModel> GetCustomerInvoicesViewModels(List<PrepareInvoicesViewModel> prepList,
        List<CustomerDashboardViewModel> customersList, List<SearchDto> invoiceNumbersList, Queue<SearchDto>? queues);
}