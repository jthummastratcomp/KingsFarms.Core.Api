using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IUsdaMemoService
{
    string GetUsdaMemo(Queue<SearchDto> queue, PrepareInvoicesViewModel viewModel);
}