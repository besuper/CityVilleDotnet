using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FranchiseService;

public sealed class OnSupply(CityVilleDbContext context) : AmfService<OnSupplyRequest>
{
    public override async Task<ASObject> HandlePacket(OnSupplyRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var user = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Franchises.Where(f => f.FranchiseType == request.FranchiseType))
            .ThenInclude(x => x.Locations)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (user is null) throw new Exception("Player not found");

        var franchise = user.Franchises.FirstOrDefault();
        if (franchise is null) throw new Exception($"Can't find franchise {request.FranchiseType}");

        var location = franchise.Locations.FirstOrDefault(l => l.Uid == request.NeighborUid);
        if (location is null) throw new Exception($"Can't find franchise location for neighbor {request.NeighborUid}");

        if (location.TimeLastOperated <= 0)
            throw new Exception("Franchise location has not been opened yet");

        if (location.TimeLastSupplied >= location.TimeLastOperated)
            throw new Exception("Franchise location has already been supplied since last open");

        var gameItem = GameSettingsManager.Instance.GetItem(request.FranchiseType);
        if (gameItem is null) throw new Exception($"Can't find game item {request.FranchiseType}");

        var commodityCost = (gameItem.CommodityRequired ?? 0) / 2;

        if (commodityCost > 0)
        {
            user.RemoveGoods(commodityCost);
        }

        // FIXME: Move MoneyCollected to harvest in receiver city with the money harvested
        location.TimeLastSupplied = ServerUtils.GetCurrentTimeSeconds();
        location.MoneyCollected = 100; // MoneyCollected is a server side var, but I don't know how it is calculated

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            // TODO: Implement star level up
            { "star_rating", location.StarRating }
        });
    }
}

public class OnSupplyRequest
{
    [AmfParam(0)] public string FranchiseType { get; set; } = string.Empty;
    [AmfParam(1)] public string NeighborUid { get; set; } = string.Empty;
}