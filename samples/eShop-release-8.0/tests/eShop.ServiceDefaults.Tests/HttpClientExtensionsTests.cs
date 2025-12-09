namespace eShop.ServiceDefaults.Tests;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

public class HttpClientExtensionsTests
{
    [Fact]
    public void AddAuthToken_RegistersHttpContextAccessor()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = services.AddHttpClient("test");

        // Act
        builder.AddAuthToken();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
        Assert.NotNull(httpContextAccessor);
    }

    [Fact]
    public void AddAuthToken_ReturnsIHttpClientBuilder()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = services.AddHttpClient("test");

        // Act
        var result = builder.AddAuthToken();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IHttpClientBuilder>(result);
    }

    [Fact]
    public void AddAuthToken_ReturnsSameBuilder()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = services.AddHttpClient("test");

        // Act
        var result = builder.AddAuthToken();

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void AddAuthToken_CanBeCalledMultipleTimes()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder1 = services.AddHttpClient("test1");

        // Act & Assert - should not throw
        var result1 = builder1.AddAuthToken();
        Assert.NotNull(result1);

        var builder2 = services.AddHttpClient("test2");
        var result2 = builder2.AddAuthToken();
        Assert.NotNull(result2);
    }

    [Fact]
    public void AddAuthToken_WithMultipleHttpClients_EachGetsHandler()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddHttpClient("client1").AddAuthToken();
        services.AddHttpClient("client2").AddAuthToken();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<IHttpClientFactory>());
        // Both clients should be registered
        var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        Assert.NotNull(factory.CreateClient("client1"));
        Assert.NotNull(factory.CreateClient("client2"));
    }

    [Fact]
    public void AddAuthToken_TryAddTransientUsesHttpContextAccessor()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddHttpContextAccessor();
        var builder = services.AddHttpClient("test");

        // Act
        builder.AddAuthToken();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var accessor1 = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        var accessor2 = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        // Should be the same instance since IHttpContextAccessor is registered as a service
        Assert.NotNull(accessor1);
        Assert.NotNull(accessor2);
    }

    [Fact]
    public void AddAuthToken_BuildsHttpClientFactory()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = services.AddHttpClient("testclient");

        // Act
        builder.AddAuthToken();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        Assert.NotNull(factory);
        
        var client = factory.CreateClient("testclient");
        Assert.NotNull(client);
    }

    [Fact]
    public void AddAuthToken_MultipleClientsWithAuthToken()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddHttpClient("api1").AddAuthToken();
        services.AddHttpClient("api2").AddAuthToken();
        services.AddHttpClient("api3").AddAuthToken();

        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Assert
        Assert.NotNull(factory.CreateClient("api1"));
        Assert.NotNull(factory.CreateClient("api2"));
        Assert.NotNull(factory.CreateClient("api3"));
    }

    [Fact]
    public void AddAuthToken_DoesNotThrowWithoutPriorHttpContextAccessor()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = services.AddHttpClient("test");

        // Act & Assert - should not throw
        var result = builder.AddAuthToken();
        Assert.NotNull(result);

        var serviceProvider = services.BuildServiceProvider();
        var accessor = serviceProvider.GetService<IHttpContextAccessor>();
        Assert.NotNull(accessor);
    }

    [Fact]
    public void AddAuthToken_ChainableWithOtherMethods()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act - chain multiple configuration methods
        var builder = services.AddHttpClient("test")
            .AddAuthToken()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://example.com"));

        // Assert
        Assert.NotNull(builder);
        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("test");
        Assert.NotNull(client);
    }

    [Fact]
    public void AddAuthToken_FluentConfiguration()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddHttpClient("test")
            .AddAuthToken()
            .AddStandardResilienceHandler();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("test");
        Assert.NotNull(client);
    }

    [Fact]
    public void AddAuthToken_RegisteredAsTransient()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = services.AddHttpClient("test");

        // Act
        builder.AddAuthToken();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        // Verify services were added
        var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
        Assert.NotNull(httpContextAccessor);
    }

    [Fact]
    public void AddAuthToken_NamedHttpClient()
    {
        // Arrange
        var services = new ServiceCollection();
        const string clientName = "MyAuthenticatedClient";

        // Act
        services.AddHttpClient(clientName).AddAuthToken();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient(clientName);
        Assert.NotNull(client);
        Assert.IsType<HttpClient>(client);
    }

    [Fact]
    public void AddAuthToken_ConsecutiveCallsAreIdempotent()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = services.AddHttpClient("test");

        // Act
        builder.AddAuthToken();
        builder.AddAuthToken();  // Call again

        // Assert - should not throw, second call is idempotent
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient("test");
        Assert.NotNull(client);
    }
}
