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

internal sealed class UnlockHubGate(CityVilleDbContext context) : AmfService<UnlockHubGateRequest>
{
    public override async Task<ASObject> HandlePacket(UnlockHubGateRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var setting = GameSettingsManager.Instance.GetPreyGroup(request.GroupId) ?? throw new DomainException(GameErrorType.InvalidData, $"Unknown prey group {request.GroupId}");
        var hubNames = setting.GetHubNames();

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => hubNames.Contains(o.ItemName)))
            .Include(x => x.PreyGroups.Where(g => g.GroupId == request.GroupId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var world = player.GetWorld();

        // Only hubs are loaded, throws when the station is not a hub of the group
        world.GetBuildingByClientId(request.StationId);

        player.GetOrCreatePreyGroup(setting.Id).UnlockGate(world.GetHubLevel(setting));

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class UnlockHubGateRequest
{
    [AmfParam(0)] public string GroupId { get; set; } = string.Empty;
    [AmfParam(1)] public int StationId { get; set; }
}

public class UnlockHubGateValidator : AbstractValidator<UnlockHubGateRequest>
{
    public UnlockHubGateValidator()
    {
        RuleFor(x => x.GroupId).NotEmpty().MaximumLength(32);
    }
}
