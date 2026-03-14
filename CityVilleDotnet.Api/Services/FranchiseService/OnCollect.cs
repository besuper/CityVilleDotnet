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
    public override async Task<ASObject> HandlePacket(OnCollectRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.Franchises.Where(f => f.FranchiseType == request.FranchiseType))
            .ThenInclude(x => x.Locations)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null) throw new Exception($"User not found with id {userId}");

        var franchise = user.Player.Franchises.FirstOrDefault();
        if (franchise is null) throw new Exception($"Can't find franchise {request.FranchiseType}");

        var location = franchise.Locations.FirstOrDefault(l => l.Uid == request.NeighborUid);
        if (location is null) throw new Exception($"Can't find franchise location for neighbor {request.NeighborUid}");

        if (location.MoneyCollected <= 0)
            throw new Exception("No money to collect from this franchise location");

        user.Player.AddCoins(location.MoneyCollected);

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
