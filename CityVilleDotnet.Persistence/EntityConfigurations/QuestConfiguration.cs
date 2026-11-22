using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class QuestConfiguration : IEntityTypeConfiguration<Quest>
{
    public void Configure(EntityTypeBuilder<Quest> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.Property(x => x.Name).HasMaxLength(64);
        builder.PrimitiveCollection(x => x.Progress).HasMaxLength(512);
        builder.PrimitiveCollection(x => x.Purchased).HasMaxLength(512);

        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.QuestType);
    }
}
