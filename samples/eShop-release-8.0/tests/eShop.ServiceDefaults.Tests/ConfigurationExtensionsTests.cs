namespace eShop.ServiceDefaults.Tests;

using Microsoft.Extensions.Configuration;
using Xunit;

public class ConfigurationExtensionsTests
{
    [Fact]
    public void GetRequiredValue_WithValidKey_ReturnsValue()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "TestKey", "TestValue" }
            })
            .Build();

        // Act
        var result = config.GetRequiredValue("TestKey");

        // Assert
        Assert.Equal("TestValue", result);
    }

    [Fact]
    public void GetRequiredValue_WithMissingKey_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => config.GetRequiredValue("MissingKey"));
        Assert.Contains("Configuration missing value for: MissingKey", ex.Message);
    }

    [Fact]
    public void GetRequiredValue_WithNullValue_ThrowsInvalidOperationException()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "NullKey", null }
            })
            .Build();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => config.GetRequiredValue("NullKey"));
        Assert.Contains("Configuration missing value for: NullKey", ex.Message);
    }

    [Fact]
    public void GetRequiredValue_WithEmptyValue_ReturnsEmptyString()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "EmptyKey", "" }
            })
            .Build();

        // Act
        var result = config.GetRequiredValue("EmptyKey");

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void GetRequiredValue_OnConfigurationSection_IncludesPathInErrorMessage()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Section:SubKey", "Value" }
            })
            .Build();

        var section = config.GetSection("Section");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => section.GetRequiredValue("MissingSubKey"));
        Assert.Contains("Configuration missing value for: Section:MissingSubKey", ex.Message);
    }

    [Fact]
    public void GetRequiredValue_WithMultipleSections_IncludesFullPathInErrorMessage()
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Level1:Level2:Key", "Value" }
            })
            .Build();

        var section = config.GetSection("Level1").GetSection("Level2");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => section.GetRequiredValue("MissingKey"));
        Assert.Contains("Level1:Level2:MissingKey", ex.Message);
    }

    [Fact]
    public void GetRequiredValue_WithSpecialCharactersInValue_ReturnsValue()
    {
        // Arrange
        var specialValue = "Value!@#$%^&*()_+-=[]{}|;:',.<>?/";
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "SpecialKey", specialValue }
            })
            .Build();

        // Act
        var result = config.GetRequiredValue("SpecialKey");

        // Assert
        Assert.Equal(specialValue, result);
    }

    [Fact]
    public void GetRequiredValue_WithWhitespaceValue_ReturnsValue()
    {
        // Arrange
        var whitespaceValue = "  value with spaces  ";
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "WhitespaceKey", whitespaceValue }
            })
            .Build();

        // Act
        var result = config.GetRequiredValue("WhitespaceKey");

        // Assert
        Assert.Equal(whitespaceValue, result);
    }

    [Fact]
    public void GetRequiredValue_WithLongValue_ReturnsValue()
    {
        // Arrange
        var longValue = new string('a', 10000);
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "LongKey", longValue }
            })
            .Build();

        // Act
        var result = config.GetRequiredValue("LongKey");

        // Assert
        Assert.Equal(longValue, result);
    }

    [Theory]
    [InlineData("SimpleKey")]
    [InlineData("Key_With_Underscores")]
    [InlineData("Key-With-Dashes")]
    [InlineData("KeyWith123Numbers")]
    public void GetRequiredValue_WithVariousKeyFormats_ReturnsValue(string keyName)
    {
        // Arrange
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { keyName, "TestValue" }
            })
            .Build();

        // Act
        var result = config.GetRequiredValue(keyName);

        // Assert
        Assert.Equal("TestValue", result);
    }
}
