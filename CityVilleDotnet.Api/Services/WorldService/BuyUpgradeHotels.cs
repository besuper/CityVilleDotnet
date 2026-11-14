using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class BuyUpgradeHotels(CityVilleDbContext context) : AmfService<BuyUpgradeHotelsRequest>
{
    public override async Task<ASObject> HandlePacket(BuyUpgradeHotelsRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .Include(x => x.InventoryItems)
            .Include(x => x.SeenFlags)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();
        var obj = world.GetBuildingByClientId(request.Params[0][0]);
        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        // TODO: Implement hotel
        if (gameItem.Upgrade?.Name is null || gameItem.Upgrade.CashCost is null || gameItem.Upgrade.IsRandom)
            return new CityVilleResponse().Error(GameErrorType.InvalidState);

        player.RemoveCash(Convert.ToInt32(gameItem.Upgrade.CashCost));

        obj.UpgradeBuilding(gameItem.GetFirstDeriveItem(gameItem), gameItem.Upgrade.Name);
        player.HandleQuestsProgress("upgradeItemByName", itemName: gameItem.Name);
        player.GiveUpgradeRewards(gameItem.Upgrade.Rewards?.Rewards ?? []);

        world.CalculatePopulation();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new List<object> { new ASObject { ["id"] = obj.WorldFlatId } });
    }
}

public class BuyUpgradeHotelsRequest
{
    [AmfParam(3)] public int[][] Params { get; set; } = [];
}

public class BuyUpgradeHotelsRequestValidator : AbstractValidator<BuyUpgradeHotelsRequest>
{
    public BuyUpgradeHotelsRequestValidator()
    {
        RuleFor(x => x.Params).NotEmpty();
        RuleFor(x => x.Params[0]).Must(ids => ids.Length == 1).When(x => x.Params.Length > 0);
    }
}
