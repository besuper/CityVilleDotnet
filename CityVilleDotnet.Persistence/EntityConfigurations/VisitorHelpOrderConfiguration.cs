using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class VisitorHelpOrderConfiguration : IEntityTypeConfiguration<VisitorHelpOrder>
{
    public void Configure(EntityTypeBuilder<VisitorHelpOrder> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasIndex(x => x.SenderId);
        builder.HasIndex(x => x.RecipientId);
        builder.HasIndex(x => new { x.OrderState, x.TransmissionStatus });
    }
}
