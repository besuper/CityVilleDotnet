using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class StartContract(CityVilleDbContext context) : AmfService<StartContractRequest>
{
    public override async Task<ASObject> HandlePacket(StartContractRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.X == request.Building.Position.X && o.Y == request.Building.Position.Y))
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z);

        if (obj is null)
            throw new Exception("Can't find building with coords");

        var contractItem = GameSettingsManager.Instance.GetItem(request.Building.ContractName);

        if (contractItem is null)
            throw new Exception($"Can't find item with contractName {request.Building.ContractName}");

        if (contractItem.Cost is not null)
            player.RemoveCoins(contractItem.Cost.Value);

        obj.StartContract(request.Building.ContractName, request.Building.State);

        player.HandleQuestsProgress("startContractByClass", className: obj.ClassName.ToString());
        player.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class StartContractRequest
{
    [AmfParam(1)] public BuildingStartContractRequest Building { get; set; } = new();
}

public class BuildingStartContractRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
    [AmfParam("state")] public WorldObjectState State { get; set; }
    [AmfParam("contractName")] public string ContractName { get; set; } = string.Empty;
}

public class StartContractValidator : AbstractValidator<StartContractRequest>
{
    public StartContractValidator()
    {
        RuleFor(x => x.Building.ContractName).NotEmpty().MaximumLength(64);
    }
}