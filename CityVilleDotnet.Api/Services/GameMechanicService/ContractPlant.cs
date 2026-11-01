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

public class ContractPlant(CityVilleDbContext context) : AmfService<ContractPlantRequest>
{
    public override async Task<ASObject> HandlePacket(ContractPlantRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByClientId(request.ObjectId);

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("contractPlant") is null)
        {
            throw new Exception($"No contractPlant mechanic found for {obj.ItemName} in game mode {request.GameMode}");
        }

        var contract = GameSettingsManager.Instance.GetItem(request.ExtraData.ContractName) ?? throw new Exception($"Can't find contract {request.ExtraData.ContractName}");

        player.PlantMechanicContract(obj, gameItem, contract);
        player.HandleQuestsProgress("startContractByName", itemName: contract.Name);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class ContractPlantRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
    [AmfParam(3)] public ContractPlantExtraData ExtraData { get; set; } = new();
}

public class ContractPlantExtraData
{
    [AmfParam("contractName")] public string ContractName { get; set; } = string.Empty;
}

public class ContractPlantRequestValidator : AbstractValidator<ContractPlantRequest>
{
    public ContractPlantRequestValidator()
    {
        RuleFor(x => x.GameMode).NotEmpty().MaximumLength(64);
        RuleFor(x => x.ExtraData.ContractName).NotEmpty().MaximumLength(64);
    }
}
