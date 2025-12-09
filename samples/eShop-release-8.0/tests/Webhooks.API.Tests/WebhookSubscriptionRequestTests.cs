namespace Webhooks.API.Tests;

using System.ComponentModel.DataAnnotations;
using Webhooks.API.Model;

public class WebhookSubscriptionRequestTests
{
    [Fact]
    public void Constructor_CreateInstance_WithAllPropertiesSet()
    {
        // Arrange
        var url = "https://example.com/webhook";
        var token = "test-token-123";
        var eventName = "OrderPaid";
        var grantUrl = "https://example.com/grant";

        // Act
        var request = new WebhookSubscriptionRequest
        {
            Url = url,
            Token = token,
            Event = eventName,
            GrantUrl = grantUrl
        };

        // Assert
        Assert.Equal(url, request.Url);
        Assert.Equal(token, request.Token);
        Assert.Equal(eventName, request.Event);
        Assert.Equal(grantUrl, request.GrantUrl);
    }

    [Fact]
    public void Validate_WithValidAllProperties_ReturnsNoErrors()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "valid-token",
            Event = "OrderPaid",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Validate_WithValidUrlAndInvalidGrantUrl_ReturnsGrantUrlError()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "valid-token",
            Event = "OrderPaid",
            GrantUrl = "not-a-valid-url"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Single(results);
        Assert.Contains("GrantUrl is not valid", results[0].ErrorMessage);
        Assert.Contains(nameof(request.GrantUrl), results[0].MemberNames);
    }

    [Fact]
    public void Validate_WithInvalidUrlAndValidGrantUrl_ReturnsUrlError()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "invalid-url",
            Token = "valid-token",
            Event = "OrderPaid",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Single(results);
        Assert.Contains("Url is not valid", results[0].ErrorMessage);
        Assert.Contains(nameof(request.Url), results[0].MemberNames);
    }

    [Fact]
    public void Validate_WithInvalidUrlAndInvalidGrantUrl_ReturnsBothErrors()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "not-a-url",
            Token = "valid-token",
            Event = "OrderPaid",
            GrantUrl = "also-not-a-url"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Equal(2, results.Count);
        var grantUrlError = results.FirstOrDefault(r => r.MemberNames.Contains(nameof(request.GrantUrl)));
        var urlError = results.FirstOrDefault(r => r.MemberNames.Contains(nameof(request.Url)));
        Assert.NotNull(grantUrlError);
        Assert.NotNull(urlError);
    }

    [Fact]
    public void Validate_WithInvalidEvent_ReturnsEventError()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "valid-token",
            Event = "InvalidEvent",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Single(results);
        Assert.Contains("InvalidEvent is invalid event name", results[0].ErrorMessage);
        Assert.Contains(nameof(request.Event), results[0].MemberNames);
    }

    [Fact]
    public void Validate_WithInvalidEventAndInvalidUrls_ReturnsAllErrors()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "not-valid",
            Token = "token",
            Event = "BadEvent",
            GrantUrl = "also-invalid"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Equal(3, results.Count);
    }

    [Fact]
    public void Validate_WithCatalogItemPriceChangeEvent_ReturnsNoErrors()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "CatalogItemPriceChange",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Validate_WithOrderShippedEvent_ReturnsNoErrors()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "OrderShipped",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Validate_WithOrderPaidEvent_ReturnsNoErrors()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Validate_WithEventNameIgnoringCase_ReturnsNoErrors()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "orderpaid",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Validate_WithMixedCaseEventName_ReturnsNoErrors()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "OrDeRpAiD",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Theory]
    [InlineData("https://example.com/webhook")]
    [InlineData("https://subdomain.example.com/webhook")]
    [InlineData("https://example.co.uk/webhook")]
    [InlineData("https://192.168.1.1/webhook")]
    [InlineData("https://localhost:8080/webhook")]
    public void Validate_WithVariousValidUrls_ReturnsNoErrors(string url)
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = url,
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Theory]
    [InlineData("https://example.com/grant")]
    [InlineData("https://subdomain.example.com/grant")]
    [InlineData("https://example.co.uk/grant")]
    [InlineData("https://192.168.1.1/grant")]
    [InlineData("https://localhost:8080/grant")]
    public void Validate_WithVariousValidGrantUrls_ReturnsNoErrors(string grantUrl)
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = grantUrl
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Theory]
    [InlineData("")]
    [InlineData("http://")]
    [InlineData("just-text")]
    [InlineData("ht!tp://invalid.com/webhook")]
    public void Validate_WithInvalidUrlFormats_ReturnsUrlError(string invalidUrl)
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = invalidUrl,
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.NotEmpty(results.Where(r => r.MemberNames.Contains(nameof(request.Url))));
    }

    [Theory]
    [InlineData("")]
    [InlineData("http://")]
    [InlineData("just-text")]
    [InlineData("ht!tp://invalid.com/grant")]
    public void Validate_WithInvalidGrantUrlFormats_ReturnsGrantUrlError(string invalidGrantUrl)
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = invalidGrantUrl
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.NotEmpty(results.Where(r => r.MemberNames.Contains(nameof(request.GrantUrl))));
    }

    [Fact]
    public void Validate_WithNullUrl_ReturnsUrlError()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = null,
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.NotEmpty(results.Where(r => r.MemberNames.Contains(nameof(request.Url))));
    }

    [Fact]
    public void Validate_WithNullGrantUrl_ReturnsGrantUrlError()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = null
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.NotEmpty(results.Where(r => r.MemberNames.Contains(nameof(request.GrantUrl))));
    }

    [Fact]
    public void Validate_WithNullEvent_ReturnsEventError()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = null,
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.NotEmpty(results.Where(r => r.MemberNames.Contains(nameof(request.Event))));
    }

    [Fact]
    public void Validate_WithEmptyEvent_ReturnsEventError()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.NotEmpty(results.Where(r => r.MemberNames.Contains(nameof(request.Event))));
    }

    [Fact]
    public void Validate_WithWhitespaceEvent_ReturnsEventError()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "   ",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.NotEmpty(results.Where(r => r.MemberNames.Contains(nameof(request.Event))));
    }

    [Fact]
    public void Validate_TokenCanBeNull()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = null,
            Event = "OrderPaid",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Validate_TokenCanBeEmpty()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "",
            Event = "OrderPaid",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Validate_ImplementsIValidatableObject()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest();

        // Act & Assert
        Assert.IsAssignableFrom<IValidatableObject>(request);
    }

    [Fact]
    public void Validate_ValidateMethodReturnsEnumerable()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<ValidationResult>>(results);
    }

    [Fact]
    public void Validate_ErrorMessageIncludesEventNameWhenInvalid()
    {
        // Arrange
        var invalidEvent = "NonExistentEvent";
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = invalidEvent,
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Single(results);
        Assert.Contains(invalidEvent, results[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WithValidComplexUrl_ReturnsNoErrors()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://api.example.com/v1/webhooks/receive?key=value&id=123",
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Validate_WithValidComplexGrantUrl_ReturnsNoErrors()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = "https://api.example.com/v1/grant?code=abc&state=xyz"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Validate_EventValidationIsCaseSensitiveForEnumNames()
    {
        // Arrange - verify that enum parsing is case-insensitive but names match
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "ORDERPAID",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Validate_WithRelativeUrl_ReturnsUrlError()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "/webhook/endpoint",
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = "https://example.com/grant"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.NotEmpty(results.Where(r => r.MemberNames.Contains(nameof(request.Url))));
    }

    [Fact]
    public void Validate_WithRelativeGrantUrl_ReturnsGrantUrlError()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = "/grant/endpoint"
        };
        var context = new ValidationContext(request);

        // Act
        var results = request.Validate(context).ToList();

        // Assert
        Assert.NotEmpty(results.Where(r => r.MemberNames.Contains(nameof(request.GrantUrl))));
    }

    [Fact]
    public void Properties_AreWritable()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest();

        // Act
        request.Url = "https://example.com/webhook";
        request.Token = "token";
        request.Event = "OrderPaid";
        request.GrantUrl = "https://example.com/grant";

        // Assert
        Assert.Equal("https://example.com/webhook", request.Url);
        Assert.Equal("token", request.Token);
        Assert.Equal("OrderPaid", request.Event);
        Assert.Equal("https://example.com/grant", request.GrantUrl);
    }

    [Fact]
    public void Properties_CanBeModifiedAfterInitialization()
    {
        // Arrange
        var request = new WebhookSubscriptionRequest
        {
            Url = "https://example.com/webhook",
            Token = "token",
            Event = "OrderPaid",
            GrantUrl = "https://example.com/grant"
        };

        // Act
        request.Event = "OrderShipped";
        request.Token = "new-token";

        // Assert
        Assert.Equal("OrderShipped", request.Event);
        Assert.Equal("new-token", request.Token);
    }
}
