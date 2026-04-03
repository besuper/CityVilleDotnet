using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Persistence;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.UserService;

public class OnInteractive(CityVilleDbContext context, ILogger<OnInteractive> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        logger.LogDebug("Received request OnInteractive {Objects}", (object?)@params);

        return GatewayService.CreateEmptyResponse();
    }
}