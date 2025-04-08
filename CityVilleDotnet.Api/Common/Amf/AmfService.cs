using CityVilleDotnet.Persistence;
using FluorineFx;

namespace CityVilleDotnet.Api.Common.Amf;

public class AmfService
{
    protected readonly CityVilleDbContext _context;
    public AmfService(CityVilleDbContext context)
    {
        _context = context;
    }
    public virtual async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken = default)
    {
        throw new Exception("Not implemented");
    }
}
