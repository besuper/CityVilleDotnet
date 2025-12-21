using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

internal sealed class ProcessVisitsBatch(CityVilleDbContext context, ILogger<ProcessVisitsBatch> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        // TODO: Add offline simulation
        if (@params.Length != 2) throw new Exception("Invalid params count");

        var idsArray = @params[0] as object[];
        var countsArray = @params[1] as object[];

        if (idsArray is null || countsArray is null) throw new Exception("Invalid params");

        var ids = idsArray.Cast<int>().ToArray();
        var counts = countsArray.Cast<int>().ToArray();
        
        if (ids.Length != counts.Length) throw new Exception("Invalid params count");

        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(w => ids.AsEnumerable().Contains(w.WorldFlatId)))
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        foreach (var obj in user.GetWorld().Objects)
        {
            var index = ids.IndexOf(obj.WorldFlatId);

            if (index == -1)
            {
                logger.LogError("Can't find count for object {WorldFlatId}", obj.WorldFlatId);
                continue;
            }

            var newCount = counts[index];
            obj.UpdateVisits(newCount);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}