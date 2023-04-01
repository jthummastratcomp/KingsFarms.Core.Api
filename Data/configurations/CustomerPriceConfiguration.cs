using KingsFarms.Core.Api.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KingsFarms.Core.Api.Data.configurations;

public class CustomerPriceConfiguration : IEntityTypeConfiguration<CustomerPrice>
{
    public void Configure(EntityTypeBuilder<CustomerPrice> builder)
    {
        //builder.HasKey(x => x.Id);
        
    }
}