using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class SocialPostConfiguration : IEntityTypeConfiguration<SocialPost>
{
    public void Configure(EntityTypeBuilder<SocialPost> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Key).HasMaxLength(128);
        builder.Property(x => x.UserMessage).HasMaxLength(256);

        builder.HasOne(x => x.Player).WithMany(x => x.SocialPosts).HasForeignKey("PlayerId").IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.TargetPlayer).WithMany().HasForeignKey("TargetPlayerId").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Helpers).WithOne().HasForeignKey("SocialPostId").IsRequired().OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ExpiresAt);
        builder.HasIndex("PlayerId", nameof(SocialPost.CreatedAt));
    }
}
