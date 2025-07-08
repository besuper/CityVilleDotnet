namespace CityVilleDotnet.Domain.Entities;

public class Player
{
    public Guid Id { get; set; }
    public string Uid { get; set; } = "333";
    public int Snuid { get; set; }
    public int LastTrackingTimestamp { get; set; } = 0;
    public List<object> PlayerNews { get; set; } = [];
    public List<object> Wishlist { get; set; } = [];
    public bool SfxDisabled { get; set; } = false;
    public bool MusicDisabled { get; set; } = false;
    public Inventory? Inventory { get; set; }
    public int Gold { get; set; } = 500;
    public int Goods { get; set; } = 100;
    public int Cash { get; set; } = 0;
    public int Level { get; set; } = 1;
    public int Xp { get; set; } = 0;
    public int SocialLevel { get; set; } = 1;
    public int SocialXp { get; set; } = 0;
    public int Energy { get; set; } = 12;
    public int EnergyMax { get; set; } = 12;
    public List<SeenFlag> SeenFlags { get; set; } = new();
    public int ExpansionsPurchased { get; set; } = 0;
    public List<Collection> Collections { get; set; } = [];
    public Dictionary<object, object> Licenses { get; set; } = new Dictionary<object, object>();
    public int RollCounter { get; set; } = 0;
    public bool IsNew { get; set; } = true;
    public bool FirstDay { get; set; } = true;
    public int CreationTimestamp { get; set; } = 0;
    public string Username { get; set; } = "";
    
    public void AddItemToCollection(string collectionName, string itemName, int amount = 1)
    {
        var collection = Collections.FirstOrDefault(x => x.Name == collectionName);
        
        if (collection is null)
        {
            collection = new Collection(collectionName);
            Collections.Add(collection);
        }
        
        collection.AddItem(itemName, amount);
    }
    
    public CollectionItem? RemoveItemFromCollection(string collectionName, string itemName, int amount = 1)
    {
        var collection = Collections.FirstOrDefault(x => x.Name == collectionName);
        
        if (collection is null)
            throw new Exception($"Collection not found: {collectionName}");

        return collection.RemoveItem(itemName, amount);
    }
    
}