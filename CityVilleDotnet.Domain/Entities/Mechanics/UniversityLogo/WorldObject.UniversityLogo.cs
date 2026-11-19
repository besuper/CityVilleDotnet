namespace CityVilleDotnet.Domain.Entities;

public partial class WorldObject
{
    public string? UniversityLogo { get; private set; }

    public void SetUniversityLogo(string? logo)
    {
        UniversityLogo = logo;
    }
}
