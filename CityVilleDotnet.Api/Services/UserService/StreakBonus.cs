using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class StreakBonus(CityVilleDbContext context) : AmfService<StreakBonusRequest>
{
    public override async Task<ASObject> HandlePacket(StreakBonusRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Can't to find user with UserId");
        
        user.AddCoins(request.Data.Amount);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class StreakBonusRequest
{
    [AmfParam(0)] public StreakBonusData Data { get; set; } = new();
}

public class StreakBonusData
{
    [AmfParam("amount")] public int Amount { get; set; }
}