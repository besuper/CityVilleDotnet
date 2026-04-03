using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FranchiseService;

public sealed class OnCollectDailyBonus(CityVilleDbContext context) : AmfService<OnCollectDailyBonusRequest>
{
    public override async Task<ASObject> HandlePacket(OnCollectDailyBonusRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Franchises.Where(f => f.FranchiseType == request.FranchiseType))
            .ThenInclude(x => x.Locations)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        player.CollectFranchisesDailyBonus(request.FranchiseType);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class OnCollectDailyBonusRequest
{
    [AmfParam(0)] public string FranchiseType { get; set; } = string.Empty;
}