using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class ExpansionsGate
{
    [XmlElement("expansionGates")] public required List<ExpansionsGateContainer> ExpansionGates { get; set; }
}

[Serializable]
public class ExpansionsGateContainer
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlElement("expansions")] public required ExpansionsContainer Expansions { get; set; }
}

[Serializable]
public class ExpansionsContainer
{
    [XmlElement("expansion")] public required List<ExpansionSetting> Expansions { get; set; }
}

[Serializable]
[XmlRoot("expansion")]
public class ExpansionSetting
{
    [XmlAttribute("permits2")] public int Permits { get; set; }
    [XmlAttribute("lightlevel")] public int LightLevel { get; set; }
    [XmlAttribute("cost")] public int Cost { get; set; }
}
