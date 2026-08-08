namespace CityVilleDotnet.Domain.Entities;

public class SamanthaWorldVersion
{
    public int Id { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    public SamanthaWorldVersion(int id, DateTime updatedAt)
    {
        Id = id;
        UpdatedAt = updatedAt;
    }
}