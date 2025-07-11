using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
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
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Player)
            .ThenInclude(x => x!.SeenFlags)
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        var actionType = @params[0] as string;

        logger.LogInformation("PerformAction type {ActionType}", actionType);

        if (actionType == "place")
        {
            await PerformPlace(user, @params, userId, cancellationToken);

            return GatewayService.CreateEmptyResponse();
        }

        if (actionType == "sell")
        {
            await PerformSell(user, @params, userId, cancellationToken);

            return GatewayService.CreateEmptyResponse();
        }

        if (actionType == "build")
        {
            await PerformBuild(user, @params, userId, cancellationToken);

            return GatewayService.CreateEmptyResponse();
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

            return GatewayService.CreateEmptyResponse();
        }

        if (actionType == "clear")
        {
            return await PerformClear(user, @params, userId, cancellationToken);
        }

        return GatewayService.CreateEmptyResponse();
    }
}