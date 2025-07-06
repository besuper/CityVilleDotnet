using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
            .ThenInclude(x => x!.Inventory)
            .ThenInclude(x => x!.Items)
            .Include(x => x.World)
            .ThenInclude(x => x!.MapRects)
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.Player)
            .ThenInclude(x => x!.SeenFlags)
            .Include(x => x.Friends.Where(x => x.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user is null)
        {
            // Create a new player
            var appUser = await context.Set<ApplicationUser>().FirstOrDefaultAsync(x => x.Id == userId.ToString(), cancellationToken) ?? throw new Exception("Can't find ApplicationUser with UserId");
            var jsonContent = await File.ReadAllTextAsync("Resources/defaultUser.json", cancellationToken);

            var userDto = JsonSerializer.Deserialize<UserDto>(jsonContent) ?? throw new Exception("UserDTO can't be null");

            user = User.CreateNewPlayer(userDto, appUser);
            user.SetupNewPlayer(appUser);

            await context.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (user.Player is null)
            throw new Exception("Player not initialized correctly");
        
        var userObj = AmfConverter.Convert(user.ToDto());

        var quests = new ASObject();

        if (!user.Player.IsNew)
            quests["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active));

        var response = new CityVilleResponse(0, 333, quests, userObj);

        return response;
    }
}