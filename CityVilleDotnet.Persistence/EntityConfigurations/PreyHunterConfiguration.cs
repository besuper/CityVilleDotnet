using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class PreyHunterConfiguration : IEntityTypeConfiguration<PreyHunter>
{
    public void Configure(EntityTypeBuilder<PreyHunter> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Position);
        builder.Property(x => x.State);
        builder.Property(x => x.Timestamp);

        builder.HasIndex("PreyGroupId", nameof(PreyHunter.Position)).IsUnique();
    }
}
