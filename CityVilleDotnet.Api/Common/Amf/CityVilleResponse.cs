using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Api.Common.Amf;

public class CityVilleResponse
{
    private int ErrorType { get; set; } = (int)GameErrorType.NoError;
    private string? ErrorData { get; set; }
    private object Metadata { get; set; } = new ASObject();
    private object GameData { get; set; } = new ASObject();

    public CityVilleResponse Error(GameErrorType errorType)
    {
        ErrorType = (int)errorType;
        return this;
    }

    public CityVilleResponse MetaData(object metadata)
    {
        Metadata = metadata;
        return this;
    }

    public CityVilleResponse Data(object data)
    {
        GameData = data;
        return this;
    }
    
    public CityVilleResponse ErrorMessage(string errorData)
    {
        ErrorData = errorData;
        return this;
    }

    public ASObject ToObject()
    {
        return new ASObject
        {
            ["errorType"] = ErrorType,
            ["errorData"] = ErrorData ?? string.Empty,
            ["metadata"] = Metadata,
            ["data"] = GameData,
            ["serverTime"] = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds
        };
    }

    public static implicit operator ASObject(CityVilleResponse d) => d.ToObject();
}