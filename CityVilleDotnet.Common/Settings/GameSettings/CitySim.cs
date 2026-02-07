using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
[XmlRoot("citysim")]
public class CitySim
{
    [XmlElement("populations")] public required PopulationContainer PopulationContainer { get; set; }
}

[Serializable]
public class PopulationContainer
{
    [XmlElement("population")] public required List<Population> Populations { get; set; }
}

[Serializable]
[XmlRoot("population")]
public class Population
{
    [XmlAttribute("id")] public required string Id { get; set; }
    
    [XmlAttribute("baseCap")]
    public string BaseCapString
    {
        get => BaseCap.ToString();
        set => BaseCap = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    [XmlIgnore] public int BaseCap { get; set; }
}