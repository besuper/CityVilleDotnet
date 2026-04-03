using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using CityVilleDotnet.Domain.GameEntities;

namespace CityVilleDotnet.Api.Services.UserService;

internal sealed class InitNeighbors(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsNoTracking()
            .Include(x => x.Friends.Where(f => f.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .Include(x => x.Player)
            .Include(x => x.Friends.Where(f => f.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x.World)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null)
            throw new Exception($"User {userId} not found");

        var neighborList = user.Friends
            .Where(f => !f.FriendUser.Player!.IsSamantha())
            .Select(friend => friend.ToNeighborDto()).ToList();

        neighborList.Add(new NeighborDto() // Samantha
        {
            Uid = "-1",
            Fake = 1,
            Level = user.Player.Level + 1, // FriendBarSlot::updateSlot
            Xp = user.Player.Xp + 10
        });

        return new CityVilleResponse().Data(new ASObject
        {
            ["neighbors"] = AmfConverter.Convert(neighborList.OrderByDescending(x => x.Xp).ToList()),
            ["neighborMax"] = 10,
        });
    }
}