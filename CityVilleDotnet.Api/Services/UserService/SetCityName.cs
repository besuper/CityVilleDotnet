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
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        var newName = (string)_params[1];

        if (newName is null)
        {
            throw new Exception("World name can't be null");
        }

        var name = user.SetWorldName(newName);

        var response = new ASObject();
        response["name"] = name;

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse(333, response);
    }
}
