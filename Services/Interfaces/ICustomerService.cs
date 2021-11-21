using System.Collections.Generic;
using HotTowel.Web.ViewModels;

namespace HotTowel.Web.Services.Interfaces
{
    public interface ICustomerService
    {
        CustomerHeaderViewModel GetCustomerHeader(int customerId);
        List<CustomerDashboardViewModel> GetCustomers(DashboardStatusEnum status);
        InvoiceBillViewModel GetAllCustomersInvoicesBillSummary(DashboardStatusEnum status);
        
    }
}