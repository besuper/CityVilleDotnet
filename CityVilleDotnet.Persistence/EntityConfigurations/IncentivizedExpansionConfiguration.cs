using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class IncentivizedExpansionConfiguration : IEntityTypeConfiguration<IncentivizedExpansion>
{
    public void Configure(EntityTypeBuilder<IncentivizedExpansion> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.ExpansionId).IsRequired().HasMaxLength(64);
        builder.Property(x => x.X);
        builder.Property(x => x.Y);
        builder.Property(x => x.StartTimestamp);
        builder.Property(x => x.IsCompleted);
        builder.Property(x => x.FailureCount);
    }
}
