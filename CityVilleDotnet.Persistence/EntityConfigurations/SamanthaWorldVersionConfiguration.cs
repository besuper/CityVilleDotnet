using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class SamanthaWorldVersionConfiguration: IEntityTypeConfiguration<SamanthaWorldVersion>
{
    public void Configure(EntityTypeBuilder<SamanthaWorldVersion> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        
        builder.Property(x => x.UpdatedAt);
    }
}
