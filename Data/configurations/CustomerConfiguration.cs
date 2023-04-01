using KingsFarms.Core.Api.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KingsFarms.Core.Api.Data.configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        //builder.HasKey(x => x.Id);
        builder.Property(x => x.Key).IsRequired().HasMaxLength(10);
        builder.Property(x => x.StoreName).HasMaxLength(250);
        builder.Property(x => x.ContactName).HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(250);
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.State).HasMaxLength(10);
        builder.Property(x => x.Zip).HasMaxLength(20);
        builder.Property(x => x.ContactPhone).HasMaxLength(20);
        //builder.HasMany<Invoice>();
    }
}