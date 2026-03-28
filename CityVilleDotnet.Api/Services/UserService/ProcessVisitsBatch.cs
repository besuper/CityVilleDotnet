using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

internal sealed class ProcessVisitsBatch(CityVilleDbContext context) : AmfService<ProcessVisitsBatchRequest>
{
    public override async Task<ASObject> HandlePacket(ProcessVisitsBatchRequest request, Guid userId, CancellationToken cancellationToken)
    {
        // TODO: Add offline simulation
        var visits = new Dictionary<int, int>();

        foreach (var (key, value) in request.Content)
        {
            if (!int.TryParse(key, out var id) || value is not ASObject actions)
                continue;

            foreach (var (_, actionInfo) in actions)
            {
                if (actionInfo is not ASObject dict || !dict.TryGetValue("count", out var flatCount))
                    continue;

                var count = Convert.ToInt32(flatCount);

                if (!visits.TryAdd(id, count))
                    visits[id] += count;
            }
        }

        var ids = visits.Keys.ToList();

        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(w => ids.Contains(w.WorldFlatId) || ids.Contains(w.TempId)))
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't find user");

        foreach (var obj in user.GetWorld().Objects)
        {
            var id = ids.Contains(obj.WorldFlatId) ? obj.WorldFlatId
                : ids.Contains(obj.TempId) ? obj.TempId
                : -1;

            if (id == -1) continue;

            obj.UpdateVisits(visits[id]);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class ProcessVisitsBatchRequest
{
    [AmfParam(0)] public ASObject Content { get; set; } = [];
}