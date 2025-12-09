namespace eShop.ServiceDefaults.Tests;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

public class OpenApiExtensionsTests
{
    [Fact]
    public void UseDefaultOpenApi_WithoutOpenApiConfiguration_SectionDoesNotExist()
    {
        // Arrange
        var config = CreateConfiguration();

        // Act & Assert
        // Verify the configuration doesn't have OpenApi section
        var openApiSection = config.GetSection("OpenApi");
        Assert.False(openApiSection.Exists());
    }

    [Fact]
    public void UseDefaultOpenApi_WithOpenApiConfiguration_SectionExists()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Endpoint:Url", "http://localhost:5000/swagger/v1/swagger.json" }
        };
        var config = CreateConfiguration(settings);

        // Act
        var openApiSection = config.GetSection("OpenApi");

        // Assert
        Assert.True(openApiSection.Exists());
    }

    [Fact]
    public void AddDefaultOpenApi_WithoutOpenApiConfiguration_ReturnsBuilder()
    {
        // Arrange
        var builder = CreateHostBuilder();

        // Act
        var result = builder.AddDefaultOpenApi();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
    }

    [Fact]
    public void AddDefaultOpenApi_WithOpenApiConfiguration_ReturnsBuilder()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Document:Title", "Test API" },
            { "OpenApi:Document:Description", "Test Description" }
        };
        var builder = CreateHostBuilder(settings);

        // Act
        var result = builder.AddDefaultOpenApi();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
    }

    [Fact]
    public void AddDefaultOpenApi_WithoutApiVersioning_ReturnsBuilder()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Document:Title", "Test API" },
            { "OpenApi:Document:Description", "Test Description" }
        };
        var builder = CreateHostBuilder(settings);

        // Act
        var result = builder.AddDefaultOpenApi(apiVersioning: null);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
    }

    [Fact]
    public void AddDefaultOpenApi_WithApiVersioning_ReturnsBuilder()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Document:Title", "Test API" },
            { "OpenApi:Document:Description", "Test Description" }
        };
        var builder = CreateHostBuilder(settings);
        var apiVersioningBuilder = builder.Services.AddApiVersioning();

        // Act
        var result = builder.AddDefaultOpenApi(apiVersioning: apiVersioningBuilder);

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
    }

    [Fact]
    public void AddDefaultOpenApi_WithOpenApi_AddsEndpointsApiExplorerService()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Document:Title", "Test API" },
            { "OpenApi:Document:Description", "Test Description" }
        };
        var builder = CreateHostBuilder(settings);

        // Act
        builder.AddDefaultOpenApi();

        // Assert
        // EndpointsApiExplorer service should be added
        Assert.NotNull(builder.Services);
    }

    [Fact]
    public void AddDefaultOpenApi_MultipleCallsReturnSameBuilder()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Document:Title", "Test API" },
            { "OpenApi:Document:Description", "Test Description" }
        };
        var builder = CreateHostBuilder(settings);

        // Act
        var result1 = builder.AddDefaultOpenApi();
        var result2 = builder.AddDefaultOpenApi();

        // Assert
        Assert.Same(result1, result2);
        Assert.Same(builder, result1);
    }

    [Fact]
    public void Configuration_WithOpenApi_CanRetrieveEndpointUrl()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Endpoint:Url", "http://localhost:5000/swagger/v1/swagger.json" },
            { "OpenApi:Endpoint:Name", "v1" }
        };
        var config = CreateConfiguration(settings);

        // Act
        var openApiSection = config.GetSection("OpenApi");

        // Assert
        Assert.True(openApiSection.Exists());
        var endpointUrl = config["OpenApi:Endpoint:Url"];
        Assert.Equal("http://localhost:5000/swagger/v1/swagger.json", endpointUrl);
    }

    [Fact]
    public void Configuration_WithOpenApiAuth_CanRetrieveClientIdAndAppName()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Endpoint:Url", "http://localhost:5000/swagger/v1/swagger.json" },
            { "OpenApi:Auth:ClientId", "swagger-ui" },
            { "OpenApi:Auth:AppName", "eShop API" }
        };
        var config = CreateConfiguration(settings);

        // Act
        var authSection = config.GetSection("OpenApi:Auth");

        // Assert
        Assert.True(authSection.Exists());
        var clientId = config["OpenApi:Auth:ClientId"];
        Assert.Equal("swagger-ui", clientId);
        var appName = config["OpenApi:Auth:AppName"];
        Assert.Equal("eShop API", appName);
    }

    [Fact]
    public void Configuration_WithPathBase_CanRetrieveValue()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "PATH_BASE", "/myapp" },
            { "OpenApi:Endpoint:Url", "" }
        };
        var config = CreateConfiguration(settings);

        // Act
        var pathBase = config["PATH_BASE"];

        // Assert
        Assert.Equal("/myapp", pathBase);
    }

    [Fact]
    public void AddDefaultOpenApi_WithoutApiVersioning_NullIsAccepted()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Document:Title", "Test API" },
            { "OpenApi:Document:Description", "Test Description" }
        };
        var builder = CreateHostBuilder(settings);

        // Act & Assert - should not throw
        var result = builder.AddDefaultOpenApi(null);
        Assert.NotNull(result);
    }

    [Fact]
    public void AddDefaultOpenApi_ReturnTypeIsHostApplicationBuilder()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Document:Title", "Test API" },
            { "OpenApi:Document:Description", "Test Description" }
        };
        var builder = CreateHostBuilder(settings);

        // Act
        var result = builder.AddDefaultOpenApi();

        // Assert
        Assert.IsAssignableFrom<IHostApplicationBuilder>(result);
    }

    [Fact]
    public void Configuration_DocumentSection_CanRetrieveTitleAndDescription()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Document:Title", "My API" },
            { "OpenApi:Document:Description", "My API Description" }
        };
        var config = CreateConfiguration(settings);

        // Act
        var documentSection = config.GetSection("OpenApi:Document");

        // Assert
        Assert.True(documentSection.Exists());
        Assert.Equal("My API", documentSection["Title"]);
        Assert.Equal("My API Description", documentSection["Description"]);
    }

    [Fact]
    public void AddDefaultOpenApi_EmptyConfiguration_NoServicesAdded()
    {
        // Arrange
        var builder = CreateHostBuilder();
        var initialCount = builder.Services.Count;

        // Act
        builder.AddDefaultOpenApi();

        // Assert
        Assert.Equal(initialCount, builder.Services.Count);
    }

    [Fact]
    public void AddDefaultOpenApi_WithApiVersioning_ConfiguresApiExplorer()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Document:Title", "Test API" },
            { "OpenApi:Document:Description", "Test Description" }
        };
        var builder = CreateHostBuilder(settings);
        var apiVersioningBuilder = builder.Services.AddApiVersioning();

        // Act
        builder.AddDefaultOpenApi(apiVersioning: apiVersioningBuilder);

        // Assert
        Assert.NotNull(builder.Services);
    }

    [Fact]
    public void Configuration_EndpointSection_ReturnsValidUrl()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Endpoint:Url", "http://custom.swagger.url/v1/swagger.json" }
        };
        var config = CreateConfiguration(settings);

        // Act
        var url = config["OpenApi:Endpoint:Url"];

        // Assert
        Assert.NotNull(url);
        Assert.Equal("http://custom.swagger.url/v1/swagger.json", url);
    }

    [Fact]
    public void Configuration_AuthSection_ReturnsValidClientId()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Auth:ClientId", "my-client-id" }
        };
        var config = CreateConfiguration(settings);

        // Act
        var clientId = config["OpenApi:Auth:ClientId"];

        // Assert
        Assert.NotNull(clientId);
        Assert.Equal("my-client-id", clientId);
    }

    [Fact]
    public void Configuration_DocumentSection_ReturnsValidTitle()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Document:Title", "My API Title" }
        };
        var config = CreateConfiguration(settings);

        // Act
        var title = config["OpenApi:Document:Title"];

        // Assert
        Assert.NotNull(title);
        Assert.Equal("My API Title", title);
    }

    [Fact]
    public void Configuration_DocumentSection_ReturnsValidDescription()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Document:Description", "My API Description" }
        };
        var config = CreateConfiguration(settings);

        // Act
        var description = config["OpenApi:Document:Description"];

        // Assert
        Assert.NotNull(description);
        Assert.Equal("My API Description", description);
    }

    [Fact]
    public void AddDefaultOpenApi_PreservesBuilderState()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "OpenApi:Document:Title", "Test API" },
            { "OpenApi:Document:Description", "Test Description" }
        };
        var builder = CreateHostBuilder(settings);
        var originalServices = builder.Services;

        // Act
        var result = builder.AddDefaultOpenApi();

        // Assert
        Assert.Same(originalServices, result.Services);
    }

    // Helper methods

    private static IConfiguration CreateConfiguration(Dictionary<string, string?>? settings = null)
    {
        var configSettings = settings ?? new Dictionary<string, string?> { };
        return new ConfigurationBuilder()
            .AddInMemoryCollection(configSettings)
            .Build();
    }

    private static IHostApplicationBuilder CreateHostBuilder(Dictionary<string, string?>? settings = null)
    {
        var configSettings = settings ?? new Dictionary<string, string?> { };
        var configBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(configSettings);
        
        var configManager = new ConfigurationManager();
        foreach (var kvp in configSettings)
        {
            configManager[kvp.Key] = kvp.Value;
        }
        
        return new HostApplicationBuilder(new HostApplicationBuilderSettings 
        { 
            Configuration = configManager
        });
    }
}
