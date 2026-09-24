namespace CityVilleDotnet.Domain.Entities;

public class Coupon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? WorldFlatId { get; set; }

    public Coupon(string name, int? worldFlatId)
    {
        Name = name;
        WorldFlatId = worldFlatId;
    }
}
