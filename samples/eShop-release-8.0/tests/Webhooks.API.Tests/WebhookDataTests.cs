namespace Webhooks.API.Tests;

using System.Text.Json;
using Webhooks.API.Model;

public class WebhookDataTests
{
    [Fact]
    public void Constructor_WithValidWebhookType_InitializesProperties()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { OrderId = 123, Amount = 99.99 };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData);
        Assert.Equal("OrderPaid", webhookData.Type);
        Assert.NotNull(webhookData.Payload);
        Assert.NotEqual(default(DateTime), webhookData.When);
    }

    [Fact]
    public void Constructor_SetsWhenToUtcNow()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;
        var hookType = WebhookType.OrderPaid;
        var data = new { };

        // Act
        var webhookData = new WebhookData(hookType, data);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(webhookData.When >= beforeCreation);
        Assert.True(webhookData.When <= afterCreation.AddSeconds(1));
    }

    [Fact]
    public void Constructor_WithOrderPaidType_SetCorrectTypeString()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.Equal("OrderPaid", webhookData.Type);
    }

    [Fact]
    public void Constructor_WithOrderShippedType_SetCorrectTypeString()
    {
        // Arrange
        var hookType = WebhookType.OrderShipped;
        var data = new { };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.Equal("OrderShipped", webhookData.Type);
    }

    [Fact]
    public void Constructor_WithCatalogItemPriceChangeType_SetCorrectTypeString()
    {
        // Arrange
        var hookType = WebhookType.CatalogItemPriceChange;
        var data = new { };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.Equal("CatalogItemPriceChange", webhookData.Type);
    }

    [Fact]
    public void Constructor_SerializesDataToJsonPayload()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { OrderId = 123 };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.Contains("OrderId", webhookData.Payload);
        Assert.Contains("123", webhookData.Payload);
    }

    [Fact]
    public void Constructor_WithComplexObject_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new
        {
            OrderId = 123,
            Amount = 99.99,
            Currency = "USD",
            Items = new[] { "item1", "item2" }
        };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        var parsedPayload = JsonSerializer.Deserialize<dynamic>(webhookData.Payload);
        Assert.NotNull(parsedPayload);
    }

    [Fact]
    public void Constructor_WithEmptyObject_SerializesAsEmptyJson()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.Equal("{}", webhookData.Payload);
    }

    [Fact]
    public void Constructor_WithNullableProperties_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new
        {
            OrderId = 123,
            Description = (string?)null,
            Amount = 99.99
        };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.Contains("null", webhookData.Payload);
    }

    [Fact]
    public void Constructor_WithStringData_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = "test string data";

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.Contains("test string data", webhookData.Payload);
    }

    [Fact]
    public void Constructor_WithIntegerData_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = 12345;

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.Equal("12345", webhookData.Payload);
    }

    [Fact]
    public void Constructor_WithBooleanData_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = true;

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.Equal("true", webhookData.Payload);
    }

    [Fact]
    public void Constructor_PayloadPropertyIsReadOnly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { OrderId = 123 };
        var webhookData = new WebhookData(hookType, data);

        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() =>
        {
            typeof(WebhookData).GetProperty("Payload")?.SetValue(webhookData, "new value");
        });
    }

    [Fact]
    public void Constructor_TypePropertyIsReadOnly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { };
        var webhookData = new WebhookData(hookType, data);

        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() =>
        {
            typeof(WebhookData).GetProperty("Type")?.SetValue(webhookData, "NewType");
        });
    }

    [Fact]
    public void Constructor_WhenPropertyIsReadOnly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { };
        var webhookData = new WebhookData(hookType, data);

        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() =>
        {
            typeof(WebhookData).GetProperty("When")?.SetValue(webhookData, DateTime.UtcNow.AddDays(1));
        });
    }

    [Fact]
    public void Constructor_MultipleInstances_HaveDifferentWhenTimes()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { };

        // Act
        var webhookData1 = new WebhookData(hookType, data);
        System.Threading.Thread.Sleep(10);
        var webhookData2 = new WebhookData(hookType, data);

        // Assert
        Assert.True(webhookData2.When >= webhookData1.When);
    }

    [Fact]
    public void Constructor_WithNestedObject_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new
        {
            Order = new
            {
                OrderId = 123,
                Customer = new
                {
                    Name = "John Doe",
                    Email = "john@example.com"
                }
            }
        };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.Contains("Order", webhookData.Payload);
        Assert.Contains("Customer", webhookData.Payload);
        Assert.Contains("John Doe", webhookData.Payload);
    }

    [Fact]
    public void Constructor_WithArrayData_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new[] { 1, 2, 3, 4, 5 };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.Contains("[", webhookData.Payload);
        Assert.Contains("]", webhookData.Payload);
    }

    [Fact]
    public void Constructor_WithListData_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new List<int> { 1, 2, 3 };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.Contains("[", webhookData.Payload);
    }

    [Fact]
    public void Constructor_WhenIsInUtc()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.True(webhookData.When.Kind == DateTimeKind.Utc);
    }

    [Fact]
    public void Constructor_WithSpecialCharactersInData_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { Description = "Test with special chars: @#$%^&*()" };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.Contains("Description", webhookData.Payload);
    }

    [Fact]
    public void Constructor_WithUnicodeData_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { Description = "Test with unicode: 你好世界" };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.Contains("Description", webhookData.Payload);
    }

    [Fact]
    public void Constructor_PayloadCanBeParsedAsJson()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var expectedOrderId = 123;
        var data = new { OrderId = expectedOrderId };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var deserialized = JsonSerializer.Deserialize<JsonElement>(webhookData.Payload, options);
        Assert.NotEqual(default, deserialized);
    }

    [Fact]
    public void Constructor_AllWebhookTypes_AreHandled()
    {
        // Arrange & Act & Assert
        foreach (var hookType in Enum.GetValues(typeof(WebhookType)).Cast<WebhookType>())
        {
            var webhookData = new WebhookData(hookType, new { });
            Assert.NotNull(webhookData.Type);
            Assert.Equal(hookType.ToString(), webhookData.Type);
        }
    }

    [Fact]
    public void Constructor_WithLargePayload_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var largeData = new
        {
            Items = Enumerable.Range(1, 1000).Select(i => new { Id = i, Name = $"Item {i}" }).ToList()
        };

        // Act
        var webhookData = new WebhookData(hookType, largeData);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.True(webhookData.Payload.Length > 1000);
    }

    [Fact]
    public void Constructor_PropertiesAreAccessible()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { OrderId = 123 };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.True(webhookData.When != default(DateTime));
        Assert.NotNull(webhookData.Type);
        Assert.NotNull(webhookData.Payload);
    }

    [Fact]
    public void Constructor_WithDecimalData_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var data = new { Price = 99.99m };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.Contains("99.99", webhookData.Payload);
    }

    [Fact]
    public void Constructor_WithGuidData_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var testGuid = Guid.NewGuid();
        var data = new { Id = testGuid };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.Contains(testGuid.ToString(), webhookData.Payload);
    }

    [Fact]
    public void Constructor_WithDateTimeData_SerializesCorrectly()
    {
        // Arrange
        var hookType = WebhookType.OrderPaid;
        var testDate = DateTime.UtcNow;
        var data = new { CreatedAt = testDate };

        // Act
        var webhookData = new WebhookData(hookType, data);

        // Assert
        Assert.NotNull(webhookData.Payload);
        Assert.Contains("CreatedAt", webhookData.Payload);
    }
}
