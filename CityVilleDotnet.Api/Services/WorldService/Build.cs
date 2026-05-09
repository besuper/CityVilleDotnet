using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Build(CityVilleDbContext context) : AmfService<BuildRequest>
{
    public override async Task<ASObject> HandlePacket(BuildRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.X == request.Building.Position.X && o.Y == request.Building.Position.Y))
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.InventoryItems)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception("Can't find building");

        if (obj.Builds is null)
            throw new Exception("Can't find `builds`");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.NumberOfStages is null)
            throw new Exception($"Game item {obj.ItemName} doesn't have number of stages defined");

        if (gameItem.EnergyCost?.Build is not null)
        {
            player.RemoveEnergy(int.Parse(gameItem.EnergyCost.Build));
        }
        else if (gameItem.EnergyCostPerBuild is not null)
        {
            player.RemoveEnergy(gameItem.EnergyCostPerBuild.Value);
        }

        obj.AddConstructionStage();

        if (obj.Stage != gameItem.NumberOfStages)
            player.CollectDoobersRewards(obj.TargetBuildingName!, construction: true);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            { "id", obj.WorldFlatId }
        });
    }
}

public class BuildRequest
{
    [AmfParam(1)] public BuildBuildingRequest Building { get; set; } = new();
}

public class BuildBuildingRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}