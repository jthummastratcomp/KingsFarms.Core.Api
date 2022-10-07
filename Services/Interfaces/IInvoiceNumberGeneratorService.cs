using System.Data;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IInvoiceNumberGeneratorService
{
    List<SearchDto> GetInvoiveNumbers(List<PrepareInvoicesViewModel> prepList, int currentColumnInDt, DataTable dtKings, DataTable dtMansi);
}