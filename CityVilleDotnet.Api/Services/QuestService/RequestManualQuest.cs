using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Persistence;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.QuestService;

internal sealed class RequestManualQuest(CityVilleDbContext context) : AmfService(context)
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
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
