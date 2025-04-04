using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.QuestService.requestManualQuest;

internal sealed class RequestManualQuest : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] _params)
    {
        if (_params.Length < 1)
        {
            return GatewayService.CreateEmptyResponse();
        }

        var questName = _params[0].ToString();

        Console.WriteLine($"Started new quest {questName}");

        var response = new CityVilleResponse(333, new ASObject() { { "questStarted", 1 } });

        return response;
    }
}
