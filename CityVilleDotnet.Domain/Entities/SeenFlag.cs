namespace CityVilleDotnet.Domain.Entities;

public class SeenFlag
{
    public int Id { get; set; }
    public string Key { get; set; }

    public SeenFlag(string key)
    {
        Key = key;
    }
}