namespace CityVilleDotnet.Domain.Entities;

public class Inventory
{
    public Guid Id { get; set; }
    public int Count { get; set; }

    /*[JsonPropertyName("Items")]
    public Dictionary<object, object> Items { get; set; }*/
}
