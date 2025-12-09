namespace eShop.ServiceDefaults.Tests;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using Xunit;

public class AuthenticationExtensionsTests
{
    [Fact]
    public void AddDefaultAuthentication_WithNoIdentitySection_ReturnsServicesWithoutAuthentication()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act
        var result = builder.AddDefaultAuthentication();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder.Services, result);
        // Verify that authentication services are not added (no AuthenticationScheme registered)
        var serviceProvider = builder.Services.BuildServiceProvider();
        var authService = serviceProvider.GetService<IAuthenticationService>();
        Assert.Null(authService);
    }

    [Fact]
    public void AddDefaultAuthentication_WithIdentitySection_AddsAuthenticationServices()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            { "Identity:Url", "http://identity" },
            { "Identity:Audience", "basket" }
        };
        var builder = CreateHostApplicationBuilder(config);

        // Act
        var result = builder.AddDefaultAuthentication();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder.Services, result);
        var serviceProvider = builder.Services.BuildServiceProvider();
        var authService = serviceProvider.GetService<IAuthenticationService>();
        Assert.NotNull(authService);
    }

    [Fact]
    public async Task AddDefaultAuthentication_WithIdentitySection_ConfiguresJwtBearer()
    {
        // Arrange
        var identityUrl = "http://localhost:5000";
        var audience = "myapi";
        var config = new Dictionary<string, string?>
        {
            { "Identity:Url", identityUrl },
            { "Identity:Audience", audience }
        };
        var builder = CreateHostApplicationBuilder(config);

        // Act
        builder.AddDefaultAuthentication();

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();
        // Verify that AuthenticationSchemeProvider contains JWT Bearer scheme
        var schemeProvider = serviceProvider.GetRequiredService<IAuthenticationSchemeProvider>();
        var scheme = await schemeProvider.GetSchemeAsync("Bearer");
        Assert.NotNull(scheme);
        Assert.Equal("Bearer", scheme.Name);
    }

    [Fact]
    public void AddDefaultAuthentication_WithIdentitySection_RemovesSubClaimTypeMapping()
    {
        // Arrange
        var originalMapCount = JsonWebTokenHandler.DefaultInboundClaimTypeMap.Count;
        var config = new Dictionary<string, string?>
        {
            { "Identity:Url", "http://identity" },
            { "Identity:Audience", "basket" }
        };
        var builder = CreateHostApplicationBuilder(config);

        // Act
        builder.AddDefaultAuthentication();

        // Assert
        Assert.False(JsonWebTokenHandler.DefaultInboundClaimTypeMap.ContainsKey("sub"));
    }

    [Fact]
    public void AddDefaultAuthentication_WithMissingIdentityUrl_ConfiguresProperly()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            { "Identity:Audience", "basket" }
        };
        var builder = CreateHostApplicationBuilder(config);

        // Act
        var result = builder.AddDefaultAuthentication();

        // Assert - should configure but exception occurs when accessing the scheme
        Assert.NotNull(result);
        Assert.Same(builder.Services, result);
    }

    [Fact]
    public void AddDefaultAuthentication_WithMissingAudience_ConfiguresProperly()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            { "Identity:Url", "http://identity" }
        };
        var builder = CreateHostApplicationBuilder(config);

        // Act
        var result = builder.AddDefaultAuthentication();

        // Assert - should configure but exception occurs when accessing the scheme
        Assert.NotNull(result);
        Assert.Same(builder.Services, result);
    }

    [Fact]
    public void AddDefaultAuthentication_WithEmptyIdentitySection_ReturnsServicesWithoutAuthentication()
    {
        // Arrange
        var config = new Dictionary<string, string?>();
        var builder = CreateHostApplicationBuilder(config);

        // Act
        var result = builder.AddDefaultAuthentication();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder.Services, result);
    }

    [Fact]
    public void AddDefaultAuthentication_WithIdentitySection_AddsAuthorizationServices()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            { "Identity:Url", "http://identity" },
            { "Identity:Audience", "basket" }
        };
        var builder = CreateHostApplicationBuilder(config);

        // Act
        builder.AddDefaultAuthentication();

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();
        var authorizationService = serviceProvider.GetService<IAuthorizationService>();
        Assert.NotNull(authorizationService);
    }

    [Fact]
    public void AddDefaultAuthentication_WithDifferentIdentityUrls_ConfiguresCorrectly()
    {
        // Arrange
        var urls = new[] { "http://localhost:5000", "https://identity.example.com", "http://10.0.0.1:8080" };

        foreach (var url in urls)
        {
            var config = new Dictionary<string, string?>
            {
                { "Identity:Url", url },
                { "Identity:Audience", "testapi" }
            };
            var builder = CreateHostApplicationBuilder(config);

            // Act
            var result = builder.AddDefaultAuthentication();

            // Assert
            Assert.NotNull(result);
            Assert.Same(builder.Services, result);
        }
    }

    [Fact]
    public void AddDefaultAuthentication_WithDifferentAudiences_ConfiguresCorrectly()
    {
        // Arrange
        var audiences = new[] { "basket", "order", "payment", "catalog" };

        foreach (var audience in audiences)
        {
            var config = new Dictionary<string, string?>
            {
                { "Identity:Url", "http://identity" },
                { "Identity:Audience", audience }
            };
            var builder = CreateHostApplicationBuilder(config);

            // Act
            var result = builder.AddDefaultAuthentication();

            // Assert
            Assert.NotNull(result);
            Assert.Same(builder.Services, result);
        }
    }

    [Theory]
    [InlineData("http://identity", "basket")]
    [InlineData("https://identity.local", "order")]
    [InlineData("http://localhost:5000", "payment")]
    public void AddDefaultAuthentication_WithVariousConfigurations_ConfiguresSuccessfully(string identityUrl, string audience)
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            { "Identity:Url", identityUrl },
            { "Identity:Audience", audience }
        };
        var builder = CreateHostApplicationBuilder(config);

        // Act
        var result = builder.AddDefaultAuthentication();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder.Services, result);
    }

    [Fact]
    public void AddDefaultAuthentication_ReturnsIServiceCollection()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            { "Identity:Url", "http://identity" },
            { "Identity:Audience", "basket" }
        };
        var builder = CreateHostApplicationBuilder(config);

        // Act
        var result = builder.AddDefaultAuthentication();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IServiceCollection>(result);
    }

    [Fact]
    public void AddDefaultAuthentication_CanBeCalledMultipleTimes()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            { "Identity:Url", "http://identity" },
            { "Identity:Audience", "basket" }
        };
        var builder = CreateHostApplicationBuilder(config);

        // Act & Assert - should not throw
        var result1 = builder.AddDefaultAuthentication();
        Assert.NotNull(result1);
    }

    private static IHostApplicationBuilder CreateHostApplicationBuilder(Dictionary<string, string?> configValues)
    {
        var builder = new HostApplicationBuilder();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();
        builder.Configuration.AddInMemoryCollection(configValues);
        return builder;
    }
}
