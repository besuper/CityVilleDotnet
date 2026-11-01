using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class ContractHarvest(CityVilleDbContext context) : AmfService<ContractHarvestRequest>
{
    public override async Task<ASObject> HandlePacket(ContractHarvestRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var globalTableProviders = GameSettingsManager.Instance.GetGlobalTableProviders();

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId || globalTableProviders.Contains(o.ItemName)))
            .Include(x => x.InventoryItems)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.GlobalTableOverrides)
            .Include(x => x.FeatureRollCounters)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByClientId(request.ObjectId);

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        var mechanic = gameItem.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("contractHarvest")
                       ?? throw new Exception($"No contractHarvest mechanic found for {obj.ItemName} in game mode {request.GameMode}");

        if (mechanic.ClassName != "HarvestInvestmentMechanic") throw new Exception($"Contract harvest {mechanic.ClassName} is not supported");

        if (gameItem.EnergyCost?.Harvest is not null)
        {
            player.RemoveEnergy(int.Parse(gameItem.EnergyCost.Harvest));
        }

        var contractName = player.HarvestInvestment(obj, gameItem);

        player.HandleQuestsProgress("harvestContractByName", itemName: contractName);
        player.HandleQuestsProgress("harvestMechanicContractByKeywords", itemName: contractName);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class ContractHarvestRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
}

public class ContractHarvestRequestValidator : AbstractValidator<ContractHarvestRequest>
{
    public ContractHarvestRequestValidator()
    {
        RuleFor(x => x.GameMode).NotEmpty().MaximumLength(64);
    }
}
