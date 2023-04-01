using KingsFarms.Core.Api.Data.Domain;

namespace KingsFarms.Core.Api.Controllers;

public interface IDbService
{
    void AddCustomer(Customer customer);
    List<Customer> GetCustomersDb();
    void RemoveCustomer(int customerId, string customerKey);
    void EditCustomer(int customerId, string customerKey, Customer customer);
}