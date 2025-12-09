namespace Webhooks.API.Tests;

using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;

public class GrantUrlTesterServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<ILogger<IGrantUrlTesterService>> _loggerMock;
    private readonly GrantUrlTesterService _service;

    public GrantUrlTesterServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _loggerMock = new Mock<ILogger<IGrantUrlTesterService>>();
        _service = new GrantUrlTesterService(_httpClientFactoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task TestGrantUrl_WithDifferentScheme_ReturnsFalse()
    {
        // Arrange
        var urlHook = "https://example.com/webhook";
        var url = "http://example.com/grant"; // Different scheme
        var token = "test-token";

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.False(result);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task TestGrantUrl_WithDifferentHost_ReturnsFalse()
    {
        // Arrange
        var urlHook = "https://example1.com/webhook";
        var url = "https://example2.com/grant"; // Different host
        var token = "test-token";

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.False(result);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task TestGrantUrl_WithDifferentPort_ReturnsFalse()
    {
        // Arrange
        var urlHook = "https://example.com:8080/webhook";
        var url = "https://example.com:9090/grant"; // Different port
        var token = "test-token";

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task TestGrantUrl_WithSameOrigin_SuccessResponseAndMatchingToken_ReturnsTrue()
    {
        // Arrange
        var urlHook = "https://example.com/webhook";
        var url = "https://example.com/grant";
        var token = "test-token";

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Headers.Add("X-eshop-whtoken", token);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.True(result);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Exactly(2)); // One for sending OPTIONS, one for response
    }

    [Fact]
    public async Task TestGrantUrl_WithSameOrigin_FailureStatusCode_ReturnsFalse()
    {
        // Arrange
        var urlHook = "https://example.com/webhook";
        var url = "https://example.com/grant";
        var token = "test-token";

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        response.Headers.Add("X-eshop-whtoken", token);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task TestGrantUrl_WithSameOrigin_MismatchedToken_ReturnsFalse()
    {
        // Arrange
        var urlHook = "https://example.com/webhook";
        var url = "https://example.com/grant";
        var token = "expected-token";

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Headers.Add("X-eshop-whtoken", "different-token");

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task TestGrantUrl_WithSameOrigin_SuccessButNoTokenInResponse_ReturnsFalse()
    {
        // Arrange
        var urlHook = "https://example.com/webhook";
        var url = "https://example.com/grant";
        var token = "test-token";

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        // No token header in response

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task TestGrantUrl_WithSameOrigin_NullToken_SuccessResponse_ReturnsTrue()
    {
        // Arrange
        var urlHook = "https://example.com/webhook";
        var url = "https://example.com/grant";
        string? token = null;

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        // No token header in response

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task TestGrantUrl_WithSameOrigin_EmptyToken_SuccessResponse_ReturnsTrue()
    {
        // Arrange
        var urlHook = "https://example.com/webhook";
        var url = "https://example.com/grant";
        var token = "   "; // Whitespace token

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        // No token header in response

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task TestGrantUrl_WithHttpException_ReturnsFalse()
    {
        // Arrange
        var urlHook = "https://example.com/webhook";
        var url = "https://example.com/grant";
        var token = "test-token";

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Connection failed"));

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.False(result);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task TestGrantUrl_WithGenericException_ReturnsFalse()
    {
        // Arrange
        var urlHook = "https://example.com/webhook";
        var url = "https://example.com/grant";
        var token = "test-token";

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("Invalid operation"));

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task TestGrantUrl_SendsOptionsRequestWithToken()
    {
        // Arrange
        var urlHook = "https://example.com/webhook";
        var url = "https://example.com/grant";
        var token = "test-token";

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Headers.Add("X-eshop-whtoken", token);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Options &&
                    msg.RequestUri == new Uri(url) &&
                    msg.Headers.Contains("X-eshop-whtoken")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.True(result);
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg =>
                msg.Method == HttpMethod.Options &&
                msg.RequestUri == new Uri(url)),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task TestGrantUrl_WithSameOriginDifferentPath_ReturnsTrue()
    {
        // Arrange
        var urlHook = "https://example.com/path1/webhook";
        var url = "https://example.com/path2/grant"; // Different path, same origin
        var token = "test-token";

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Headers.Add("X-eshop-whtoken", token);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.True(result); // Path difference should not matter
    }

    [Fact]
    public async Task TestGrantUrl_WithDefaultPort_SameAsExplicitPort_ReturnsTrue()
    {
        // Arrange
        var urlHook = "https://example.com/webhook"; // Default port 443
        var url = "https://example.com:443/grant"; // Explicit port 443
        var token = "test-token";

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Headers.Add("X-eshop-whtoken", token);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task TestGrantUrl_WithMultipleTokenHeaderValues_UsesFirstValue()
    {
        // Arrange
        var urlHook = "https://example.com/webhook";
        var url = "https://example.com/grant";
        var token = "first-token";

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Headers.Add("X-eshop-whtoken", new[] { "first-token", "second-token" });

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _service.TestGrantUrl(urlHook, url, token);

        // Assert
        Assert.True(result);
    }
}
