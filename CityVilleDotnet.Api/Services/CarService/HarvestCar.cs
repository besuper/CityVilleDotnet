using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.CarService;

public class HarvestCar(CityVilleDbContext context) : AmfService<HarvestCarRequest>
{
    public override async Task<ASObject> HandlePacket(HarvestCarRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var globalTableProviders = GameSettingsManager.Instance.GetGlobalTableProviders();

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Cars)
            .Include(x => x.CraftingParts)
            .Include(x => x.FeatureRollCounters)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.InventoryItems)
            .Include(x => x.GlobalTableOverrides)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => globalTableProviders.Contains(o.ItemName)))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var settings = GameSettingsManager.Instance.GetSettings();
        var carItem = GameSettingsManager.Instance.GetItem(request.CarName);

        if (carItem is null || !settings.AvailableCarTypes.Split(',').Contains(request.CarName) || !player.HasCar(request.CarName))
        {
            return new CityVilleResponse().Error(GameErrorType.InvalidData);
        }

        var maxHarvestValue = Math.Max(settings.CarHarvestBuildingModifier * settings.CarHarvestModifier, 100) * carItem.GetCarTier();

        if (request.HarvestValue > maxHarvestValue)
        {
            return new CityVilleResponse().Error(GameErrorType.InvalidData);
        }

        player.CollectCarHarvestRewards(request.CarName);
        player.AddCoins((int)Math.Round(request.HarvestValue));
        player.HandleQuestsProgress("carHarvest");

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class HarvestCarRequest
{
    [AmfParam(0)] public string CarName { get; set; } = string.Empty;
    [AmfParam(1)] public double HarvestValue { get; set; }
    [AmfParam(2)] public int CollectionRand { get; set; }
    [AmfParam(3)] public string PartTableName { get; set; } = string.Empty;
}

public class HarvestCarRequestValidator : AbstractValidator<HarvestCarRequest>
{
    public HarvestCarRequestValidator()
    {
        RuleFor(x => x.CarName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.HarvestValue).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PartTableName).MaximumLength(64);
    }
}
