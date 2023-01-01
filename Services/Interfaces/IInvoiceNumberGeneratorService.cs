using System.Data;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IInvoiceNumberGeneratorService
{
    List<SearchDto> GetInvoiceNumbers(List<PrepareInvoicesViewModel> prepList, DateTime weekDate, int currentColumnInDt, DataTable dtKings, DataTable dtMansi);
}