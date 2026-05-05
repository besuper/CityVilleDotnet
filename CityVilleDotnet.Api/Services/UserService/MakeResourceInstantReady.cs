using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class MakeResourceInstantReady(CityVilleDbContext context, ILogger<MakeResourceInstantReady> logger) : AmfService<MakeResourceInstantReadyRequest>
{
    public override async Task<ASObject> HandlePacket(MakeResourceInstantReadyRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.WorldFlatId == request.BuildingId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();
        var obj = world.GetBuildingById(request.BuildingId);

        if (obj is null) throw new Exception("Building not found");

        if (obj.State != WorldObjectState.Planted)
            return new CityVilleResponse().Error(GameErrorType.InvalidState);

        var cost = obj.GetCostToMakeReady();

        logger.LogDebug("Bought instant finish for {BuildingClassType} cost {Cost}", obj.ClassName, cost);
        
        if (player.Cash < cost)
            return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

        player.RemoveCash(cost);
        obj.SetReadyToHarvest();

        await context.SaveChangesAsync(cancellationToken);
        return GatewayService.CreateEmptyResponse();
    }
}

public class MakeResourceInstantReadyRequest
{
    [AmfParam(0)] public int BuildingId { get; set; }
}