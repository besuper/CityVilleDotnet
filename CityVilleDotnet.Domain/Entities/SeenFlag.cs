namespace CityVilleDotnet.Domain.Entities;

public class SeenFlag
{
    public Guid Id { get; set; }
    public string Key { get; set; }

    public SeenFlag(string key)
    {
        Id = Guid.NewGuid();
        Key = key;
    }
}