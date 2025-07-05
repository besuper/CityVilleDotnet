using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.QuestService;

internal sealed class RequestManualQuest(ILogger<RequestManualQuest> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        if (@params.Length < 1)
            return GatewayService.CreateEmptyResponse();

        var questName = @params[0].ToString();

        logger.LogDebug("Started new quest {QuestName}", questName);

        var response = new CityVilleResponse(333, new ASObject { { "questStarted", 1 } });

        return response;
    }
}
