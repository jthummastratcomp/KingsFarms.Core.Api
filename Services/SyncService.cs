using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Data.Providers;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels.Customer;
using KingsFarms.Core.Api.ViewModels.Harvest;

namespace KingsFarms.Core.Api.Services;

public class SyncService : ISyncService
{
    private readonly IBedService _bedService;
    private readonly IHarvestService _harvestService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWeeklyOrdersUsdaService _weeklyOrdersUsdaService;

    public SyncService(IBedService bedService, IHarvestService harvestService, IWeeklyOrdersUsdaService weeklyOrdersUsdaService, IUnitOfWork unitOfWork)
    {
        _bedService = bedService;
        _harvestService = harvestService;
        _weeklyOrdersUsdaService = weeklyOrdersUsdaService;
        _unitOfWork = unitOfWork;
    }

    public string SyncBeds()
    {
        var list = _bedService.GetBedsInfo();


        foreach (var vm in list)
        {
            var bedNumber = Utils.ParseToInteger(vm.BedNumber.ToLower().Replace("bed", "").Trim());

            var bed = _unitOfWork.BedsRepo.Find(x => x.Number == bedNumber).FirstOrDefault();
            if (bed == null)
            {
                _unitOfWork.BedsRepo.Add(new Bed {Number = bedNumber, Section = vm.SectionDisplay, PlantedDate = vm.PlantedDate, PlantsCount = vm.PlantsCount});
            }
            else
            {
                bed.Section = vm.SectionDisplay;
                bed.PlantedDate = vm.PlantedDate;
                bed.PlantsCount = vm.PlantsCount;

                _unitOfWork.BedsRepo.Update(bed);
            }
        }

        _unitOfWork.SaveChanges();
        return "Synchronized Beds";
    }

    public string SyncHarvests()
    {
        var list = _harvestService.GetAllHarvestData();


        foreach (var vm in list)
        {
            var bed = _unitOfWork.BedsRepo.Get(vm.BedNumber);
            if (bed == null) continue;

            var harvestForBedDate = _unitOfWork.HarvestRepo.Find(x => x.BedId == bed.Id && x.HarvestDate == vm.HarvestDate).FirstOrDefault();
            if (harvestForBedDate == null)
            {
                var harvest = new Harvest
                {
                    HarvestDate = vm.HarvestDate,
                    Quantity = vm.HarvestQty,
                    BedId = bed.Id
                };
                _unitOfWork.HarvestRepo.Add(harvest);
            }
            else
            {
                harvestForBedDate.Quantity = vm.HarvestQty;
                _unitOfWork.HarvestRepo.Update(harvestForBedDate);
            }
        }

        _unitOfWork.SaveChanges();
        return "Synchronized Harvests";
    }

    public string SyncCustomers()
    {
        var list = _weeklyOrdersUsdaService.GetCustomersFromOrdersFile();

        foreach (var vm in list)
        {
            var customer = _unitOfWork.CustomerRepo.Find(x => x.Key == vm.CustomerHeader.CustomerKey).FirstOrDefault();
            if (customer == null)
                AddCustomerToRepo(vm);
            else
                UpdateCustomerOnRepo(customer, vm);
        }

        _unitOfWork.SaveChanges();
        return "Synchronized Customers";
    }


    public string SyncInvoices()
    {
        var weeks = _weeklyOrdersUsdaService.GetInvoiceWeeksListForYear(2023);
        foreach (var week in weeks)
        {
            if (week.Data == "2022-12-31") continue;

            var list = _weeklyOrdersUsdaService.LoadInvoicesForWeek(week.Data, CompanyEnum.Kings);

            foreach (var vm in list)
            {
                var customer = _unitOfWork.CustomerRepo.Find(x => x.Key == vm.CustomerHeader.CustomerKey).FirstOrDefault();
                if (customer != null)
                {
                    var invoice = _unitOfWork.InvoiceRepo.Find(x => x.InvoiceNumber == vm.InvoiceNumber).FirstOrDefault();
                    if (invoice == null)
                    {
                        _unitOfWork.InvoiceRepo.Add(new Invoice
                        {
                            InvoiceNumber = vm.InvoiceNumber,
                            InvoiceDate = vm.InvoiceDate,
                            DueDate = vm.DueDate,
                            Quantity = vm.Cost.Quantity,
                            Rate = vm.Price.Rate,
                            Amount = vm.Bill.Billed,
                            Memo = vm.Memo,
                            CustomerId = customer.Id
                        });
                    }
                    else
                    {
                        invoice.InvoiceDate = vm.InvoiceDate;
                        invoice.DueDate = vm.DueDate;
                        invoice.Quantity = vm.Cost.Quantity;
                        invoice.Rate = vm.Price.Rate;
                        invoice.Amount = vm.Bill.Billed;
                        invoice.Memo = vm.Memo;
                    }
                }
            }
        }

        _unitOfWork.SaveChanges();
        return "Synchronized Invoices";
        ;
    }

    public string AddCustomer(CustomerDashboardViewModel vm)
    {
        AddCustomerToRepo(vm);
        _unitOfWork.SaveChanges();
        return "Customer Added";
    }

    public string SyncCustomers(List<CustomerHeaderViewModel> list)
    {
        foreach (var vm in list) _unitOfWork.CustomerRepo.Add(new Customer {Key = vm.CustomerKey, City = vm.Address.City});
        _unitOfWork.SaveChanges();
        return "Synchronized Customers";
    }

    public string SaveHarvestData(HarvestViewModel viewModel)
    {
        _unitOfWork.HarvestRepo.Add(new Harvest
        {
            BedId = viewModel.BedNumber, HarvestDate = viewModel.HarvestDate, Quantity = viewModel.HarvestQty
        });
        _unitOfWork.SaveChanges();
        return "Saved Harvest Data";
    }

    private void UpdateCustomerOnRepo(Customer customer, CustomerDashboardViewModel vm)
    {
        customer.Name = vm.CustomerHeader.StoreName;
        customer.Address = vm.CustomerHeader.Address.FirstLineDisplay;
        customer.City = vm.CustomerHeader.Address.City;
        customer.StoreName = vm.CustomerHeader.StoreName;
        customer.Zip = vm.CustomerHeader.Address.Zip;
        customer.ContactName = vm.CustomerHeader.Contact.Name;
        _unitOfWork.CustomerRepo.Update(customer);
    }

    private void AddCustomerToRepo(CustomerDashboardViewModel vm)
    {
        _unitOfWork.CustomerRepo.Add(new Customer
        {
            Key = vm.CustomerHeader.CustomerKey,
            Name = vm.CustomerHeader.StoreName,
            Address = vm.CustomerHeader.Address.FirstLineDisplay,
            City = vm.CustomerHeader.Address.City,
            StoreName = vm.CustomerHeader.StoreName,
            Zip = vm.CustomerHeader.Address.Zip,
            ContactName = vm.CustomerHeader.Contact.Name
        });
    }
}