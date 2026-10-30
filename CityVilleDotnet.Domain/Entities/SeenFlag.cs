namespace CityVilleDotnet.Domain.Entities;

public class SeenFlag
{
    public int Id { get; set; }
    public string Key { get; set; }
    public int? Value { get; set; }

    public SeenFlag(string key, int? value = null)
    {
        Key = key;
        Value = value;
    }
}
