using System.Collections.Generic;
using HotTowel.Web.ViewModels;
using Intuit.Ipp.Data;

namespace HotTowel.Web.Services.Interfaces
{
    public interface IInvoiceLoadService
    {
        List<Invoice> LoadCustomerInvoices(int customerId, DashboardStatusEnum status);
        List<Invoice> LoadAllInvoices(DashboardStatusEnum status);
    }
}