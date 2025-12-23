using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Common.Extensions;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Persistence;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.UserService;

public class UpdateTopFriends(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        // TODO: understand what this service is used for
        //var topFriends = @params.GetObjectArray(0);

        return GatewayService.CreateEmptyResponse();
    }
}