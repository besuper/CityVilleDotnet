using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class UserDto
{
    [JsonPropertyName("userInfo")] public required UserInfoDto UserInfo { get; set; }
    [JsonPropertyName("franchises")] public List<FranchiseDto> Franchises { get; set; } = new();
}

public static class UserDtoMapper
{
    public static UserDto ToDto(this User model)
    {
        var player = model.Player?.ToDto();

        player.Neighbors = model.Friends.Select(x => x.ToNeighborDto()).ToList();

        return new UserDto()
        {
            UserInfo = new UserInfoDto
            {
                CreationTimestamp = model.Player.CreationTimestamp,
                FirstDay = model.Player.FirstDay,
                IsNew = model.Player.IsNew,
                Player = player,
                World = model.World?.ToDto(),
                Username = model.Player.Username,
                WorldName = model.World.WorldName,
            },
            Franchises = model.Player.Franchises.Select(x => x.ToDto()).ToList(),
        };
    }
}