using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FranchiseService;

public sealed class OnCollect(CityVilleDbContext context) : AmfService<OnCollectRequest>
{
    public override async Task<ASObject> HandlePacket(OnCollectRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Franchises.Where(f => f.FranchiseType == request.FranchiseType))
            .ThenInclude(x => x.Locations)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var franchise = player.Franchises.FirstOrDefault();
        if (franchise is null) throw new Exception($"Can't find franchise {request.FranchiseType}");

        var location = franchise.Locations.FirstOrDefault(l => l.Uid == request.NeighborUid);
        if (location is null) throw new Exception($"Can't find franchise location for neighbor {request.NeighborUid}");
        
        player.AddCoins(location.MoneyCollected);

        location.MoneyCollected = 0;
        location.TimeLastCollected = ServerUtils.GetCurrentTimeSeconds();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class OnCollectRequest
{
    [AmfParam(0)] public string FranchiseType { get; set; } = string.Empty;
    [AmfParam(1)] public string NeighborUid { get; set; } = string.Empty;
}