namespace KingsFarms.Core.Api.Data.Providers;

public interface IUnitOfWork : IDisposable
{
    IDbContext Context { get; }
    int Save();
}