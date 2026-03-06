using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Build(CityVilleDbContext context) : AmfService<BuildRequest>
{
    public override async Task<ASObject> HandlePacket(BuildRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        if (user.Player is null) throw new Exception("Player not found for user");

        var obj = user.GetWorld().GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception("Can't find building");

        if (obj.Builds is null)
            throw new Exception("Can't find `builds`");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.NumberOfStages is null)
            throw new Exception($"Game item {obj.ItemName} doesn't have number of stages defined");

        if (gameItem.EnergyCost?.Build is not null)
        {
            var energyCost = int.Parse(gameItem.EnergyCost.Build);

            if (!user.Player!.RemoveEnergy(energyCost))
            {
                // FIXME: Return error response
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
            }
        }
        else if (gameItem.EnergyCostPerBuild is not null)
        {
            if (!user.Player!.RemoveEnergy(gameItem.EnergyCostPerBuild.Value))
            {
                // FIXME: Return error response
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
            }
        }

        obj.AddConstructionStage();

        if (obj.Stage != gameItem.NumberOfStages)
        {
            user.Player!.CollectDoobersRewards(obj.ItemName, obj.ClassName);
        }

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