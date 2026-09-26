using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class MacroObjectConfiguration : IEntityTypeConfiguration<MacroObject>
{
    public void Configure(EntityTypeBuilder<MacroObject> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
        builder.Property(x => x.ParentItemName).IsRequired().HasMaxLength(64);
    }
}
