namespace DoneLogger.Tests;

using DoneLogger.Models;
using DoneLogger.Services;

public class ConfigServiceTests
{
    private static AppConfig MakeConfig(params string[] categoryNames) => new()
    {
        DataRoot = @"C:\data",
        Editor = "notepad",
        Categories = categoryNames
            .Select((name, i) => new Category { Name = name, IsDefault = i == 0 })
            .ToList()
    };

    [Fact]
    public void Validate_AcceptsWellFormedConfig()
    {
        ConfigService.Validate(MakeConfig("Work", "Admin"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_RejectsEmptyCategoryName(string name)
    {
        Assert.Throws<InvalidOperationException>(() => ConfigService.Validate(MakeConfig(name)));
    }

    [Fact]
    public void Validate_RejectsCategoryNameWithControlCharacters()
    {
        // A newline in a category name would inject extra lines into headings.
        Assert.Throws<InvalidOperationException>(() => ConfigService.Validate(MakeConfig("Work\n## Fake")));
    }

    [Fact]
    public void Validate_RejectsCategoryNameWithSurroundingWhitespace()
    {
        // Headings are matched with TrimEnd, so trailing whitespace would
        // create sections that can never be found again.
        Assert.Throws<InvalidOperationException>(() => ConfigService.Validate(MakeConfig("Work ")));
    }

    [Theory]
    [InlineData("What I did")]
    [InlineData("What is next")]
    public void Validate_RejectsReservedSectionNames(string name)
    {
        Assert.Throws<InvalidOperationException>(() => ConfigService.Validate(MakeConfig(name)));
    }

    [Fact]
    public void Validate_RejectsDuplicateCategoryNames()
    {
        Assert.Throws<InvalidOperationException>(() => ConfigService.Validate(MakeConfig("Work", "Work")));
    }
}
