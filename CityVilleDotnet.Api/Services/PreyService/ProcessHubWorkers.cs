using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.PreyService;

internal sealed class ProcessHubWorkers(CityVilleDbContext context) : AmfService<ProcessHubWorkersRequest>
{
    public override async Task<ASObject> HandlePacket(ProcessHubWorkersRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var setting = GameSettingsManager.Instance.GetPreyGroup(request.GroupId) ?? throw new DomainException(GameErrorType.InvalidData, $"Unknown prey group {request.GroupId}");
        var hubNames = setting.GetHubNames();

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => hubNames.Contains(o.ItemName)))
            .Include(x => x.PreyGroups.Where(g => g.GroupId == request.GroupId))
            .ThenInclude(x => x.Hunters)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var group = player.GetOrCreatePreyGroup(setting.Id);
        group.SyncHunters(setting, player.GetWorld().GetHubLevel(setting), ServerUtils.GetCurrentTimeSeconds());

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject { { "frontendData", group.ToHunterWorkersAsObject() } });
    }
}

public class ProcessHubWorkersRequest
{
    [AmfParam(0)] public string GroupId { get; set; } = string.Empty;
}

public class ProcessHubWorkersValidator : AbstractValidator<ProcessHubWorkersRequest>
{
    public ProcessHubWorkersValidator()
    {
        RuleFor(x => x.GroupId).NotEmpty().MaximumLength(32);
    }
}
