using KingsFarms.Core.Api.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace KingsFarms.Core.Api.Data.Db;

public class KingsFarmsDbContext : DbContext
{
    public KingsFarmsDbContext(DbContextOptions<KingsFarmsDbContext> options) : base(options)
    {
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
}