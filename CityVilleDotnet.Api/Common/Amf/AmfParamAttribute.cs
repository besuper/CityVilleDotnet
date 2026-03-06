namespace CityVilleDotnet.Api.Common.Amf;

[AttributeUsage(AttributeTargets.Property)]
public class AmfParamAttribute : Attribute
{
    public int Index { get; } = -1;
    public string? Key { get; }

    public AmfParamAttribute(int index) => Index = index;
    public AmfParamAttribute(string key) => Key = key;
}
