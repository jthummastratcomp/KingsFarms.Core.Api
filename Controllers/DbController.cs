using Microsoft.AspNetCore.Mvc;

namespace KingsFarms.Core.Api.Controllers;

// [ApiController]
public class DbController : ApiControllerBase //ControllerBase
{
    [HttpGet(CoreApiRoutes.GetCustomersDb)]
    public List<CustomerDbViewModel> GetCustomers()
    {
        return Mediator.Send(new GetCustomersDbRequest()).Result;
    }

    [HttpPost(CoreApiRoutes.AddCustomerDb)]
    public CustomerDbViewModel AddCustomer(CustomerDbViewModel vm)
    {
        return Mediator.Send(new AddCustomerDbRequest {ViewModel = vm}).Result;
    }

    [HttpPost(CoreApiRoutes.EditCustomerDb)]
    public CustomerDbViewModel EditCustomer(string customerIdOrKey, CustomerDbViewModel vm)
    {
        return Mediator.Send(new EditCustomerDbRequest {CustomerIdOrKey = customerIdOrKey, ViewModel = vm}).Result;
    }

    [HttpPost(CoreApiRoutes.RemoveCustomerDb)]
    public CustomerDbViewModel RemoveCustomer(string customerIdOrKey, CustomerDbViewModel vm)
    {
        return Mediator.Send(new RemoveCustomerDbRequest {ViewModel = vm}).Result;
    }
}