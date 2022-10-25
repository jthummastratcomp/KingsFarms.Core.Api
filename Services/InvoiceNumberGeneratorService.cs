using System.Data;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services;

public class InvoiceNumberGeneratorService : IInvoiceNumberGeneratorService
{
    public List<SearchDto> GetInvoiveNumbers(List<PrepareInvoicesViewModel> prepList, int currentColumnInDt, DataTable dtKings, DataTable dtMansi)
    {
        var list = new List<SearchDto>();

        foreach (var viewModel in prepList)
        {
            var invoiceNumber = GetNewInvoiceNumber(viewModel.CustomerKey, currentColumnInDt, dtKings, dtMansi);
            list.Add(new SearchDto { Id = viewModel.CustomerKey, Data = invoiceNumber });
        }

        return list;
    }

    private static string GetNewInvoiceNumber(string customerKey, int currentColumn, DataTable dtKings, DataTable dtMansi)
    {
        if (currentColumn <= 2) return $"{customerKey}-101/{DateTime.Today:yy}";

        var newInvoiceNumber = 101;

        newInvoiceNumber = GetLastInvoiceNumberFromDataTable(customerKey, currentColumn, dtKings, newInvoiceNumber);
        newInvoiceNumber = GetLastInvoiceNumberFromDataTable(customerKey, currentColumn, dtMansi, newInvoiceNumber);

        return $"{customerKey}-{newInvoiceNumber}/{DateTime.Today:yy}";
    }

    private static int GetLastInvoiceNumberFromDataTable(string customerKey, int currentColumn, DataTable dataTable, int newInvoiceNumber)
    {
        var found = false;
        foreach (DataRow dataRow in dataTable.Rows)
        {
            if (dataRow[0].ToString() == customerKey) found = true;
            if (!found) continue;

            //loop thru each week up to one week prior to current week
            for (var col = 5; col < currentColumn; col += 4)
            {
                var weekQty = Utils.ParseToInteger(dataRow[col].ToString());
                if (weekQty > 0) newInvoiceNumber += 1;
            }

            break;
        }

        return newInvoiceNumber;
    }
}