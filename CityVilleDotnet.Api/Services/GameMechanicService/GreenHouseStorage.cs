using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class GreenHouseStorage(CityVilleDbContext context) : AmfService<GreenHouseStorageRequest>
{
    public override async Task<ASObject> HandlePacket(GreenHouseStorageRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        if (request.Action == "catalogPurchase")
        {
            if (!request.ExtraData.TryGetValue("plotname", out var plotNameObj))
                throw new Exception("Invalid plot name");

            var plotName = plotNameObj as string ?? throw new Exception("Invalid plot name");

            var contractItem = GameSettingsManager.Instance.GetItem(plotName) ?? throw new Exception($"Can't find plot item for {plotName}");

            var world = player.GetWorld();

            var worldObj = world.GetBuildingByClientId(request.ObjectId) ?? throw new Exception("Building not found");

            if (worldObj.GetClassName() != BuildingClassType.GreenHouse) throw new Exception($"Can't update green house storage for {worldObj.GetClassName()}");

            var gameItem = GameSettingsManager.Instance.GetItem(worldObj.GetItemName()) ?? throw new Exception($"Can't find game item for {worldObj.GetItemName()}");

            if (gameItem.NumCrop is null) throw new Exception("Num crop not found");
            
            if (contractItem.Cost is not null)
                player.RemoveCoins(contractItem.Cost.Value * gameItem.NumCrop.Value);
            
            worldObj.StartContract(plotName, WorldObjectState.Planted);
        }
        
        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class GreenHouseStorageRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string Action { get; set; } = string.Empty;
    [AmfParam(3)] public Dictionary<string, object> ExtraData { get; set; } = new();
}