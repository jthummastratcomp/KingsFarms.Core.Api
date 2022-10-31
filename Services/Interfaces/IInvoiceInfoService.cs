using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IInvoiceInfoService
{
    CustomerInvoicesViewModel PrepareInvoice(DateTime viewModelWeek, int viewModelWeekQty, CustomerDashboardViewModel? custumerDto, string? invoiceNumber, string? usdaMemo);
}