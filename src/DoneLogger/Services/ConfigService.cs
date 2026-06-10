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

    // Section headings the log file format reserves for its own use; a category
    // with one of these names would silently corrupt the file structure.
    private static readonly string[] ReservedSectionNames = { "What I did", "What is next" };

    public static void Validate(AppConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.DataRoot))
            throw new InvalidOperationException("config.json: 'dataRoot' is required.");
        if (config.Categories == null || config.Categories.Count == 0)
            throw new InvalidOperationException("config.json: 'categories' must have at least one entry.");
        if (config.Categories.Count(c => c.IsDefault) != 1)
            throw new InvalidOperationException("config.json: exactly one category must have 'isDefault' set to true.");

        foreach (var name in config.Categories.Select(c => c.Name))
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("config.json: every category needs a non-empty 'name'.");
            if (name.Any(char.IsControl))
                throw new InvalidOperationException($"config.json: category name '{name}' must not contain control characters.");
            if (name.Trim() != name)
                throw new InvalidOperationException($"config.json: category name '{name}' must not have leading or trailing whitespace.");
            if (ReservedSectionNames.Contains(name))
                throw new InvalidOperationException($"config.json: '{name}' is a reserved section name and cannot be used as a category.");
        }

        if (config.Categories.Select(c => c.Name).Distinct().Count() != config.Categories.Count)
            throw new InvalidOperationException("config.json: category names must be unique.");
    }
}
