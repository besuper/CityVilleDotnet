using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Common.Extensions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

internal sealed class ProcessVisitsBatch(CityVilleDbContext context, ILogger<ProcessVisitsBatch> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        // TODO: Add offline simulation
        var content = @params[0] as ASObject;

        if (content is null) throw new Exception("ProcessVisitsBatch content is null");

        // Key => ID
        // Value => visits count
        var visits = new Dictionary<int, int>();

        foreach (var (key, value) in content)
        {
            if (key is null || value is null) continue;

            if (int.TryParse(key, out var id))
            {
                var dictValue = value as ASObject;

                if (dictValue is null) continue;

                foreach (var (action, actionInformations) in dictValue)
                {
                    logger.LogDebug($"Add count for ID {id} with ACTION {action}");

                    var dictActionInformations = actionInformations as ASObject;

                    if (dictActionInformations is null)
                    {
                        logger.LogDebug($"Action information is not a dictionary?");
                        continue;
                    }

                    if (dictActionInformations.TryGetValue("count", out var flatCount))
                    {
                        var count = int.Parse(flatCount.ToString());

                        if (visits.ContainsKey(id))
                        {
                            visits[id] += count;
                        }
                        else
                        {
                            visits[id] = count;
                        }
                    }
                    else
                    {
                        logger.LogDebug($"Unable to find count inside dict {dictActionInformations}");
                    }
                }
            }
        }

        var ids = visits.Keys.ToList();
        var counts = visits.Values.ToList();

        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(w => w.ClassName == BuildingClassType.Business && (ids.Contains(w.WorldFlatId) || ids.Contains(w.TempId))))
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        foreach (var obj in user.GetWorld().Objects)
        {
            var index = ids.IndexOf(obj.WorldFlatId);

            if (index == -1)
            {
                // Not found with current server ID, check with clientId
                index = ids.IndexOf(obj.TempId);
            }

            if (index == -1) continue;

            var newCount = counts[index];
            obj.UpdateVisits(newCount);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}