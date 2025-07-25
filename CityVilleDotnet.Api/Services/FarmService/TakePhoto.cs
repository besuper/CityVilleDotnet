using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.FarmService;

public class TakePhoto(ILogger<TakePhoto> logger) : AmfService
{
    public override Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        // TODO: This endpoint is not used to take any photos, currently used as a debug method

        var content = @params[0];

        logger.LogInformation($"Received debug from client : {content}");

        return Task.FromResult(GatewayService.CreateEmptyResponse());
    }
}