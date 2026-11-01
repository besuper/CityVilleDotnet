using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class BuyMechanicDataGate(CityVilleDbContext context) : AmfService<BuyMechanicDataGateRequest>
{
    public override async Task<ASObject> HandlePacket(BuyMechanicDataGateRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.TempId == request.Building.Id || o.WorldFlatId == request.Building.Id))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByClientId(request.Building.Id);
        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");
        var gate = gameItem.GetGates().FirstOrDefault(x => x.Name == request.Params[1]) ?? throw new Exception($"Can't find gate {request.Params[1]} for {obj.ItemName}");

        player.BuyInvestmentGate(obj, gate);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class BuyMechanicDataGateRequest
{
    [AmfParam(1)] public BuyMechanicDataGateBuildingRequest Building { get; set; } = new();
    [AmfParam(3)] public string[] Params { get; set; } = [];
}

public class BuyMechanicDataGateBuildingRequest
{
    [AmfParam("id")] public int Id { get; set; }
}

public class BuyMechanicDataGateRequestValidator : AbstractValidator<BuyMechanicDataGateRequest>
{
    public BuyMechanicDataGateRequestValidator()
    {
        RuleFor(x => x.Params).Must(x => x.Length == 2);
        RuleForEach(x => x.Params).NotEmpty().MaximumLength(64);
    }
}
