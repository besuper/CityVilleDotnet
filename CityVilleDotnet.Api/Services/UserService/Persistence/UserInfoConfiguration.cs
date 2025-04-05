using CityVilleDotnet.Api.Services.UserService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVilleDotnet.Api.Services.UserService.Persistence;

public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.FirstDay);
        builder.Property(x => x.WorldName);
        builder.Property(x => x.IsNew);
        builder.Property(x => x.CreationTimestamp);
        builder.Property(x => x.Username);

        builder.HasOne(x => x.Player);
        builder.HasOne(x => x.World);
    }
}
