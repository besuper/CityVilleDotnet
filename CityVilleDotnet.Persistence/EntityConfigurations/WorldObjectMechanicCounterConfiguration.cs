using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class WorldObjectMechanicCounterConfiguration : IEntityTypeConfiguration<WorldObjectMechanicCounter>
{
    public void Configure(EntityTypeBuilder<WorldObjectMechanicCounter> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.MechanicType).IsRequired().HasMaxLength(64);
        builder.Property(x => x.Count);
    }
}
