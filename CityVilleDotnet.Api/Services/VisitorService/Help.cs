using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Common.Extensions;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.VisitorService;

public class Help(CityVilleDbContext context, ILogger<Help> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var name = (string)@params[0]; // visitorHelp
        var type = (string)@params[1]; // m_type
        var helpParams = (ASObject)@params[2];

        logger.LogInformation($"Received visitor help from {userId}: {name} {type}");

        var recipientId = Convert.ToInt32(helpParams["recipientID"]);
        var helpTargets = helpParams.GetObjectArray("helpTargets");

        if (helpTargets is null) throw new Exception("Can't find help targets");
        
        var currentUser = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.VisitorHelpOrders)
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x!.VisitorHelpOrders)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (currentUser?.Player is null)
            throw new Exception($"Can't find user with userId {userId}");

        var reputation = 0;
        var coins = 0;
        var goods = 0;

        switch (type)
        {
            case "residenceCollectRent":
                reputation = GameSettingsManager.Instance.GetInt("FriendVisitResidenceRepGain");
                coins = GameSettingsManager.Instance.GetInt("FriendHelpDefaultCoinReward");
                break;
            case "wildernessClear":
                reputation = GameSettingsManager.Instance.GetInt("FriendVisitWildernessRepGain");
                coins = GameSettingsManager.Instance.GetInt("FriendHelpDefaultCoinReward");
                break;
            case "businessSendTour":
                reputation = GameSettingsManager.Instance.GetInt("FriendVisitBusinessRepGain");
                coins = GameSettingsManager.Instance.GetInt("FriendHelpDefaultCoinReward");
                break;
            case "plotHarvest":
                reputation = GameSettingsManager.Instance.GetInt("FriendVisitPlotRepGain");
                goods = GameSettingsManager.Instance.GetInt("FriendHelpDefaultGoodsReward");
                break;
            default:
                throw new Exception($"Not implemented help type {type}");
        }
        
        currentUser.HandleQuestsProgress("visitorHelp", type);

        var targetFriend = currentUser.Friends.FirstOrDefault(x => x.FriendUser.Player!.Snuid == recipientId);

        if (targetFriend?.FriendUser.Player is null) throw new Exception($"Can't find friend with recipientId {recipientId}");
        if (targetFriend.EnergyLeft <= 0) return GatewayService.CreateEmptyResponse();

        currentUser.Player.AddCoins(coins);
        currentUser.Player.AddGoods(goods);
        currentUser.Player.AddSocialXp(reputation);

        targetFriend.EnergyLeft -= 1;
        
        var intHelpTargets = helpTargets.Select(Convert.ToInt32).ToArray();
        var newOrder = false;

        // Create batch visitor help order
        // Order reset with energy reset or if the batch is accepted
        var senderHelpOrder = currentUser.Player.VisitorHelpOrders.FirstOrDefault(x =>
            x.TransmissionStatus == TransmissionStatus.Sent &&
            x.OrderState == OrderState.Pending &&
            x.Status == VisitorHelpStatus.Unclaimed &&
            x.SenderId == (string)helpParams["senderID"] &&
            x.RecipientId == (string)helpParams["recipientID"]);

        if (senderHelpOrder is null)
        {
            senderHelpOrder = new VisitorHelpOrder
            {
                SenderId = (string)helpParams["senderID"],
                RecipientId = (string)helpParams["recipientID"],
                Status = VisitorHelpStatus.Unclaimed,
                OrderState = OrderState.Pending,
                OrderType = OrderType.VisitorHelp,
                TransmissionStatus = TransmissionStatus.Sent,
                TimeSent = Convert.ToInt64(helpParams["timeSent"]),
                HelpTargets = helpTargets.Select(Convert.ToInt32).ToArray()
            };

            newOrder = true;
        }
        else
        {
            senderHelpOrder.HelpTargets = senderHelpOrder.HelpTargets.Concat(intHelpTargets).ToArray();
        }

        var receiveHelpOrder = targetFriend.FriendUser.Player.VisitorHelpOrders.FirstOrDefault(x =>
            x.TransmissionStatus == TransmissionStatus.Received &&
            x.OrderState == OrderState.Pending &&
            x.Status == VisitorHelpStatus.Unclaimed &&
            x.SenderId == (string)helpParams["senderID"] &&
            x.RecipientId == (string)helpParams["recipientID"]);

        if (receiveHelpOrder is null)
        {
            receiveHelpOrder = new VisitorHelpOrder
            {
                SenderId = (string)helpParams["senderID"],
                RecipientId = (string)helpParams["recipientID"],
                Status = VisitorHelpStatus.Unclaimed,
                OrderState = OrderState.Pending,
                OrderType = OrderType.VisitorHelp,
                TransmissionStatus = TransmissionStatus.Received,
                TimeSent = Convert.ToInt64(helpParams["timeSent"]),
                HelpTargets = helpTargets.Select(Convert.ToInt32).ToArray()
            };

            newOrder = true;
        }
        else
        {
            receiveHelpOrder.HelpTargets = receiveHelpOrder.HelpTargets.Concat(intHelpTargets).ToArray();
        }

        if (newOrder)
        {
            targetFriend.FriendUser.Player!.AddVisitorHelpOrder(receiveHelpOrder);
            currentUser.Player.AddVisitorHelpOrder(senderHelpOrder);
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}