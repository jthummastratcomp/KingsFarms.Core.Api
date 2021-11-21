using System;
using System.Collections.Generic;
using System.Linq;
using HotTowel.Web.Helpers;
using HotTowel.Web.Services.Interfaces;
using HotTowel.Web.ViewModels;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;

namespace HotTowel.Web.Services
{
    public class GenerateWeeklyOrdersService : IGenerateWeeklyOrdersService
    {
        private readonly IIntuitDataService _intuitDataService;
        private readonly IWeeklyOrdersService _weeklyOrdersService;

        public GenerateWeeklyOrdersService(IIntuitDataService intuitDataService, IWeeklyOrdersService weeklyOrdersService)
        {
            _intuitDataService = intuitDataService;
            _weeklyOrdersService = weeklyOrdersService;
        }


        public List<string> GenerateInvoicesForWeek(string week, CompanyEnum company)
        {
            var invoicesToGenerate = _weeklyOrdersService.LoadInvoicesForWeek(week, company);
            if (!Utils.HasRows(invoicesToGenerate)) return new List<string> { "No invoices to generate" };

            var dataService = _intuitDataService.GetDataService();

            var messages = new List<string>();

            foreach (var viewModel in invoicesToGenerate)
            {
                if (viewModel.Cost.Quantity == 0) continue;

                var customer = FindOrCreateCustomer(viewModel, dataService, messages);
                if (customer != null) CreateInvoiceForWeek(customer, Utils.ParseToDateTime(week).GetValueOrDefault(), viewModel, dataService, messages);
            }

            messages.Add("Invoices generated successfully");
            return messages;
        }

        private static Customer FindOrCreateCustomer(CustomerInvoicesViewModel viewModel, DataService dataService, List<string> messages)
        {
            var foundCustomer = dataService.FindAll(new Customer()).FirstOrDefault(x => x.DisplayName == viewModel.CustomerHeader.CustomerKey);
            if (foundCustomer != null) return foundCustomer;

            var customer = new Customer
            {
                CompanyName = viewModel.CustomerHeader.StoreName,
                GivenName = viewModel.CustomerHeader.Contact.FirstName,
                FamilyName = viewModel.CustomerHeader.Contact.LastName,
                //Title = customerKey,
                DisplayName = viewModel.CustomerHeader.CustomerKey,
                PrimaryEmailAddr = new EmailAddress { Address = viewModel.CustomerHeader.Contact.Email },
                PrimaryPhone = new TelephoneNumber { FreeFormNumber = GetFormattedPhoneNumbers(viewModel.CustomerHeader.Contact.Phone) },
                BillAddr = GetCustomerAddress(viewModel.CustomerHeader.Address),
                ShipAddr = GetCustomerAddress(viewModel.CustomerHeader.Address)
            };

            var addedCustomer = dataService.Add(customer);
            messages.Add($"Created new customer {customer.DisplayName}");
            return addedCustomer;
        }

        private static Invoice CreateInvoiceForWeek(Customer customer, DateTime week, CustomerInvoicesViewModel viewModel, DataService dataService, List<string> messages)
        {
            var allInvoices = dataService.FindAll(new Invoice()).Where(x => x.DueDateSpecified && x.DueDate.ToShortDateString() == week.ToShortDateString()).ToList();
            var invoiceForWeekFound = allInvoices.FirstOrDefault(x => x.DocNumber.StartsWith(viewModel.CustomerHeader.CustomerKey));
            if (invoiceForWeekFound != null)
            {
                messages.Add($"Invoice {invoiceForWeekFound.DocNumber} already exists");
                return invoiceForWeekFound;
            }

            var term = dataService.FindAll(new Term()).FirstOrDefault(x => x.Name == "Due on receipt");

            var invoice = new Invoice
            {
                CustomerRef = new ReferenceType { Value = customer.Id },
                Line = AddInvoiceLines(viewModel, dataService),
                DueDate = week,
                TxnDate = week,
                TxnDateSpecified = true,
                ShipDate = week,
                ShipDateSpecified = true,
                AllowOnlinePayment = true,
                AllowOnlinePaymentSpecified = true,
                AllowOnlineACHPayment = true,
                AllowOnlineACHPaymentSpecified = true,
                AutoDocNumber = false,
                AutoDocNumberSpecified = true,
                DocNumber = viewModel.InvoiceNumber,
                //TrackingNum = new Random().Next().ToString(),
                BillEmail = new EmailAddress { Address = viewModel.CustomerHeader.Contact.Email }
            };
            if (term != null) invoice.SalesTermRef = new ReferenceType { Value = term.Id };

            var addedInvoice = dataService.Add(invoice);
            messages.Add($"Invoice {viewModel.InvoiceNumber} created");
            return addedInvoice;
        }

        private static Line[] AddInvoiceLines(CustomerInvoicesViewModel viewModel, DataService dataService)
        {
            var lineItems = new List<Line>
            {
                AddLineItem("Curry Leaf", "Organically grown Curry Leaves (in lbs)",
                    viewModel.Cost.Quantity,
                    viewModel.Price.Rate,
                    FindOrCreateCurryLeafIncomeAccount(dataService), dataService),

                AddLineItem("Shipping", viewModel.Price.ShipmentRate > 0 ? string.Empty : "Discounted",
                    viewModel.Price.ShipmentRate > 0 ? 1 : 0,
                    viewModel.Price.ShipmentRate > 0 ? viewModel.Bill.ShipmentCost : 0,
                    FindOrCreateShippingIncomeAccount(dataService), dataService)
            };

            if (viewModel.CustomerHeader.CustomerKey == "MYT-DEN")
            {
                lineItems.Add(AddLineItem("Credit Card Charges", string.Empty,
                    1,
                    viewModel.Bill.ChargesDiscounts,
                    FindOrCreateServicesIncomeAccount(dataService), dataService));
                
            }
            if (viewModel.CustomerHeader.CustomerKey == "TRI-CHA")
            {
                lineItems.Add(AddLineItem("Discount", "2 lbs of leaf at no charge",
                    1,
                    viewModel.Bill.ChargesDiscounts,
                    FindOrCreateServicesIncomeAccount(dataService), dataService));
            }

            return lineItems.ToArray();
        }

        private static Line AddLineItem(string itemName, string itemDesc, decimal quantity, decimal unitPrice, Account account, DataService dataService)
        {
            var item = FindOrCreateItem(itemName, itemDesc, unitPrice, account, dataService);

            var line = new Line
            {
                Description = itemDesc,
                AnyIntuitObject = new SalesItemLineDetail
                {
                    Qty = quantity,
                    QtySpecified = true,
                    AnyIntuitObject = unitPrice,
                    ItemElementName = ItemChoiceType.UnitPrice,
                    ItemRef = new ReferenceType { Value = item.Id, name = item.Name }
                },
                DetailType = LineDetailTypeEnum.SalesItemLineDetail,
                DetailTypeSpecified = true,
                Amount = quantity * unitPrice,
                AmountSpecified = true
            };
            return line;
        }

        private static Item FindOrCreateItem(string itemName, string itemDesc, decimal unitPrice, Account account, DataService dataService)
        {
            var foundCurryLeafItem = dataService.FindAll(new Item()).FirstOrDefault(x => x.Name == itemName && x.Active);
            if (foundCurryLeafItem != null) return foundCurryLeafItem;

            var item = new Item
            {
                Name = itemName,
                Description = itemDesc,
                Type = account.Name == "Services" ?ItemTypeEnum.Service :  ItemTypeEnum.NonInventory,
                TypeSpecified = true,
                UnitPrice = unitPrice,
                UnitPriceSpecified = true,
                Active = true,
                ActiveSpecified = true,
                Taxable = false,
                TaxableSpecified = true,
                IncomeAccountRef = new ReferenceType { name = account.Name, Value = account.Id },
                ExpenseAccountRef = new ReferenceType { name = account.Name, Value = account.Id },
                TrackQtyOnHand = false,
                TrackQtyOnHandSpecified = true
            };

            var addedItem = dataService.Add(item);
            return addedItem;
        }

        private static Account FindOrCreateCurryLeafIncomeAccount(DataService dataService)
        {
            var salesAccount = dataService.FindAll(new Account()).FirstOrDefault(x => x.Name == "Curry Leaf Income" && x.Active);
            if (salesAccount != null) return salesAccount;

            var account = new Account { Name = "Curry Leaf Income" };

            account.FullyQualifiedName = account.Name;
            account.Classification = AccountClassificationEnum.Revenue;
            account.ClassificationSpecified = true;
            account.AccountType = AccountTypeEnum.Income;
            account.AccountTypeSpecified = true;

            account.CurrencyRef = new ReferenceType
            {
                name = "United States Dollar",
                Value = "USD"
            };

            return dataService.Add(account);
        }

        private static Account FindOrCreateShippingIncomeAccount(DataService dataService)
        {
            var salesAccount = dataService.FindAll(new Account()).FirstOrDefault(x => x.Name == "Shipping Income" && x.Active);
            if (salesAccount != null) return salesAccount;

            var account = new Account { Name = "Shipping Income" };

            account.FullyQualifiedName = account.Name;
            account.Classification = AccountClassificationEnum.Revenue;
            account.ClassificationSpecified = true;
            account.AccountType = AccountTypeEnum.Income;
            account.AccountTypeSpecified = true;

            account.CurrencyRef = new ReferenceType
            {
                name = "United States Dollar",
                Value = "USD"
            };

            return dataService.Add(account);
        }

        private static Account FindOrCreateServicesIncomeAccount(DataService dataService)
        {
            var salesAccount = dataService.FindAll(new Account()).FirstOrDefault(x => x.Name == "Services" && x.Active);
            if (salesAccount != null) return salesAccount;

            var account = new Account { Name = "Services" };

            account.FullyQualifiedName = account.Name;
            account.Classification = AccountClassificationEnum.Revenue;
            account.ClassificationSpecified = true;
            account.AccountType = AccountTypeEnum.Income;
            account.AccountTypeSpecified = true;

            account.CurrencyRef = new ReferenceType
            {
                name = "United States Dollar",
                Value = "USD"
            };

            return dataService.Add(account);
        }

        private static string GetFormattedPhoneNumbers(string phone)
        {
            return string.IsNullOrEmpty(phone) ? string.Empty : phone.Replace("(", string.Empty).Replace(")", string.Empty).Replace(".", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty).Trim();
        }

        private static PhysicalAddress GetCustomerAddress(AddressViewModel dto)
        {
            return new PhysicalAddress
            {
                City = dto.City,
                PostalCode = dto.Zip,
                Line1 = dto.Street,
                CountrySubDivisionCode = dto.State
            };
        }
    }
}