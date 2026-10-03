using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class SocialNetworkUserDto
{
    [JsonPropertyName("zid")] public int Zid { get; set; }

    [JsonPropertyName("snuid")] public int Snuid { get; set; }

    [JsonPropertyName("snid")] public int Snid { get; set; }

    [JsonPropertyName("name")] public required string Name { get; set; }

    [JsonPropertyName("first_name")] public required string FirstName { get; set; }

    [JsonPropertyName("pic_square")] public required string Picture { get; set; }

    [JsonPropertyName("sex")] public required string Gender { get; set; }

    [JsonPropertyName("locale")] public required string Locale { get; set; }
}

public static class SocialNetworkUserDtoMapper
{
    public static SocialNetworkUserDto ToSocialNetworkUserDto(this Friend model, string baseUrl)
    {
        var friend = model.GetFriend();

        return new SocialNetworkUserDto
        {
            Zid = friend.Snuid,
            Snuid = friend.Snuid,
            Snid = friend.Snuid,
            FirstName = friend.Username,
            Name = friend.Username,
            Picture = friend.ProfilePictureUrl is not null ? $"{baseUrl}{friend.ProfilePictureUrl}" : $"{baseUrl}/blank.png",
            Gender = "M",
            Locale = "EN"
        };
    }
}