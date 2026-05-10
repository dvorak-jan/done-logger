namespace DoneLogger.Tests;

using DoneLogger.Models;
using DoneLogger.Services;
using DoneLogger.Tests.Helpers;

public class SummaryServiceTests
{
    private static AppConfig MakeConfig(string dataRoot) => new()
    {
        DataRoot = dataRoot,
        Editor = "notepad",
        Categories =
        [
            new Category { Name = "Work", IsDefault = true },
            new Category { Name = "Admin" }
        ]
    };

    private static void AddWhatIDidItem(LogService svc, DateTime date, string item)
    {
        string path = svc.GetLogPath(date);
        var lines = File.ReadAllLines(path).ToList();
        int idx = lines.IndexOf("## What I did");
        lines.Insert(idx + 1, item);
        File.WriteAllLines(path, lines);
    }

    // ── What I did ────────────────────────────────────────────────

    [Fact]
    public void GenerateSummary_AppendsDatesToWhatIDidItems()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);
        var summarySvc = new SummaryService(logSvc, config, trackSvc, tmp.Path);

        var date = new DateTime(2026, 1, 15);
        logSvc.CreateLog(date);
        AddWhatIDidItem(logSvc, date, "- Fixed a bug");

        string summaryPath = summarySvc.GenerateSummary(date, date);
        string content = File.ReadAllText(summaryPath);

        Assert.Contains("- Fixed a bug [2026-01-15]", content);
    }

    [Fact]
    public void GenerateSummary_ItemsFromMultipleDaysHaveCorrectDates()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);
        var summarySvc = new SummaryService(logSvc, config, trackSvc, tmp.Path);

        var day1 = new DateTime(2026, 1, 14);
        var day2 = new DateTime(2026, 1, 15);
        logSvc.CreateLog(day1);
        logSvc.CreateLog(day2);
        AddWhatIDidItem(logSvc, day1, "- Task A");
        AddWhatIDidItem(logSvc, day2, "- Task B");

        string content = File.ReadAllText(summarySvc.GenerateSummary(day1, day2));

        Assert.Contains("- Task A [2026-01-14]", content);
        Assert.Contains("- Task B [2026-01-15]", content);
    }

    // ── Time aggregation ──────────────────────────────────────────

    [Fact]
    public void GenerateSummary_AggregatesTimeAcrossDays()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);
        var summarySvc = new SummaryService(logSvc, config, trackSvc, tmp.Path);

        var day1 = new DateTime(2026, 1, 14);
        var day2 = new DateTime(2026, 1, 15);
        logSvc.CreateLog(day1);
        logSvc.CreateLog(day2);
        logSvc.UpdateCategoryTime(logSvc.GetLogPath(day1), "Work", 60);
        logSvc.UpdateCategoryTime(logSvc.GetLogPath(day2), "Work", 90);

        string content = File.ReadAllText(summarySvc.GenerateSummary(day1, day2));

        // Work section should show 2h30m
        int workIdx = content.IndexOf("## Work");
        int totalIdx = content.IndexOf("## Total");
        string workSection = content[workIdx..totalIdx];
        Assert.Contains("- 2h30m", workSection);
    }

    [Fact]
    public void GenerateSummary_TotalSumsAllCategories()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);
        var summarySvc = new SummaryService(logSvc, config, trackSvc, tmp.Path);

        var date = new DateTime(2026, 1, 15);
        logSvc.CreateLog(date);
        logSvc.UpdateCategoryTime(logSvc.GetLogPath(date), "Work", 60);
        logSvc.UpdateCategoryTime(logSvc.GetLogPath(date), "Admin", 30);

        string content = File.ReadAllText(summarySvc.GenerateSummary(date, date));

        int totalIdx = content.IndexOf("## Total");
        string totalSection = content[totalIdx..];
        Assert.Contains("- 1h30m", totalSection);
    }

    // ── Edge cases ────────────────────────────────────────────────

    [Fact]
    public void GenerateSummary_EmptyRange_ProducesEmptySections()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);
        var summarySvc = new SummaryService(logSvc, config, trackSvc, tmp.Path);

        var from = new DateTime(2026, 1, 1);
        var to = new DateTime(2026, 1, 31);

        string content = File.ReadAllText(summarySvc.GenerateSummary(from, to));

        Assert.Contains("## What I did", content);
        Assert.Contains("## Total", content);
        Assert.Contains("- 0h00m", content);
    }

    [Fact]
    public void GenerateSummary_ReturnsPathToSummaryFile()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);
        var summarySvc = new SummaryService(logSvc, config, trackSvc, tmp.Path);

        string path = summarySvc.GenerateSummary(new DateTime(2026, 1, 1), new DateTime(2026, 1, 31));

        Assert.True(File.Exists(path));
        Assert.Equal("summary.md", System.IO.Path.GetFileName(path));
    }
}
