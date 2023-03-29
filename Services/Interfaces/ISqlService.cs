using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.ViewModels.Customer;
using KingsFarms.Core.Api.ViewModels.Harvest;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface ISqlService
{
    public List<Bed> GetBeds();
    public List<Harvest> GetHarvests();
    public List<Customer> GetCustomers();
    public List<Invoice> GetInvoices();
}