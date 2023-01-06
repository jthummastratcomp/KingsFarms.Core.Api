using System.ComponentModel.DataAnnotations;
using KingsFarms.Core.Api.Data.Db;
using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Data.Repositories;

namespace KingsFarms.Core.Api.Data.Providers;

public class UnitOfWork : IUnitOfWork
{
    private readonly KingsFarmsDbContext _context;
    

    public UnitOfWork(KingsFarmsDbContext context)
    {
        _context = context;
        CustomerRepository = new CustomerRepository(_context);
        InvoiceRepository = new InvoiceRepository(_context);
    }

    public IRepository<Customer> CustomerRepository { get; }
    public IRepository<Invoice> InvoiceRepository { get; }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}


//public sealed class UnitOfWork : IUnitOfWork
//{
//    public UnitOfWork(IDbContext context)
//    {
//        Context = context;
//    }

//    public IDbContext Context { get; }

//    public void Dispose()
//    {
//        Context.Dispose();
//    }

//    public int Save()
//    {
//        try
//        {
//            var result = Context.SaveChanges();
//            return result;
//        }
//        catch (Exception ex)
//        {

//            throw new ValidationException(ex.GetBaseException().Message)
//            {
//                HelpLink = null,
//                HResult = 0,
//                Source = null
//            };
                
//        }
//    }
//}