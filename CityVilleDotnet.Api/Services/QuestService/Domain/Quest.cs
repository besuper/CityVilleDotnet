using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.QuestService.Domain;

public class Quest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("complete")]
    public int Complete { get; set; }

    [JsonPropertyName("progress")]
    public int[] Progress { get; set; }

    [JsonPropertyName("purchased")]
    public int[] Purchased { get; set; }
}
