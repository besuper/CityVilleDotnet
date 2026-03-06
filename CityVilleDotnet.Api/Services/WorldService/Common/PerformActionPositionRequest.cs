using CityVilleDotnet.Api.Common.Amf;

namespace CityVilleDotnet.Api.Services.WorldService.Common;

public class PerformActionPositionRequest
{
    [AmfParam("x")] public int X { get; set; }
    [AmfParam("y")] public int Y { get; set; }
    [AmfParam("z")] public int Z { get; set; }
}