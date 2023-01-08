using KingsFarms.Core.Api.Data.Domain;

namespace KingsFarms.Core.Api.Data.Providers;

public interface ICustomerDataProvider : IBaseDataProvider<Customer>
{
    //Customer? GetCustomer(int id);
    //List<Customer> GetCustomers();
    //int Save(Customer modifiedCustomer);
}

public interface IBaseDataProvider<T> where T : DomainObject
{
    T? GetById(int id);
    List<T> GetAll();
    int Save(T modifiedDomainObject);
}