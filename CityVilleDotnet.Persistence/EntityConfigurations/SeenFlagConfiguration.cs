using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class SeenFlagConfiguration : IEntityTypeConfiguration<SeenFlag>
{
    public void Configure(EntityTypeBuilder<SeenFlag> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Key).HasMaxLength(32);
    }
}