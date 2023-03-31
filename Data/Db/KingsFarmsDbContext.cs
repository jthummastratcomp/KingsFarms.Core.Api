using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Data.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KingsFarms.Core.Api.Data.Db;

public sealed class KingsFarmsDbContext : DbContext, IDbContext
{
    public KingsFarmsDbContext()
    {
    }

    public KingsFarmsDbContext(DbContextOptions<KingsFarmsDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.AutoDetectChangesEnabled = true;
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Bed> Beds => Set<Bed>();
    public DbSet<Harvest> Harvests => Set<Harvest>();

    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<HorseFarm> HorseFarms => Set<HorseFarm>();
    public DbSet<HorseFarmLoad> HorseFarmLoads => Set<HorseFarmLoad>();


    public DbSet<T> GetSet<T>() where T : class
    {
        return Set<T>();
    }

    public EntityEntry<T> GetEntry<T>(T entity) where T : class
    {
        return Entry(entity);
    }
    
}