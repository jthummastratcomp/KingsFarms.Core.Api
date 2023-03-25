using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.ViewModels;
using KingsFarms.Core.Api.ViewModels.Customer;
using KingsFarms.Core.Api.ViewModels.Invoice;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IWeeklyOrdersUsdaService
{
    List<SearchDto> GetInvoiceWeeksListForYear(int year);
    List<CustomerInvoicesViewModel> LoadInvoicesForWeek(string? week, CompanyEnum company);
    List<CustomerDashboardViewModel> GetCustomersFromOrdersFile();
}