using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Api.Services.UserService;

internal sealed class InitUser(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .AsNoTracking()
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.World)
            .ThenInclude(x => x!.MapRects)
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.Player)
            .ThenInclude(x => x!.SeenFlags)
            .Include(x => x.Friends.Where(f => f.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Licenses)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Franchises)
            .ThenInclude(x => x.Locations)
            .Include(x => x.Player)
            .ThenInclude(x => x!.LotOrders) // FIXME: Limit orders
            .Include(x => x.Player)
            .ThenInclude(x => x!.VisitorHelpOrders) // FIXME: Limit orders
            .Include(x => x.Player)
            .ThenInclude(x => x!.Masteries)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user is null)
        {
            // Create a new player
            var appUser = await context.Set<ApplicationUser>().FirstOrDefaultAsync(x => x.Id == userId.ToString(), cancellationToken) ?? throw new Exception("Can't find ApplicationUser with UserId");
            // FIXME: Use start world inside GameSettings
            var jsonContent = await File.ReadAllTextAsync("Resources/startWorld.json", cancellationToken);

            var defaultWorld = JsonSerializer.Deserialize<WorldDto>(jsonContent) ?? throw new Exception("WorldDto can't be null");

            user = User.CreateNewPlayer(defaultWorld, appUser);
            user.SetupNewPlayer(appUser);

            await context.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (user.Player is null)
            throw new Exception("Player not initialized correctly");

        // Handle energy regeneration
        var trackedUser = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Include(x => x.Player)
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(y => y.TempId != -1))
            .FirstOrDefaultAsync(cancellationToken);

        if (trackedUser?.Player is null || trackedUser.World is null)
            throw new Exception("Player not found for user");

        trackedUser.Player.UpdateEnergy();
        trackedUser.World.CleanTempIDs();

        user.Player.UpdateEnergy(); // This will not save

        await context.SaveChangesAsync(cancellationToken);

        var userObj = AmfConverter.Convert(user.ToDto());

        var quests = new ASObject();

        if (!user.Player.IsNew)
            quests["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active).Select(x => x.ToDto()).ToList());

        return new CityVilleResponse().Data(userObj).MetaData(quests);
    }
}