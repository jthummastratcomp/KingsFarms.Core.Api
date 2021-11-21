using System.Collections.Generic;
using HotTowel.Web.ViewModels;

namespace HotTowel.Web.Services.Interfaces
{
    public interface IInvoiceService
    {
        List<CustomerInvoicesViewModel> GetAllCustomerInvoices(DashboardStatusEnum status);
        List<CustomerInvoicesViewModel> GetCustomerInvoices(int customerId, DashboardStatusEnum status);
        InvoiceBillViewModel GetCustomerInvoicesBillSummary(int customerId, DashboardStatusEnum status);
        CustomerPriceViewModel GetCustomerInvoicesPriceSummary(int customerId, DashboardStatusEnum status);
        ShipmentsBillViewModel GetCustomerShipmentSummary(int customerId, DashboardStatusEnum status);
        void AddCustomerInvoice(CustomerInvoicesViewModel viewModel);
        List<ShipmentsBillViewModel> GetYearlyShipmentsBills(int customerId, DashboardStatusEnum status);
        //WeeklyShipmentsGraphViewModel GetCustomerWeeklyShipmentsForGraph(int customerId, DashboardStatusEnum status);
        List<ShipmentsBillViewModel> GetAllCustomersYearlyShipmentsBills(DashboardStatusEnum status);

        List<SearchDto> GetAllCustomerWeeklyShipmentsForGraph(DashboardStatusEnum status);
        List<SearchDto> GetCustomerWeeklyShipmentsForGraph(int customerId, DashboardStatusEnum status);
    }
}