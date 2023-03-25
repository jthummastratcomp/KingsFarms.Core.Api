using KingsFarms.Core.Api.ViewModels.Customer;
using KingsFarms.Core.Api.ViewModels.Invoice;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IInvoiceInfoService
{
    CustomerInvoicesViewModel PrepareInvoice(DateTime viewModelWeek, int viewModelWeekQty, CustomerDashboardViewModel? custumerDto, string? invoiceNumber, string? usdaMemo);
}