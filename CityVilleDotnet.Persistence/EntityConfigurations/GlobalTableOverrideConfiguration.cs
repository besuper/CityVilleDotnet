using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class GlobalTableOverrideConfiguration : IEntityTypeConfiguration<GlobalTableOverride>
{
    public void Configure(EntityTypeBuilder<GlobalTableOverride> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Keyword).HasMaxLength(64);
        builder.Property(x => x.Table).HasMaxLength(128);
        builder.Property(x => x.Source).HasMaxLength(64);
    }
}
