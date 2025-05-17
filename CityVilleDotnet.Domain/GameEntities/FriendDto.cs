using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class FriendDto
{
    public string UserName { get; set; }
    public bool Requested { get; set; }
    public int Level { get; set; }
    public FriendshipStatus Status { get; set; }
}

public static class FriendDtoMapper
{
    public static FriendDto ToDto(this Friend model)
    {
        return new FriendDto()
        {
            UserName = model.FriendUser.UserInfo.Username,
            Level = model.FriendUser.UserInfo.Player.Level,
            Status = model.Status,
            Requested = model.Requested
        };
    }
}
