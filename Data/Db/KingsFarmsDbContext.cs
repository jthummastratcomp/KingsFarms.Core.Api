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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            //.UseSqlServer("Server=tcp:kingsfarms.database.windows.net,1433;Initial Catalog=kingsfarmsDEV;Persist Security Info=False;User ID=jthumma-admin;Password=j+humm@-@dm1n;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=600;")
            .UseSqlServer(
                "Server=tcp:kingsfarmssqlserver.database.windows.net,1433;Initial Catalog=kingsfarmsdb;Persist Security Info=False;User ID=jthumma;Password=Jthumm@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}