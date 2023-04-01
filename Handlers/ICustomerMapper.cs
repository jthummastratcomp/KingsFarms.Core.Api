using KingsFarms.Core.Api.Data.Domain;

namespace KingsFarms.Core.Api.Controllers;

public interface ICustomerMapper
{
    Customer MapCustomerDbViewModelToCustomer(CustomerDbViewModel vm);
    CustomerDbViewModel MapCustomerToCustomerDBViewModel(Customer customer);
    List<CustomerDbViewModel> MapCustomerListToCustomerDbViewModelList(List<Customer> list);
}