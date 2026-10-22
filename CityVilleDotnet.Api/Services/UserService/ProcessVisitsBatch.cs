using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

internal sealed class ProcessVisitsBatch(CityVilleDbContext context, ILogger<ProcessVisitsBatch> logger) : AmfService<ProcessVisitsBatchRequest>
{
    public override async Task<ASObject> HandlePacket(ProcessVisitsBatchRequest request, Guid playerId, CancellationToken cancellationToken)
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

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects.Where(w => ids.Contains(w.WorldFlatId) || ids.Contains(w.TempId)))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var world = player.GetWorld();

        foreach (var obj in world.Objects)
        {
            var id = ids.Contains(obj.WorldFlatId) ? obj.WorldFlatId
                : ids.Contains(obj.TempId) ? obj.TempId
                : -1;

            if (id == -1) continue;

            if (!obj.UpdateVisits(visits[id]))
            {
                logger.LogWarning("Visits refused for player {PlayerId} on building {ObjectId} (flat id {FlatId}) {ItemName} {ClassName} {State}", playerId, obj.Id, id, obj.ItemName, obj.ClassName, obj.State);
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class ProcessVisitsBatchRequest
{
    [AmfParam(0)] public ASObject Content { get; set; } = [];
}