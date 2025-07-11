using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class CollectionContainer
{
    [XmlElement("collection")] public required List<CollectionSetting> Collections { get; set; }
}

[Serializable]
[XmlRoot("collection")]
public class CollectionSetting
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlElement("collectables")] public required CollectableContainer Collectables { get; set; }
}

[Serializable]
public class CollectableContainer
{
    [XmlElement("collectable")] public required List<Collectable> Collectables { get; set; }
}

[Serializable]
[XmlRoot("collectable")]
public class Collectable
{
    [XmlAttribute("name")] public required string Name { get; set; }
}