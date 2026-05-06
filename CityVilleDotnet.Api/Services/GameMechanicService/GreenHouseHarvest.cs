using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class GreenHouseHarvest(CityVilleDbContext context) : AmfService<GreenHouseHarvestRequest>
{
    public override async Task<ASObject> HandlePacket(GreenHouseHarvestRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.WorldFlatId == request.ObjectId))
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.InventoryItems)
            .Include(x => x.Masteries)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var worldObj = world.GetBuildingById(request.ObjectId) ?? throw new Exception("Building not found");

        if (worldObj.GetClassName() != BuildingClassType.GreenHouse) throw new Exception($"Can't update green house for {worldObj.GetClassName()}");

        var gameItem = GameSettingsManager.Instance.GetItem(worldObj.ItemName) ?? throw new Exception($"Can't find game item for {worldObj.GetItemName()}");

        if (gameItem.NumCrop is null) throw new Exception("Num crop not found");

        var plotObj = worldObj.GetGreenHousePlot();

        if (!plotObj.CanHarvest()) throw new Exception("Green house is not ready to harvest");
        if (plotObj.ContractName is null) throw new Exception("Invalid contract name");
        
        var plotItem = GameSettingsManager.Instance.GetItem(plotObj.ContractName) ?? throw new Exception($"Can't find game item for {worldObj.ContractName}");

        worldObj.HarvestGreenHouse();

        for (var i = 0; i < gameItem.NumCrop; i++)
        {
            player.CollectDoobersRewards(plotObj.ContractName, coinMultiplier: 1);
        }
        
        if (plotItem.HasMasteries())
            player.IncrementMastery(plotItem.Name, gameItem.NumCrop.Value);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class GreenHouseHarvestRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(3)] public Dictionary<string, object> ExtraData { get; set; } = new();
}