using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CityVilleDotnet.Api.Services.UserService;

internal sealed class InitUser(CityVilleDbContext context) : AmfService(context)
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsNoTracking()
            .Include(x => x.Quests)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x.Options)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x.Inventory)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x.Commodities)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.World)
            .ThenInclude(x => x.MapRects)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.World)
            .ThenInclude(x => x.Objects)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            // Create a new player

            var appUser = await context.Set<ApplicationUser>().FirstOrDefaultAsync(x => x.Id == userId.ToString(), cancellationToken);
            string jsonContent = File.ReadAllText("Resources/defaultUser.json");

            user = JsonSerializer.Deserialize<User>(jsonContent);
            user.SetupNewPlayer(appUser);

            await context.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        var userObj = AmfConverter.Convert(user);

        var quests = new ASObject();

        if (!user.UserInfo.IsNew)
        {
            quests["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active));
        }

        var response = new CityVilleResponse(0, 333, quests, userObj);

        return response;
    }
}