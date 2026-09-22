using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CityVilleDotnet.Api.Common.Logging;

public sealed record LogEntry(string FileName, DateOnly? Date, string Time, string Level, string Message);

public sealed record LogSearchResult(List<LogEntry> Entries, bool Truncated);

public sealed partial class LogFileReader
{
    public const int MaxResults = 2000;

    private readonly string _directory;
    private readonly string _searchPattern;

    public LogFileReader(IConfiguration configuration)
    {
        var path = configuration.GetSection("Serilog:WriteTo").GetChildren()
            .Where(x => x["Name"] == "File")
            .Select(x => x["Args:path"])
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? "./logs/log.txt";

        var fullPath = Path.GetFullPath(path);

        _directory = Path.GetDirectoryName(fullPath) ?? Directory.GetCurrentDirectory();
        _searchPattern = $"{Path.GetFileNameWithoutExtension(fullPath)}*{Path.GetExtension(fullPath)}";
    }

    public List<string> GetFiles()
    {
        if (!Directory.Exists(_directory))
            return [];

        return Directory.EnumerateFiles(_directory, _searchPattern)
            .Select(x => Path.GetFileName(x))
            .OrderByDescending(x => x, StringComparer.Ordinal)
            .ToList();
    }

    public async Task<LogSearchResult> SearchAsync(IReadOnlyCollection<string> fileNames, string? query, string? level, CancellationToken ct)
    {
        var results = new List<LogEntry>();
        var truncated = false;

        foreach (var fileName in fileNames)
        {
            // Keep only the most recent matches of the file, the result is displayed newest first
            var matches = new Queue<LogEntry>();

            await foreach (var entry in ReadEntriesAsync(fileName, ct))
            {
                if (level is not null && !entry.Level.Equals(level, StringComparison.OrdinalIgnoreCase)) continue;
                if (query is not null && !entry.Message.Contains(query, StringComparison.OrdinalIgnoreCase)) continue;

                matches.Enqueue(entry);

                if (matches.Count > MaxResults - results.Count)
                {
                    matches.Dequeue();
                    truncated = true;
                }
            }

            results.AddRange(matches.Reverse());

            if (results.Count >= MaxResults)
            {
                truncated = true;
                break;
            }
        }

        return new LogSearchResult(results, truncated);
    }

    private async IAsyncEnumerable<LogEntry> ReadEntriesAsync(string fileName, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        var date = ParseDate(fileName);

        await using var stream = new FileStream(Path.Combine(_directory, fileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
        using var reader = new StreamReader(stream);

        string? time = null;
        string? entryLevel = null;
        var message = new StringBuilder();

        while (await reader.ReadLineAsync(ct) is { } line)
        {
            var match = EntryStartRegex().Match(line);

            if (!match.Success)
            {
                // Multiline entry (exceptions, stack traces)
                if (time is not null) message.AppendLine().Append(line);
                continue;
            }

            if (time is not null)
                yield return new LogEntry(fileName, date, time, entryLevel!, message.ToString());

            time = match.Groups[1].Value;
            entryLevel = match.Groups[2].Value;
            message.Clear().Append(line, match.Length, line.Length - match.Length);
        }

        if (time is not null)
            yield return new LogEntry(fileName, date, time, entryLevel!, message.ToString());
    }

    private static DateOnly? ParseDate(string fileName)
    {
        var match = FileDateRegex().Match(fileName);

        return match.Success && DateOnly.TryParseExact(match.Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : null;
    }

    [GeneratedRegex(@"^(\d{2}:\d{2}:\d{2}) \[(\w+)\] ?")]
    private static partial Regex EntryStartRegex();

    [GeneratedRegex(@"\d{8}")]
    private static partial Regex FileDateRegex();
}
