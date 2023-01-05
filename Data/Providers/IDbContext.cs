using KingsFarms.Core.Api.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KingsFarms.Core.Api.Data.Providers;

public interface IDbContext
{
    int SaveChanges();
    void Dispose();
    DbSet<T> GetSet<T>() where T : class;
    EntityEntry<T> GetEntry<T>(T entity) where T : class;

    DbSet<Customer> Customers { get;  }
    DbSet<Invoice> Invoices { get;  }
    DbSet<Bed> Beds { get; }
    DbSet<Harvest> Harvests { get; }

    DbSet<Shipment> Shipments { get; }
    DbSet<HorseFarm> HorseFarms { get; }
    DbSet<HorseFarmLoad> HorseFarmLoads { get; }
}