using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KingsFarms.Core.Api.Data.Providers;

public interface IDbContext
{
    int SaveChanges();
    void Dispose();
    DbSet<T> GetSet<T>() where T : class;
    EntityEntry<T> GetEntry<T>(T entity) where T : class;
}