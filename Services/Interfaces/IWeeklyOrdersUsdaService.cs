using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IWeeklyOrdersUsdaService
{
    
    List<CustomerInvoicesViewModel> LoadInvoicesForWeek(string? week, CompanyEnum company);
    
}