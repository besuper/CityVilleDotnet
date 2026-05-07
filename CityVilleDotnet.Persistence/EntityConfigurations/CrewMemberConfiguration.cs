using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class CrewMemberConfiguration : IEntityTypeConfiguration<CrewMember>
{
    public void Configure(EntityTypeBuilder<CrewMember> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.Player).WithMany().OnDelete(DeleteBehavior.Restrict);
    }
}