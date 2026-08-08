using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.QuestService;

public class HandleQuestProgress(CityVilleDbContext context) : AmfService<HandleQuestProgressRequest>
{
    public override async Task<ASObject> HandlePacket(HandleQuestProgressRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        // params
        // 0: action type (onValidCityName)

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Quests)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .Include(x => x.SeenFlags)
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Can't to find user with UserId");

        player.HandleQuestsProgress(request.ActionType);
        player.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        var rep = new ASObject
        {
            ["QuestComponent"] = AmfConverter.Convert(player.Quests.ToQuestComponent())
        };

        return new CityVilleResponse().MetaData(rep);
    }
}

public class HandleQuestProgressRequest
{
    [AmfParam(0)] public string ActionType { get; set; } = string.Empty;
}

public class HandleQuestProgressValidator : AbstractValidator<HandleQuestProgressRequest>
{
    public HandleQuestProgressValidator()
    {
        RuleFor(x => x.ActionType).NotEmpty().MaximumLength(64);
    }
}