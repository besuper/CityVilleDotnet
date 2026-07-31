using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class FranchiseLocationConfiguration : IEntityTypeConfiguration<FranchiseLocation>
{
    public void Configure(EntityTypeBuilder<FranchiseLocation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.FranchiseName).HasMaxLength(64);
        builder.Property(x => x.ObjectId).HasMaxLength(64);
        builder.Property(x => x.Uid).HasMaxLength(64);
    }
}