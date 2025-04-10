﻿using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.QuestService;

public class HandleQuestProgress(CityVilleDbContext context) : AmfService(context)
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        // params
        // 0: action type (onValidCityName)

        var actionType = (string)_params[0];

        var user = await context.Set<User>()
            .Include(x => x.Quests)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.World)
            .ThenInclude(x => x.CitySim)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        user.handleQuestProgress(actionType);
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        var rep = new ASObject();
        rep["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active));

        return new CityVilleResponse(0, 333, rep);
    }
}
