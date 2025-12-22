using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class ExpansionsContainer
{
    [XmlElement("expansion")] public required List<ExpansionSetting> Expansions { get; set; }
}

[Serializable]
[XmlRoot("expansion")]
public class ExpansionSetting
{
    [XmlAttribute("num")] public required string Num { get; set; }
    [XmlAttribute("level")] public required string Level { get; set; }
    [XmlAttribute("permits")] public required string Permits { get; set; }
    [XmlAttribute("cost")] public required string Cost { get; set; }
    [XmlAttribute("goldMultiplier")] public required string GoldMultiplier { get; set; }
}
