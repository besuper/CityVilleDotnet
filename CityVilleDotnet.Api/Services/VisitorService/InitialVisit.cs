using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Persistence;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.VisitorService;

public class InitialVisit(CityVilleDbContext context, ILogger<InitialVisit> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        // TODO: Use those params
        var type = (string)@params[0]; // neighborVisit
        var content = (ASObject)@params[1]; // { recipientId, senderId }

        var response = new ASObject
        {
            ["energyLeft"] = 4,
        };

        return new CityVilleResponse(333, AmfConverter.Convert(response));
    }
}