using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FranchiseService;

public sealed class OnCollectDailyBonus(CityVilleDbContext context) : AmfService<OnCollectDailyBonusRequest>
{
    public override async Task<ASObject> HandlePacket(OnCollectDailyBonusRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.Franchises.Where(f => f.FranchiseType == request.FranchiseType))
            .ThenInclude(x => x.Locations)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null) throw new Exception($"User not found with id {userId}");

        user.Player.CollectFranchisesDailyBonus(request.FranchiseType);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class OnCollectDailyBonusRequest
{
    [AmfParam(0)] public string FranchiseType { get; set; } = string.Empty;
}