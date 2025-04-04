using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.QuestService.Domain;

public class UserQuest
{
    [JsonPropertyName("active")]
    public Dictionary<string, Quest> Active { get; set; }

    [JsonPropertyName("completed")]
    public List<Quest> Completed { get; set; }

    [JsonPropertyName("pending")]
    public List<Quest> Pending { get; set; }
}
