using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class NeighborDto
{
    // _loc6_.uid,_loc6_.gold,_loc6_.xp,_loc6_.level,null,_loc6_.cityname,_loc8_.snUser.picture,_loc8_.snUser.firstName,_loc9_,_loc6_.socialLevel,false,false
    
    [JsonPropertyName("uid")] public string Uid { get; set; }

    [JsonPropertyName("fake")] public int? Fake { get; set; }

    [JsonPropertyName("level")] public int Level { get; set; }
    [JsonPropertyName("zid")] public int Zid { get; set; }
    [JsonPropertyName("snuid")] public int Snuid { get; set; }
    [JsonPropertyName("snid")] public int Snid { get; set; }
}

public static class NeighborDtoMapper
{
    public static NeighborDto ToNeighborDto(this Friend model)
    {
        return new NeighborDto()
        {
            Uid = model.FriendUser.Player.Uid,
            Zid = model.FriendUser.Player.Snuid,
            Snuid = model.FriendUser.Player.Snuid,
            Snid = model.FriendUser.Player.Snuid,
            Level = model.FriendUser.Player.Level,
        };
    }
}