namespace DoneLogger.Models;

public class AppConfig
{
    public string DataRoot { get; set; } = string.Empty;
    public string Editor { get; set; } = string.Empty;
    public List<Category> Categories { get; set; } = new();
}
