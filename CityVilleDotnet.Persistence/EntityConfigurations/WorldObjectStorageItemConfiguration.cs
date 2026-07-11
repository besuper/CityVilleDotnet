using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class WorldObjectStorageItemConfiguration : IEntityTypeConfiguration<WorldObjectStorageItem>
{
    public void Configure(EntityTypeBuilder<WorldObjectStorageItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
        builder.Property(x => x.Amount);
    }
}
