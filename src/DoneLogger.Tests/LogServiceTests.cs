namespace DoneLogger.Tests;

using DoneLogger.Models;
using DoneLogger.Services;
using DoneLogger.Tests.Helpers;

public class LogServiceTests
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

    // ── CreateLog ─────────────────────────────────────────────────

    [Fact]
    public void CreateLog_CreatesFileWithAllSections()
    {
        using var tmp = new TempDir();
        var svc = new LogService(MakeConfig(tmp.Path));
        var date = new DateTime(2026, 1, 15);

        svc.CreateLog(date);

        string content = File.ReadAllText(svc.GetLogPath(date));
        Assert.Contains("# 2026-01-15", content);
        Assert.Contains("## What I did", content);
        Assert.Contains("## What is next", content);
        Assert.Contains("## Work", content);
        Assert.Contains("## Admin", content);
    }

    [Fact]
    public void CreateLog_ThrowsIfLogAlreadyExists()
    {
        using var tmp = new TempDir();
        var svc = new LogService(MakeConfig(tmp.Path));
        var date = new DateTime(2026, 1, 15);
        svc.CreateLog(date);

        Assert.Throws<InvalidOperationException>(() => svc.CreateLog(date));
    }

    [Fact]
    public void CreateLog_CarriesForwardWhatIsNextItems()
    {
        using var tmp = new TempDir();
        var svc = new LogService(MakeConfig(tmp.Path));
        var day1 = new DateTime(2026, 1, 14);
        var day2 = new DateTime(2026, 1, 15);

        svc.CreateLog(day1);
        var lines = File.ReadAllLines(svc.GetLogPath(day1)).ToList();
        int idx = lines.IndexOf("## What is next");
        lines.Insert(idx + 1, "- Finish the report");
        File.WriteAllLines(svc.GetLogPath(day1), lines);

        svc.CreateLog(day2);

        string content2 = File.ReadAllText(svc.GetLogPath(day2));
        Assert.Contains("- Finish the report", content2);
    }

    [Fact]
    public void CreateLog_RemovesWhatIsNextFromPreviousLog()
    {
        using var tmp = new TempDir();
        var svc = new LogService(MakeConfig(tmp.Path));
        var day1 = new DateTime(2026, 1, 14);
        var day2 = new DateTime(2026, 1, 15);

        svc.CreateLog(day1);
        var lines = File.ReadAllLines(svc.GetLogPath(day1)).ToList();
        int idx = lines.IndexOf("## What is next");
        lines.Insert(idx + 1, "- Finish the report");
        File.WriteAllLines(svc.GetLogPath(day1), lines);

        svc.CreateLog(day2);

        string content1 = File.ReadAllText(svc.GetLogPath(day1));
        Assert.DoesNotContain("## What is next", content1);
        Assert.DoesNotContain("- Finish the report", content1);
    }

    // ── UpdateCategoryTime ────────────────────────────────────────

    [Fact]
    public void UpdateCategoryTime_InsertsEntryWhenSectionIsEmpty()
    {
        using var tmp = new TempDir();
        var svc = new LogService(MakeConfig(tmp.Path));
        var date = new DateTime(2026, 1, 15);
        svc.CreateLog(date);

        svc.UpdateCategoryTime(svc.GetLogPath(date), "Work", 90);

        string content = File.ReadAllText(svc.GetLogPath(date));
        Assert.Contains("- 1h30m", content);
    }

    [Fact]
    public void UpdateCategoryTime_AccumulatesOntoExistingEntry()
    {
        using var tmp = new TempDir();
        var svc = new LogService(MakeConfig(tmp.Path));
        var date = new DateTime(2026, 1, 15);
        svc.CreateLog(date);
        string path = svc.GetLogPath(date);

        svc.UpdateCategoryTime(path, "Work", 60);
        svc.UpdateCategoryTime(path, "Work", 90);

        string content = File.ReadAllText(path);
        Assert.Contains("- 2h30m", content);
        Assert.DoesNotContain("- 1h00m", content);
    }

    [Fact]
    public void UpdateCategoryTime_DoesNothingForZeroMinutes()
    {
        using var tmp = new TempDir();
        var svc = new LogService(MakeConfig(tmp.Path));
        var date = new DateTime(2026, 1, 15);
        svc.CreateLog(date);
        string original = File.ReadAllText(svc.GetLogPath(date));

        svc.UpdateCategoryTime(svc.GetLogPath(date), "Work", 0);

        Assert.Equal(original, File.ReadAllText(svc.GetLogPath(date)));
    }

    [Fact]
    public void UpdateCategoryTime_ThrowsWhenCategoryNotInConfigOrFile()
    {
        using var tmp = new TempDir();
        var svc = new LogService(MakeConfig(tmp.Path));
        var date = new DateTime(2026, 1, 15);
        svc.CreateLog(date);

        Assert.Throws<InvalidOperationException>(() =>
            svc.UpdateCategoryTime(svc.GetLogPath(date), "NonExistent", 30));
    }

    [Fact]
    public void UpdateCategoryTime_AppendsSectionWhenCategoryInConfigButMissingFromFile()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var svc = new LogService(config);
        var date = new DateTime(2026, 1, 15);
        svc.CreateLog(date);

        // Simulate a category added to config after the file was created.
        config.Categories = config.Categories.Append(new Category { Name = "Research" }).ToList();

        string path = svc.GetLogPath(date);
        svc.UpdateCategoryTime(path, "Research", 45);

        string content = File.ReadAllText(path);
        int researchIdx = content.IndexOf("## Research");
        Assert.True(researchIdx > 0);
        Assert.Contains("- 0h45m", content[researchIdx..]);
    }

    [Fact]
    public void UpdateCategoryTime_DoesNotCrossIntoAdjacentCategory()
    {
        using var tmp = new TempDir();
        var svc = new LogService(MakeConfig(tmp.Path));
        var date = new DateTime(2026, 1, 15);
        svc.CreateLog(date);
        string path = svc.GetLogPath(date);

        svc.UpdateCategoryTime(path, "Work", 60);
        svc.UpdateCategoryTime(path, "Admin", 30);

        string content = File.ReadAllText(path);
        // Both entries must be present and distinct
        int workIdx = content.IndexOf("## Work");
        int adminIdx = content.IndexOf("## Admin");
        string workSection = content[workIdx..adminIdx];
        string adminSection = content[adminIdx..];
        Assert.Contains("- 1h00m", workSection);
        Assert.Contains("- 0h30m", adminSection);
    }

    // ── GetAllLogDates ────────────────────────────────────────────

    [Fact]
    public void GetAllLogDates_ReturnsAllCreatedDates()
    {
        using var tmp = new TempDir();
        var svc = new LogService(MakeConfig(tmp.Path));
        var dates = new[] { new DateTime(2026, 1, 13), new DateTime(2026, 1, 14), new DateTime(2026, 1, 15) };
        foreach (var d in dates) svc.CreateLog(d);

        var result = svc.GetAllLogDates();

        Assert.Equal(3, result.Count);
        foreach (var d in dates)
            Assert.Contains(d.Date, result.Select(r => r.Date));
    }

    [Fact]
    public void GetAllLogDates_ReturnsEmptyWhenDataRootMissing()
    {
        var svc = new LogService(MakeConfig(@"C:\does\not\exist\ever"));

        var result = svc.GetAllLogDates();

        Assert.Empty(result);
    }
}
