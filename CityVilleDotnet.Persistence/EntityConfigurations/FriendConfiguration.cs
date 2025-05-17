using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class FriendConfiguration : IEntityTypeConfiguration<Friend>
{
    public void Configure(EntityTypeBuilder<Friend> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasOne(x => x.User).WithMany(x => x.Friends).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.FriendUser).WithMany().OnDelete(DeleteBehavior.Restrict);
    }
}
