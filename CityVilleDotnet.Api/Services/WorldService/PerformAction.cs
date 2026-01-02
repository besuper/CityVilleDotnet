using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction(CityVilleDbContext context, ILogger<PerformAction> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        // FIXME: Optimize this
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Player)
            .ThenInclude(x => x!.SeenFlags)
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        if (user.Player is null) throw new Exception("Player not found for user");

        var actionType = @params[0] as string;

        logger.LogDebug("PerformAction type {ActionType}", actionType);

        if (actionType == "place")
        {
            return await PerformPlace(user, @params, userId, cancellationToken);
        }

        if (actionType == "sell")
        {
            await PerformSell(user, @params, userId, cancellationToken);

            return GatewayService.CreateEmptyResponse();
        }

        if (actionType == "build")
        {
            await PerformBuild(user, @params, userId, cancellationToken);

            return new CityVilleResponse().MetaData(CreateQuestComponentResponse(user));
        }

        if (actionType == "finish")
        {
            return await PerformFinish(user, @params, userId, cancellationToken);
        }

        if (actionType == "openBusiness")
        {
            var response = await PerformOpenBusiness(user, @params, userId, cancellationToken);

            return response ?? GatewayService.CreateEmptyResponse();
        }

        if (actionType == "harvest")
        {
            return await PerformHarvest(user, @params, userId, cancellationToken);
        }

        if (actionType == "startContract")
        {
            await PerformStartContract(user, @params, userId, cancellationToken);

            return new CityVilleResponse().MetaData(CreateQuestComponentResponse(user));
        }

        if (actionType == "clear")
        {
            return await PerformClear(user, @params, userId, cancellationToken);
        }

        return GatewayService.CreateEmptyResponse();
    }
    
    public static ASObject CreateQuestComponentResponse(User user)
    {
        var quests = new ASObject
        {
            ["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active).Select(x => x.ToDto()))
        };

        return quests;
    }
}