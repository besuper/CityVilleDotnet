using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class StreakBonus(CityVilleDbContext context) : AmfService<StreakBonusRequest>
{
    public override async Task<ASObject> HandlePacket(StreakBonusRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>().FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.AddCoins(request.Data.Amount);

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