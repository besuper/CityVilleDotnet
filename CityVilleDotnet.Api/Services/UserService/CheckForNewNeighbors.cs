using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.UserService;

public class CheckForNewNeighbors : AmfService
{
    public override Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        // Response is not used in the client
        return Task.FromResult(GatewayService.CreateEmptyResponse());
    }
}