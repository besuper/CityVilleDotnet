using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Common.Persistence;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Api.Services.UserService.Domain;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService.completeTutorial;

public class CompleteTutorial(CityVilleDbContext context) : AmfService(context)
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId)
    {
        var user = await context.Set<User>()
            .Include(x => x.UserInfo)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            throw new Exception("User should not be null");
        }

        user.CompleteTutorial();

        await context.SaveChangesAsync();

        return GatewayService.CreateEmptyResponse();
    }
}
