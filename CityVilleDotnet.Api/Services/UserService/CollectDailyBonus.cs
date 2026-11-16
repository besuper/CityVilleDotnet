using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Pages;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class CollectDailyBonus(CityVilleDbContext context, IConfiguration configuration) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        var experiments = (configuration.GetSection("experiments").Get<List<Experiment>>() ?? []).ToDictionary(x => x.Name, x => x.Variant);

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.EnergyModifier > 0))
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var randomReward = player.CollectDailyBonus(GameSettingsManager.Instance.GetDailyBonusDays(experiments));

        await context.SaveChangesAsync(cancellationToken);

        if (randomReward is null)
        {
            return new CityVilleResponse();
        }

        return new CityVilleResponse().Data(new ASObject { ["randomReward"] = randomReward });
    }
}
