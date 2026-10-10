using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.AchievementsService;

internal sealed class Update : AmfService
{
    public override Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        return Task.FromResult(GatewayService.CreateEmptyResponse());
    }
}
