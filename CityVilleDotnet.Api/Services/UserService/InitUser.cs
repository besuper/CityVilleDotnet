using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
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

            .Include(x => x.Player)
            .ThenInclude(x => x.Inventory)

            .Include(x => x.Player)
            .ThenInclude(x => x.Commodities)

            .Include(x => x.World)
            .ThenInclude(x => x.MapRects)

            .Include(x => x.World)
            .ThenInclude(x => x.Objects)

            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            // Create a new player
            var appUser = await context.Set<ApplicationUser>().FirstOrDefaultAsync(x => x.Id == userId.ToString(), cancellationToken);
            string jsonContent = File.ReadAllText("Resources/defaultUser.json");

            var userDto = JsonSerializer.Deserialize<UserDto>(jsonContent);

            user = User.CreateNewPlayer(userDto, appUser);
            user.SetupNewPlayer(appUser);

            await context.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        var userObj = AmfConverter.Convert(user.ToDto());

        var quests = new ASObject();

        if (!user.Player.IsNew)
        {
            quests["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active));
        }

        var response = new CityVilleResponse(0, 333, quests, userObj);

        return response;
    }
}