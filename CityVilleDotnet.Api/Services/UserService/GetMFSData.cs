using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class GetMFSData(CityVilleDbContext context, IHttpContextAccessor httpContextAccessor) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsNoTracking()
            .Include(u => u.Friends)
            .ThenInclude(x => x.FriendPlayer)
            .FirstOrDefaultAsync(u => u.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var baseUrl = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}";

        var friends = player.Friends.Where(p => !p.FriendPlayer.IsSamantha()).Select(f => f.ToSocialNetworkUserDto(baseUrl)).ToList();

        return new CityVilleResponse().Data(new ASObject
        {
            ["nonAppFriends"] = AmfConverter.Convert(friends),
            ["appFriends"] = new List<object>()
        });
    }
}