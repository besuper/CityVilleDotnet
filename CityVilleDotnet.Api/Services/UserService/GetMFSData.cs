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
        var user = await context.Set<User>()
            .AsNoTracking()
            .Include(u => u.Player)
            .Include(u => u.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .FirstOrDefaultAsync(u => u.Player.Id == playerId, cancellationToken);

        if (user is null) throw new Exception("User not found");
        
        var baseUrl = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}";

        var friends = user.Friends.Where(p => !p.FriendUser.Player.IsSamantha()).Select(f => f.ToSocialNetworkUserDto(baseUrl)).ToList();

        return new CityVilleResponse().Data(new ASObject
        {
            ["nonAppFriends"] = AmfConverter.Convert(friends),
            ["appFriends"] = new List<object>()
        });
    }
}