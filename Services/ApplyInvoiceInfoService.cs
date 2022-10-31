using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services;

public class ApplyInvoiceInfoService : IApplyInvoiceInfoService
{
    private readonly IInvoiceInfoService _invoiceInfoService;

    public ApplyInvoiceInfoService(IInvoiceInfoService invoiceInfoService)
    {
        _invoiceInfoService = invoiceInfoService;
    }

    public List<CustomerInvoicesViewModel> GetCustomerInvoicesViewModels(
        List<PrepareInvoicesViewModel> prepList,
        List<CustomerDashboardViewModel> customerList,
        List<SearchDto> invoiceNumbersList,
        List<SearchDto> lots)
    {
        return (from viewModel in prepList
            let usdaMemo = lots.FirstOrDefault(x => x.Id == viewModel.CustomerKey)?.Data
            let dto = invoiceNumbersList.FirstOrDefault(x => x.Id == viewModel.CustomerKey)
            let invoiceNumber = dto == null ? string.Empty : dto.Data
            let customerDto = customerList.FirstOrDefault(x => x.CustomerHeader.CustomerKey == viewModel.CustomerKey)
            let invoiceDate = viewModel.Week.AddDays(2) //Monday
            select _invoiceInfoService.PrepareInvoice(invoiceDate, viewModel.WeekQty, customerDto, invoiceNumber, usdaMemo)).ToList();
    }
}