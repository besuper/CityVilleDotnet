using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class WorldObjectSlotConfiguration : IEntityTypeConfiguration<WorldObjectSlot>
{
    public void Configure(EntityTypeBuilder<WorldObjectSlot> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.SlotIndex);
        builder.Property(x => x.ItemName).IsRequired().HasMaxLength(64);
    }
}
