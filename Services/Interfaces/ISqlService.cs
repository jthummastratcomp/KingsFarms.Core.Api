using KingsFarms.Core.Api.Data.Domain;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface ISqlService
{
    //public List<Bed> GetBeds();
    //public List<Harvest> GetHarvests();
    public List<Customer> GetCustomers();
    //public List<Invoice> GetInvoices();
}