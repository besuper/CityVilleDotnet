using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Pages;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class UnlockCityAtNight(CityVilleDbContext context, IConfiguration configuration) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        var variant = (configuration.GetSection("experiments").Get<List<Experiment>>() ?? []).FirstOrDefault(x => x.Name == "cv_CityatNights_Priceunlock")?.Variant ?? 0;

        var player = await context.Set<Player>()
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.UnlockCityAtNight(GameSettingsManager.Instance.GetSettings().GetCityAtNightUnlockPrice(variant));

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}
