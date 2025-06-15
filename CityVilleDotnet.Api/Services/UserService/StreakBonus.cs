using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class StreakBonus(CityVilleDbContext context, ILogger<StreakBonus> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .Include(x => x.Player)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't to find user with UserId");

        var data = (Dictionary<string, object>)@params[0];
        var amount = (int)data["amount"];
        var maxesReached = (int)data["maxesReached"];

        logger.LogInformation($"Streak bonus {amount} coins [{maxesReached}]");

        user.AddGold(amount);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}