using CityVilleDotnet.Common.Utils;

namespace CityVilleDotnet.Domain.Entities;

public class Flag
{
    public int Id { get; }
    public string Name { get; private set; } = string.Empty;
    public int Value { get; private set; } = 0;
    public DateTimeOffset LastModified { get; private set; }

    public Flag() {}
    
    public Flag(string name, int value)
    {
        Name = name;
        Value = value;
        LastModified = ServerUtils.TimeProvider.GetUtcNow();
    }

    public void SetValue(int value)
    {
        Value = value;
        LastModified = ServerUtils.TimeProvider.GetUtcNow();
    }
}
