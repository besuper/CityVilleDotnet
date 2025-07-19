using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class SetCityName(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var newName = (string)@params[1] ?? throw new Exception("World name can't be null");

        var world = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Select(x => x.World)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Can't to find world with UserId");

        var name = world.SetWorldName(newName);

        var response = new ASObject
        {
            ["name"] = name
        };

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(response);
    }
}