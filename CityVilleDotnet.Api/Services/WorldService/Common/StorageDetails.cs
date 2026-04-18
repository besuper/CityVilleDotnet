using CityVilleDotnet.Api.Common.Amf;

namespace CityVilleDotnet.Api.Services.WorldService.Common;

public class StorageDetails
{
    [AmfParam("storageKey")] public string Key { get; set; } = string.Empty;
}