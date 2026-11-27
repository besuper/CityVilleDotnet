using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public sealed class MaintenanceHarvestLoop(CityVilleDbContext context) : AmfService<MaintenanceHarvestLoopRequest>
{
    public override async Task<ASObject> HandlePacket(MaintenanceHarvestLoopRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByClientId(request.ObjectId);
        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        player.FinishWonderMaintenanceWithCash(obj, gameItem, ServerUtils.GetCurrentUtcDateTime());

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class MaintenanceHarvestLoopRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(3)] public MaintenanceHarvestLoopParamsRequest Params { get; set; } = new();
}

public class MaintenanceHarvestLoopParamsRequest
{
    [AmfParam("operation")] public string Operation { get; set; } = string.Empty;
}

public class MaintenanceHarvestLoopRequestValidator : AbstractValidator<MaintenanceHarvestLoopRequest>
{
    public MaintenanceHarvestLoopRequestValidator()
    {
        RuleFor(x => x.Params.Operation).Equal("finishWithCash");
    }
}
