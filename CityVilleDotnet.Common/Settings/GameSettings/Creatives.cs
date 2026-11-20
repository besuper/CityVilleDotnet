using System.Xml.Serialization;

namespace CityVilleDotnet.Common.Settings.GameSettings;

[Serializable]
[XmlRoot("creatives")]
public class CreativesContainer
{
    [XmlArray("feeds"), XmlArrayItem("feed")] public List<FeedCreative> Feeds { get; set; } = [];
}

[Serializable]
public class FeedCreative
{
    [XmlAttribute("name")] public required string Name { get; set; }
    [XmlElement("attachment")] public FeedCreativeAttachment? Attachment { get; set; }
}

[Serializable]
public class FeedCreativeAttachment
{
    [XmlAttribute("name")] public string? Name { get; set; }
    [XmlAttribute("description")] public string? Description { get; set; }
    [XmlElement("media")] public List<FeedCreativeMedia> Media { get; set; } = [];
}

[Serializable]
public class FeedCreativeMedia
{
    [XmlAttribute("src")] public string? Src { get; set; }
}
