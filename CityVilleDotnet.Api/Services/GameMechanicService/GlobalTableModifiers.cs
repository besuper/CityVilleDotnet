using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

// Only client side transaction, just implemented to ack
internal sealed class GlobalTableModifiers : AmfService
{
    public override Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        return Task.FromResult(GatewayService.CreateEmptyResponse());
    }
}
