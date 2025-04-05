using CityVilleDotnet.Api.Services.UserService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Api.Services.UserService.Persistence;

public class WorldObjectConfiguration : IEntityTypeConfiguration<WorldObject>
{
    public void Configure(EntityTypeBuilder<WorldObject> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(x => x.Position);
    }
}
