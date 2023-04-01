using KingsFarms.Core.Api.Data.Domain;

namespace KingsFarms.Core.Api.Controllers;

public interface IDbService
{
    void AddCustomer(Customer customer);
    List<Customer> GetCustomersDb();
    void DeleteCustomer(int customerId);
    void EditCustomer(int customerId, string customerKey, Customer customer);
}