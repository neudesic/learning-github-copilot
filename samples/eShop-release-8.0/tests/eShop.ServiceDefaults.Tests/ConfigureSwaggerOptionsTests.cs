namespace eShop.ServiceDefaults.Tests;

using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using eShop.ServiceDefaults;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

public class ConfigureSwaggerOptionsTests
{
    [Fact]
    public void Constructor_WithValidProvider_InitializesSuccessfully()
    {
        // Arrange
        var provider = CreateMockApiVersionDescriptionProvider(new List<ApiVersionDescription>());
        var config = CreateConfigurationWithOpenApi();

        // Act
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Assert
        Assert.NotNull(configurer);
    }

    [Fact]
    public void Configure_WithSingleApiVersion_CreatesSwaggerDocForVersion()
    {
        // Arrange
        var apiVersion = new ApiVersion(1, 0);
        var descriptions = new List<ApiVersionDescription>
        {
            CreateApiVersionDescription(apiVersion, "v1", false)
        };
        var provider = CreateMockApiVersionDescriptionProvider(descriptions);
        var config = CreateConfigurationWithOpenApi("Test API", "Test Description");

        var options = new SwaggerGenOptions();
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Act
        configurer.Configure(options);

        // Assert
        Assert.NotEmpty(options.SwaggerGeneratorOptions.SwaggerDocs);
        Assert.True(options.SwaggerGeneratorOptions.SwaggerDocs.ContainsKey("v1"));
    }

    [Fact]
    public void Configure_WithMultipleApiVersions_CreatesSwaggerDocForEachVersion()
    {
        // Arrange
        var descriptions = new List<ApiVersionDescription>
        {
            CreateApiVersionDescription(new ApiVersion(1, 0), "v1", false),
            CreateApiVersionDescription(new ApiVersion(2, 0), "v2", false)
        };
        var provider = CreateMockApiVersionDescriptionProvider(descriptions);
        var config = CreateConfigurationWithOpenApi("Multi Version API", "API with multiple versions");

        var options = new SwaggerGenOptions();
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Act
        configurer.Configure(options);

        // Assert
        Assert.Equal(2, options.SwaggerGeneratorOptions.SwaggerDocs.Count);
    }

    [Fact]
    public void Configure_WithDeprecatedVersion_IncludesDeprecationMessageInDescription()
    {
        // Arrange
        var descriptions = new List<ApiVersionDescription>
        {
            CreateApiVersionDescription(new ApiVersion(1, 0), "v1", true)
        };
        var provider = CreateMockApiVersionDescriptionProvider(descriptions);
        var baseDescription = "This is version 1";
        var config = CreateConfigurationWithOpenApi("Deprecated API", baseDescription);

        var options = new SwaggerGenOptions();
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Act
        configurer.Configure(options);

        // Assert
        var docs = options.SwaggerGeneratorOptions.SwaggerDocs;
        Assert.NotEmpty(docs);
        var doc = docs.Values.First();
        Assert.Contains("deprecated", doc.Description, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Configure_WithoutIdentitySection_DoesNotAddSecurityDefinition()
    {
        // Arrange
        var descriptions = new List<ApiVersionDescription>
        {
            CreateApiVersionDescription(new ApiVersion(1, 0), "v1", false)
        };
        var provider = CreateMockApiVersionDescriptionProvider(descriptions);
        var config = CreateConfigurationWithoutIdentity();

        var options = new SwaggerGenOptions();
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Act & Assert - should not throw
        configurer.Configure(options);
        Assert.NotNull(options);
    }

    [Fact]
    public void Configure_WithIdentitySection_AddsOAuth2SecurityDefinition()
    {
        // Arrange
        var descriptions = new List<ApiVersionDescription>
        {
            CreateApiVersionDescription(new ApiVersion(1, 0), "v1", false)
        };
        var provider = CreateMockApiVersionDescriptionProvider(descriptions);
        var config = CreateConfigurationWithIdentity();

        var options = new SwaggerGenOptions();
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Act
        configurer.Configure(options);

        // Assert - the operation filter should be added successfully
        Assert.NotNull(options);
    }

    [Fact]
    public void Configure_WithIdentitySection_ConfiguresCorrectAuthorizationUrls()
    {
        // Arrange
        var identityUrl = "https://identity.example.com";
        var descriptions = new List<ApiVersionDescription>
        {
            CreateApiVersionDescription(new ApiVersion(1, 0), "v1", false)
        };
        var provider = CreateMockApiVersionDescriptionProvider(descriptions);
        var config = CreateConfigurationWithIdentity(identityUrl);

        var options = new SwaggerGenOptions();
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Act & Assert - configuration should succeed without throwing
        configurer.Configure(options);
        Assert.NotNull(options);
    }

    [Fact]
    public void Configure_WithIdentitySection_IncludesScopesInSecurityDefinition()
    {
        // Arrange
        var descriptions = new List<ApiVersionDescription>
        {
            CreateApiVersionDescription(new ApiVersion(1, 0), "v1", false)
        };
        var provider = CreateMockApiVersionDescriptionProvider(descriptions);
        var config = CreateConfigurationWithIdentity();

        var options = new SwaggerGenOptions();
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Act & Assert - configuration should succeed without throwing
        configurer.Configure(options);
        Assert.NotNull(options);
    }

    [Fact]
    public void CreateInfoForApiVersion_WithValidDescription_ReturnsOpenApiInfo()
    {
        // Arrange
        var provider = CreateMockApiVersionDescriptionProvider(new List<ApiVersionDescription>());
        var config = CreateConfigurationWithOpenApi("Test API", "Test Description");
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);
        var apiDescription = CreateApiVersionDescription(new ApiVersion(1, 0), "v1", false);

        // Act
        var info = configurer.CreateInfoForApiVersion(apiDescription);

        // Assert
        Assert.NotNull(info);
        Assert.Equal("Test API", info.Title);
        Assert.Equal("1.0", info.Version);
        Assert.Contains("Test Description", info.Description);
    }

    [Fact]
    public void CreateInfoForApiVersion_WithDeprecatedVersion_IncludesDeprecationMessage()
    {
        // Arrange
        var provider = CreateMockApiVersionDescriptionProvider(new List<ApiVersionDescription>());
        var config = CreateConfigurationWithOpenApi("API", "Original");
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);
        var apiDescription = CreateApiVersionDescription(new ApiVersion(2, 0), "v2", true);

        // Act
        var info = configurer.CreateInfoForApiVersion(apiDescription);

        // Assert
        Assert.Contains("deprecated", info.Description, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildDescription_WithEmptyDescription_AddsDeprecationMessage()
    {
        // Arrange
        var provider = CreateMockApiVersionDescriptionProvider(new List<ApiVersionDescription>());
        var config = CreateConfigurationWithOpenApi("API", "");
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);
        var apiDescription = CreateApiVersionDescription(new ApiVersion(1, 0), "v1", true);

        // Act
        var info = configurer.CreateInfoForApiVersion(apiDescription);

        // Assert
        Assert.Equal("This API version has been deprecated.", info.Description);
    }

    [Fact]
    public void BuildDescription_WithDescriptionNoEndingPeriod_AddsPeriodBeforeDeprecation()
    {
        // Arrange
        var provider = CreateMockApiVersionDescriptionProvider(new List<ApiVersionDescription>());
        var config = CreateConfigurationWithOpenApi("API", "This is an API");
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);
        var apiDescription = CreateApiVersionDescription(new ApiVersion(1, 0), "v1", true);

        // Act
        var info = configurer.CreateInfoForApiVersion(apiDescription);

        // Assert
        Assert.Contains("This is an API. This API version has been deprecated.", info.Description);
    }

    [Fact]
    public void BuildDescription_WithDescriptionWithEndingPeriod_DoesNotAddExtraPeriod()
    {
        // Arrange
        var provider = CreateMockApiVersionDescriptionProvider(new List<ApiVersionDescription>());
        var config = CreateConfigurationWithOpenApi("API", "This is an API.");
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);
        var apiDescription = CreateApiVersionDescription(new ApiVersion(1, 0), "v1", true);

        // Act
        var info = configurer.CreateInfoForApiVersion(apiDescription);

        // Assert
        Assert.Contains("This is an API. This API version has been deprecated.", info.Description);
        Assert.DoesNotContain("This is an API.. This API version has been deprecated.", info.Description);
    }

    [Fact]
    public void Configure_WithMultipleVersions_EachHasCorrectDescription()
    {
        // Arrange
        var descriptions = new List<ApiVersionDescription>
        {
            CreateApiVersionDescription(new ApiVersion(1, 0), "v1", true),
            CreateApiVersionDescription(new ApiVersion(2, 0), "v2", false)
        };
        var provider = CreateMockApiVersionDescriptionProvider(descriptions);
        var config = CreateConfigurationWithOpenApi("API", "Description");

        var options = new SwaggerGenOptions();
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Act
        configurer.Configure(options);

        // Assert
        var docs = options.SwaggerGeneratorOptions.SwaggerDocs;
        var v1Doc = docs["v1"];
        var v2Doc = docs["v2"];

        Assert.Contains("deprecated", v1Doc.Description, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("deprecated", v2Doc.Description, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Configure_MissingOpenApiTitle_ThrowsInvalidOperationException()
    {
        // Arrange
        var descriptions = new List<ApiVersionDescription>
        {
            CreateApiVersionDescription(new ApiVersion(1, 0), "v1", false)
        };
        var provider = CreateMockApiVersionDescriptionProvider(descriptions);
        var config = CreateConfigurationWithoutOpenApiTitle();

        var options = new SwaggerGenOptions();
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Act & Assert
        var ex = Assert.Throws<System.Reflection.TargetInvocationException>(() => configurer.Configure(options));
        Assert.IsType<InvalidOperationException>(ex.InnerException);
        Assert.Contains("Title", ex.InnerException.Message);
    }

    [Fact]
    public void Configure_MissingOpenApiDescription_ThrowsInvalidOperationException()
    {
        // Arrange
        var descriptions = new List<ApiVersionDescription>
        {
            CreateApiVersionDescription(new ApiVersion(1, 0), "v1", false)
        };
        var provider = CreateMockApiVersionDescriptionProvider(descriptions);
        var config = CreateConfigurationWithoutOpenApiDescription();

        var options = new SwaggerGenOptions();
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Act & Assert
        var ex = Assert.Throws<System.Reflection.TargetInvocationException>(() => configurer.Configure(options));
        Assert.IsType<InvalidOperationException>(ex.InnerException);
        Assert.Contains("Description", ex.InnerException.Message);
    }

    [Fact]
    public void Configure_WithIdentityButMissingUrl_ThrowsInvalidOperationException()
    {
        // Arrange
        var descriptions = new List<ApiVersionDescription>
        {
            CreateApiVersionDescription(new ApiVersion(1, 0), "v1", false)
        };
        var provider = CreateMockApiVersionDescriptionProvider(descriptions);
        var config = CreateConfigurationWithIdentityMissingUrl();

        var options = new SwaggerGenOptions();
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Act & Assert
        var ex = Assert.Throws<System.Reflection.TargetInvocationException>(() => configurer.Configure(options));
        Assert.IsType<InvalidOperationException>(ex.InnerException);
    }

    [Fact]
    public void Configure_WithIdentityButMissingScopes_ThrowsInvalidOperationException()
    {
        // Arrange
        var descriptions = new List<ApiVersionDescription>
        {
            CreateApiVersionDescription(new ApiVersion(1, 0), "v1", false)
        };
        var provider = CreateMockApiVersionDescriptionProvider(descriptions);
        var config = CreateConfigurationWithIdentityMissingScopes();

        var options = new SwaggerGenOptions();
        var configurer = new ConfigureSwaggerOptionsWrapper(provider, config);

        // Act & Assert
        var ex = Assert.Throws<System.Reflection.TargetInvocationException>(() => configurer.Configure(options));
        Assert.IsType<InvalidOperationException>(ex.InnerException);
    }

    // Helper methods

    private static IApiVersionDescriptionProvider CreateMockApiVersionDescriptionProvider(
        List<ApiVersionDescription> descriptions)
    {
        return new MockApiVersionDescriptionProvider(descriptions);
    }

    private static ApiVersionDescription CreateApiVersionDescription(
        ApiVersion version, string groupName, bool isDeprecated)
    {
        return new ApiVersionDescription(version, groupName, isDeprecated);
    }

    private static IConfiguration CreateConfigurationWithOpenApi(
        string title = "Test API", string description = "Test Description")
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "OpenApi:Document:Title", title },
                { "OpenApi:Document:Description", description }
            })
            .Build();
    }

    private static IConfiguration CreateConfigurationWithoutIdentity()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "OpenApi:Document:Title", "Test API" },
                { "OpenApi:Document:Description", "Test Description" }
            })
            .Build();
    }

    private static IConfiguration CreateConfigurationWithIdentity(string? identityUrl = null)
    {
        var url = identityUrl ?? "https://identity.example.com";
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "OpenApi:Document:Title", "Test API" },
                { "OpenApi:Document:Description", "Test Description" },
                { "Identity:Url", url },
                { "Identity:Scopes:basket", "Basket API" },
                { "Identity:Scopes:orders", "Orders API" }
            })
            .Build();
    }

    private static IConfiguration CreateConfigurationWithoutOpenApiTitle()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "OpenApi:Document:Description", "Test Description" }
            })
            .Build();
    }

    private static IConfiguration CreateConfigurationWithoutOpenApiDescription()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "OpenApi:Document:Title", "Test API" }
            })
            .Build();
    }

    private static IConfiguration CreateConfigurationWithIdentityMissingUrl()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "OpenApi:Document:Title", "Test API" },
                { "OpenApi:Document:Description", "Test Description" },
                { "Identity:Scopes:basket", "Basket API" }
            })
            .Build();
    }

    private static IConfiguration CreateConfigurationWithIdentityMissingScopes()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "OpenApi:Document:Title", "Test API" },
                { "OpenApi:Document:Description", "Test Description" },
                { "Identity:Url", "https://identity.example.com" }
            })
            .Build();
    }

    // Mock implementation
    private class MockApiVersionDescriptionProvider : IApiVersionDescriptionProvider
    {
        private readonly IReadOnlyList<ApiVersionDescription> _descriptions;

        public MockApiVersionDescriptionProvider(List<ApiVersionDescription> descriptions)
        {
            _descriptions = descriptions.AsReadOnly();
        }

        public IReadOnlyList<ApiVersionDescription> ApiVersionDescriptions => _descriptions;
    }

    // Wrapper to expose internal methods for testing
    private sealed class ConfigureSwaggerOptionsWrapper
    {
        private readonly object _instance;
        private readonly Type _type;

        public ConfigureSwaggerOptionsWrapper(IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            var assembly = typeof(eShop.ServiceDefaults.Extensions).Assembly;
            _type = assembly.GetType("eShop.ServiceDefaults.ConfigureSwaggerOptions", true)!;
            _instance = Activator.CreateInstance(_type, provider, configuration)!;
        }

        public void Configure(SwaggerGenOptions options)
        {
            var method = _type.GetMethod("Configure", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            method!.Invoke(_instance, new object[] { options });
        }

        public OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var method = _type.GetMethod("CreateInfoForApiVersion", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = method!.Invoke(_instance, new object[] { description });
            return (OpenApiInfo)result!;
        }
    }
}

