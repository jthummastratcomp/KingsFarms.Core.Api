using System.Collections.Generic;
using Intuit.Ipp.Data;

namespace HotTowel.Web.Services.Interfaces
{
    public interface IInvoiceLoadLoopService
    {
        //void SetDataService(DataService dataService);
        List<Invoice> LoadAllInvoices();
    }
}