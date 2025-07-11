using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Uid).HasMaxLength(64);
        builder.Property(x => x.Snuid).ValueGeneratedOnAdd();
        builder.Property(x => x.LastTrackingTimestamp);
        builder.Property(x => x.Gold);
        builder.Property(x => x.Cash);
        builder.Property(x => x.Level);
        builder.Property(x => x.Xp);
        builder.Property(x => x.Energy);
        builder.Property(x => x.EnergyMax);
        builder.Property(x => x.ExpansionsPurchased);
        builder.Property(x => x.RollCounter);
        builder.Property(x => x.Username).HasMaxLength(32);

        // TODO: Implement these
        builder.Ignore(x => x.PlayerNews);
        builder.Ignore(x => x.Wishlist);
        builder.Ignore(x => x.Licenses);

        builder.HasMany(x => x.InventoryItems).WithOne();
        builder.HasMany(x => x.SeenFlags);
        builder.HasMany(x => x.Collections)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}