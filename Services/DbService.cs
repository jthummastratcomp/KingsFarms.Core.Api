using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Data.Providers;

namespace KingsFarms.Core.Api.Controllers;

internal class DbService : IDbService
{
    private readonly IUnitOfWork _unitOfWork;

    public DbService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void AddCustomer(Customer customer)
    {
        _unitOfWork.CustomerRepo.Add(customer);
        _unitOfWork.SaveChanges();
    }

    public List<Customer> GetCustomersDb()
    {
        return _unitOfWork.CustomerRepo.All().ToList();
    }

    public void DeleteCustomer(int customerId)
    {
        throw new NotImplementedException();
    }

    public void EditCustomer(int customerId, string customerKey, Customer modifiedCustomer)
    {
        var currentCustomer = customerId > 0 ? _unitOfWork.CustomerRepo.Get(customerId) : _unitOfWork.CustomerRepo.Find(x => x.Key == customerKey).FirstOrDefault();
        if (currentCustomer == null) return;

        currentCustomer.Key = modifiedCustomer.Key;
        currentCustomer.StoreName = modifiedCustomer.StoreName;
        currentCustomer.ContactName = modifiedCustomer.ContactName;
        currentCustomer.Address = modifiedCustomer.Address;
        currentCustomer.City = modifiedCustomer.City;
        currentCustomer.State = modifiedCustomer.State;
        currentCustomer.Zip = modifiedCustomer.Zip;
        currentCustomer.ContactPhone = modifiedCustomer.ContactPhone;
        currentCustomer.Email = modifiedCustomer.Email;

        _unitOfWork.SaveChanges();
    }
}