using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction : AmfService
{
    private readonly CityVilleDbContext _context;
    private readonly ILogger<PerformAction> _logger;

    public PerformAction(CityVilleDbContext context, ILogger<PerformAction> logger) : base(context)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await _context.Set<User>()
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.World)
            .ThenInclude(x => x.Objects)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.World)
            .ThenInclude(x => x.CitySim)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x.Commodities)
            .ThenInclude(x => x.Storage)
            .Include(x => x.Quests)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new Exception("User can't be null");
        }

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

        return GatewayService.CreateEmptyResponse();
    }
}
