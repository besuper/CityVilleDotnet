using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class LotOrderConfiguration : IEntityTypeConfiguration<LotOrder>
{
    public void Configure(EntityTypeBuilder<LotOrder> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.ResourceType).HasMaxLength(64);
        builder.Property(x => x.OrderResourceName).HasMaxLength(64);
    }
}
