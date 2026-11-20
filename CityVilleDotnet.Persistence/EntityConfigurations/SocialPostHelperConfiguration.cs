using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class SocialPostHelperConfiguration : IEntityTypeConfiguration<SocialPostHelper>
{
    public void Configure(EntityTypeBuilder<SocialPostHelper> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.Helper).WithMany().HasForeignKey("HelperId").IsRequired().OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex("SocialPostId", "HelperId").IsUnique();
        builder.HasIndex(x => x.HelpedAt);
    }
}
