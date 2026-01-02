using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class WorldObjectConfiguration : IEntityTypeConfiguration<WorldObject>
{
    public void Configure(EntityTypeBuilder<WorldObject> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.ItemName).HasMaxLength(64);
        builder.Property(x => x.ClassName).HasMaxLength(64);
        builder.Property(x => x.TargetBuildingClass).IsRequired(false).HasMaxLength(64);
        builder.Property(x => x.TargetBuildingName).IsRequired(false).HasMaxLength(64);
        builder.Property(x => x.State).HasMaxLength(32);
        builder.Property(x => x.ContractName).IsRequired(false).HasMaxLength(64);

        builder.HasOne(x => x.FranchiseLocation);
    }
}