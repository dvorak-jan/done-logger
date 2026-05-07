namespace DoneLogger.Models;

public class TrackingState
{
    public bool Active { get; set; }
    public DateTime StartTime { get; set; }
    public string Category { get; set; } = string.Empty;
}
