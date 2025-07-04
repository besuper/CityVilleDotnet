﻿using FluorineFx;

namespace CityVilleDotnet.Api.Common.Amf;

public class CityVilleResponse
{
    public int ErrorType { get; private set; } = 0;
    public int UserId { get; private set; }
    public object Metadata { get; private set; } = new ASObject();
    public object Data { get; private set; } = new ASObject();
    public double ServerTime { get; private set; }

    public CityVilleResponse(int errorType, int userId, object metadata, object data)
    {
        ErrorType = errorType;
        UserId = userId;
        Metadata = metadata;
        Data = data;
    }

    public CityVilleResponse(int errorType, int userId, object metadata)
    {
        ErrorType = errorType;
        UserId = userId;
        Metadata = metadata;
    }

    public CityVilleResponse(int userId, object data)
    {
        UserId = userId;
        Data = data;
    }

    public CityVilleResponse(int errorType, int userId)
    {
        ErrorType = errorType;
        UserId = userId;
    }

    private ASObject ToObject()
    {
        return new ASObject
        {
            ["errorType"] = ErrorType,
            //obj["userId"] = 333;
            ["metadata"] = Metadata,
            ["data"] = Data,
            ["serverTime"] = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds
        };
    }

    public static implicit operator ASObject(CityVilleResponse d) => d.ToObject();
}