using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
public class WorldConfigsContainer
{
    [XmlElement("worldConfig")] public List<WorldConfigItem> WorldConfigs { get; set; } = [];
}

[Serializable]
public class WorldConfigItem
{
    [XmlAttribute("name")] public required string Name { get; set; }

    [XmlAttribute("startCameraX")] public int StartCameraX { get; set; }

    [XmlAttribute("startCameraY")] public int StartCameraY { get; set; }

    [XmlArray("ftueGrants")]
    [XmlArrayItem("ftueGrant")]
    public List<FtueGrantItem> FtueGrants { get; set; } = [];

    [XmlElement("appraisal")] public string? AppraisalId { get; set; }

    [XmlElement("enableIncentivizedExpansions")] public bool EnableIncentivizedExpansions { get; set; } = true;
}

[Serializable]
public class FtueGrantItem
{
    [XmlAttribute("type")] public required string Type { get; set; }

    [XmlAttribute("value")] public int Value { get; set; }
}
