using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Api.Services.UserService;

internal sealed class InitUser(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        var user = await context.Set<Player>()
            .AsSplitQuery()
            .AsNoTracking()
            .Include(x => x.Quests.OrderBy(q => q.Order))
            .Include(x => x.InventoryItems)
            .ThenInclude(x => x.StoredObject)
            .Include(x => x.World)
            .ThenInclude(x => x!.MapRects)
            .Include(x => x.World)
            .ThenInclude(x => x!.IncentivizedExpansions)
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.CrewMembers)
            .ThenInclude(x => x.Player)
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.SeenFlags)
            .Include(x => x.Friends.Where(f => f.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendPlayer)
            .ThenInclude(x => x!.World)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Licenses)
            .Include(x => x.Franchises)
            .ThenInclude(x => x.Locations)
            .Include(x => x.LotOrders) // FIXME: Limit orders
            .Include(x => x.VisitorHelpOrders) // FIXME: Limit orders
            .Include(x => x.Masteries)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (user is null)
            throw new Exception("Player not initialized correctly");

        // Handle energy regeneration
        var trackedUser = await context.Set<Player>()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(y => y.TempId != -1 || y.EnergyModifier > 0))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (trackedUser is null)
            throw new Exception("Player not found for user");

        // Remove the first building if user refresh before completing the tutorial to avoid being stuck
        if (trackedUser.IsNew)
        {
            var firstBuilding = trackedUser.GetWorld().Objects.FirstOrDefault(y => y.TempId != -1);

            if (firstBuilding is not null)
            {
                user.GetWorld().RemoveBuilding(user.GetWorld().Objects.FirstOrDefault(y => y.TempId != -1)!);
                trackedUser.GetWorld().RemoveBuilding(firstBuilding);
                
                context.Remove(firstBuilding);
            }
        }

        trackedUser.UpdateEnergy();
        trackedUser.GetWorld().CleanTempIDs();
        
        user.UpdateEnergy(); // This will not save

        await context.SaveChangesAsync(cancellationToken);

        var userObj = AmfConverter.Convert(user.ToDto());

        var quests = new ASObject();

        if (!user.IsNew)
            quests["QuestComponent"] = AmfConverter.Convert(user.Quests.Select(x => x.ToDto()).ToList());

        return new CityVilleResponse().Data(userObj).MetaData(quests);
    }
}