namespace CityVilleDotnet.Api.Services.UserService.Domain;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class User
{
    [JsonPropertyName("idCounter")]
    public int IdCounter { get; set; }

    [JsonPropertyName("userInfo")]
    public UserInfo UserInfo { get; set; }

    [JsonPropertyName("Franchises")]
    public List<object> Franchises { get; set; } = new List<object>();

    internal int GetGold()
    {
        return UserInfo.Player.Gold;
    }

    internal int GetExperience()
    {
        return UserInfo.Player.Xp;
    }

    internal int GetGoods()
    {
        return UserInfo.Player.Commodities.Storage.Goods;
    }

    internal int GetCash()
    {
        return UserInfo.Player.Cash;
    }

    internal int GetLevel()
    {
        return UserInfo.Player.Level;
    }

    internal World GetWorld()
    {
        return UserInfo.World;
    }

    internal void AddGold(int amount)
    {
        UserInfo.Player.Gold += amount;
    }
}

public class UserInfo
{
    [JsonPropertyName("worldName")]
    public string WorldName { get; set; }

    [JsonPropertyName("is_new")]
    public bool IsNew { get; set; }

    [JsonPropertyName("firstDay")]
    public bool FirstDay { get; set; }

    [JsonPropertyName("creationTimestamp")]
    public int CreationTimestamp { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("player")]
    public Player Player { get; set; }

    [JsonPropertyName("world")]
    public World World { get; set; }
}

public class Player
{
    [JsonPropertyName("uid")]
    public int Uid { get; set; }

    [JsonPropertyName("lastTrackingTimestamp")]
    public int LastTrackingTimestamp { get; set; }

    [JsonPropertyName("playerNews")]
    public List<object> PlayerNews { get; set; }

    [JsonPropertyName("neighbors")]
    public List<object> Neighbors { get; set; }

    [JsonPropertyName("wishlist")]
    public List<object> Wishlist { get; set; }

    [JsonPropertyName("options")]
    public Options Options { get; set; }

    [JsonPropertyName("commodities")]
    public Commodities Commodities { get; set; }

    [JsonPropertyName("inventory")]
    public Inventory Inventory { get; set; }

    [JsonPropertyName("gold")]
    public int Gold { get; set; }

    [JsonPropertyName("cash")]
    public int Cash { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("xp")]
    public int Xp { get; set; }

    [JsonPropertyName("energy")]
    public int Energy { get; set; }

    [JsonPropertyName("energyMax")]
    public int EnergyMax { get; set; }

    [JsonPropertyName("seenFlags")]
    public Dictionary<object, object> SeenFlags { get; set; }

    [JsonPropertyName("expansionsPurchased")]
    public int ExpansionsPurchased { get; set; }

    [JsonPropertyName("collections")]
    public Dictionary<object, object> Collections { get; set; }

    [JsonPropertyName("completedCollections")]
    public Dictionary<object, object> CompletedCollections { get; set; }

    [JsonPropertyName("licenses")]
    public Dictionary<object, object> Licenses { get; set; }

    [JsonPropertyName("rollCounter")]
    public int RollCounter { get; set; }
}

public class Options
{
    [JsonPropertyName("sfxDisabled")]
    public bool SfxDisabled { get; set; }

    [JsonPropertyName("musicDisabled")]
    public bool MusicDisabled { get; set; }
}

public class Commodities
{
    [JsonPropertyName("storage")]
    public Storage Storage { get; set; }
}

public class Storage
{
    [JsonPropertyName("goods")]
    public int Goods { get; set; }
}

public class Inventory
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("Items")]
    public Dictionary<object, object> Items { get; set; }
}

public class World
{
    [JsonPropertyName("sizeX")]
    public int SizeX { get; set; } = 36;

    [JsonPropertyName("sizeY")]
    public int SizeY { get; set; } = 36;

    [JsonPropertyName("mapRects")]
    public List<MapRect> MapRects { get; set; }

    [JsonPropertyName("citySim")]
    public CitySim CitySim { get; set; }

    [JsonPropertyName("objects")]
    public List<WorldObject> Objects { get; set; }
}

public class MapRect
{
    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }
}

public class CitySim
{
    [JsonPropertyName("population")]
    public int Population { get; set; }

    [JsonPropertyName("populationCap")]
    public int PopulationCap { get; set; }

    [JsonPropertyName("potentialPopulation")]
    public int PotentialPopulation { get; set; }
}

public class WorldObject
{
    [JsonPropertyName("itemName")]
    public string ItemName { get; set; }

    [JsonPropertyName("className")]
    public string ClassName { get; set; }

    [JsonPropertyName("contractName")]
    public string? ContractName { get; set; }

    [JsonPropertyName("components")]
    public object? Components { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }

    [JsonPropertyName("tempId")]
    public int TempId { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("direction")]
    public int Direction { get; set; }

    [JsonPropertyName("position")]
    public WorldObjectPosition Position { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}

public class WorldObjectPosition
{
    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    [JsonPropertyName("z")]
    public int? Z { get; set; }
}