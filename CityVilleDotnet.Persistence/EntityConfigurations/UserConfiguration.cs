using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        // TODO: Implement franchises
        builder.Ignore(x => x.Franchises);

        builder.HasOne(x => x.Player);
        builder.HasOne(x => x.World);

        builder.HasOne(x => x.AppUser);
        builder.HasMany(x => x.Quests);
        builder.HasMany(x => x.Friends);
    }
}
