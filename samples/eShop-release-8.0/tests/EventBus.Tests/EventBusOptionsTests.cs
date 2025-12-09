using eShop.EventBusRabbitMQ;

namespace EventBus.Tests;

public class EventBusOptionsTests
{
    [Fact]
    public void Constructor_ShouldCreateInstanceWithDefaultValues()
    {
        // Arrange & Act
        var options = new EventBusOptions();

        // Assert
        Assert.NotNull(options);
        Assert.Equal(10, options.RetryCount);
    }

    [Fact]
    public void RetryCount_ShouldHaveDefaultValueOfTen()
    {
        // Arrange & Act
        var options = new EventBusOptions();

        // Assert
        Assert.Equal(10, options.RetryCount);
    }

    [Fact]
    public void RetryCount_ShouldBeSettable()
    {
        // Arrange
        var options = new EventBusOptions();
        var newRetryCount = 5;

        // Act
        options.RetryCount = newRetryCount;

        // Assert
        Assert.Equal(newRetryCount, options.RetryCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(100)]
    public void RetryCount_ShouldAcceptVariousPositiveValues(int retryCount)
    {
        // Arrange
        var options = new EventBusOptions();

        // Act
        options.RetryCount = retryCount;

        // Assert
        Assert.Equal(retryCount, options.RetryCount);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-5)]
    [InlineData(int.MinValue)]
    public void RetryCount_ShouldAcceptNegativeValues(int retryCount)
    {
        // Arrange
        var options = new EventBusOptions();

        // Act
        options.RetryCount = retryCount;

        // Assert
        Assert.Equal(retryCount, options.RetryCount);
    }

    [Fact]
    public void SubscriptionClientName_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var options = new EventBusOptions();

        // Assert
        Assert.Null(options.SubscriptionClientName);
    }

    [Fact]
    public void SubscriptionClientName_ShouldBeSettable()
    {
        // Arrange
        var options = new EventBusOptions();
        var clientName = "TestClient";

        // Act
        options.SubscriptionClientName = clientName;

        // Assert
        Assert.Equal(clientName, options.SubscriptionClientName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("SimpleClientName")]
    [InlineData("Client.With.Dots")]
    [InlineData("Client-With-Dashes")]
    [InlineData("Client_With_Underscores")]
    [InlineData("ClientWith123Numbers")]
    public void SubscriptionClientName_ShouldAcceptVariousStringValues(string clientName)
    {
        // Arrange
        var options = new EventBusOptions();

        // Act
        options.SubscriptionClientName = clientName;

        // Assert
        Assert.Equal(clientName, options.SubscriptionClientName);
    }

    [Fact]
    public void SubscriptionClientName_ShouldBeResettable()
    {
        // Arrange
        var options = new EventBusOptions();
        var initialName = "InitialName";
        var newName = "NewName";

        // Act
        options.SubscriptionClientName = initialName;
        options.SubscriptionClientName = newName;

        // Assert
        Assert.Equal(newName, options.SubscriptionClientName);
    }

    [Fact]
    public void SubscriptionClientName_ShouldBeNullableAfterSet()
    {
        // Arrange
        var options = new EventBusOptions();

        // Act
        options.SubscriptionClientName = "SomeName";
        options.SubscriptionClientName = null;

        // Assert
        Assert.Null(options.SubscriptionClientName);
    }

    [Fact]
    public void MultipleInstances_ShouldHaveIndependentValues()
    {
        // Arrange & Act
        var options1 = new EventBusOptions { SubscriptionClientName = "Client1", RetryCount = 5 };
        var options2 = new EventBusOptions { SubscriptionClientName = "Client2", RetryCount = 15 };

        // Assert
        Assert.Equal("Client1", options1.SubscriptionClientName);
        Assert.Equal(5, options1.RetryCount);
        Assert.Equal("Client2", options2.SubscriptionClientName);
        Assert.Equal(15, options2.RetryCount);
    }

    [Fact]
    public void Properties_ShouldBeSettableViaInitializer()
    {
        // Arrange & Act
        var options = new EventBusOptions
        {
            SubscriptionClientName = "MyClient",
            RetryCount = 7
        };

        // Assert
        Assert.Equal("MyClient", options.SubscriptionClientName);
        Assert.Equal(7, options.RetryCount);
    }

    [Fact]
    public void RetryCount_ShouldPersistMultipleChanges()
    {
        // Arrange
        var options = new EventBusOptions();

        // Act
        options.RetryCount = 5;
        var firstValue = options.RetryCount;
        options.RetryCount = 20;
        var secondValue = options.RetryCount;

        // Assert
        Assert.Equal(5, firstValue);
        Assert.Equal(20, secondValue);
        Assert.Equal(20, options.RetryCount);
    }

    [Fact]
    public void SubscriptionClientName_ShouldPersistMultipleChanges()
    {
        // Arrange
        var options = new EventBusOptions();

        // Act
        options.SubscriptionClientName = "FirstName";
        var firstName = options.SubscriptionClientName;
        options.SubscriptionClientName = "SecondName";
        var secondName = options.SubscriptionClientName;

        // Assert
        Assert.Equal("FirstName", firstName);
        Assert.Equal("SecondName", secondName);
        Assert.Equal("SecondName", options.SubscriptionClientName);
    }

    [Fact]
    public void RetryCount_ShouldAllowMaxIntValue()
    {
        // Arrange
        var options = new EventBusOptions();

        // Act
        options.RetryCount = int.MaxValue;

        // Assert
        Assert.Equal(int.MaxValue, options.RetryCount);
    }

    [Fact]
    public void SubscriptionClientName_ShouldAllowLongStrings()
    {
        // Arrange
        var options = new EventBusOptions();
        var longName = new string('a', 1000);

        // Act
        options.SubscriptionClientName = longName;

        // Assert
        Assert.Equal(longName, options.SubscriptionClientName);
        Assert.Equal(1000, options.SubscriptionClientName.Length);
    }

    [Fact]
    public void SubscriptionClientName_ShouldAllowUnicodeCharacters()
    {
        // Arrange
        var options = new EventBusOptions();
        var unicodeName = "Client_日本語_العربية_🚀";

        // Act
        options.SubscriptionClientName = unicodeName;

        // Assert
        Assert.Equal(unicodeName, options.SubscriptionClientName);
    }

    [Fact]
    public void Class_ShouldBePublic()
    {
        // Arrange & Act
        var type = typeof(EventBusOptions);

        // Assert
        Assert.True(type.IsPublic);
    }

    [Fact]
    public void SubscriptionClientName_ShouldBePublicProperty()
    {
        // Arrange & Act
        var property = typeof(EventBusOptions).GetProperty(nameof(EventBusOptions.SubscriptionClientName));

        // Assert
        Assert.NotNull(property);
        Assert.True(property.CanRead);
        Assert.True(property.CanWrite);
    }

    [Fact]
    public void RetryCount_ShouldBePublicProperty()
    {
        // Arrange & Act
        var property = typeof(EventBusOptions).GetProperty(nameof(EventBusOptions.RetryCount));

        // Assert
        Assert.NotNull(property);
        Assert.True(property.CanRead);
        Assert.True(property.CanWrite);
    }
}
