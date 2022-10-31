using System.Data;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IPrepareUsdaInvoiceService
{
    List<CustomerInvoicesViewModel> CustomerInvoicesViewModels(CompanyEnum company,
        DataTable dtCustomer, DataTable dtKings, DataTable dtMansi,
        List<CustomerInvoicesViewModel> list, int year, DateTime weekDate, int currentColumnInDt, List<SearchDto> lots);
}