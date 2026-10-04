using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class BuyStatusGate(CityVilleDbContext context) : AmfService<BuyStatusGateRequest>
{
    public override async Task<ASObject> HandlePacket(BuyStatusGateRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.TempId == request.Building.Id || o.WorldFlatId == request.Building.Id))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByClientId(request.Building.Id);
        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");
        var gate = gameItem.GetStatusGate(request.Params[0]) ?? throw new Exception($"Can't find status gate {request.Params[0]} for {obj.ItemName}");

        player.BuyStatusGate(obj, gate);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class BuyStatusGateRequest
{
    [AmfParam(1)] public BuyStatusGateBuildingRequest Building { get; set; } = new();
    [AmfParam(3)] public string[] Params { get; set; } = [];
}

public class BuyStatusGateBuildingRequest
{
    [AmfParam("id")] public int Id { get; set; }
}

public class BuyStatusGateRequestValidator : AbstractValidator<BuyStatusGateRequest>
{
    public BuyStatusGateRequestValidator()
    {
        RuleFor(x => x.Params).NotEmpty();
        RuleForEach(x => x.Params).NotEmpty().MaximumLength(64);
    }
}
