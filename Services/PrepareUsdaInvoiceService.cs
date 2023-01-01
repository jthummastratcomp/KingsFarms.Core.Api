using System.Data;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Mappers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services;

public class PrepareUsdaInvoiceService : IPrepareUsdaInvoiceService
{
    private readonly IInvoiceNumberGeneratorService _generatorService;
    private readonly IApplyInvoiceInfoService _invoiceInfoService;

    public PrepareUsdaInvoiceService(IInvoiceNumberGeneratorService generatorService, IApplyInvoiceInfoService invoiceInfoService)
    {
        _generatorService = generatorService;
        _invoiceInfoService = invoiceInfoService;
    }

    public List<CustomerInvoicesViewModel> CustomerInvoicesViewModels(CompanyEnum company,
        DataTable dtCustomer, DataTable dtKings, DataTable dtMansi,
        List<CustomerInvoicesViewModel> list, int year, DateTime weekDate, int currentColumnInDt,
        List<SearchDto> lots)
    {
        var customersList = Mapper.MapToCustomerDashboardViewModelList(dtCustomer);

        var dtSource = company is CompanyEnum.Kings or CompanyEnum.KingsSandbox
            ? dtKings
            : company == CompanyEnum.Mansi
                ? dtMansi
                : null;

        if (dtSource == null) return list;

        var prepList = PrepCustomerInvoicesList(weekDate, currentColumnInDt, dtSource, customersList);

        var invoiceNumbersList = _generatorService.GetInvoiceNumbers(prepList, weekDate, currentColumnInDt, dtKings, dtMansi);

        list = _invoiceInfoService.GetCustomerInvoicesViewModels(prepList, customersList, invoiceNumbersList, lots);

        return list;
    }

    private static List<PrepareInvoicesViewModel> PrepCustomerInvoicesList(DateTime weekDate, int currentColumnInDt, DataTable dtSource,
        IReadOnlyCollection<CustomerDashboardViewModel> customersList)
    {
        var list = new List<PrepareInvoicesViewModel>();

        if (dtSource.Rows.Count <= 0) return list;

        const int startRow = 1;

        for (var row = startRow; row < dtSource.Rows.Count; row++)
        {
            var dataRow = dtSource.Rows[row];

            var customerKey = dataRow[0].ToString();
            if (string.IsNullOrEmpty(customerKey)) continue;

            var viewModel = customersList.FirstOrDefault(x => x.CustomerHeader.CustomerKey == customerKey);
            if (viewModel == null) continue;

            var weekQty = Utils.ParseToInteger(dataRow[currentColumnInDt].ToString());
            if (weekQty == 0) continue;

            list.Add(new PrepareInvoicesViewModel
            {
                CustomerKey = customerKey,
                WeekQty = weekQty,
                Week = weekDate
            });
        }

        return list;
    }
}