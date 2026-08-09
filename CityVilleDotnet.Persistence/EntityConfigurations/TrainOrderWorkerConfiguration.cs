using CityVilleDotnet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Persistence.EntityConfigurations;

public class TrainOrderWorkerConfiguration : IEntityTypeConfiguration<TrainOrderWorker>
{
    public void Configure(EntityTypeBuilder<TrainOrderWorker> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Zid);
    }
}
