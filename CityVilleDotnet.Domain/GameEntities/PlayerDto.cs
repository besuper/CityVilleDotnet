using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.EnumExtensions;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public class PlayerDto
{
    [JsonPropertyName("uid")] public string Uid { get; set; } = "333";

    [JsonPropertyName("lastTrackingTimestamp")]
    public int LastTrackingTimestamp { get; set; } = 0;

    [JsonPropertyName("playerNews")] public List<object> PlayerNews { get; set; } = [];

    [JsonPropertyName("neighbors")] public List<NeighborDto> Neighbors { get; set; } = [];

    [JsonPropertyName("wishlist")] public List<object> Wishlist { get; set; } = [];

    [JsonPropertyName("options")] public OptionsDto? Options { get; set; }

    [JsonPropertyName("commodities")] public CommoditiesDto? Commodities { get; set; }

    [JsonPropertyName("inventory")] public InventoryDto? Inventory { get; set; }

    [JsonPropertyName("gold")] public int Gold { get; set; } = 500;

    [JsonPropertyName("cash")] public int Cash { get; set; } = 0;

    [JsonPropertyName("level")] public int Level { get; set; } = 1;

    [JsonPropertyName("xp")] public int Xp { get; set; } = 0;

    [JsonPropertyName("socialLevel")] public int SocialLevel { get; set; } = 1;

    [JsonPropertyName("socialXp")] public int SocialXp { get; set; } = 0;

    [JsonPropertyName("energy")] public int Energy { get; set; } = 12;

    [JsonPropertyName("energyMax")] public int EnergyMax { get; set; } = 12;
    [JsonPropertyName("lastEnergyCheck")] public int LastEnergyCheck { get; set; } = 0;

    [JsonPropertyName("seenFlags")] public ASObject SeenFlags { get; set; } = new ASObject();

    [JsonPropertyName("expansionsPurchased")]
    public int ExpansionsPurchased { get; set; } = 0;

    [JsonPropertyName("collections")] public ASObject Collections { get; set; } = new();

    [JsonPropertyName("completedCollections")]
    public ASObject CompletedCollections { get; set; } = new();

    [JsonPropertyName("licenses")] public ASObject Licenses { get; set; } = new();
    
    [JsonPropertyName("Orders")] public ASObject Orders { get; set; } = new();
    
    [JsonPropertyName("rollCounter")] public int RollCounter { get; set; } = 0;
}

public static class PlayerDtoMapper
{
    public static PlayerDto ToDto(this Player model)
    {
        return new PlayerDto()
        {
            Uid = model.Uid,
            Cash = model.Cash,
            Collections = new ASObject(model.Collections
                .GroupBy(item => item.Name)
                .ToDictionary(
                    group => group.Key, object (group) => new ASObject(
                        group.SelectMany(x => x.Items).ToDictionary(x => x.Name, x => (object)x.Amount))
                )),
            Commodities = new CommoditiesDto
            {
                Storage = new StorageDto
                {
                    Goods = model.Goods
                }
            },
            CompletedCollections = new ASObject(model.Collections.Where(x => x.Completed > 0).ToDictionary(x => x.Name, x => (object)x.Completed)),
            Energy = model.Energy,
            EnergyMax = model.EnergyMax,
            LastEnergyCheck = model.GetLastCheckEnergyTimestamp(),
            ExpansionsPurchased = model.ExpansionsPurchased,
            Gold = model.Gold,
            Inventory = new InventoryDto {
                Count = model.CountIventoryItems(),
                Items = new ASObject(model.InventoryItems.ToDictionary(x => x.Name, x => (object)x.Amount))
            },
            LastTrackingTimestamp = model.LastTrackingTimestamp,
            Level = model.Level,
            Licenses = new ASObject(model.Licenses.ToDictionary(x => x.Name, x => (object)x.Amount)),
            Neighbors = [],
            Options = new OptionsDto
            {
                MusicDisabled = model.MusicDisabled,
                SfxDisabled = model.SfxDisabled,
            },
            PlayerNews = model.PlayerNews,
            RollCounter = model.RollCounter,
            SeenFlags = new ASObject(model.SeenFlags.ToDictionary(x => x.Key, x => (object)true)),
            Wishlist = model.Wishlist,
            Xp = model.Xp,
            SocialLevel = model.SocialLevel,
            SocialXp = model.SocialXp,
            Orders = BuildOrdersAsObject(model)
        };
    }
    
    private static ASObject BuildOrdersAsObject(Player model)
    {
        var root = new ASObject();

        // TODO: Add VisitorHelp and TrainOrder
        foreach (var order in model.LotOrders)
        {
            var orderTypeKey = order.OrderType.ToDescriptionString();              // "order_lot"
            var transmissionKey = order.TransmissionStatus.ToDescriptionString();  // "sent"/"received"
            var stateKey = order.OrderState.ToDescriptionString();                 // "pending"/"accepted"/"denied"
            
            var isReceived = transmissionKey == "received";
            var otherUserId = isReceived ? $"{order.SenderId}:{order.SenderId}" : $"{order.RecipientId}:{order.RecipientId}";
            
            if (!root.ContainsKey(orderTypeKey))
                root[orderTypeKey] = new ASObject();
            
            var byTransmission = (ASObject)root[orderTypeKey]!;

            if (!byTransmission.ContainsKey(transmissionKey))
                byTransmission[transmissionKey] = new ASObject();
            
            var byState = (ASObject)byTransmission[transmissionKey]!;

            if (!byState.ContainsKey(stateKey))
                byState[stateKey] = new ASObject();
            
            var byOtherUser = (ASObject)byState[stateKey]!;

            if (!byOtherUser.ContainsKey(otherUserId))
                byOtherUser[otherUserId] = new ASObject();
            
            var orderParams = new ASObject
            {
                ["senderID"] = order.SenderId,
                ["recipientID"] = order.RecipientId,
                ["timeSent"] = order.TimeSent,
                ["lastTimeReminded"] = order.LastTimeReminded,
                ["orderType"] = orderTypeKey,
                ["orderState"] = stateKey,
                ["transmissionStatus"] = transmissionKey,

                ["lotId"] = order.LotId,
                ["resourceType"] = order.ResourceType,
                ["orderResourceName"] = order.OrderResourceName,
                ["constructionCount"] = order.ConstructionCount,
                ["offsetX"] = order.OffsetX,
                ["offsetY"] = order.OffsetY
            };

            byOtherUser[otherUserId] = orderParams;
        }
        
        foreach (var order in model.VisitorHelpOrders)
        {
            var orderTypeKey = order.OrderType.ToDescriptionString();              // "order_lot"
            var transmissionKey = order.TransmissionStatus.ToDescriptionString();  // "sent"/"received"
            var stateKey = order.OrderState.ToDescriptionString();                 // "pending"/"accepted"/"denied"
            
            var isReceived = transmissionKey == "received";
            var otherUserId = isReceived ? $"{order.SenderId}:{order.SenderId}" : $"{order.RecipientId}:{order.RecipientId}";
            
            if (!root.ContainsKey(orderTypeKey))
                root[orderTypeKey] = new ASObject();
            
            var byTransmission = (ASObject)root[orderTypeKey]!;

            if (!byTransmission.ContainsKey(transmissionKey))
                byTransmission[transmissionKey] = new ASObject();
            
            var byState = (ASObject)byTransmission[transmissionKey]!;

            if (!byState.ContainsKey(stateKey))
                byState[stateKey] = new ASObject();
            
            var byOtherUser = (ASObject)byState[stateKey]!;

            if (!byOtherUser.ContainsKey(otherUserId))
                byOtherUser[otherUserId] = new ASObject();
            
            var orderParams = new ASObject
            {
                ["senderID"] = order.SenderId,
                ["recipientID"] = order.RecipientId,
                ["timeSent"] = order.TimeSent,
                ["lastTimeReminded"] = order.LastTimeReminded,
                ["orderType"] = orderTypeKey,
                ["orderState"] = stateKey,
                ["transmissionStatus"] = transmissionKey,

                ["helpTargets"] = order.HelpTargets,
                ["status"] = order.Status.ToDescriptionString()
            };

            byOtherUser[otherUserId] = orderParams;
        }

        return root;
    }

}