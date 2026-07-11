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

internal sealed class StreakData(CityVilleDbContext context) : AmfService<StreakDataRequest>
{
    public override async Task<ASObject> HandlePacket(StreakDataRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByClientId(request.ObjectId) ?? throw new Exception($"Can't find building with id {request.ObjectId}");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.Mechanics is null)
            throw new Exception("No mechanics found for this item");

        var gameMode = gameItem.Mechanics.GetMechanicByGameMode(request.GameMode);

        if (gameMode is null)
            throw new Exception($"No mechanics game mode {request.GameMode} found");

        var mechanic = gameMode.GetMechanicItemByType("streakData");

        if (mechanic is null)
            throw new Exception("No streakData mechanic found for this item");

        if (!request.ExtraData.TryGetValue("action", out var action))
            throw new Exception("Action not found in extra data");

        var negativeStreakRewards = gameItem.GetNegativeStreakRewards();

        obj.UpdateStreakData(mechanic.ActiveDuration, mechanic.InactiveDuration, mechanic.MaxStreakLength, negativeStreakRewards);

        if (action.Equals("supply"))
        {
            if (obj.ActivationTime.HasValue)
                return new CityVilleResponse().Error(GameErrorType.InvalidState);

            if (mechanic.ConsumableType is not null && mechanic.ConsumableQuantity > 0)
            {
                var hasEnough = mechanic.ConsumableType switch
                {
                    "goods" => player.Goods >= mechanic.ConsumableQuantity,
                    "premium_goods" => player.PremiumGoods >= mechanic.ConsumableQuantity,
                    _ => false
                };

                if (!hasEnough)
                    return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

                switch (mechanic.ConsumableType)
                {
                    case "goods":
                        player.RemoveGoods(mechanic.ConsumableQuantity);
                        break;
                    case "premium_goods":
                        player.RemovePremiumGoods(mechanic.ConsumableQuantity);
                        break;
                }
            }

            obj.Supply(mechanic.MaxStreakLength, gameItem.GetPositiveStreakRewards(), gameItem.GetStreakMaxEffect());
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class StreakDataRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
    [AmfParam(3)] public Dictionary<string, object> ExtraData { get; set; } = new();
}