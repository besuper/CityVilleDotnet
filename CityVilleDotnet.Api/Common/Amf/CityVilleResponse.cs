using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Api.Common.Amf;

public class CityVilleResponse
{
    private int ErrorType { get; set; } = (int)GameErrorType.NoError;
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

    private ASObject ToObject()
    {
        return new ASObject
        {
            ["errorType"] = ErrorType,
            ["metadata"] = Metadata,
            ["data"] = GameData,
            ["serverTime"] = ServerUtils.GetCurrentTime()
        };
    }

    public static implicit operator ASObject(CityVilleResponse d) => d.ToObject();
}