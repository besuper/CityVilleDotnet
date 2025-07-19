using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
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

        // TODO: Find out how to use helpParams, and send the help

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

        var currentUser = await context.Set<User>()
            .Include(x => x.Player)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (currentUser?.Player is null)
            throw new Exception($"Can't find user with userId {userId}");

        currentUser.Player.AddCoins(coins);
        currentUser.Player.AddGoods(goods);
        currentUser.Player.AddSocialXp(reputation);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}