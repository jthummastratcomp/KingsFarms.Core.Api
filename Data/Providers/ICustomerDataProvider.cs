using KingsFarms.Core.Api.Data.Domain;
using System.Net;

namespace KingsFarms.Core.Api.Data.Providers;

public interface ICustomerDataProvider
{
    Customer? GetCustomer(int id);
    List<Customer> GetCustomers();
    int Save(Customer modifiedCustomer);
}