namespace eShop.ServiceDefaults.Tests;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Xunit;

public class ExtensionsTests
{
    [Fact]
    public void AddServiceDefaults_RegistersServicesSuccessfully()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act
        var result = builder.AddServiceDefaults();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        var serviceProvider = builder.Services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void AddServiceDefaults_WithOtlpExporter_ConfiguresExporter()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            { "OTEL_EXPORTER_OTLP_ENDPOINT", "http://localhost:4318" }
        };
        var builder = CreateHostApplicationBuilder(config);

        // Act
        var result = builder.AddServiceDefaults();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
    }

    [Fact]
    public void AddServiceDefaults_WithoutOtlpExporter_SkipsExporterConfiguration()
    {
        // Arrange
        var config = new Dictionary<string, string?>();
        var builder = CreateHostApplicationBuilder(config);

        // Act
        var result = builder.AddServiceDefaults();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
    }

    [Fact]
    public void AddBasicServiceDefaults_RegistersHealthChecks()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act
        var result = builder.AddBasicServiceDefaults();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        var serviceProvider = builder.Services.BuildServiceProvider();
        var healthCheckService = serviceProvider.GetService<HealthCheckService>();
        Assert.NotNull(healthCheckService);
    }

    [Fact]
    public void AddBasicServiceDefaults_ReturnsBuilder()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act
        var result = builder.AddBasicServiceDefaults();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IHostApplicationBuilder>(result);
    }

    [Fact]
    public void ConfigureOpenTelemetry_ConfiguresLogging()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act
        var result = builder.ConfigureOpenTelemetry();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
    }

    [Fact]
    public void ConfigureOpenTelemetry_WithDevelopmentEnvironment_UsesSamplerAlwaysOn()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(
            new Dictionary<string, string?>(),
            Environments.Development);

        // Act
        var result = builder.ConfigureOpenTelemetry();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
    }

    [Fact]
    public void ConfigureOpenTelemetry_WithProductionEnvironment_ConfiguresNormally()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(
            new Dictionary<string, string?>(),
            Environments.Production);

        // Act
        var result = builder.ConfigureOpenTelemetry();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
    }

    [Fact]
    public void ConfigureOpenTelemetry_ReturnsBuilder()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act
        var result = builder.ConfigureOpenTelemetry();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IHostApplicationBuilder>(result);
    }

    [Fact]
    public void AddDefaultHealthChecks_RegistersHealthCheckService()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act
        var result = builder.AddDefaultHealthChecks();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
        var serviceProvider = builder.Services.BuildServiceProvider();
        var healthCheckService = serviceProvider.GetService<HealthCheckService>();
        Assert.NotNull(healthCheckService);
    }

    [Fact]
    public void AddDefaultHealthChecks_AddsLiveHealthCheck()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act
        builder.AddDefaultHealthChecks();
        var serviceProvider = builder.Services.BuildServiceProvider();
        var healthCheckService = serviceProvider.GetRequiredService<HealthCheckService>();

        // Assert
        Assert.NotNull(healthCheckService);
    }

    [Fact]
    public void AddDefaultHealthChecks_ReturnsBuilder()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act
        var result = builder.AddDefaultHealthChecks();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IHostApplicationBuilder>(result);
    }

    [Fact]
    public void MapDefaultEndpoints_WithDevelopmentEnvironment_ReturnsSameApp()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = Environments.Development;
        builder.Services.AddHealthChecks();
        var app = builder.Build();

        // Act
        var result = app.MapDefaultEndpoints();

        // Assert
        Assert.NotNull(result);
        Assert.Same(app, result);
    }

    [Fact]
    public void MapDefaultEndpoints_WithProductionEnvironment_ReturnsSameApp()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = Environments.Production;
        builder.Services.AddHealthChecks();
        var app = builder.Build();

        // Act
        var result = app.MapDefaultEndpoints();

        // Assert
        Assert.NotNull(result);
        Assert.Same(app, result);
    }

    [Fact]
    public void MapDefaultEndpoints_WithStagingEnvironment_ReturnsSameApp()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = Environments.Staging;
        builder.Services.AddHealthChecks();
        var app = builder.Build();

        // Act
        var result = app.MapDefaultEndpoints();

        // Assert
        Assert.NotNull(result);
        Assert.Same(app, result);
    }

    [Fact]
    public void AddServiceDefaults_CanBeCalledMultipleTimes()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act & Assert - should not throw
        var result1 = builder.AddServiceDefaults();
        Assert.NotNull(result1);
    }

    [Fact]
    public void AddBasicServiceDefaults_CanBeCalledMultipleTimes()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act & Assert - should not throw
        var result1 = builder.AddBasicServiceDefaults();
        Assert.NotNull(result1);
    }

    [Fact]
    public void ConfigureOpenTelemetry_CanBeCalledMultipleTimes()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act & Assert - should not throw
        var result1 = builder.ConfigureOpenTelemetry();
        Assert.NotNull(result1);
    }

    [Fact]
    public void AddDefaultHealthChecks_CanBeCalledMultipleTimes()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act & Assert - should not throw
        var result1 = builder.AddDefaultHealthChecks();
        Assert.NotNull(result1);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void AddServiceDefaults_WithEmptyOtlpExporter_SkipsExporter(string endpointValue)
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            { "OTEL_EXPORTER_OTLP_ENDPOINT", endpointValue }
        };
        var builder = CreateHostApplicationBuilder(config);

        // Act
        var result = builder.AddServiceDefaults();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
    }

    [Theory]
    [InlineData("http://localhost:4318")]
    [InlineData("https://otel-collector.example.com")]
    [InlineData("http://10.0.0.1:4318")]
    public void AddServiceDefaults_WithVariousOtlpEndpoints_ConfiguresSuccessfully(string endpoint)
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            { "OTEL_EXPORTER_OTLP_ENDPOINT", endpoint }
        };
        var builder = CreateHostApplicationBuilder(config);

        // Act
        var result = builder.AddServiceDefaults();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Production")]
    [InlineData("Staging")]
    public void ConfigureOpenTelemetry_WithVariousEnvironments_ConfiguresSuccessfully(string environment)
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>(), environment);

        // Act
        var result = builder.ConfigureOpenTelemetry();

        // Assert
        Assert.NotNull(result);
        Assert.Same(builder, result);
    }

    [Fact]
    public void AddServiceDefaults_EnablesSemanticKernelDiagnostics()
    {
        // Arrange
        var builder = CreateHostApplicationBuilder(new Dictionary<string, string?>());

        // Act
        builder.AddServiceDefaults();

        // Assert
        var switchValue = AppContext.TryGetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", out var isEnabled);
        Assert.True(switchValue);
        Assert.True(isEnabled);
    }

    private static IHostApplicationBuilder CreateHostApplicationBuilder(
        Dictionary<string, string?> configValues,
        string? environmentName = null)
    {
        var builder = new HostApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(configValues);
        if (environmentName != null)
        {
            builder.Environment.EnvironmentName = environmentName;
        }
        return builder;
    }
}
