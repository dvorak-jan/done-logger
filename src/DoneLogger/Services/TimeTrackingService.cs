namespace DoneLogger.Services;

using DoneLogger.Models;
using System.Text.Json;

public class TimeTrackingService
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private readonly LogService _logService;
    private readonly string _statePath;

    public TimeTrackingService(LogService logService, string? baseDir = null)
    {
        _logService = logService;
        _statePath = Path.Combine(baseDir ?? AppContext.BaseDirectory, "state.json");
    }

    public TrackingState? LoadState()
    {
        if (!File.Exists(_statePath)) return null;
        try
        {
            return JsonSerializer.Deserialize<TrackingState>(File.ReadAllText(_statePath), JsonOpts);
        }
        catch
        {
            return null;
        }
    }

    public void StartTracking(string category, DateTime? customTime = null)
    {
        var state = new TrackingState
        {
            Active = true,
            StartTime = customTime ?? DateTime.Now,
            Category = category
        };
        File.WriteAllText(_statePath, JsonSerializer.Serialize(state, JsonOpts));
    }

    // Returns a message to show the user when midnight was crossed, null otherwise.
    public string? StopTracking(DateTime? customStopTime = null)
    {
        var state = LoadState() ?? throw new InvalidOperationException("No active tracking session.");

        DateTime stopTime = customStopTime ?? DateTime.Now;
        DateTime startTime = state.StartTime;
        string? midnightMessage = null;

        if (stopTime.Date == startTime.Date)
        {
            int minutes = RoundToFiveMinutes((stopTime - startTime).TotalMinutes);
            _logService.UpdateCategoryTime(_logService.GetLogPath(startTime.Date), state.Category, minutes);
        }
        else
        {
            DateTime midnight = startTime.Date.AddDays(1);
            int minutesBefore = RoundToFiveMinutes((midnight - startTime).TotalMinutes);
            int minutesAfter = RoundToFiveMinutes((stopTime - midnight).TotalMinutes);

            if (minutesBefore > 0)
                _logService.UpdateCategoryTime(_logService.GetLogPath(startTime.Date), state.Category, minutesBefore);

            if (!_logService.LogExists(stopTime.Date))
                _logService.CreateLog(stopTime.Date);

            if (minutesAfter > 0)
                _logService.UpdateCategoryTime(_logService.GetLogPath(stopTime.Date), state.Category, minutesAfter);

            midnightMessage = $"Session crossed midnight.\n" +
                              $"  {startTime:yyyy-MM-dd}: {FormatMinutes(minutesBefore)} added to '{state.Category}'\n" +
                              $"  {stopTime:yyyy-MM-dd}: {FormatMinutes(minutesAfter)} added to '{state.Category}'";
        }

        ClearState();
        return midnightMessage;
    }

    public void ClearState()
    {
        if (File.Exists(_statePath))
            File.Delete(_statePath);
    }

    private static int RoundToFiveMinutes(double totalMinutes) =>
        (int)Math.Round(totalMinutes / 5.0, MidpointRounding.AwayFromZero) * 5;

    private static string FormatMinutes(int minutes)
    {
        int h = minutes / 60;
        int m = minutes % 60;
        return h > 0 ? $"{h}h {m:D2}m" : $"{m}m";
    }
}
