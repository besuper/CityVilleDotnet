using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

public class UpdateWorldSummary : AmfService
{
    public override Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        // Client sends worldId but onComplete is empty
        return Task.FromResult(GatewayService.CreateEmptyResponse());
    }
}
