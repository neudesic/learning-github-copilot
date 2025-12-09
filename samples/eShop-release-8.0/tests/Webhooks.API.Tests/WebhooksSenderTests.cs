namespace Webhooks.API.Tests;

using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using Webhooks.API.Model;

public class WebhooksSenderTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<ILogger<WebhooksSender>> _loggerMock;
    private readonly WebhooksSender _sender;

    public WebhooksSenderTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _loggerMock = new Mock<ILogger<WebhooksSender>>();
        _sender = new WebhooksSender(_httpClientFactoryMock.Object, _loggerMock.Object);
    }

    private void SetupHttpClientFactory(HttpClient httpClient)
    {
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
    }

    [Fact]
    public async Task SendAll_WithSingleSubscriber_SendsWebhookToCorrectUrl()
    {
        // Arrange
        var destUrl = "https://webhook.example.com/receiver";
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = destUrl,
                Type = WebhookType.OrderPaid,
                Token = "test-token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { OrderId = 123 });

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg =>
                msg.RequestUri == new Uri(destUrl) &&
                msg.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_WithMultipleSubscribers_SendsWebhookToAllUrls()
    {
        // Arrange
        var subscriber1 = new WebhookSubscription
        {
            Id = 1,
            DestUrl = "https://webhook1.example.com/receiver",
            Type = WebhookType.OrderPaid,
            Token = "token1",
            UserId = "user1"
        };

        var subscriber2 = new WebhookSubscription
        {
            Id = 2,
            DestUrl = "https://webhook2.example.com/receiver",
            Type = WebhookType.OrderPaid,
            Token = "token2",
            UserId = "user2"
        };

        var subscribers = new[] { subscriber1, subscriber2 };
        var webhookData = new WebhookData(WebhookType.OrderPaid, new { OrderId = 456 });

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(2),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_WithToken_IncludesTokenInRequestHeader()
    {
        // Arrange
        var token = "secret-webhook-token";
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = token,
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg =>
                msg.Headers.Contains("X-eshop-whtoken") &&
                msg.Headers.GetValues("X-eshop-whtoken").First() == token),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_WithoutToken_DoesNotIncludeTokenHeader()
    {
        // Arrange
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = null,
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg =>
                !msg.Headers.Contains("X-eshop-whtoken")),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_WithEmptyToken_DoesNotIncludeTokenHeader()
    {
        // Arrange
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = "   ",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg =>
                !msg.Headers.Contains("X-eshop-whtoken")),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_SendsJsonSerializedData()
    {
        // Arrange
        var testPayload = new { OrderId = 789, Amount = 99.99 };
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = "token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, testPayload);

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg =>
                msg.Content != null &&
                msg.Content.Headers.ContentType != null &&
                msg.Content.Headers.ContentType.MediaType == "application/json"),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_UsesPostMethod()
    {
        // Arrange
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = "token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg => msg.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_WithEmptySubscriberList_CompletesSuccessfully()
    {
        // Arrange
        var subscribers = Array.Empty<WebhookSubscription>();
        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var httpClient = new HttpClient();
        SetupHttpClientFactory(httpClient);

        // Act & Assert
        await _sender.SendAll(subscribers, webhookData);
    }

    [Fact]
    public async Task SendAll_ConvertsWebhookDataToJson()
    {
        // Arrange
        string? capturedContent = null;

        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = "token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { OrderId = 123 });

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, ct) =>
            {
                if (req.Content != null)
                {
                    capturedContent = req.Content.ReadAsStringAsync(ct).Result;
                }
            })
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        Assert.NotNull(capturedContent);
        Assert.Contains("OrderPaid", capturedContent!);
    }

    [Fact]
    public async Task SendAll_WithHttpRequestException_StillProcessesOtherSubscribers()
    {
        // Arrange
        var subscriber1 = new WebhookSubscription
        {
            Id = 1,
            DestUrl = "https://webhook1.example.com/receiver",
            Type = WebhookType.OrderPaid,
            Token = "token1",
            UserId = "user1"
        };

        var subscriber2 = new WebhookSubscription
        {
            Id = 2,
            DestUrl = "https://webhook2.example.com/receiver",
            Type = WebhookType.OrderPaid,
            Token = "token2",
            UserId = "user2"
        };

        var subscribers = new[] { subscriber1, subscriber2 };
        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg => msg.RequestUri == new Uri(subscriber1.DestUrl)),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Connection failed"));

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg => msg.RequestUri == new Uri(subscriber2.DestUrl)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(() =>
            _sender.SendAll(subscribers, webhookData));

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task SendAll_LogsDebugMessageWhenDebugLevelEnabled()
    {
        // Arrange
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = "token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        _loggerMock.Setup(l => l.IsEnabled(LogLevel.Debug)).Returns(true);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task SendAll_DoesNotLogDebugMessageWhenDebugLevelDisabled()
    {
        // Arrange
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = "token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        _loggerMock.Setup(l => l.IsEnabled(LogLevel.Debug)).Returns(false);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    [Fact]
    public async Task SendAll_WithUtf8ContentEncoding()
    {
        // Arrange
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = "token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg =>
                msg.Content != null && msg.Content.Headers.ContentType != null &&
                msg.Content.Headers.ContentType.CharSet == "utf-8"),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_WithAbsoluteUri_ValidatesUri()
    {
        // Arrange
        var destUrl = "https://webhook.example.com/receiver";
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = destUrl,
                Type = WebhookType.OrderPaid,
                Token = "token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg =>
                msg.RequestUri != null &&
                msg.RequestUri.IsAbsoluteUri &&
                msg.RequestUri == new Uri(destUrl, UriKind.Absolute)),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_ConcurrentExecution_ProcessesAllSubscribersConcurrently()
    {
        // Arrange
        var subscribers = Enumerable.Range(1, 10)
            .Select(i => new WebhookSubscription
            {
                Id = i,
                DestUrl = $"https://webhook{i}.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = $"token{i}",
                UserId = $"user{i}"
            })
            .ToArray();

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert - Verify all 10 requests were made
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(10),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_MultipleTokenHeaders_AddsTokenToRequest()
    {
        // Arrange
        var token1 = "token1";
        var token2 = "token2";

        var subscriber1 = new WebhookSubscription
        {
            Id = 1,
            DestUrl = "https://webhook1.example.com/receiver",
            Type = WebhookType.OrderPaid,
            Token = token1,
            UserId = "user1"
        };

        var subscriber2 = new WebhookSubscription
        {
            Id = 2,
            DestUrl = "https://webhook2.example.com/receiver",
            Type = WebhookType.OrderPaid,
            Token = token2,
            UserId = "user2"
        };

        var subscribers = new[] { subscriber1, subscriber2 };
        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg =>
                msg.RequestUri == new Uri(subscriber1.DestUrl) &&
                msg.Headers.Contains("X-eshop-whtoken") &&
                msg.Headers.GetValues("X-eshop-whtoken").First() == token1),
            ItExpr.IsAny<CancellationToken>());

        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg =>
                msg.RequestUri == new Uri(subscriber2.DestUrl) &&
                msg.Headers.Contains("X-eshop-whtoken") &&
                msg.Headers.GetValues("X-eshop-whtoken").First() == token2),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_WithDifferentWebhookTypes_SendsCorrectType()
    {
        // Arrange
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.CatalogItemPriceChange,
                Token = "token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.CatalogItemPriceChange, new { ItemId = 456 });

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_WithOrderShippedType_SendsCorrectly()
    {
        // Arrange
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderShipped,
                Token = "token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderShipped, new { TrackingNumber = "ABC123" });

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_CreatesHttpClientFromFactory()
    {
        // Arrange
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = "token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        _httpClientFactoryMock.Verify(f => f.CreateClient(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task SendAll_WithNullOrEmptyPayload_StillSends()
    {
        // Arrange
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = "token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg => msg.Content != null),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAll_RequestContentIsNotNull()
    {
        // Arrange
        var subscribers = new[]
        {
            new WebhookSubscription
            {
                Id = 1,
                DestUrl = "https://webhook.example.com/receiver",
                Type = WebhookType.OrderPaid,
                Token = "token",
                UserId = "user123"
            }
        };

        var webhookData = new WebhookData(WebhookType.OrderPaid, new { });

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        SetupHttpClientFactory(httpClient);

        // Act
        await _sender.SendAll(subscribers, webhookData);

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg =>
                msg.Content != null &&
                msg.Content.Headers.ContentType != null),
            ItExpr.IsAny<CancellationToken>());
    }
}
