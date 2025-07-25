using CityVilleDotnet.Api.Common.Amf;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.UserService;

public class GetZEventCount : AmfService
{
    public override Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        // Not useful for now
        
        var response = new ASObject
        {
            { "count", 0 }
        };

        return Task.FromResult(new CityVilleResponse().Data(response).ToObject());
    }
}