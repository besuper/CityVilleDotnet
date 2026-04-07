using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class UpdateWorldSummary(CityVilleDbContext context) : AmfService<UpdateWorldSummaryRequest>
{
    public override async Task<ASObject> HandlePacket(UpdateWorldSummaryRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>().FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        player.SwitchWorld(request.Type);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class UpdateWorldSummaryRequest
{
    [AmfParam(0)] public WorldType Type { get; set; }
}