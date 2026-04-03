using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.VisitorService;

public class Help(CityVilleDbContext context, ILogger<Help> logger) : AmfService<HelpRequest>
{
    public override async Task<ASObject> HandlePacket(HelpRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        logger.LogDebug("Received visitor help from {UserId}: {RequestName} {RequestType}", playerId, request.Name, request.Type);

        // TODO: Improve this query
        var currentUser = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.VisitorHelpOrders)
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x!.VisitorHelpOrders)
            .Include(x => x.Player)
            .ThenInclude(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => request.HelpParams.HelpTargets.Contains(o.WorldFlatId)))
            .FirstOrDefaultAsync(x => x.Player.Id == playerId, cancellationToken);

        if (currentUser?.Player is null)
            throw new Exception("Player not found");

        var reputation = 0;
        var coins = 0;
        var goods = 0;

        switch (request.Type)
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
                throw new Exception($"Not implemented help type {request.Type}");
        }

        var targetFriend = currentUser.Friends.FirstOrDefault(x => x.FriendUser.Player!.Snuid == Convert.ToInt32(request.HelpParams.RecipientId));

        if (targetFriend?.FriendUser.Player is null) throw new Exception($"Can't find friend with recipientId {request.HelpParams.RecipientId}");
        if (targetFriend.EnergyLeft <= 0) return GatewayService.CreateEmptyResponse();

        currentUser.GetPlayer().HandleQuestsProgress("visitorHelp", request.Type);

        var world = targetFriend.FriendUser.GetPlayer().GetWorld();

        if (request.Type == "businessSendTour")
        {
            foreach (var targetId in request.HelpParams.HelpTargets)
            {
                var obj = world.GetBuildingById(targetId);

                if (obj is null)
                {
                    logger.LogError("Can't send visitor help for {HelpParamsRecipientId} on building ID {TargetId}", request.HelpParams.RecipientId, targetId);
                    return new CityVilleResponse().Error(GameErrorType.InvalidData);
                }

                currentUser.GetPlayer().HandleQuestsProgress("sendTourNeighborBusinessByName", obj.ItemName, obj.ItemName);
            }
        }

        currentUser.Player.AddCoins(coins);
        currentUser.Player.AddGoods(goods);
        currentUser.Player.AddSocialXp(reputation);

        targetFriend.EnergyLeft -= 1;

        var newOrder = false;

        // Create batch visitor help order
        // Order reset with energy reset or if the batch is accepted
        var senderHelpOrder = currentUser.Player.VisitorHelpOrders.FirstOrDefault(x =>
            x.TransmissionStatus == TransmissionStatus.Sent &&
            x.OrderState == OrderState.Pending &&
            x.Status == VisitorHelpStatus.Unclaimed &&
            x.SenderId == request.HelpParams.SenderId &&
            x.RecipientId == request.HelpParams.RecipientId);

        if (senderHelpOrder is null)
        {
            senderHelpOrder = new VisitorHelpOrder
            {
                SenderId = request.HelpParams.SenderId,
                RecipientId = request.HelpParams.RecipientId,
                Status = VisitorHelpStatus.Unclaimed,
                OrderState = OrderState.Pending,
                OrderType = OrderType.VisitorHelp,
                TransmissionStatus = TransmissionStatus.Sent,
                TimeSent = request.HelpParams.TimeSent,
                HelpTargets = request.HelpParams.HelpTargets
            };

            newOrder = true;
        }
        else
        {
            senderHelpOrder.HelpTargets = senderHelpOrder.HelpTargets.Concat(request.HelpParams.HelpTargets).ToArray();
        }

        var receiveHelpOrder = targetFriend.FriendUser.Player.VisitorHelpOrders.FirstOrDefault(x =>
            x.TransmissionStatus == TransmissionStatus.Received &&
            x.OrderState == OrderState.Pending &&
            x.Status == VisitorHelpStatus.Unclaimed &&
            x.SenderId == request.HelpParams.SenderId &&
            x.RecipientId == request.HelpParams.RecipientId);

        if (receiveHelpOrder is null)
        {
            receiveHelpOrder = new VisitorHelpOrder
            {
                SenderId = request.HelpParams.SenderId,
                RecipientId = request.HelpParams.RecipientId,
                Status = VisitorHelpStatus.Unclaimed,
                OrderState = OrderState.Pending,
                OrderType = OrderType.VisitorHelp,
                TransmissionStatus = TransmissionStatus.Received,
                TimeSent = request.HelpParams.TimeSent,
                HelpTargets = request.HelpParams.HelpTargets
            };

            newOrder = true;
        }
        else
        {
            receiveHelpOrder.HelpTargets = receiveHelpOrder.HelpTargets.Concat(request.HelpParams.HelpTargets).ToArray();
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

public class HelpRequest
{
    [AmfParam(0)] public string Name { get; set; } = string.Empty;
    [AmfParam(1)] public string Type { get; set; } = string.Empty;
    [AmfParam(2)] public HelpParamsRequest HelpParams { get; set; } = new();
}

public class HelpParamsRequest
{
    [AmfParam("senderID")] public string SenderId { get; set; } = string.Empty;
    [AmfParam("recipientID")] public string RecipientId { get; set; } = string.Empty;
    [AmfParam("helpTargets")] public int[] HelpTargets { get; set; } = [];
    [AmfParam("timeSent")] public long TimeSent { get; set; }
}