using System.Data;
using System.Diagnostics.CodeAnalysis;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Mappers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services;

public class PrepareInvoiceService : IPrepareInvoiceService
{
    private readonly IHarvestService _harvestService;
    private readonly IInvoiceNumberGeneratorService _generatorService;
    private readonly IUsdaQueueService _queueService;
    private readonly IApplyInvoiceInfoService _invoiceInfoService;

    public PrepareInvoiceService(IHarvestService harvestService, IInvoiceNumberGeneratorService generatorService, IUsdaQueueService queueService, IApplyInvoiceInfoService invoiceInfoService)
    {
        _harvestService = harvestService;
        _generatorService = generatorService;
        _queueService = queueService;
        _invoiceInfoService = invoiceInfoService;
    }
    public List<CustomerInvoicesViewModel> CustomerInvoicesViewModels(CompanyEnum company, DataTable dtCustomer, DataTable dtKings, DataTable dtMansi, List<CustomerInvoicesViewModel> list, int year, DateTime weekDate, int currentColumnInDt)
    {
        var customersList = Mapper.MapToCustomerDashboardViewModelList(dtCustomer);

        var dtSource = company is CompanyEnum.Kings or CompanyEnum.KingsSandbox
            ? dtKings
            : company == CompanyEnum.Mansi
                ? dtMansi
                : null;

        if (dtSource == null) return list;


        var harvestList = company is CompanyEnum.Kings or CompanyEnum.KingsSandbox
            ? _harvestService.GetHarvestDataBySeason(year)
            : null;

        var prepList = PrepareInvoicesViewModels(weekDate, currentColumnInDt, dtSource, customersList);

        var invoiceNumbersList = _generatorService.GetInvoiveNumbers(prepList, currentColumnInDt, dtKings, dtMansi);

        var queues = _queueService.GetQueues(weekDate, harvestList, prepList);

        list = _invoiceInfoService.GetCustomerInvoicesViewModels(prepList, customersList, invoiceNumbersList, queues);


        return list;
    }

    

    private static List<PrepareInvoicesViewModel> PrepareInvoicesViewModels(DateTime weekDate, int currentColumnInDt, DataTable dtSource, List<CustomerDashboardViewModel> customersList)
    {
        var list = new List<PrepareInvoicesViewModel>();

        if (dtSource.Rows.Count <= 0) return list;

        var startRow = 1;

        for (var row = startRow; row < dtSource.Rows.Count; row++)
        {
            var dataRow = dtSource.Rows[row];

            var customerKey = dataRow[0].ToString();
            if (string.IsNullOrEmpty(customerKey)) continue;

            var viewModel = customersList.FirstOrDefault(x => x.CustomerHeader.CustomerKey == customerKey);
            if (viewModel == null) continue;

            var weekQty = Utils.ParseToInteger(dataRow[currentColumnInDt].ToString());
            if (weekQty == 0) continue;

            var boxSize = weekQty % 12 == 0
                ? 12
                : weekQty % 10 == 0
                    ? 10
                    : weekQty % 5 == 0
                        ? 5
                        : 0;

            if (boxSize == 0) continue;

            list.Add(new PrepareInvoicesViewModel { CustomerKey = customerKey, WeekQty = weekQty, BoxSize = boxSize, Week = weekDate });
        }

        return list.OrderByDescending(x => x.BoxSize).ThenBy(x => x.CustomerKey).ToList();
    }
}