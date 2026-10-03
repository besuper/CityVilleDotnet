namespace CityVilleDotnet.Domain.Entities;

public class FeatureRollCounter(string feature, int count)
{
    public int Id { get; }
    public string Feature { get; private set; } = feature;
    public int Count { get; private set; } = count;

    public void Increment()
    {
        Count++;
    }
}
