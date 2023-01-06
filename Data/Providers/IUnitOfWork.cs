using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Data.Repositories;

namespace KingsFarms.Core.Api.Data.Providers;

//public interface IUnitOfWork : IDisposable
//{
//    IDbContext Context { get; }
//    int Save();
//}

public interface IUnitOfWork
{
    IRepository<Customer> CustomerRepo { get; }
    IRepository<Invoice> InvoiceRepo { get; }
    IRepository<Bed> BedsRepo { get; }
    IRepository<Harvest> HarvestRepo { get; }
    void SaveChanges();
}