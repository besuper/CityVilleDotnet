using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class UserDto
{
    [JsonPropertyName("userInfo")]
    public UserInfoDto? UserInfo { get; set; }
}

public static class UserDtoMapper
{
    public static UserDto ToDto(this User model)
    {
        return new UserDto()
        {
            UserInfo = new UserInfoDto
            {
                CreationTimestamp = model.Player.CreationTimestamp,
                FirstDay = model.Player.FirstDay,
                IsNew = model.Player.IsNew,
                Player = model.Player?.ToDto(),
                World = model.World?.ToDto(),
                Username = model.Player.Username,
                WorldName = model.World.WorldName
            }
        };
    }
}
