using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.QuestService;

internal sealed class RequestManualQuest(ILogger<RequestManualQuest> _logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        if (_params.Length < 1)
            return GatewayService.CreateEmptyResponse();

        var questName = _params[0].ToString();

        _logger.LogDebug($"Started new quest {questName}");

        var response = new CityVilleResponse(333, new ASObject() { { "questStarted", 1 } });

        return response;
    }
}
