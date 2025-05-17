using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.UserService;

internal sealed class InitNeighbors(CityVilleDbContext context) : AmfService
{
    // _loc6_.uid,_loc6_.gold,_loc6_.xp,_loc6_.level,null,_loc6_.cityname,_loc8_.snUser.picture,_loc8_.snUser.firstName,_loc9_,_loc6_.socialLevel,false,false
    public class NeighborResponse
    {
        [JsonPropertyName("neighbors")]
        public List<Neighbor> Neighbors { get; set; }
    }

    public class Neighbor
    {
        [JsonPropertyName("uid")]
        public int Uid { get; set; }

        [JsonPropertyName("fake")]
        public int? Fake { get; set; }

        [JsonPropertyName("level")]
        public int? Level { get; set; }
    }

    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .Include(x => x.Friends.Where(x => x.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        var response = new NeighborResponse()
        {
            Neighbors = new List<Neighbor>()
            {
                new Neighbor() // Samantha
                {
                    Uid = -1,
                    Fake = 1,
                    Level = 5 // FriendBarSlot::updateSlot
                }
            }
        };

        foreach (var friend in user.Friends)
        {
            response.Neighbors.Add(new Neighbor()
            {
                Uid = friend.FriendUser.Player.Uid,
                Level = friend.FriendUser.Player.Level,
            });
        }

        var obj = AmfConverter.Convert(response);

        return new CityVilleResponse(333, obj);
    }
}
