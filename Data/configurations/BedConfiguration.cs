using KingsFarms.Core.Api.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KingsFarms.Core.Api.Data.configurations;

public class BedConfiguration : IEntityTypeConfiguration<Bed>
{
    public void Configure(EntityTypeBuilder<Bed> builder)
    {
        //builder.HasKey(x => x.Id);
        //builder.HasMany<Harvest>();
    }
}