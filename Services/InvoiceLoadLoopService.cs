using System.Collections.Generic;
using System.Configuration;
using HotTowel.Web.Cache;
using HotTowel.Web.Helpers;
using HotTowel.Web.Services.Interfaces;
using Intuit.Ipp.Data;

namespace HotTowel.Web.Services
{
    public class InvoiceLoadLoopService : IInvoiceLoadLoopService
    {
        private readonly IIntuitDataService _intuitDataService;

        public InvoiceLoadLoopService(IIntuitDataService intuitDataService)
        {
            _intuitDataService = intuitDataService;
        }

        [CacheTimeout]
        public List<Invoice> LoadAllInvoices()
        {
            var invoiceToLoad = Utils.ParseToInteger(ConfigurationManager.AppSettings["InvoicesToLoad"]);
            var loopCount = invoiceToLoad / 1000;

            var invoiceList = new List<Invoice>();

            for (var i = 0; i < loopCount; i++)
            {
                var start = i * 1000 + 1;

                var invoices = _intuitDataService.GetDataService().FindAll(new Invoice(), start, 1000);
                invoiceList.AddRange(invoices);
            }

            return invoiceList;
        }
    }
}