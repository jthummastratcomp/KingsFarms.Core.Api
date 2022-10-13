using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces
{
    public interface IWeeklyOrdersService
    {
        List<SearchDto> GetInvoiceWeeksListForYear(int year);
        //List<CustomerInvoicesViewModel> LoadInvoicesForWeek(string? week, CompanyEnum company);
        List<CustomerDashboardViewModel> GetCustomersFromOrdersFile();
    }
}