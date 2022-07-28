using HotTowel.Core.Api.Enums;
using HotTowel.Core.Api.ViewModels;

namespace HotTowel.Core.Api.Services.Interfaces
{
    public interface IWeeklyOrdersService
    {
        List<SearchDto> GetInvoiceWeeksListForYear();
        List<CustomerInvoicesViewModel> LoadInvoicesForWeek(string? week, CompanyEnum company);
    }
}