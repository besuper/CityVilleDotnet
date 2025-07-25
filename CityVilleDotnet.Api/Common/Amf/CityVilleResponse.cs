using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Api.Common.Amf;

public class CityVilleResponse
{
    private int ErrorType { get; set; } = (int)GameErrorType.NoError;
    private string? ErrorData { get; set; }
    private ASObject Metadata { get; set; } = new ASObject();
    private object PacketData { get; set; } = new ASObject();

    public CityVilleResponse Error(GameErrorType errorType)
    {
        ErrorType = (int)errorType;
        return this;
    }

    public CityVilleResponse MetaData(object metadata)
    {
        Metadata = (ASObject)metadata;
        return this;
    }

    public CityVilleResponse Data(object data)
    {
        PacketData = data;
        return this;
    }

    public CityVilleResponse ErrorMessage(string errorData)
    {
        ErrorData = errorData;
        return this;
    }

    public CityVilleResponse GameData(object data)
    {
        Metadata["gamedata"] = data;
        return this;
    }

    public CityVilleResponse PushData(object data)
    {
        Metadata["pushdata"] = data;
        return this;
    }

    public ASObject ToObject()
    {
        return new ASObject
        {
            ["errorType"] = ErrorType,
            ["errorData"] = ErrorData ?? string.Empty,
            ["metadata"] = Metadata,
            ["data"] = PacketData,
            ["serverTime"] = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds
        };
    }

    public static implicit operator ASObject(CityVilleResponse d) => d.ToObject();
}