using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using CityVilleDotnet.Domain.GameEntities;

namespace CityVilleDotnet.Api.Services.UserService;

internal sealed class InitNeighbors(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsNoTracking()
            .Include(x => x.Friends.Where(f => f.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendPlayer)
            .ThenInclude(x => x.Worlds.Where(w => w.Type == WorldType.Main))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null)
            throw new Exception("Player not found");

        var neighborList = player.Friends
            .Where(f => !f.FriendPlayer.IsSamantha())
            .Select(friend => friend.ToNeighborDto()).ToList();

        neighborList.Add(new NeighborDto() // Samantha
        {
            Uid = "-1",
            Fake = 1,
            Level = player.Level + 1, // FriendBarSlot::updateSlot
            Xp = player.Xp + 10
        });

        return new CityVilleResponse().Data(new ASObject
        {
            ["neighbors"] = AmfConverter.Convert(neighborList.OrderByDescending(x => x.Xp).ToList()),
            ["neighborMax"] = 10,
        });
    }
}