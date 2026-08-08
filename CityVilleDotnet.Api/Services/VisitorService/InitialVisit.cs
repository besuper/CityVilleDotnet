using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.VisitorService;

public class InitialVisit(CityVilleDbContext context) : AmfService<InitialVisitRequest>
{
    public override async Task<ASObject> HandlePacket(InitialVisitRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        if (request.Type != "neighborVisit") throw new Exception("Invalid type");

        var currentUser = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.VisitorHelpOrders)
            .Include(x => x.Friends.Where(f => f.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendPlayer)
            .ThenInclude(x => x!.VisitorHelpOrders)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.InventoryItems)
            .Include(x => x.SeenFlags)
            .Include(x => x.Franchises)
            .ThenInclude(x => x.Locations)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (currentUser is null) throw new Exception("Can't find user with UserId");

        var recipientId = Convert.ToInt32(request.Content.RecipientId);
        var targetFriend = currentUser.Friends.FirstOrDefault(x => x.FriendPlayer.Snuid == recipientId);

        if (targetFriend?.FriendPlayer is null) throw new Exception("Can't find friend with recipientId");

        // TODO: Implement rewards system (https://cityville.fandom.com/wiki/Neighbors)
        var currentTimestamp = ServerUtils.GetCurrentTime();

        if (currentTimestamp - targetFriend.LastEnergyLeftReset >= 23 * 60 * 60 * 1000L)
        {
            targetFriend.EnergyLeft = 5;
            targetFriend.LastEnergyLeftReset = currentTimestamp;

            // Clean all orders from the previous friendship help batch even if its pending/unclaimed
            var sentOrders = currentUser.VisitorHelpOrders.Where(x => x.RecipientId == request.Content.RecipientId && x.SenderId == request.Content.SenderId).ToList();
            var receivedOrders = targetFriend.FriendPlayer.VisitorHelpOrders.Where(x => x.RecipientId == request.Content.RecipientId && x.SenderId == request.Content.SenderId).ToList();

            context.RemoveRange(sentOrders);
            context.RemoveRange(receivedOrders);
        }
        
        currentUser.HandleQuestsProgress("neighborVisit", recipientId == -1 ? "-1" : "");

        var response = new ASObject
        {
            ["energyLeft"] = targetFriend.EnergyLeft,
        };

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(response);
    }
}

public class InitialVisitRequest
{
    [AmfParam(0)] public string Type { get; set; } = string.Empty;
    [AmfParam(1)] public InitialVisitContent Content { get; set; } = new();
}

public class InitialVisitContent
{
    [AmfParam("recipientId")] public string RecipientId { get; set; } = string.Empty;
    [AmfParam("senderId")] public string SenderId { get; set; } = string.Empty;
}