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
        //Database.SetInitializer<MavenContext>(null);
        //Configuration.ProxyCreationEnabled = false;
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


    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);

    //    modelBuilder.Entity<Bed>().HasData(
    //        new Bed { Id=1, Name = "1", Section = "MidWest" },
    //        new Bed { Id=2, Name = "2", Section = "MidWest" },
    //        new Bed { Id=3, Name = "3", Section = "MidWest" }
    //    );

    //    modelBuilder.Entity<Harvest>().HasData(
    //        new Harvest {Id=1, HarvestDate = DateTime.Today.AddDays(-10), Quantity = 230 },
    //        new Harvest {Id=2, HarvestDate = DateTime.Today.AddDays(-5), Quantity = 120 },
    //        new Harvest {Id=3, HarvestDate = DateTime.Today, Quantity = 24 }
    //    );
    //}
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
            .UseSqlServer(
                "Server=tcp:kingsfarms.database.windows.net,1433;Initial Catalog=kingsfarmsDEV;Persist Security Info=False;User ID=jthumma-admin;Password=j+humm@-@dm1n;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=600;")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}