using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class GameFriendData
{
    [JsonPropertyName("zid")]
    public int Zid { get; set; }

    [JsonPropertyName("snuid")]
    public int Snuid { get; set; }

    [JsonPropertyName("snid")]
    public int Snid { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("pic_square")]
    public string Picture { get; set; }

    [JsonPropertyName("sex")]
    public string Gender { get; set; }
}
