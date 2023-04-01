using KingsFarms.Core.Api.Helpers;
using MediatR;

namespace KingsFarms.Core.Api.Controllers;

public class GetCustomersDbRequestHandler : IRequestHandler<GetCustomersDbRequest, List<CustomerDbViewModel>>
{
    private readonly IDbService _dbService;
    private readonly ICustomerMapper _customerMapper;

    public GetCustomersDbRequestHandler(IDbService dbService, ICustomerMapper customerMapper)
    {
        _dbService = dbService;
        _customerMapper = customerMapper;
    }

    public async Task<List<CustomerDbViewModel>> Handle(GetCustomersDbRequest dbRequest, CancellationToken cancellationToken)
    {
        var list = _dbService.GetCustomersDb();

        return _customerMapper.MapCustomerListToCustomerDbViewModelList(list);
    }
}

public class AddCustomerRequestHandler : IRequestHandler<AddCustomerDbRequest, CustomerDbViewModel>
{
    private readonly IDbService _dbService;
    private readonly ICustomerMapper _customerMapper;

    public AddCustomerRequestHandler(IDbService dbService, ICustomerMapper customerMapper)
    {
        _dbService = dbService;
        _customerMapper = customerMapper;
    }

    public async Task<CustomerDbViewModel> Handle(AddCustomerDbRequest dbRequest, CancellationToken cancellationToken)
    {
        var customer = _customerMapper.MapCustomerDbViewModelToCustomer(dbRequest.ViewModel);
        _dbService.AddCustomer(customer);

        return dbRequest.ViewModel;
    }
}


public class EditCustomerRequestHandler : IRequestHandler<EditCustomerDbRequest, CustomerDbViewModel>
{
    private readonly IDbService _dbService;
    private readonly ICustomerMapper _customerMapper;

    public EditCustomerRequestHandler(IDbService dbService, ICustomerMapper customerMapper)
    {
        _dbService = dbService;
        _customerMapper = customerMapper;
    }

    public async Task<CustomerDbViewModel> Handle(EditCustomerDbRequest dbRequest, CancellationToken cancellationToken)
    {
        var customer = _customerMapper.MapCustomerDbViewModelToCustomer(dbRequest.ViewModel);

        
        _dbService.EditCustomer(customer.Id, customer.Key, customer);

        return dbRequest.ViewModel;
    }
}