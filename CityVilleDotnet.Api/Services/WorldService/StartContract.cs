using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
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
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.Building.Id || o.TempId == request.Building.Id))
            .ThenInclude(x => x.Workers)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByClientId(request.Building.Id);

        if (obj is null)
            throw new Exception("Can't find building with coords");

        var contractItem = GameSettingsManager.Instance.GetItem(request.Building.ContractName);

        if (contractItem is null)
            throw new Exception($"Can't find item with contractName {request.Building.ContractName}");

        if (obj.ClassName is BuildingClassType.Plot or BuildingClassType.Factory)
        {
            if (obj.ClassName == BuildingClassType.Plot && obj.State != WorldObjectState.Plowed)
                throw new DomainException(GameErrorType.InvalidState);

            player.PayContract(contractItem);
        }
        else if (contractItem.Cost is not null)
        {
            player.RemoveCoins(contractItem.Cost.Value);
        }

        obj.StartContract(request.Building.ContractName, request.Building.State, ServerUtils.GetActionTime(request.ClientEnqueueTime));

        player.HandleQuestsProgress("startContractByClass", className: obj.ClassName.ToString());
        player.HandleQuestsProgress("startContractByName", itemName: request.Building.ContractName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class StartContractRequest
{
    [AmfParam(1)] public BuildingStartContractRequest Building { get; set; } = new();
    [AmfParam(2)] public long? ClientEnqueueTime { get; set; }
}

public class BuildingStartContractRequest
{
    [AmfParam("state")] public WorldObjectState State { get; set; }
    [AmfParam("contractName")] public string ContractName { get; set; } = string.Empty;
    [AmfParam("id")] public int Id { get; set; }
}

public class StartContractValidator : AbstractValidator<StartContractRequest>
{
    public StartContractValidator()
    {
        RuleFor(x => x.Building.ContractName).NotEmpty().MaximumLength(64);
    }
}