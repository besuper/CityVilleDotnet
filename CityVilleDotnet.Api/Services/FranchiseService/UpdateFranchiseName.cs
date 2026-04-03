using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FranchiseService;

public class UpdateFranchiseName(CityVilleDbContext context) : AmfService<UpdateFranchiseNameRequest>
{
    public override async Task<ASObject> HandlePacket(UpdateFranchiseNameRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Franchises)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Can't find player with UserId");

        player.UpdateFranchiseName(request.FranchiseType, request.FranchiseName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            { "name", request.FranchiseName }
        });
    }
}

public class UpdateFranchiseNameRequest
{
    [AmfParam(0)] public string FranchiseType { get; set; } = string.Empty;
    [AmfParam(1)] public string FranchiseName { get; set; } = string.Empty;
}