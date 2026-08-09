using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.TrainService;

public class SpeedUp(CityVilleDbContext context) : AmfService<SpeedUpRequest>
{
    public override async Task<ASObject> HandlePacket(SpeedUpRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.TrainOrder)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var currentTime = ServerUtils.GetCurrentTimeSeconds();
        var order = player.GetWorld().TrainOrder ?? throw new Exception("No train order in progress");

        if (order.HasArrived(currentTime))
            throw new Exception($"Train {order.ItemName} has already arrived");

        player.RemoveCash(order.GetSpeedUpCost());
        order.SpeedUp(currentTime);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class SpeedUpRequest
{
    [AmfParam(0)] public string Bucket { get; set; } = string.Empty;
}

public class SpeedUpValidator : AbstractValidator<SpeedUpRequest>
{
    public SpeedUpValidator()
    {
        RuleFor(x => x.Bucket).NotEmpty().MaximumLength(16);
    }
}
