using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.GameEntities;

namespace CityVilleDotnet.Api.Services.UserService;

internal sealed class InitNeighbors(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .Include(x => x.Friends.Where(x => x.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user is null)
            throw new Exception($"User {userId} not found");

        var neighborList = user.Friends.Select(friend => friend.ToNeighborDto()).ToList();

        neighborList.Add(new NeighborDto() // Samantha
        {
            Uid = "-1",
            Fake = 1,
            Level = 5 // FriendBarSlot::updateSlot
        });

        var response = new ASObject
        {
            ["neighbors"] = AmfConverter.Convert(neighborList)
        };

        return new CityVilleResponse(333, response);
    }
}