namespace DoneLogger.Services;

using DoneLogger.Models;
using System.Text;
using System.Text.RegularExpressions;

public class SummaryService
{
    private static readonly Regex TimeEntryPattern = new(@"^- \d+h\d{2}m$", RegexOptions.Compiled);
    private static readonly Regex TimeValuePattern = new(@"- (\d+)h(\d{2})m", RegexOptions.Compiled);

    private readonly LogService _logService;
    private readonly AppConfig _config;
    private readonly TimeTrackingService _trackingService;
    private readonly string _summaryPath;

    public SummaryService(LogService logService, AppConfig config, TimeTrackingService trackingService, string? baseDir = null)
    {
        _logService = logService;
        _config = config;
        _trackingService = trackingService;
        _summaryPath = Path.Combine(baseDir ?? AppContext.BaseDirectory, "summary.md");
    }

    public string GenerateSummary(DateTime from, DateTime to)
    {
        var dates = _logService.GetAllLogDates()
            .Where(d => d.Date >= from.Date && d.Date <= to.Date)
            .OrderBy(d => d)
            .ToList();

        var whatIDidItems = new List<string>();
        var categoryMinutes = new Dictionary<string, int>();

        foreach (var date in dates)
            ParseLog(_logService.GetLogPath(date), date, whatIDidItems, categoryMinutes);

        if (to.Date >= DateTime.Today)
        {
            var state = _trackingService.LoadState();
            if (state?.Active == true && state.StartTime.Date <= DateTime.Today)
            {
                DateTime sessionStart = state.StartTime.Date < DateTime.Today
                    ? DateTime.Today
                    : state.StartTime;
                int liveMinutes = RoundToFiveMinutes((DateTime.Now - sessionStart).TotalMinutes);
                if (liveMinutes > 0)
                {
                    categoryMinutes.TryGetValue(state.Category, out int existing);
                    categoryMinutes[state.Category] = existing + liveMinutes;
                }
            }
        }

        var sb = new StringBuilder();
        sb.AppendLine($"# Summary for the period from {from:yyyy-MM-dd} to {to:yyyy-MM-dd}");
        sb.AppendLine();
        sb.AppendLine("## What I did");
        foreach (var item in whatIDidItems)
            sb.AppendLine(item);
        sb.AppendLine();

        int totalMinutes = 0;
        foreach (var cat in _config.Categories)
        {
            if (categoryMinutes.TryGetValue(cat.Name, out int mins) && mins > 0)
            {
                sb.AppendLine($"## {cat.Name}");
                sb.AppendLine($"- {FormatTime(mins)}");
                sb.AppendLine();
                totalMinutes += mins;
            }
        }

        sb.AppendLine("## Total");
        sb.AppendLine($"- {FormatTime(totalMinutes)}");

        File.WriteAllText(_summaryPath, sb.ToString().TrimEnd() + Environment.NewLine, Encoding.UTF8);
        return _summaryPath;
    }

    private static void ParseLog(string path, DateTime date, List<string> whatIDidItems, Dictionary<string, int> categoryMinutes)
    {
        string? currentSection = null;

        foreach (var line in File.ReadLines(path))
        {
            if (line.StartsWith("# ")) continue;

            if (line.StartsWith("## "))
            {
                currentSection = line[3..].Trim();
                continue;
            }

            if (string.IsNullOrWhiteSpace(line)) continue;

            if (currentSection == "What I did" && line.StartsWith("- ") && !TimeEntryPattern.IsMatch(line.Trim()))
            {
                whatIDidItems.Add($"{line} [{date:yyyy-MM-dd}]");
            }
            else if (currentSection != null
                     && currentSection != "What I did"
                     && currentSection != "What is next"
                     && TimeEntryPattern.IsMatch(line.Trim()))
            {
                var match = TimeValuePattern.Match(line.Trim());
                if (match.Success)
                {
                    int mins = int.Parse(match.Groups[1].Value) * 60 + int.Parse(match.Groups[2].Value);
                    categoryMinutes.TryGetValue(currentSection, out int existing);
                    categoryMinutes[currentSection] = existing + mins;
                }
            }
        }
    }

    private static int RoundToFiveMinutes(double totalMinutes) =>
        (int)Math.Round(totalMinutes / 5.0, MidpointRounding.AwayFromZero) * 5;

    private static string FormatTime(int totalMinutes)
    {
        int h = totalMinutes / 60;
        int m = totalMinutes % 60;
        return $"{h}h{m:D2}m";
    }
}
