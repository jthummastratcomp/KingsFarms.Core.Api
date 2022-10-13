using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services;

public class ApplyInvoiceInfoService : IApplyInvoiceInfoService
{
    private readonly IUsdaMemoService _memoService;
    private readonly IInvoiceInfoService _invoiceInfoService;

    public ApplyInvoiceInfoService(IUsdaMemoService memoService, IInvoiceInfoService invoiceInfoService)
    {
        _memoService = memoService;
        _invoiceInfoService = invoiceInfoService;
    }
    public List<CustomerInvoicesViewModel> GetCustomerInvoicesViewModels(List<PrepareInvoicesViewModel> prepList, List<CustomerDashboardViewModel> customerList, List<SearchDto> invoiceNumbersList, Queue<SearchDto>? queue)
    {
        var list = new List<CustomerInvoicesViewModel>();
        foreach (var viewModel in prepList)
        {
            var usdaMemo = _memoService.GetUsdaMemo(queue, viewModel);

            var dto = invoiceNumbersList.FirstOrDefault(x => x.Id == viewModel.CustomerKey);
            var invoiceNumber = dto == null ? string.Empty : dto.Data;

            var custumerDto = customerList.FirstOrDefault(x => x.CustomerHeader.CustomerKey == viewModel.CustomerKey);


            list.Add(_invoiceInfoService.PrepareInvoice(viewModel.Week, viewModel.WeekQty, custumerDto, invoiceNumber, usdaMemo));
        }

        return list;
    }

    
}