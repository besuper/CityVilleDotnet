using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.QuestService;

public class HandleQuestProgress(CityVilleDbContext context) : AmfService<HandleQuestProgressRequest>
{
    public override async Task<ASObject> HandlePacket(HandleQuestProgressRequest request, Guid userId, CancellationToken cancellationToken)
    {
        // params
        // 0: action type (onValidCityName)

        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user is null) throw new Exception("Can't to find user with UserId");

        user.HandleQuestsProgress(request.ActionType);
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        var rep = new ASObject
        {
            ["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active).Select(x => x.ToDto()))
        };

        return new CityVilleResponse().MetaData(rep);
    }
}

public class HandleQuestProgressRequest
{
    [AmfParam(0)] public string ActionType { get; set; } = string.Empty;
}