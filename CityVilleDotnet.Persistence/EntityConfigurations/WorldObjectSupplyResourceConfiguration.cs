using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class WorldObjectSupplyResourceConfiguration : IEntityTypeConfiguration<WorldObjectSupplyResource>
{
    public void Configure(EntityTypeBuilder<WorldObjectSupplyResource> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Position);
        builder.Property(x => x.ItemName).IsRequired().HasMaxLength(64);
    }
}
