namespace CityVilleDotnet.Domain.Entities;

public class MacroObject
{
    public int Id { get; set; }
    public string Name { get; private set; }
    public string ParentItemName { get; private set; }

    private MacroObject()
    {
    }

    public MacroObject(string name, string parentItemName)
    {
        Name = name;
        ParentItemName = parentItemName;
    }
}
