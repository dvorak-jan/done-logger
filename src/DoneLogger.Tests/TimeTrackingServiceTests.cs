namespace DoneLogger.Tests;

using DoneLogger.Models;
using DoneLogger.Services;
using DoneLogger.Tests.Helpers;

public class TimeTrackingServiceTests
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

    // ── Rounding ──────────────────────────────────────────────────

    [Theory]
    [InlineData(0,  0)]   // zero stays zero
    [InlineData(2,  0)]   // 2 min rounds down to 0
    [InlineData(3,  5)]   // 3 min rounds up to 5
    [InlineData(7,  5)]   // 7 min rounds down to 5
    [InlineData(8,  10)]  // 8 min rounds up to 10
    [InlineData(30, 30)]  // exact multiple unchanged
    [InlineData(62, 60)]  // just over an hour
    public void StopTracking_RoundsToNearestFiveMinutes(int actualMinutes, int expectedMinutes)
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);

        var date = new DateTime(2026, 1, 15);
        logSvc.CreateLog(date);
        var start = new DateTime(2026, 1, 15, 10, 0, 0);
        trackSvc.StartTracking("Work", start);
        trackSvc.StopTracking(start.AddMinutes(actualMinutes));

        string content = File.ReadAllText(logSvc.GetLogPath(date));
        if (expectedMinutes == 0)
        {
            Assert.DoesNotContain("- 0h00m", content);
        }
        else
        {
            int h = expectedMinutes / 60;
            int m = expectedMinutes % 60;
            Assert.Contains($"- {h}h{m:D2}m", content);
        }
    }

    // ── Same-day session ──────────────────────────────────────────

    [Fact]
    public void StopTracking_SameDay_WritesTimeToCorrectCategory()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);

        var date = new DateTime(2026, 1, 15);
        logSvc.CreateLog(date);
        trackSvc.StartTracking("Admin", new DateTime(2026, 1, 15, 9, 0, 0));
        trackSvc.StopTracking(new DateTime(2026, 1, 15, 10, 0, 0));

        string content = File.ReadAllText(logSvc.GetLogPath(date));
        int adminIdx = content.IndexOf("## Admin");
        string adminSection = content[adminIdx..];
        Assert.Contains("- 1h00m", adminSection);
        Assert.DoesNotContain("- 1h00m", content[..adminIdx]); // not in Work section
    }

    [Fact]
    public void StopTracking_SameDay_ReturnNullMidnightMessage()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);

        logSvc.CreateLog(new DateTime(2026, 1, 15));
        trackSvc.StartTracking("Work", new DateTime(2026, 1, 15, 9, 0, 0));
        string? msg = trackSvc.StopTracking(new DateTime(2026, 1, 15, 10, 0, 0));

        Assert.Null(msg);
    }

    [Fact]
    public void StopTracking_ClearsStateAfterStop()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);

        logSvc.CreateLog(new DateTime(2026, 1, 15));
        trackSvc.StartTracking("Work", new DateTime(2026, 1, 15, 9, 0, 0));
        trackSvc.StopTracking(new DateTime(2026, 1, 15, 10, 0, 0));

        Assert.Null(trackSvc.LoadState());
    }

    // ── Midnight crossing ─────────────────────────────────────────

    [Fact]
    public void StopTracking_MidnightCross_UpdatesBothDayLogs()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);

        var day1 = new DateTime(2026, 1, 14);
        var day2 = new DateTime(2026, 1, 15);
        logSvc.CreateLog(day1);
        logSvc.CreateLog(day2);

        // 30 min before midnight, 30 min after
        trackSvc.StartTracking("Work", new DateTime(2026, 1, 14, 23, 30, 0));
        trackSvc.StopTracking(new DateTime(2026, 1, 15, 0, 30, 0));

        Assert.Contains("- 0h30m", File.ReadAllText(logSvc.GetLogPath(day1)));
        Assert.Contains("- 0h30m", File.ReadAllText(logSvc.GetLogPath(day2)));
    }

    [Fact]
    public void StopTracking_MidnightCross_CreatesNextDayLogIfMissing()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);

        logSvc.CreateLog(new DateTime(2026, 1, 14));
        // day2 intentionally not created

        trackSvc.StartTracking("Work", new DateTime(2026, 1, 14, 23, 30, 0));
        trackSvc.StopTracking(new DateTime(2026, 1, 15, 0, 30, 0));

        Assert.True(logSvc.LogExists(new DateTime(2026, 1, 15)));
    }

    [Fact]
    public void StopTracking_MidnightCross_ReturnsMidnightMessage()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);

        logSvc.CreateLog(new DateTime(2026, 1, 14));
        trackSvc.StartTracking("Work", new DateTime(2026, 1, 14, 23, 30, 0));
        string? msg = trackSvc.StopTracking(new DateTime(2026, 1, 15, 0, 30, 0));

        Assert.NotNull(msg);
        Assert.Contains("midnight", msg, StringComparison.OrdinalIgnoreCase);
    }

    // ── StartTracking / LoadState ─────────────────────────────────

    [Fact]
    public void StartTracking_PersistsStateWithCorrectValues()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);

        var startTime = new DateTime(2026, 1, 15, 9, 0, 0);
        trackSvc.StartTracking("Admin", startTime);

        var state = trackSvc.LoadState();
        Assert.NotNull(state);
        Assert.True(state.Active);
        Assert.Equal("Admin", state.Category);
        Assert.Equal(startTime, state.StartTime);
    }

    [Fact]
    public void LoadState_ReturnsNullWhenNoStateFile()
    {
        using var tmp = new TempDir();
        var config = MakeConfig(tmp.Path);
        var logSvc = new LogService(config);
        var trackSvc = new TimeTrackingService(logSvc, tmp.Path);

        Assert.Null(trackSvc.LoadState());
    }
}
