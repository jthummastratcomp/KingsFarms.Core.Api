using System;
using System.Collections.Generic;
using System.Linq;
using HotTowel.Web.Cache;
using HotTowel.Web.Services.Interfaces;
using HotTowel.Web.ViewModels;
using Intuit.Ipp.Data;

namespace HotTowel.Web.Services
{
    public class InvoiceLoadService : IInvoiceLoadService
    {
        private readonly IInvoiceLoadLoopService _loadLoopService;

        public InvoiceLoadService(IInvoiceLoadLoopService loadLoopService)
        {
            _loadLoopService = loadLoopService;
        }

        public List<Invoice> LoadAllInvoices(DashboardStatusEnum status)
        {
            var list = _loadLoopService.LoadAllInvoices();

            if (status == DashboardStatusEnum.CurrentYear) return list.Where(x => x.DueDateSpecified && x.DueDate.Year == DateTime.Today.Year).ToList();
            return status == DashboardStatusEnum.LastYear ? list.Where(x => x.DueDateSpecified && x.DueDate.Year == DateTime.Today.AddYears(-1).Year).ToList() : list;
        }

        [CacheTimeout]
        public List<Invoice> LoadCustomerInvoices(int customerId, DashboardStatusEnum status)
        {
            //return _loadLoopService.LoadAllInvoices().Where(x => x.CustomerRef != null && x.CustomerRef.Value == customerId.ToString()).ToList();
            return LoadAllInvoices(status).Where(x => x.CustomerRef != null && x.CustomerRef.Value == customerId.ToString()).ToList();
        }
    }
}