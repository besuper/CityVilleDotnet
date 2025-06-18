using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class MapRectConfiguration : IEntityTypeConfiguration<MapRect>
{
    public void Configure(EntityTypeBuilder<MapRect> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.X).IsRequired();
        builder.Property(x => x.Y).IsRequired();
        builder.Property(x => x.Width).IsRequired();
        builder.Property(x => x.Height).IsRequired();
    }
}