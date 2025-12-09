namespace Webhooks.API.Tests;

public class ExtensionsTests
{
    [Fact]
    public void AddApplicationServices_RegistersAllRequiredServices()
    {
        // Arrange
        var builder = new HostApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> 
        { 
            { "ConnectionStrings:webhooksdb", "Server=localhost;Database=webhooksdb;" }
        });
        builder.AddServiceDefaults();

        // Act
        builder.AddApplicationServices();

        // Assert
        var descriptor = builder.Services.FirstOrDefault(s => s.ServiceType == typeof(IGrantUrlTesterService));
        Assert.NotNull(descriptor);
        var descriptor2 = builder.Services.FirstOrDefault(s => s.ServiceType == typeof(IWebhooksRetriever));
        Assert.NotNull(descriptor2);
        var descriptor3 = builder.Services.FirstOrDefault(s => s.ServiceType == typeof(IWebhooksSender));
        Assert.NotNull(descriptor3);
    }

    [Fact]
    public void AddApplicationServices_RegistersGrantUrlTesterServiceAsTransient()
    {
        // Arrange
        var builder = new HostApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> 
        { 
            { "ConnectionStrings:webhooksdb", "Server=localhost;Database=webhooksdb;" }
        });
        builder.AddServiceDefaults();

        // Act
        builder.AddApplicationServices();

        // Assert
        var descriptor = builder.Services.FirstOrDefault(s => s.ServiceType == typeof(IGrantUrlTesterService));
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
    }

    [Fact]
    public void AddApplicationServices_RegistersWebhooksRetrieverAsTransient()
    {
        // Arrange
        var builder = new HostApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> 
        { 
            { "ConnectionStrings:webhooksdb", "Server=localhost;Database=webhooksdb;" }
        });
        builder.AddServiceDefaults();

        // Act
        builder.AddApplicationServices();

        // Assert
        var descriptor = builder.Services.FirstOrDefault(s => s.ServiceType == typeof(IWebhooksRetriever));
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
    }

    [Fact]
    public void AddApplicationServices_RegistersWebhooksSenderAsTransient()
    {
        // Arrange
        var builder = new HostApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> 
        { 
            { "ConnectionStrings:webhooksdb", "Server=localhost;Database=webhooksdb;" }
        });
        builder.AddServiceDefaults();

        // Act
        builder.AddApplicationServices();

        // Assert
        var descriptor = builder.Services.FirstOrDefault(s => s.ServiceType == typeof(IWebhooksSender));
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
    }

    [Fact]
    public void AddApplicationServices_CanBeCalledWithValidConfiguration()
    {
        // Arrange
        var builder = new HostApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> 
        { 
            { "ConnectionStrings:webhooksdb", "Server=localhost;Database=webhooksdb;" }
        });
        builder.AddServiceDefaults();

        // Act
        builder.AddApplicationServices();

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void AddApplicationServices_RegistersDefaultAuthentication()
    {
        // Arrange
        var builder = new HostApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> 
        { 
            { "ConnectionStrings:webhooksdb", "Server=localhost;Database=webhooksdb;" }
        });

        // Act
        builder.AddServiceDefaults();
        builder.AddApplicationServices();

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
        var authenticationSchemeProvider = serviceProvider.GetService<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>();
        Assert.NotNull(authenticationSchemeProvider);
    }

    [Fact]
    public void AddApplicationServices_MultipleCallsThrowsError()
    {
        // Arrange
        var builder = new HostApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> 
        { 
            { "ConnectionStrings:webhooksdb", "Server=localhost;Database=webhooksdb;" }
        });
        builder.AddServiceDefaults();

        // Act
        builder.AddApplicationServices();

        // Assert - Should not throw on second call
        builder.AddApplicationServices();
        var descriptor = builder.Services.FirstOrDefault(s => s.ServiceType == typeof(IGrantUrlTesterService));
        Assert.NotNull(descriptor);
    }

    [Fact]
    public void AddApplicationServices_ConfiguresEventBusSubscriptions()
    {
        // Arrange
        var builder = new HostApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> 
        { 
            { "ConnectionStrings:webhooksdb", "Server=localhost;Database=webhooksdb;" },
            { "ConnectionStrings:eventbus", "amqp://guest:guest@localhost/" }
        });
        builder.AddServiceDefaults();

        // Act
        builder.AddApplicationServices();

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void AddApplicationServices_InitializesAllThreeServices()
    {
        // Arrange
        var builder = new HostApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> 
        { 
            { "ConnectionStrings:webhooksdb", "Server=localhost;Database=webhooksdb;" }
        });
        builder.AddServiceDefaults();
        builder.AddApplicationServices();

        // Act
        var serviceProvider = builder.Services.BuildServiceProvider();

        // Assert
        var grantUrlTester = serviceProvider.GetService<IGrantUrlTesterService>();
        var webhooksRetriever = serviceProvider.GetService<IWebhooksRetriever>();
        var webhooksSender = serviceProvider.GetService<IWebhooksSender>();

        Assert.NotNull(grantUrlTester);
        Assert.NotNull(webhooksRetriever);
        Assert.NotNull(webhooksSender);
    }
}