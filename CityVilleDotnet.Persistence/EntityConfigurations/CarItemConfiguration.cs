using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class CarItemConfiguration : IEntityTypeConfiguration<CarItem>
{
    public void Configure(EntityTypeBuilder<CarItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasMaxLength(64);
    }
}
