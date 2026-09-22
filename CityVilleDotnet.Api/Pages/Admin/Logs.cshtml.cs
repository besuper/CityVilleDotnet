using CityVilleDotnet.Api.Common.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityVilleDotnet.Api.Pages.Admin;

public class LogsModel(LogFileReader logFileReader) : PageModel
{
    public const string AllFiles = "all";
    public static readonly string[] Levels = ["Verbose", "Debug", "Information", "Warning", "Error", "Fatal"];

    private const int PageSize = 100;
    private const int MaxSearchLength = 256;

    [BindProperty(SupportsGet = true)] public string? SelectedFile { get; set; }
    [BindProperty(SupportsGet = true)] public string? Search { get; set; }
    [BindProperty(SupportsGet = true)] public string? Level { get; set; }
    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    public List<string> Files { get; set; } = [];
    public List<LogEntry> Entries { get; set; } = [];
    public int TotalCount { get; set; }
    public bool Truncated { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public async Task OnGetAsync(CancellationToken ct)
    {
        Files = logFileReader.GetFiles();

        if (SelectedFile != AllFiles && (SelectedFile is null || !Files.Contains(SelectedFile)))
            SelectedFile = Files.FirstOrDefault();

        if (SelectedFile is null)
            return;

        if (Level is not null && !Levels.Contains(Level))
            Level = null;

        var search = string.IsNullOrWhiteSpace(Search) ? null : Search.Trim();

        if (search?.Length > MaxSearchLength)
            search = search[..MaxSearchLength];

        var result = await logFileReader.SearchAsync(SelectedFile == AllFiles ? Files : [SelectedFile], search, Level, ct);

        CurrentPage = Math.Max(1, CurrentPage);
        TotalCount = result.Entries.Count;
        Truncated = result.Truncated;
        Entries = result.Entries
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }

    public static string GetLevelBadge(string level) => level switch
    {
        "Fatal" or "Error" => "badge-danger",
        "Warning" => "badge-warning",
        "Information" => "badge-info",
        _ => "badge-secondary"
    };
}
