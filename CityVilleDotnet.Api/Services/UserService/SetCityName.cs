using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class SetCityName(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .Include(x => x.World)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't to find user with UserId");

        var newName = (string)_params[1] ?? throw new Exception("World name can't be null");
        var name = user.SetWorldName(newName);

        var response = new ASObject
        {
            ["name"] = name
        };

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse(333, response);
    }
}
