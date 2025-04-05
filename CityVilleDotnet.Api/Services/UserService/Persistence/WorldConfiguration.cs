using CityVilleDotnet.Api.Services.UserService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Api.Services.UserService.Persistence;

public class WorldConfiguration : IEntityTypeConfiguration<World>
{
    public void Configure(EntityTypeBuilder<World> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.SizeX);
        builder.Property(x => x.SizeY);

        builder.HasMany(x => x.MapRects);
        builder.HasMany(x => x.Objects);

        builder.OwnsOne(x => x.CitySim);
    }
}
