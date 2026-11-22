using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class PreyGroupConfiguration : IEntityTypeConfiguration<PreyGroup>
{
    public void Configure(EntityTypeBuilder<PreyGroup> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.GroupId).HasMaxLength(32).IsRequired();
        builder.Property(x => x.NumPreysCaptured);
        builder.Property(x => x.GateUnlockLevel);
        builder.Property(x => x.LastQueueRefill);
        builder.PrimitiveCollection(x => x.Preys).HasMaxLength(64);

        builder.HasIndex("PlayerId", nameof(PreyGroup.GroupId)).IsUnique();

        builder.HasMany(x => x.Captures)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Hunters)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
