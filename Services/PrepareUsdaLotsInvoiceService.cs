using System.Data;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Mappers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services;

public class PrepareUsdaLotsInvoiceService : IPrepareUsdaLotsInvoiceService
{
    private readonly IInvoiceNumberGeneratorService _generatorService;
    private readonly IHarvestService _harvestService;
    private readonly IApplyInvoiceInfoService _invoiceInfoService;
    private readonly IUsdaQueueService _queueService;

    public PrepareUsdaLotsInvoiceService(IHarvestService harvestService, IInvoiceNumberGeneratorService generatorService, IUsdaQueueService queueService, IApplyInvoiceInfoService invoiceInfoService)
    {
        _harvestService = harvestService;
        _generatorService = generatorService;
        _queueService = queueService;
        _invoiceInfoService = invoiceInfoService;
    }

    public List<CustomerInvoicesViewModel> CustomerInvoicesViewModels(CompanyEnum company, DataTable dtCustomer, DataTable dtKings, DataTable dtMansi,
        List<CustomerInvoicesViewModel> list, int year, DateTime weekDate, int currentColumnInDt, DataTable dtLot)
    {
        var customersList = Mapper.MapToCustomerDashboardViewModelList(dtCustomer);

        var dtSource = company is CompanyEnum.Kings or CompanyEnum.KingsSandbox
            ? dtKings
            : company == CompanyEnum.Mansi
                ? dtMansi
                : null;

        if (dtSource == null) return list;


        //var harvestList = company is CompanyEnum.Kings or CompanyEnum.KingsSandbox
        //    ? _harvestService.GetHarvestDataBySeason(year)
        //    : null;

        var prepList = PrepareUsdaInvoicesViewModels(weekDate, currentColumnInDt, dtSource, customersList, dtLot);

        var invoiceNumbersList = _generatorService.GetInvoiveNumbers(prepList, currentColumnInDt, dtKings, dtMansi);

        //var queues = _queueService.GetQueues(weekDate, harvestList, prepList);

        //list = _invoiceInfoService.GetCustomerInvoicesViewModels(prepList, customersList, invoiceNumbersList, queues);
        list = _invoiceInfoService.GetCustomerInvoicesViewModels(prepList, customersList, invoiceNumbersList, new List<SearchDto>());

        return list;
    }


    private static List<PrepareInvoicesViewModel> PrepareUsdaInvoicesViewModels(DateTime weekDate, int currentColumnInDt, DataTable dtSource, List<CustomerDashboardViewModel> customersList, DataTable dtLot)
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

            //var harvestDate = dataRow[currentColumnInDt + 1].ToString();
            //var bed = dataRow[currentColumnInDt + 2].ToString();
            //var lot = dataRow[currentColumnInDt + 3].ToString();

            var usdaInfo = new UsdaInfoDto();
            for (var i = 0; i < dtLot.Rows.Count; i++)
            {
                var lotRow = dtLot.Rows[i];
                if (lotRow[0].ToString() != customerKey) continue;

                var date = Utils.ParseToDateTime(lotRow[1].ToString());
                var harvestDate = date.GetValueOrDefault().ToString("MMdd");
                var bed = lotRow[2].ToString();
                var lot = lotRow[3].ToString();

                usdaInfo = new UsdaInfoDto
                {
                    HarvestDate = string.IsNullOrEmpty(harvestDate) ? string.Empty : harvestDate,
                    Bed = string.IsNullOrEmpty(bed) ? string.Empty : bed,
                    Lot = string.IsNullOrEmpty(lot) ? string.Empty : lot
                };

                break;
            }


            list.Add(new PrepareInvoicesViewModel
            {
                CustomerKey = customerKey, WeekQty = weekQty,
                UsdaInfo = usdaInfo,
                Week = weekDate
            });
        }

        return list;
    }
}