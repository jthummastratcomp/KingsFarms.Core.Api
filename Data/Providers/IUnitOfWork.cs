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
    IRepository<Customer> CustomerRepository { get; }
    IRepository<Invoice> InvoiceRepository { get; }
    void SaveChanges();
}