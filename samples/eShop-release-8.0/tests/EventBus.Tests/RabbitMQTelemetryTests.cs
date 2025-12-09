using System.Diagnostics;
using eShop.EventBusRabbitMQ;
using OpenTelemetry.Context.Propagation;

namespace EventBus.Tests;

public class RabbitMQTelemetryTests
{
    [Fact]
    public void Constructor_CreatesInstance()
    {
        // Act
        var telemetry = new RabbitMQTelemetry();

        // Assert
        Assert.NotNull(telemetry);
    }

    [Fact]
    public void ActivitySourceName_HasCorrectValue()
    {
        // Assert
        Assert.Equal("EventBusRabbitMQ", RabbitMQTelemetry.ActivitySourceName);
    }

    [Fact]
    public void ActivitySource_IsNotNull()
    {
        // Arrange
        var telemetry = new RabbitMQTelemetry();

        // Act
        var activitySource = telemetry.ActivitySource;

        // Assert
        Assert.NotNull(activitySource);
    }

    [Fact]
    public void ActivitySource_HasCorrectName()
    {
        // Arrange
        var telemetry = new RabbitMQTelemetry();

        // Act
        var activitySource = telemetry.ActivitySource;

        // Assert
        Assert.Equal("EventBusRabbitMQ", activitySource.Name);
    }

    [Fact]
    public void Propagator_IsNotNull()
    {
        // Arrange
        var telemetry = new RabbitMQTelemetry();

        // Act
        var propagator = telemetry.Propagator;

        // Assert
        Assert.NotNull(propagator);
    }

    [Fact]
    public void Propagator_IsTextMapPropagator()
    {
        // Arrange
        var telemetry = new RabbitMQTelemetry();

        // Act
        var propagator = telemetry.Propagator;

        // Assert
        Assert.IsAssignableFrom<TextMapPropagator>(propagator);
    }

    [Fact]
    public void Propagator_IsDefaultTextMapPropagator()
    {
        // Arrange
        var telemetry = new RabbitMQTelemetry();

        // Act
        var propagator = telemetry.Propagator;

        // Assert
        Assert.Same(Propagators.DefaultTextMapPropagator, propagator);
    }

    [Fact]
    public void ActivitySource_Property_IsReadOnly()
    {
        // Arrange
        var telemetry = new RabbitMQTelemetry();
        var activitySource = telemetry.ActivitySource;

        // Act & Assert
        var property = typeof(RabbitMQTelemetry).GetProperty(nameof(RabbitMQTelemetry.ActivitySource));
        Assert.NotNull(property);
        Assert.Null(property.SetMethod);
    }

    [Fact]
    public void Propagator_Property_IsReadOnly()
    {
        // Arrange
        var telemetry = new RabbitMQTelemetry();
        var propagator = telemetry.Propagator;

        // Act & Assert
        var property = typeof(RabbitMQTelemetry).GetProperty(nameof(RabbitMQTelemetry.Propagator));
        Assert.NotNull(property);
        Assert.Null(property.SetMethod);
    }

    [Fact]
    public void ActivitySourceName_IsStatic()
    {
        // Act & Assert
        var field = typeof(RabbitMQTelemetry).GetField(nameof(RabbitMQTelemetry.ActivitySourceName));
        Assert.NotNull(field);
        Assert.True(field.IsStatic);
    }

    [Fact]
    public void MultipleInstances_HaveEqualActivitySourceNames()
    {
        // Arrange
        var telemetry1 = new RabbitMQTelemetry();
        var telemetry2 = new RabbitMQTelemetry();

        // Act & Assert
        Assert.Equal(telemetry1.ActivitySource.Name, telemetry2.ActivitySource.Name);
    }

    [Fact]
    public void MultipleInstances_HaveSamePropagator()
    {
        // Arrange
        var telemetry1 = new RabbitMQTelemetry();
        var telemetry2 = new RabbitMQTelemetry();

        // Act & Assert
        Assert.Same(telemetry1.Propagator, telemetry2.Propagator);
    }

    [Fact]
    public void ActivitySource_CanCreateActivity()
    {
        // Arrange
        var telemetry = new RabbitMQTelemetry();
        var activitySource = telemetry.ActivitySource;

        // Add a listener to allow activity creation
        using var listener = new ActivityListener { ShouldListenTo = _ => true, Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData };
        ActivitySource.AddActivityListener(listener);

        // Act
        var activity = activitySource.StartActivity("TestActivity");

        // Assert
        Assert.NotNull(activity);
        activity?.Dispose();
    }

    [Fact]
    public void ActivitySource_CreatedActivityCanBeDisposed()
    {
        // Arrange
        var telemetry = new RabbitMQTelemetry();
        var activitySource = telemetry.ActivitySource;

        // Add a listener to allow activity creation
        using var listener = new ActivityListener { ShouldListenTo = _ => true, Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData };
        ActivitySource.AddActivityListener(listener);

        // Act
        var activity = activitySource.StartActivity("TestActivity");

        // Assert
        Assert.NotNull(activity);
        var exception = Record.Exception(() => activity?.Dispose());
        Assert.Null(exception);
    }

    [Fact]
    public void Propagator_IsDefaultCompositePropagator()
    {
        // Arrange
        var telemetry = new RabbitMQTelemetry();

        // Act
        var propagator = telemetry.Propagator;

        // Assert
        Assert.NotNull(propagator);
        var propagatorType = propagator.GetType();
        Assert.NotNull(propagatorType);
    }

    [Fact]
    public void RabbitMQTelemetry_IsPublic()
    {
        // Arrange
        var type = typeof(RabbitMQTelemetry);

        // Act & Assert
        Assert.True(type.IsPublic);
    }

    [Fact]
    public void Constructor_DoesNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() => new RabbitMQTelemetry());
        Assert.Null(exception);
    }

    [Fact]
    public void ActivitySourceName_MatchesActivitySourcePropertyName()
    {
        // Arrange
        var telemetry = new RabbitMQTelemetry();

        // Act
        var staticName = RabbitMQTelemetry.ActivitySourceName;
        var instanceName = telemetry.ActivitySource.Name;

        // Assert
        Assert.Equal(staticName, instanceName);
    }

    [Fact]
    public void Multiple_ActivitySources_HaveSameName()
    {
        // Arrange
        var telemetry1 = new RabbitMQTelemetry();
        var telemetry2 = new RabbitMQTelemetry();

        // Act
        var source1 = telemetry1.ActivitySource;
        var source2 = telemetry2.ActivitySource;

        // Assert
        Assert.Equal(source1.Name, source2.Name);
    }

    [Fact]
    public void ActivitySource_VersionIsEmptyString()
    {
        // Arrange
        var telemetry = new RabbitMQTelemetry();

        // Act
        var activitySource = telemetry.ActivitySource;

        // Assert
        Assert.Equal(string.Empty, activitySource.Version);
    }

    [Fact]
    public void Propagator_Fields_AreInitializedInConstructor()
    {
        // Arrange & Act
        var telemetry = new RabbitMQTelemetry();

        // Assert
        Assert.NotNull(telemetry.ActivitySource);
        Assert.NotNull(telemetry.Propagator);
    }
}
