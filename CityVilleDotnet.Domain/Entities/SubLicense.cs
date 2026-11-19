namespace CityVilleDotnet.Domain.Entities;

public class SubLicense
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public int Mask { get; private set; }

    public SubLicense(string name)
    {
        Name = name;
    }

    public bool Has(int key)
    {
        return (Mask & (1 << key)) != 0;
    }

    public void Add(int key)
    {
        Mask |= 1 << key;
    }
}
