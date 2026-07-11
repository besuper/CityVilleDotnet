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
        builder.Property(x => x.RemodelItemName).IsRequired(false).HasMaxLength(64);

        builder.HasIndex(x => new { x.Id, x.WorldFlatId }).IsUnique();
        builder.HasIndex(x => x.WorldFlatId);
        builder.HasIndex(x => x.ClassName);

        builder.HasOne(x => x.FranchiseLocation);
        builder.HasMany(x => x.CrewMembers).WithOne().IsRequired();
        builder.HasMany(x => x.MechanicCounters).WithOne().IsRequired();
        builder.HasMany(x => x.StorageItems).WithOne().IsRequired();
        builder.HasMany(x => x.Slots).WithOne().IsRequired();
        
        builder.Property(x => x.ActivationTime);
        builder.Property(x => x.InactiveTime);
        builder.Property(x => x.StreakLength);
        builder.Property(x => x.EnergyModifier);
    }
}