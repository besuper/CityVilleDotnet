using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.RegularExpressions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Api.Common.Social;

public class FeedTextRenderer(IWebHostEnvironment webHost, ILogger<FeedTextRenderer> logger)
{
    private const string FeedsPrefix = "Feeds:";
    private static readonly Regex UnknownToken = new(@"\{\w+\}", RegexOptions.Compiled);

    private readonly ConcurrentDictionary<string, Dictionary<string, Dictionary<string, string>>> _textsByLocale = new();

    public FeedText Render(SocialPost post, string authorName, string culture)
    {
        var creative = GameSettingsManager.Instance.GetFeedCreative(post.Key);

        if (creative?.Attachment is null)
        {
            return new FeedText(null, null, null);
        }

        var locale = culture.Replace('-', '_');
        var tokens = ReadTokens(post.Data);

        tokens["user"] = authorName;
        tokens.TryAdd("cityname", authorName);
        tokens.TryAdd("cityName", authorName);

        var imageSrc = creative.Attachment.Media.FirstOrDefault(x => !string.IsNullOrEmpty(x.Src))?.Src;
        var translatedName = Translate(creative.Attachment.Name, locale, tokens);
        var translatedDescription = Translate(creative.Attachment.Description, locale, tokens);

        return new FeedText(translatedName, translatedDescription, imageSrc is null ? null : "/" + imageSrc);
    }

    private string? Translate(string? key, string locale, Dictionary<string, string> tokens)
    {
        if (key is null || !key.StartsWith(FeedsPrefix))
        {
            return null;
        }

        key = key[FeedsPrefix.Length..];

        var text = GetText("feeds", key, locale);

        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        foreach (var (token, value) in tokens)
        {
            text = text.Replace("{" + token + "}", value);
        }

        return UnknownToken.Replace(text, string.Empty);
    }

    public string ItemName(string itemName, string culture)
    {
        var text = GetText("items", $"{itemName}_friendlyName", culture.Replace('-', '_'));

        return string.IsNullOrWhiteSpace(text) ? itemName : UnknownToken.Replace(text, " ").Trim();
    }

    private string? GetText(string bundle, string key, string locale)
    {
        foreach (var candidate in new[] { locale, "en_US" })
        {
            if (_textsByLocale.GetOrAdd(candidate, ReadTextsFile).GetValueOrDefault(bundle)?.TryGetValue(key, out var text) == true)
            {
                return text;
            }
        }

        return null;
    }

    private Dictionary<string, Dictionary<string, string>> ReadTextsFile(string locale)
    {
        var path = Path.Combine(webHost.WebRootPath, "lang", $"social_texts_{locale}.json");

        if (!File.Exists(path))
        {
            logger.LogWarning("Missing social wall texts ({Path})", path);
            return [];
        }

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(path)) ?? [];
        }
        catch (JsonException e)
        {
            logger.LogError(e, "Invalid social wall texts ({Path})", path);
            return [];
        }
    }

    // Replace {} tokens like {user}, {cityName}, ...
    private static Dictionary<string, string> ReadTokens(string data)
    {
        var tokens = new Dictionary<string, string>();

        using var document = JsonDocument.Parse(string.IsNullOrEmpty(data) ? "{}" : data);

        if (document.RootElement.ValueKind != JsonValueKind.Object)
        {
            return tokens;
        }

        foreach (var property in document.RootElement.EnumerateObject())
        {
            if (property.Value.ValueKind is JsonValueKind.String or JsonValueKind.Number)
            {
                tokens[property.Name] = property.Value.ToString();
            }
        }

        return tokens;
    }
}

public record FeedText(string? Title, string? Description, string? ImageUrl);