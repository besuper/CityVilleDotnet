using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
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

namespace CityVilleDotnet.Api.Services.PreyService;

internal sealed class CapturePrey(CityVilleDbContext context) : AmfService<CapturePreyRequest>
{
    public override async Task<ASObject> HandlePacket(CapturePreyRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var setting = GameSettingsManager.Instance.GetPreyGroup(request.GroupId) ?? throw new DomainException(GameErrorType.InvalidData, $"Unknown prey group {request.GroupId}");
        var hubNames = setting.GetHubNames();

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => hubNames.Contains(o.ItemName)))
            .Include(x => x.PreyGroups.Where(g => g.GroupId == request.GroupId))
            .ThenInclude(x => x.Hunters)
            .Include(x => x.PreyGroups.Where(g => g.GroupId == request.GroupId))
            .ThenInclude(x => x.Captures)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.CapturePrey(setting, player.GetWorld().GetHubLevel(setting), request.PreyId, request.CrewSlots, bought: false, ServerUtils.GetCurrentTimeSeconds());

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class CapturePreyRequest
{
    [AmfParam(0)] public string GroupId { get; set; } = string.Empty;
    [AmfParam(1)] public int PreyId { get; set; }
    [AmfParam(2)] public int[] CrewSlots { get; set; } = [];
}

public class CapturePreyValidator : AbstractValidator<CapturePreyRequest>
{
    public CapturePreyValidator()
    {
        RuleFor(x => x.GroupId).NotEmpty().MaximumLength(32);
        RuleFor(x => x.CrewSlots).Must(x => x.Length <= 10);
    }
}
