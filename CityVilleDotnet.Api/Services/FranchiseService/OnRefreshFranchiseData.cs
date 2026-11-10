using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FranchiseService;

public sealed class OnRefreshFranchiseData(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsNoTracking()
            .Include(x => x.Franchises)
            .ThenInclude(x => x.Locations)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        return new CityVilleResponse().Data(new ASObject
        {
            ["franchises"] = AmfConverter.Convert(player.Franchises.Select(x => x.ToDto()).ToList())
        });
    }
}
