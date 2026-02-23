using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class MasteryConfiguration : IEntityTypeConfiguration<Mastery>
{
    public void Configure(EntityTypeBuilder<Mastery> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.ItemName).HasMaxLength(64);
    }
}