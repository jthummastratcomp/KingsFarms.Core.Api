using KingsFarms.Core.Api.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KingsFarms.Core.Api.Data.configurations;

public class HarvestConfiguration : IEntityTypeConfiguration<Harvest>
{
    public void Configure(EntityTypeBuilder<Harvest> builder)
    {
        //builder.HasKey(x => x.Id);
        //builder.HasOne<Bed>();
    }
}