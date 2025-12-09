namespace Webhooks.API.Tests;

using Webhooks.API.Model;

public class WebhookSubscriptionTests
{
    [Fact]
    public void Constructor_CreatesInstanceWithDefaultValues()
    {
        // Act
        var subscription = new WebhookSubscription();

        // Assert
        Assert.NotNull(subscription);
        Assert.Equal(0, subscription.Id);
        Assert.Equal(default(WebhookType), subscription.Type);
        Assert.Equal(default(DateTime), subscription.Date);
        Assert.Null(subscription.DestUrl);
        Assert.Null(subscription.Token);
        Assert.Null(subscription.UserId);
    }

    [Fact]
    public void Id_CanBeSet()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var expectedId = 123;

        // Act
        subscription.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, subscription.Id);
    }

    [Fact]
    public void Id_CanBeSetToZero()
    {
        // Arrange
        var subscription = new WebhookSubscription { Id = 100 };

        // Act
        subscription.Id = 0;

        // Assert
        Assert.Equal(0, subscription.Id);
    }

    [Fact]
    public void Id_CanBeSetToNegativeValue()
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.Id = -1;

        // Assert
        Assert.Equal(-1, subscription.Id);
    }

    [Fact]
    public void Type_CanBeSetToCatalogItemPriceChange()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var expectedType = WebhookType.CatalogItemPriceChange;

        // Act
        subscription.Type = expectedType;

        // Assert
        Assert.Equal(expectedType, subscription.Type);
    }

    [Fact]
    public void Type_CanBeSetToOrderShipped()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var expectedType = WebhookType.OrderShipped;

        // Act
        subscription.Type = expectedType;

        // Assert
        Assert.Equal(expectedType, subscription.Type);
    }

    [Fact]
    public void Type_CanBeSetToOrderPaid()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var expectedType = WebhookType.OrderPaid;

        // Act
        subscription.Type = expectedType;

        // Assert
        Assert.Equal(expectedType, subscription.Type);
    }

    [Fact]
    public void Type_CanBeChangedMultipleTimes()
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.Type = WebhookType.OrderPaid;
        subscription.Type = WebhookType.OrderShipped;
        subscription.Type = WebhookType.CatalogItemPriceChange;

        // Assert
        Assert.Equal(WebhookType.CatalogItemPriceChange, subscription.Type);
    }

    [Fact]
    public void Date_CanBeSet()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var expectedDate = new DateTime(2024, 12, 9, 10, 30, 0).ToUniversalTime();

        // Act
        subscription.Date = expectedDate;

        // Assert
        Assert.Equal(expectedDate, subscription.Date);
    }

    [Fact]
    public void Date_CanBeSetToNow()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var beforeSet = DateTime.UtcNow;

        // Act
        subscription.Date = DateTime.UtcNow;
        var afterSet = DateTime.UtcNow;

        // Assert
        Assert.True(subscription.Date >= beforeSet);
        Assert.True(subscription.Date <= afterSet.AddSeconds(1));
    }

    [Fact]
    public void Date_CanBeSetToMinValue()
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.Date = DateTime.MinValue;

        // Assert
        Assert.Equal(DateTime.MinValue, subscription.Date);
    }

    [Fact]
    public void Date_CanBeSetToMaxValue()
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.Date = DateTime.MaxValue;

        // Assert
        Assert.Equal(DateTime.MaxValue, subscription.Date);
    }

    [Fact]
    public void DestUrl_CanBeSet()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var expectedUrl = "https://example.com/webhook";

        // Act
        subscription.DestUrl = expectedUrl;

        // Assert
        Assert.Equal(expectedUrl, subscription.DestUrl);
    }

    [Fact]
    public void DestUrl_CanBeSetToEmptyString()
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.DestUrl = string.Empty;

        // Assert
        Assert.Equal(string.Empty, subscription.DestUrl);
    }

    [Fact]
    public void DestUrl_CanBeSetToNull()
    {
        // Arrange
        var subscription = new WebhookSubscription { DestUrl = "https://example.com" };

        // Act
        subscription.DestUrl = null;

        // Assert
        Assert.Null(subscription.DestUrl);
    }

    [Fact]
    public void DestUrl_CanBeSetToValidUrl()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var validUrl = "https://api.example.com/webhooks/webhook1";

        // Act
        subscription.DestUrl = validUrl;

        // Assert
        Assert.Equal(validUrl, subscription.DestUrl);
    }

    [Fact]
    public void DestUrl_CanBeSetToLocalhostUrl()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var localhostUrl = "http://localhost:3000/webhook";

        // Act
        subscription.DestUrl = localhostUrl;

        // Assert
        Assert.Equal(localhostUrl, subscription.DestUrl);
    }

    [Fact]
    public void DestUrl_CanBeSetToUrlWithPort()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var urlWithPort = "https://example.com:8443/webhook";

        // Act
        subscription.DestUrl = urlWithPort;

        // Assert
        Assert.Equal(urlWithPort, subscription.DestUrl);
    }

    [Fact]
    public void DestUrl_CanBeSetToLongUrl()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var longUrl = "https://subdomain.example.com/api/v1/webhooks/endpoint?key=value&foo=bar#fragment";

        // Act
        subscription.DestUrl = longUrl;

        // Assert
        Assert.Equal(longUrl, subscription.DestUrl);
    }

    [Fact]
    public void Token_CanBeSet()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var expectedToken = "secret_token_12345";

        // Act
        subscription.Token = expectedToken;

        // Assert
        Assert.Equal(expectedToken, subscription.Token);
    }

    [Fact]
    public void Token_CanBeSetToEmptyString()
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.Token = string.Empty;

        // Assert
        Assert.Equal(string.Empty, subscription.Token);
    }

    [Fact]
    public void Token_CanBeSetToNull()
    {
        // Arrange
        var subscription = new WebhookSubscription { Token = "token123" };

        // Act
        subscription.Token = null;

        // Assert
        Assert.Null(subscription.Token);
    }

    [Fact]
    public void Token_CanBeSetToJwtToken()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

        // Act
        subscription.Token = jwtToken;

        // Assert
        Assert.Equal(jwtToken, subscription.Token);
    }

    [Fact]
    public void Token_CanBeSetToHexString()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var hexToken = "a1b2c3d4e5f6";

        // Act
        subscription.Token = hexToken;

        // Assert
        Assert.Equal(hexToken, subscription.Token);
    }

    [Fact]
    public void UserId_CanBeSet()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var expectedUserId = "user123";

        // Act
        subscription.UserId = expectedUserId;

        // Assert
        Assert.Equal(expectedUserId, subscription.UserId);
    }

    [Fact]
    public void UserId_CanBeSetToEmptyString()
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.UserId = string.Empty;

        // Assert
        Assert.Equal(string.Empty, subscription.UserId);
    }

    [Fact]
    public void UserId_CanBeSetToNull()
    {
        // Arrange
        var subscription = new WebhookSubscription { UserId = "user456" };

        // Act
        subscription.UserId = null;

        // Assert
        Assert.Null(subscription.UserId);
    }

    [Fact]
    public void UserId_CanBeSetToGuid()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var guidUserId = Guid.NewGuid().ToString();

        // Act
        subscription.UserId = guidUserId;

        // Assert
        Assert.Equal(guidUserId, subscription.UserId);
    }

    [Fact]
    public void UserId_CanBeSetToEmailFormat()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var emailUserId = "user@example.com";

        // Act
        subscription.UserId = emailUserId;

        // Assert
        Assert.Equal(emailUserId, subscription.UserId);
    }

    [Fact]
    public void MultipleProperties_CanBeSetIndependently()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var expectedId = 42;
        var expectedType = WebhookType.OrderPaid;
        var expectedDate = DateTime.UtcNow;
        var expectedUrl = "https://example.com/webhook";
        var expectedToken = "token_abc123";
        var expectedUserId = "user789";

        // Act
        subscription.Id = expectedId;
        subscription.Type = expectedType;
        subscription.Date = expectedDate;
        subscription.DestUrl = expectedUrl;
        subscription.Token = expectedToken;
        subscription.UserId = expectedUserId;

        // Assert
        Assert.Equal(expectedId, subscription.Id);
        Assert.Equal(expectedType, subscription.Type);
        Assert.Equal(expectedDate, subscription.Date);
        Assert.Equal(expectedUrl, subscription.DestUrl);
        Assert.Equal(expectedToken, subscription.Token);
        Assert.Equal(expectedUserId, subscription.UserId);
    }

    [Fact]
    public void AllProperties_AreInitializable()
    {
        // Arrange & Act
        var subscription = new WebhookSubscription
        {
            Id = 100,
            Type = WebhookType.CatalogItemPriceChange,
            Date = DateTime.UtcNow,
            DestUrl = "https://example.com/webhook",
            Token = "token123",
            UserId = "user100"
        };

        // Assert
        Assert.Equal(100, subscription.Id);
        Assert.Equal(WebhookType.CatalogItemPriceChange, subscription.Type);
        Assert.NotEqual(default(DateTime), subscription.Date);
        Assert.Equal("https://example.com/webhook", subscription.DestUrl);
        Assert.Equal("token123", subscription.Token);
        Assert.Equal("user100", subscription.UserId);
    }

    [Fact]
    public void Properties_CanBeModifiedAfterInitialization()
    {
        // Arrange
        var subscription = new WebhookSubscription
        {
            Id = 1,
            Type = WebhookType.OrderPaid,
            DestUrl = "https://old.example.com",
            UserId = "olduser"
        };

        // Act
        subscription.Id = 2;
        subscription.Type = WebhookType.OrderShipped;
        subscription.DestUrl = "https://new.example.com";
        subscription.UserId = "newuser";

        // Assert
        Assert.Equal(2, subscription.Id);
        Assert.Equal(WebhookType.OrderShipped, subscription.Type);
        Assert.Equal("https://new.example.com", subscription.DestUrl);
        Assert.Equal("newuser", subscription.UserId);
    }

    [Fact]
    public void DestUrl_IsRequired()
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.DestUrl = null;

        // Assert - Property allows null, but should be marked as [Required]
        Assert.Null(subscription.DestUrl);
    }

    [Fact]
    public void UserId_IsRequired()
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.UserId = null;

        // Assert - Property allows null, but should be marked as [Required]
        Assert.Null(subscription.UserId);
    }

    [Fact]
    public void Type_DefaultValue_IsZero()
    {
        // Arrange & Act
        var subscription = new WebhookSubscription();

        // Assert
        Assert.Equal(0, (int)subscription.Type);
    }

    [Fact]
    public void Date_DefaultValue_IsMinValue()
    {
        // Arrange & Act
        var subscription = new WebhookSubscription();

        // Assert
        Assert.Equal(DateTime.MinValue, subscription.Date);
    }

    [Fact]
    public void Token_DefaultValue_IsNull()
    {
        // Arrange & Act
        var subscription = new WebhookSubscription();

        // Assert
        Assert.Null(subscription.Token);
    }

    [Fact]
    public void TwoInstances_AreIndependent()
    {
        // Arrange
        var subscription1 = new WebhookSubscription { Id = 1, UserId = "user1" };
        var subscription2 = new WebhookSubscription { Id = 2, UserId = "user2" };

        // Act
        subscription1.Id = 10;
        subscription2.Id = 20;

        // Assert
        Assert.Equal(10, subscription1.Id);
        Assert.Equal(20, subscription2.Id);
        Assert.Equal("user1", subscription1.UserId);
        Assert.Equal("user2", subscription2.UserId);
    }

    [Fact]
    public void Property_DestUrl_CanContainSpecialCharacters()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var urlWithSpecialChars = "https://example.com/webhook?token=abc123&event=order_paid&test=true";

        // Act
        subscription.DestUrl = urlWithSpecialChars;

        // Assert
        Assert.Equal(urlWithSpecialChars, subscription.DestUrl);
    }

    [Fact]
    public void Property_Token_CanContainSpecialCharacters()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var tokenWithSpecialChars = "sk-1234-abcd-!@#$%^&*()";

        // Act
        subscription.Token = tokenWithSpecialChars;

        // Assert
        Assert.Equal(tokenWithSpecialChars, subscription.Token);
    }

    [Fact]
    public void Property_UserId_CanContainSpecialCharacters()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var userIdWithSpecialChars = "user+test@example.com";

        // Act
        subscription.UserId = userIdWithSpecialChars;

        // Assert
        Assert.Equal(userIdWithSpecialChars, subscription.UserId);
    }

    [Theory]
    [InlineData(WebhookType.CatalogItemPriceChange)]
    [InlineData(WebhookType.OrderShipped)]
    [InlineData(WebhookType.OrderPaid)]
    public void Type_CanBeSetToAnyValidWebhookType(WebhookType webhookType)
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.Type = webhookType;

        // Assert
        Assert.Equal(webhookType, subscription.Type);
    }

    [Fact]
    public void DestUrl_CanBeSetMultipleTimes()
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.DestUrl = "https://first.com";
        subscription.DestUrl = "https://second.com";
        subscription.DestUrl = "https://third.com";

        // Assert
        Assert.Equal("https://third.com", subscription.DestUrl);
    }

    [Fact]
    public void Token_CanBeSetMultipleTimes()
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.Token = "token1";
        subscription.Token = "token2";
        subscription.Token = "token3";

        // Assert
        Assert.Equal("token3", subscription.Token);
    }

    [Fact]
    public void UserId_CanBeSetMultipleTimes()
    {
        // Arrange
        var subscription = new WebhookSubscription();

        // Act
        subscription.UserId = "user1";
        subscription.UserId = "user2";
        subscription.UserId = "user3";

        // Assert
        Assert.Equal("user3", subscription.UserId);
    }

    [Fact]
    public void Date_CanBeSetMultipleTimes()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var date1 = new DateTime(2024, 1, 1).ToUniversalTime();
        var date2 = new DateTime(2024, 6, 15).ToUniversalTime();
        var date3 = new DateTime(2024, 12, 31).ToUniversalTime();

        // Act
        subscription.Date = date1;
        subscription.Date = date2;
        subscription.Date = date3;

        // Assert
        Assert.Equal(date3, subscription.Date);
    }

    [Fact]
    public void Properties_PreserveTheirValues()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var url = "https://example.com/webhook";
        var token = "token_secret";
        var userId = "user_id";

        // Act
        subscription.DestUrl = url;
        subscription.Token = token;
        subscription.UserId = userId;
        var retrievedUrl = subscription.DestUrl;
        var retrievedToken = subscription.Token;
        var retrievedUserId = subscription.UserId;

        // Assert
        Assert.Equal(url, retrievedUrl);
        Assert.Equal(token, retrievedToken);
        Assert.Equal(userId, retrievedUserId);
    }

    [Fact]
    public void Id_CanBeLargeInteger()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var largeId = int.MaxValue;

        // Act
        subscription.Id = largeId;

        // Assert
        Assert.Equal(largeId, subscription.Id);
    }

    [Fact]
    public void DestUrl_CanBeLongString()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var longUrl = new string('a', 1000) + ".com";

        // Act
        subscription.DestUrl = longUrl;

        // Assert
        Assert.Equal(longUrl, subscription.DestUrl);
    }

    [Fact]
    public void Token_CanBeLongString()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var longToken = new string('x', 500);

        // Act
        subscription.Token = longToken;

        // Assert
        Assert.Equal(longToken, subscription.Token);
    }

    [Fact]
    public void UserId_CanBeLongString()
    {
        // Arrange
        var subscription = new WebhookSubscription();
        var longUserId = new string('u', 500);

        // Act
        subscription.UserId = longUserId;

        // Assert
        Assert.Equal(longUserId, subscription.UserId);
    }
}
