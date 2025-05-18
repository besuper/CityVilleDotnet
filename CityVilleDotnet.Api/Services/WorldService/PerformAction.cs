using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction(CityVilleDbContext _context, ILogger<PerformAction> _logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await _context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x.Objects)
            .Include(x => x.Player)
            .ThenInclude(x => x.Commodities)
            .ThenInclude(x => x.Storage)
            .Include(x => x.Player)
            .ThenInclude(x => x.Inventory)
            .ThenInclude(x => x.Items)
            .Include(x => x.Quests)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        var actionType = _params[0] as string;

        _logger.LogInformation($"PerformAction type {actionType}");

        if (actionType == "place")
        {
            await PerformPlace(user, _params, userId, cancellationToken);

            return GatewayService.CreateEmptyResponse();
        }

        if (actionType == "build")
        {
            await PerformBuild(user, _params, userId, cancellationToken);

            return GatewayService.CreateEmptyResponse();
        }

        if (actionType == "finish")
        {
            return await PerformFinish(user, _params, userId, cancellationToken);
        }

        if (actionType == "openBusiness")
        {
            var response = await PerformOpenBusiness(user, _params, userId, cancellationToken);

            if (response is not null)
            {
                return response;
            }

            return GatewayService.CreateEmptyResponse();
        }

        if (actionType == "harvest")
        {
            return await PerformHarvest(user, _params, userId, cancellationToken);
        }

        if (actionType == "startContract")
        {
            await PerformStartContract(user, _params, userId, cancellationToken);

            return GatewayService.CreateEmptyResponse();
        }

        if (actionType == "clear")
        {
            return await PerformClear(user, _params, userId, cancellationToken);
        }

        return GatewayService.CreateEmptyResponse();
    }
}
