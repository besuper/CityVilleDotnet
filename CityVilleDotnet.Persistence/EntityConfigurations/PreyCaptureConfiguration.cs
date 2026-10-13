using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class PreyCaptureConfiguration : IEntityTypeConfiguration<PreyCapture>
{
    public void Configure(EntityTypeBuilder<PreyCapture> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.PreyId);
        builder.Property(x => x.Count);

        builder.HasIndex("PreyGroupId", nameof(PreyCapture.PreyId)).IsUnique();
    }
}
