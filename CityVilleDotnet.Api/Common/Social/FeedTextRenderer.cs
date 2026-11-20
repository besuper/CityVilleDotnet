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

    private readonly ConcurrentDictionary<string, Dictionary<string, string>> _feedTextsByLocale = new();

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

        if (!GetFeedTexts(locale).TryGetValue(key, out var text) && !GetFeedTexts("en_US").TryGetValue(key, out text))
        {
            return null;
        }

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

    private Dictionary<string, string> GetFeedTexts(string locale)
    {
        return _feedTextsByLocale.GetOrAdd(locale, LoadLocaleFeedTexts);
    }

    private Dictionary<string, string> LoadLocaleFeedTexts(string locale)
    {
        var path = Path.Combine(webHost.WebRootPath, "lang", $"feeds_{locale}.json");

        if (!File.Exists(path))
        {
            logger.LogWarning("Missing feed texts ({Path})", path);
            return [];
        }

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path)) ?? [];
        }
        catch (JsonException e)
        {
            logger.LogError(e, "Invalid feed texts ({Path})", path);
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