namespace DoneLogger.Services;

using DoneLogger.Models;
using System.Text.Json;

public static class ConfigService
{
    private static readonly string ConfigPath =
        Path.Combine(AppContext.BaseDirectory, "config.json");

    public static AppConfig Load()
    {
        if (!File.Exists(ConfigPath))
            throw new FileNotFoundException(
                $"config.json not found at:\n{ConfigPath}\n\nCreate it before starting the application.");

        var json = File.ReadAllText(ConfigPath);
        var config = JsonSerializer.Deserialize<AppConfig>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidOperationException("config.json could not be parsed.");

        Validate(config);
        return config;
    }

    private static void Validate(AppConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.DataRoot))
            throw new InvalidOperationException("config.json: 'dataRoot' is required.");
        if (config.Categories == null || config.Categories.Count == 0)
            throw new InvalidOperationException("config.json: 'categories' must have at least one entry.");
        if (config.Categories.Count(c => c.IsDefault) != 1)
            throw new InvalidOperationException("config.json: exactly one category must have 'isDefault' set to true.");
    }
}
