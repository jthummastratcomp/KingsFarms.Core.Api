using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Data.Providers;
using KingsFarms.Core.Api.Services.Interfaces;

namespace KingsFarms.Core.Api.Services;

public class SqlService : ISqlService
{
    
    private readonly IUnitOfWork _unitOfWork;
    

    public SqlService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public List<Bed> GetBeds()
    {
        return _unitOfWork.BedsRepo.All().ToList();
    }

    public List<Harvest> GetHarvests()
    {
        return _unitOfWork.HarvestRepo.All().ToList();
    }

    public List<Customer> GetCustomers()
    {
        return _unitOfWork.CustomerRepo.All().ToList();
    }

    public List<Invoice> GetInvoices()
    {
        return _unitOfWork.InvoiceRepo.All().ToList();
    }
    
}