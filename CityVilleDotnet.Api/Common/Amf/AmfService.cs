﻿using FluorineFx;

namespace CityVilleDotnet.Api.Common.Amf;

public class AmfService
{
    public AmfService()
    {
    }
    public virtual Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        throw new Exception("Not implemented");
    }
}
