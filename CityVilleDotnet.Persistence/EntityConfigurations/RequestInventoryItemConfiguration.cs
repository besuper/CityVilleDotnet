using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class RequestInventoryItemConfiguration : IEntityTypeConfiguration<RequestInventoryItem>
{
    public void Configure(EntityTypeBuilder<RequestInventoryItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.InventoryName).HasMaxLength(64);
        builder.Property(x => x.ItemName).HasMaxLength(64);
    }
}
