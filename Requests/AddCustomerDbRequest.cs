using MediatR;

namespace KingsFarms.Core.Api.Controllers;

public class GetCustomersDbRequest : IRequest<List<CustomerDbViewModel>>
{
    
}

public class AddCustomerDbRequest : IRequest<CustomerDbViewModel>
{
    public CustomerDbViewModel ViewModel { get; set; }
}

public class EditCustomerDbRequest : IRequest<CustomerDbViewModel>
{
    public string CustomerIdOrKey { get; set; }
    public CustomerDbViewModel ViewModel { get; set; }
}

public class RemoveCustomerDbRequest : IRequest<CustomerDbViewModel>
{
    public string CustomerIdOrKey { get; set; }
    public CustomerDbViewModel ViewModel { get; set; }
}