using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FranchiseService;

public class UpdateFranchiseName(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var franchiseType = (string)@params[0];
        var franchiseName = (string)@params[1];
        
        var player = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Franchises)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (player is null) throw new Exception("Can't find player with UserId");
        
        player.UpdateFranchiseName(franchiseType, franchiseName);

        await context.SaveChangesAsync(cancellationToken);
        
        var response = new ASObject
        {
            { "name", franchiseName }
        };

        return new CityVilleResponse().Data(response).ToObject();
    }
}