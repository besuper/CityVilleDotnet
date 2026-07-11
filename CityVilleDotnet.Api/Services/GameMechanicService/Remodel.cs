using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public sealed class Remodel(CityVilleDbContext context) : AmfService<RemodelRequest>
{
    public override async Task<ASObject> HandlePacket(RemodelRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByClientId(request.ObjectId) ?? throw new Exception($"Can't find building with id {request.ObjectId}");

        if (player.Level < GameSettingsManager.Instance.GetSettings().RemodelingRequiredLevel)
            return new CityVilleResponse().Error(GameErrorType.InvalidState);

        if (request.GameMode == "catalogPurchase")
        {
            if (!request.ExtraData.TryGetValue("itemName", out var itemNameValue) || itemNameValue is not string skinItemName || string.IsNullOrEmpty(skinItemName))
                return new CityVilleResponse().Error(GameErrorType.MissingData);

            var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");
            var baseItem = gameItem.GetFirstDeriveItem(gameItem);
            var definition = baseItem.GetRemodelDefinitionByName(skinItemName);

            if (definition is null)
                return new CityVilleResponse().Error(GameErrorType.InvalidData);

            var skinItem = GameSettingsManager.Instance.GetItem(skinItemName) ?? throw new Exception($"Can't find game item for {skinItemName}");

            if (skinItem.Cash > 0)
                player.RemoveCash(definition.OverrideCash > 0 ? definition.OverrideCash : skinItem.Cash.Value);
            else
                player.RemoveCoins(definition.OverrideCost > 0 ? definition.OverrideCost : skinItem.Cost ?? 0);

            obj.StartRemodel(skinItemName);
        }
        else if (request.GameMode is "GMPlay" or "GMRemodel")
        {
            if (!obj.IsRemodeling())
                return new CityVilleResponse().Error(GameErrorType.InvalidState);

            player.RemoveEnergy(1);

            if (obj.AddRemodelBuild())
            {
                var remodelXp = obj.FinishRemodel();

                player.AddXp(remodelXp);
                world.CalculatePopulation();

                player.HandleQuestsProgress("buildingremodeled");
                player.CheckCompletedQuests();
            }
        }
        else
        {
            throw new Exception($"Unknown remodel game mode {request.GameMode}");
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class RemodelRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
    [AmfParam(3)] public Dictionary<string, object> ExtraData { get; set; } = new();
}
