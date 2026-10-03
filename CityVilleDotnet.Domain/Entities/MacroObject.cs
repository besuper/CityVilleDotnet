namespace CityVilleDotnet.Domain.Entities;

public class MacroObject
{
    public int Id { get; set; }
    public string Name { get; private set; } = string.Empty;
    public string ParentItemName { get; private set; } = string.Empty;

    private MacroObject()
    {
    }

    public MacroObject(string name, string parentItemName)
    {
        Name = name;
        ParentItemName = parentItemName;
    }
}
