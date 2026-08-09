using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class TrainOrderConfiguration : IEntityTypeConfiguration<TrainOrder>
{
    public void Configure(EntityTypeBuilder<TrainOrder> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.ItemName).HasMaxLength(64).IsRequired();
        builder.Property(x => x.CommodityName).HasMaxLength(32).IsRequired();
        builder.Property(x => x.Operation);
        builder.Property(x => x.TimeSent);

        builder.HasMany(x => x.Workers).WithOne().IsRequired();
    }
}
