using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.VisitorService;

public class InitialVisit(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var type = (string)@params[0]; // neighborVisit
        var content = (ASObject)@params[1]; // { recipientId, senderId }

        if (type != "neighborVisit") throw new Exception("Invalid type");

        var recipientId = Convert.ToInt32(content["recipientId"]);

        var currentUser = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.VisitorHelpOrders)
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x!.VisitorHelpOrders)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (currentUser?.Player is null) throw new Exception("Can't find user with UserId");

        var targetFriend = currentUser.Friends.FirstOrDefault(x => x.FriendUser.Player!.Snuid == recipientId);

        if (targetFriend?.FriendUser.Player is null) throw new Exception("Can't find friend with recipientId");

        // TODO: Implement rewards system (https://cityville.fandom.com/wiki/Neighbors)
        var currentTimestamp = ServerUtils.GetCurrentTime();

        if (currentTimestamp - targetFriend.LastEnergyLeftReset >= 23 * 60 * 60 * 1000L)
        {
            targetFriend.EnergyLeft = 5;
            targetFriend.LastEnergyLeftReset = currentTimestamp;
            
            // Clean all orders from the previous friendship help batch even if its pending/unclaimed
            var sentOrders = currentUser.Player.VisitorHelpOrders.Where(x => x.RecipientId == (string)content["recipientId"] && x.SenderId == (string)content["senderId"]).ToList();
            var receivedOrders = targetFriend.FriendUser.Player.VisitorHelpOrders.Where(x => x.RecipientId == (string)content["senderId"] && x.SenderId == (string)content["recipientId"]).ToList();
            
            context.RemoveRange(sentOrders);
            context.RemoveRange(receivedOrders);
        }

        var response = new ASObject
        {
            ["energyLeft"] = targetFriend.EnergyLeft,
        };

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(response);
    }
}