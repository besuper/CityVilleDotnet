namespace CityVilleDotnet.Domain.Entities;

public class GlobalTableOverride(string keyword, string table, string source)
{
    public int Id { get; }
    public string Keyword { get; private set; } = keyword;
    public string Table { get; private set; } = table;
    public string Source { get; private set; } = source;
}
