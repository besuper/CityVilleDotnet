using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FranchiseService;

public class UpdateFranchiseName(CityVilleDbContext context) : AmfService<UpdateFranchiseNameRequest>
{
    public override async Task<ASObject> HandlePacket(UpdateFranchiseNameRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var player = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Franchises)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken);

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
