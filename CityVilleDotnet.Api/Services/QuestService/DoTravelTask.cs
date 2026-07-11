using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.QuestService;

public class DoTravelTask(CityVilleDbContext context) : AmfService<DoTravelTaskRequest>
{
    public override async Task<ASObject> HandlePacket(DoTravelTaskRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        player.HandleQuestsProgress("travel", null, request.WorldId);
        player.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class DoTravelTaskRequest
{
    [AmfParam(0)] public string WorldId { get; set; } = string.Empty;
}

public class DoTravelTaskRequestValidator : AbstractValidator<DoTravelTaskRequest>
{
    public DoTravelTaskRequestValidator()
    {
        RuleFor(x => x.WorldId).NotEmpty().MaximumLength(32);
    }
}
