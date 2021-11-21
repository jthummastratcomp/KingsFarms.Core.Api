using System.Collections.Generic;
using System.Linq;
using HotTowel.Web.Cache;
using HotTowel.Web.Controllers;
using HotTowel.Web.Helpers;
using HotTowel.Web.Services.Interfaces;
using HotTowel.Web.ViewModels;
using Intuit.Ipp.Data;

namespace HotTowel.Web.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerLoadService _customerLoadService;
        private readonly IInvoiceService _invoiceService;

        public CustomerService(ICustomerLoadService customerLoadService, IInvoiceService invoiceService)
        {
            _customerLoadService = customerLoadService;
            _invoiceService = invoiceService;
        }

        [CacheTimeout]
        public CustomerHeaderViewModel GetCustomerHeader(int customerId)
        {
            var c = _customerLoadService.LoadAllCustomers().FirstOrDefault(x => x.Id == customerId.ToString());
            return c == null ? new CustomerHeaderViewModel() : GetCustomerHeader(c);
        }

        [CacheTimeout]
        public List<CustomerDashboardViewModel> GetCustomers(DashboardStatusEnum status)
        {
            var list = new List<CustomerDashboardViewModel>();
            list.AddRange(from Customer c in _customerLoadService.LoadAllCustomers()
                select new CustomerDashboardViewModel
                {
                    Id = Utils.ParseToInteger(c.Id),
                    CustomerHeader = GetCustomerHeader(c),
                    Bill = _invoiceService.GetCustomerInvoicesBillSummary(Utils.ParseToInteger(c.Id), status),
                    Price = _invoiceService.GetCustomerInvoicesPriceSummary(Utils.ParseToInteger(c.Id), status),
                    Shipment = _invoiceService.GetCustomerShipmentSummary(Utils.ParseToInteger(c.Id), status)
                });

            //return list;
            return list.Where(x=>x.Bill.Billed > 0).ToList();
        }

        public InvoiceBillViewModel GetAllCustomersInvoicesBillSummary(DashboardStatusEnum status)
        {
            var list = GetCustomers(status);

            var billedTotal = list.Sum(x => x.Bill.Billed);
            var paidTotal = list.Sum(x => x.Bill.Paid);
            var balanceTotal = list.Sum(x => x.Bill.Balance);
            return new InvoiceBillViewModel { Billed = billedTotal, Paid = paidTotal, Balance = balanceTotal };
        }


        private static CustomerHeaderViewModel GetCustomerHeader(Customer customer)
        {
            if (customer == null) return new CustomerHeaderViewModel();
            return new CustomerHeaderViewModel
            {
                CustomerKey = customer.DisplayName,
                StoreName = customer.CompanyName,
                Address = new AddressViewModel
                {
                    Street = customer.ShipAddr == null ? string.Empty : customer.ShipAddr.Line1,
                    City = customer.ShipAddr == null ? string.Empty : customer.ShipAddr.City,
                    State = customer.ShipAddr == null ? string.Empty : customer.ShipAddr.CountrySubDivisionCode,
                    Zip = customer.ShipAddr == null ? string.Empty : customer.ShipAddr.PostalCode
                },
                Contact = GetCustomerContact(customer)
            };
        }

        private static ContactViewModel GetCustomerContact(Customer c)
        {
            return new ContactViewModel
            {
                //Name = string.IsNullOrEmpty(c.ContactName) ? $"{c.GivenName} {c.FamilyName}" : c.ContactName,
                FirstName = string.IsNullOrEmpty(c.GivenName) ? c.ContactName : c.GivenName,
                LastName = c.FamilyName,
                Phone = c.PrimaryPhone == null ? string.Empty : c.PrimaryPhone.FreeFormNumber,
                Email = c.PrimaryEmailAddr == null ? string.Empty : c.PrimaryEmailAddr.Address
            };
        }
    }
}