using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class StartContract(CityVilleDbContext context) : AmfService<StartContractRequest>
{
    public override async Task<ASObject> HandlePacket(StartContractRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.Player.Id == playerId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        if (user.Player is null) throw new Exception("Player not found");

        var obj = user.GetPlayer().GetWorld().GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z);

        if (obj is null)
            throw new Exception("Can't find building with coords");

        var contractItem = GameSettingsManager.Instance.GetItem(request.Building.ContractName ?? "");

        if (contractItem is null)
            throw new Exception($"Can't find item with contractName {request.Building.ContractName}");

        if (contractItem.Cost is not null)
        {
            if (contractItem.Cost > user.Player!.Gold)
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

            user.Player!.RemoveCoins(contractItem.Cost.Value);
        }

        obj.StartContract(request.Building.ContractName!, request.Building.State);

        user.HandleQuestsProgress("startContractByClass", className: obj.ClassName.ToString());
        user.CheckCompletedQuests();

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
    [AmfParam("contractName")] public string? ContractName { get; set; }
}