using System.ComponentModel.DataAnnotations;

namespace KingsFarms.Core.Api.Data.Providers;

public sealed class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IDbContext context)
    {
        Context = context;
    }

    public IDbContext Context { get; }

    public void Dispose()
    {
        Context.Dispose();
    }

    public int Save()
    {
        try
        {
            var result = Context.SaveChanges();
            return result;
        }
        catch (Exception ex)
        {

            throw new ValidationException(ex.GetBaseException().Message)
            {
                HelpLink = null,
                HResult = 0,
                Source = null
            };
                
        }
    }
}