using KingsFarms.Core.Api.ViewModels;
using KingsFarms.Core.Api.ViewModels.Customer;
using KingsFarms.Core.Api.ViewModels.Invoice;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IApplyInvoiceInfoService
{
    //List<CustomerInvoicesViewModel> GetCustomerInvoicesViewModels(List<PrepareInvoicesViewModel> prepList,
    //    List<CustomerDashboardViewModel> customersList, List<SearchDto> invoiceNumbersList, Queue<SearchDto>? queues);

    List<CustomerInvoicesViewModel> GetCustomerInvoicesViewModels(List<PrepareInvoicesViewModel> prepList,
        List<CustomerDashboardViewModel> customersList, List<SearchDto> invoiceNumbersList, List<SearchDto> lots);
}