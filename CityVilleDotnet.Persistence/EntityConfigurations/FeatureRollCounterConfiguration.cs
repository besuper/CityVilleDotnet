using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class FeatureRollCounterConfiguration : IEntityTypeConfiguration<FeatureRollCounter>
{
    public void Configure(EntityTypeBuilder<FeatureRollCounter> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Feature).HasMaxLength(64);
    }
}
