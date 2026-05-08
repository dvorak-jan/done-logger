namespace DoneLogger.Services;

using DoneLogger.Models;
using System.Text;
using System.Text.RegularExpressions;

public class LogService
{
    private static readonly Regex TimeEntryPattern = new(@"^- \d+h\d{2}m$", RegexOptions.Compiled);
    private static readonly Regex LogFileNamePattern = new(@"^\d{4}-\d{2}-\d{2}\.md$", RegexOptions.Compiled);
    private static readonly Regex TimeValuePattern = new(@"- (\d+)h(\d{2})m", RegexOptions.Compiled);

    private readonly AppConfig _config;

    public LogService(AppConfig config) => _config = config;

    public string GetLogPath(DateTime date) =>
        Path.Combine(_config.DataRoot, date.ToString("yyyy"), date.ToString("MM"), date.ToString("yyyy-MM-dd") + ".md");

    public bool LogExists(DateTime date) => File.Exists(GetLogPath(date));

    public IReadOnlyList<DateTime> GetAllLogDates()
    {
        if (!Directory.Exists(_config.DataRoot)) return Array.Empty<DateTime>();

        return Directory
            .GetFiles(_config.DataRoot, "*.md", SearchOption.AllDirectories)
            .Where(f => LogFileNamePattern.IsMatch(Path.GetFileName(f)))
            .Select(f => DateTime.ParseExact(Path.GetFileNameWithoutExtension(f), "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture))
            .OrderByDescending(d => d)
            .ToList();
    }

    public string? FindMostRecentLogPath()
    {
        if (!Directory.Exists(_config.DataRoot)) return null;

        return Directory
            .GetFiles(_config.DataRoot, "*.md", SearchOption.AllDirectories)
            .Where(f => LogFileNamePattern.IsMatch(Path.GetFileName(f)))
            .OrderBy(f => Path.GetFileNameWithoutExtension(f))
            .LastOrDefault();
    }

    public void CreateLog(DateTime date)
    {
        string path = GetLogPath(date);

        if (File.Exists(path))
            throw new InvalidOperationException($"A log for {date:yyyy-MM-dd} already exists.");

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        List<string> carryForward = new();
        string? previousPath = FindMostRecentLogPath();

        if (previousPath != null)
        {
            carryForward = ExtractSectionLines(previousPath, "What is next");
            DeleteSection(previousPath, "What is next");
        }

        var sb = new StringBuilder();
        sb.AppendLine($"# {date:yyyy-MM-dd}");
        sb.AppendLine();
        sb.AppendLine("## What I did");
        sb.AppendLine();
        sb.AppendLine("## What is next");
        foreach (var line in carryForward)
            sb.AppendLine(line);
        sb.AppendLine();

        foreach (var category in _config.Categories)
        {
            sb.AppendLine($"## {category.Name}");
            sb.AppendLine();
        }

        File.WriteAllText(path, sb.ToString().TrimEnd() + Environment.NewLine, Encoding.UTF8);
    }

    public void UpdateCategoryTime(string filePath, string categoryName, int minutes)
    {
        if (minutes <= 0) return;

        var lines = File.ReadAllLines(filePath).ToList();

        int sectionStart = lines.FindIndex(l => l.TrimEnd() == $"## {categoryName}");
        if (sectionStart < 0)
            throw new InvalidOperationException($"Category '{categoryName}' not found in {Path.GetFileName(filePath)}.");

        int sectionEnd = lines.Count;
        for (int i = sectionStart + 1; i < lines.Count; i++)
        {
            if (lines[i].StartsWith("## ") || lines[i].StartsWith("# "))
            {
                sectionEnd = i;
                break;
            }
        }

        int timeLineIndex = -1;
        for (int i = sectionStart + 1; i < sectionEnd; i++)
        {
            if (TimeEntryPattern.IsMatch(lines[i].TrimEnd()))
            {
                timeLineIndex = i;
                break;
            }
        }

        if (timeLineIndex >= 0)
        {
            int existing = ParseTimeMinutes(lines[timeLineIndex]);
            lines[timeLineIndex] = FormatTimeEntry(existing + minutes);
        }
        else
        {
            lines.Insert(sectionStart + 1, FormatTimeEntry(minutes));
        }

        File.WriteAllLines(filePath, lines, Encoding.UTF8);
    }

    private List<string> ExtractSectionLines(string filePath, string sectionName)
    {
        var result = new List<string>();
        bool inSection = false;

        foreach (var line in File.ReadLines(filePath))
        {
            if (line.TrimEnd() == $"## {sectionName}")
            {
                inSection = true;
                continue;
            }
            if (inSection)
            {
                if (line.StartsWith("## ") || line.StartsWith("# ")) break;
                if (!string.IsNullOrWhiteSpace(line))
                    result.Add(line);
            }
        }

        return result;
    }

    private void DeleteSection(string filePath, string sectionName)
    {
        var lines = File.ReadAllLines(filePath).ToList();

        int sectionStart = lines.FindIndex(l => l.TrimEnd() == $"## {sectionName}");
        if (sectionStart < 0) return;

        int sectionEnd = lines.Count;
        for (int i = sectionStart + 1; i < lines.Count; i++)
        {
            if (lines[i].StartsWith("## ") || lines[i].StartsWith("# "))
            {
                sectionEnd = i;
                break;
            }
        }

        // Remove the heading + all content up to (but not including) the next section.
        // The blank line before the heading is preserved and naturally becomes the
        // separator between the surrounding sections.
        lines.RemoveRange(sectionStart, sectionEnd - sectionStart);

        File.WriteAllLines(filePath, lines, Encoding.UTF8);
    }

    private int ParseTimeMinutes(string timeEntry)
    {
        var match = TimeValuePattern.Match(timeEntry.Trim());
        if (!match.Success) return 0;
        return int.Parse(match.Groups[1].Value) * 60 + int.Parse(match.Groups[2].Value);
    }

    private static string FormatTimeEntry(int totalMinutes)
    {
        int hours = totalMinutes / 60;
        int mins = totalMinutes % 60;
        return $"- {hours}h{mins:D2}m";
    }
}
