using System.Collections.Generic;
using HotTowel.Web.ViewModels;

namespace HotTowel.Web.Services.Interfaces
{
    public interface IWeeklyOrdersService
    {
        List<SearchDto> GetInvoiceWeeksListForYear();
        List<CustomerInvoicesViewModel> LoadInvoicesForWeek(string week, CompanyEnum company);
    }
}