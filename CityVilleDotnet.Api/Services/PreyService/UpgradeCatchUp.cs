using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.PreyService;

internal sealed class UpgradeCatchUp(CityVilleDbContext context) : AmfService<UpgradeCatchUpRequest>
{
    public override async Task<ASObject> HandlePacket(UpgradeCatchUpRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var setting = GameSettingsManager.Instance.GetPreyGroup(request.GroupId) ?? throw new DomainException(GameErrorType.InvalidData, $"Unknown prey group {request.GroupId}");

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .Include(x => x.PreyGroups.Where(g => g.GroupId == request.GroupId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var world = player.GetWorld();
        var station = world.GetBuildingByClientId(request.StationId);
        var stationHub = setting.GetHubByName(station.ItemName) ?? throw new DomainException(GameErrorType.InvalidData, $"{station.ItemName} is not a hub of {setting.Id}");

        var targetLevel = Math.Max(world.GetHubLevel(setting), player.GetOrCreatePreyGroup(setting.Id).GateUnlockLevel);
        var targetHub = setting.GetHubByLevel(targetLevel) ?? throw new DomainException(GameErrorType.InvalidState, $"No hub of level {targetLevel} in {setting.Id}");

        if (stationHub.Level >= targetHub.Level)
            return new CityVilleResponse().Error(GameErrorType.InvalidState);

        var gameItem = GameSettingsManager.Instance.GetItem(station.ItemName) ?? throw new Exception($"Can't find game item for {station.ItemName}");

        station.UpgradeBuilding(gameItem.GetFirstDeriveItem(gameItem), targetHub.Name);
        world.CalculatePopulation();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class UpgradeCatchUpRequest
{
    [AmfParam(0)] public string GroupId { get; set; } = string.Empty;
    [AmfParam(1)] public int StationId { get; set; }
}

public class UpgradeCatchUpValidator : AbstractValidator<UpgradeCatchUpRequest>
{
    public UpgradeCatchUpValidator()
    {
        RuleFor(x => x.GroupId).NotEmpty().MaximumLength(32);
    }
}
