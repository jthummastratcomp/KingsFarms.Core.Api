using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HotTowel.Web.Cache;
using HotTowel.Web.Helpers;
using HotTowel.Web.Services.Interfaces;
using HotTowel.Web.ViewModels;
using Intuit.Ipp.Data;

namespace HotTowel.Web.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceLoadService _invoiceLoadService;

        public InvoiceService(IInvoiceLoadService invoiceLoadService)
        {
            _invoiceLoadService = invoiceLoadService;
        }

        [CacheTimeout]
        public List<CustomerInvoicesViewModel> GetAllCustomerInvoices(DashboardStatusEnum status)
        {
            var invoices = _invoiceLoadService.LoadAllInvoices(status)
                .OrderByDescending(x => x.TxnDate).ToList();

            return !Utils.HasRows(invoices) ? new List<CustomerInvoicesViewModel>() : GetCustomerInvoicesViewModels(0, invoices);
        }

        [CacheTimeout]
        public List<CustomerInvoicesViewModel> GetCustomerInvoices(int customerId, DashboardStatusEnum status)
        {
            var invoices = _invoiceLoadService.LoadCustomerInvoices(customerId, status)
                .OrderByDescending(x => x.TxnDate).ToList();

            return !Utils.HasRows(invoices) ? new List<CustomerInvoicesViewModel>() : GetCustomerInvoicesViewModels(customerId, invoices);
        }

        public InvoiceBillViewModel GetCustomerInvoicesBillSummary(int customerId, DashboardStatusEnum status)
        {
            var list = GetCustomerInvoices(customerId, status);

            var billedTotal = list.Sum(x => x.Bill.Billed);
            var paidTotal = list.Sum(x => x.Bill.Paid);
            var balanceTotal = list.Sum(x => x.Bill.Balance);
            return new InvoiceBillViewModel { Billed = billedTotal, Paid = paidTotal, Balance = balanceTotal };
        }

        public CustomerPriceViewModel GetCustomerInvoicesPriceSummary(int customerId, DashboardStatusEnum status)
        {
            var list = GetCustomerInvoices(customerId, status);
            if (!Utils.HasRows(list)) return new CustomerPriceViewModel();

            var rate = list.First().Price.Rate;
            var shipmentRate = list.First().Price.ShipmentRate;
            return new CustomerPriceViewModel { Rate = rate, ShipmentRate = shipmentRate };
        }

        public ShipmentsBillViewModel GetCustomerShipmentSummary(int customerId, DashboardStatusEnum status)
        {
            var list = GetYearlyShipmentsBills(customerId, status);
            return list.FirstOrDefault();
        }

        public void AddCustomerInvoice(CustomerInvoicesViewModel viewModel)
        {
        }

        public List<ShipmentsBillViewModel> GetAllCustomersYearlyShipmentsBills(DashboardStatusEnum status)
        {
            var list = GetAllCustomerInvoices(status);
            return !Utils.HasRows(list) ? new List<ShipmentsBillViewModel>() : GetShipmentsBillViewModels(list);
        }

        public List<ShipmentsBillViewModel> GetYearlyShipmentsBills(int customerId, DashboardStatusEnum status)
        {
            var list = GetCustomerInvoices(customerId, status);
            return !Utils.HasRows(list) ? new List<ShipmentsBillViewModel>() : GetShipmentsBillViewModels(list);
        }

        public List<SearchDto> GetCustomerWeeklyShipmentsForGraph(int customerId, DashboardStatusEnum status)
        {
            var list = GetCustomerInvoices(customerId, status);
            return !Utils.HasRows(list) ? new List<SearchDto>() : GetWeeklyShipmentsForGraph(status, list);
            
        }

        public List<SearchDto> GetAllCustomerWeeklyShipmentsForGraph(DashboardStatusEnum status)
        {
            var list = GetAllCustomerInvoices(status);
            return !Utils.HasRows(list) ? new List<SearchDto>() : GetWeeklyShipmentsForGraph(status, list);
        }

        private static List<SearchDto> GetWeeklyShipmentsForGraph(DashboardStatusEnum status, List<CustomerInvoicesViewModel> list)
        {
            var weeksOfYear = Utils.GetWeeksOfYear(status == DashboardStatusEnum.CurrentYear ? 2021 : 2020);
            foreach (var weekDto in weeksOfYear)
            {
                weekDto.DataType = $"{weekDto.Id} {weekDto.Data}";
                var monday = Utils.ParseToDateTime(weekDto.Data).GetValueOrDefault();
                var mondayInvoices = list.Where(x => x.DueDate == monday).ToList();
                if (Utils.HasRows(mondayInvoices))
                {
                    weekDto.Data = mondayInvoices.Sum(x=>x.Cost.Quantity).ToString("##");
                }
            }

            return weeksOfYear;
        }

        //public WeeklyShipmentsGraphViewModel GetCustomerWeeklyShipmentsForGraph(int customerId, DashboardStatusEnum status)
        //{
        //    var list = GetCustomerInvoices(customerId, status);
        //    if (!Utils.HasRows(list)) return new WeeklyShipmentsGraphViewModel();

        //    var groupedList = GetInvoicesGroupedByYear(list);

        //    var categoryWeeks = new List<string>();
        //    for (var i = 0; i < 53; i++) categoryWeeks.Add($"Week {i + 1}");


        //    var weeklyShipments = new List<WeeklyShipmentsViewModel>();
        //    foreach (var byYearGroup in groupedList)
        //    {
        //        var year = byYearGroup.Key;
        //        weeklyShipments.Add(new WeeklyShipmentsViewModel
        //            { Year = year, ShippedQuantityPerWeek = GetWeeklyShippedForYear(year, byYearGroup) });
        //    }

        //    return new WeeklyShipmentsGraphViewModel
        //    {
        //        Categories = categoryWeeks, Series = weeklyShipments
        //    };
        //}

        private static List<CustomerInvoicesViewModel> GetCustomerInvoicesViewModels(int customerId, List<Invoice> invoices)
        {
            var list = new List<CustomerInvoicesViewModel>();
            list.AddRange(from Invoice c in invoices
                let lineItemLeaf = GetLineItem(c, "1")
                let lineItemLeafDetail = GetLineItemDetail(c, "1")
                let lineItemShipment = GetLineItem(c, "2")
                select new CustomerInvoicesViewModel
                {
                    Id = Utils.ParseToInteger(c.Id),
                    CustomerId = customerId,
                    InvoiceNumber = string.IsNullOrEmpty(c.DocNumber) ? "N/A" : c.DocNumber,
                    InvoiceDate = c.TxnDateSpecified ? c.TxnDate : DateTime.MinValue,
                    DueDate = c.DueDateSpecified ? c.DueDate : DateTime.MinValue,
                    Cost = new InvoiceCostViewModel
                    {
                        Quantity = lineItemLeafDetail?.Qty ?? 0,
                        Amount = lineItemLeaf?.Amount ?? 0
                    },
                    Price = new CustomerPriceViewModel
                    {
                        Rate = lineItemLeafDetail?.AnyIntuitObject == null
                            ? 0
                            : Utils.ParseToDecimal(lineItemLeafDetail.AnyIntuitObject.ToString()),
                        ShipmentRate = lineItemShipment?.Amount ?? 0
                    },
                    Bill = new InvoiceBillViewModel
                    {
                        Billed = c.TotalAmt,
                        Paid = c.TotalAmt - c.Balance,
                        Balance = c.Balance
                    }
                });

            return list;
        }

        private static List<ShipmentsBillViewModel> GetShipmentsBillViewModels(List<CustomerInvoicesViewModel> list)
        {
            var groupedList = GetInvoicesGroupedByYear(list);

            var returnList = new List<ShipmentsBillViewModel>();

            foreach (var byYearGroup in groupedList)
                returnList.Add(new ShipmentsBillViewModel
                {
                    Year = byYearGroup.Key,
                    NumberOfShipments = byYearGroup.Count(),
                    ShippedQuantity = byYearGroup.Sum(x => x.Cost.Quantity),
                    Bill = new InvoiceBillViewModel
                    {
                        Billed = byYearGroup.Sum(x => x.Bill.Billed),
                        Paid = byYearGroup.Sum(x => x.Bill.Paid),
                        Balance = byYearGroup.Sum(x => x.Bill.Balance)
                    }
                });

            return returnList.OrderByDescending(x => x.Year).ToList();
        }

        private static IEnumerable<IGrouping<int, CustomerInvoicesViewModel>> GetInvoicesGroupedByYear(IEnumerable<CustomerInvoicesViewModel> list)
        {
            return from invoice in list
                group invoice by invoice.InvoiceDate.Year
                into listByYear
                orderby listByYear.Key
                select listByYear;
        }

        private static List<int> GetWeeklyShippedForYear(int year, IGrouping<int, CustomerInvoicesViewModel> byYearGroup)
        {
            var seriesShippedQtyByWeek = new List<int>();
            for (var i = 0; i < 53; i++) seriesShippedQtyByWeek.Add(0); //default 0 lbs shipped per week

            foreach (var viewModel in byYearGroup)
            {
                var weekOfYear = Utils.GetWeekOfYear(viewModel.InvoiceDate);
                seriesShippedQtyByWeek[weekOfYear - 1] = Utils.ParseToInteger(viewModel.Cost.Quantity.ToString("N0"));
            }

            return seriesShippedQtyByWeek;
        }


        private static Line GetLineItem(Invoice invoice, string lineNum)
        {
            if (!Utils.HasRows(invoice.Line)) return null;
            var lineItem = invoice.Line.FirstOrDefault(x => x.LineNum == lineNum);
            return lineItem;
        }

        private static SalesItemLineDetail GetLineItemDetail(Invoice invoice, string lineNum)
        {
            var lineItem = GetLineItem(invoice, lineNum);
            if (!(lineItem?.AnyIntuitObject is SalesItemLineDetail lineItemDetail)) return null;
            return lineItemDetail;
        }
    }
}