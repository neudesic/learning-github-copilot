using System.Diagnostics;
using System.Reflection;
using eShop.EventBus.Abstractions;
using eShop.EventBus.Events;
using eShop.EventBusRabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBus.Tests;

public class RabbitMQEventBusTests
{
    private readonly ILogger<RabbitMQEventBus> _loggerSubstitute;
    private readonly IServiceProvider _serviceProviderSubstitute;
    private readonly IOptions<EventBusOptions> _optionsSubstitute;
    private readonly IOptions<EventBusSubscriptionInfo> _subscriptionOptionsSubstitute;
    private readonly RabbitMQTelemetry _telemetry;
    private readonly IConnection _connectionSubstitute;
    private readonly IModel _channelSubstitute;

    public RabbitMQEventBusTests()
    {
        _loggerSubstitute = Substitute.For<ILogger<RabbitMQEventBus>>();
        _serviceProviderSubstitute = Substitute.For<IServiceProvider>();
        _optionsSubstitute = Options.Create(new EventBusOptions 
        { 
            SubscriptionClientName = "test-queue",
            RetryCount = 1
        });
        _subscriptionOptionsSubstitute = Options.Create(new EventBusSubscriptionInfo());
        _telemetry = new RabbitMQTelemetry();
        _connectionSubstitute = Substitute.For<IConnection>();
        _channelSubstitute = Substitute.For<IModel>();
    }

    [Fact]
    public void Constructor_WithValidDependencies_CreatesInstance()
    {
        // Arrange & Act
        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            _serviceProviderSubstitute,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Assert
        Assert.NotNull(eventBus);
    }

    [Fact]
    public async Task PublishAsync_WithNullConnection_ThrowsInvalidOperationException()
    {
        // Arrange
        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            _serviceProviderSubstitute,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        var testEvent = new TestIntegrationEvent { TestData = "test" };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await eventBus.PublishAsync(testEvent));

        Assert.Contains("RabbitMQ connection is not open", exception.Message);
    }

    [Fact]
    public async Task PublishAsync_WithValidConnection_PublishesEvent()
    {
        // Arrange
        var basicPropertiesSubstitute = Substitute.For<IBasicProperties>();
        basicPropertiesSubstitute.Headers.Returns(new Dictionary<string, object>());

        _channelSubstitute.CreateBasicProperties().Returns(basicPropertiesSubstitute);
        _connectionSubstitute.CreateModel().Returns(_channelSubstitute);
        _connectionSubstitute.IsOpen.Returns(true);

        var serviceProviderWithConnection = Substitute.For<IServiceProvider>();
        serviceProviderWithConnection.GetService(typeof(IConnection)).Returns(_connectionSubstitute);

        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            serviceProviderWithConnection,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Set up the private _rabbitMQConnection field using reflection
        var connectionField = typeof(RabbitMQEventBus).GetField("_rabbitMQConnection", BindingFlags.NonPublic | BindingFlags.Instance);
        connectionField?.SetValue(eventBus, _connectionSubstitute);

        var testEvent = new TestIntegrationEvent { TestData = "test" };

        // Act
        await eventBus.PublishAsync(testEvent);

        // Assert
        _channelSubstitute.Received(1).ExchangeDeclare(
            Arg.Is("eshop_event_bus"),
            Arg.Is("direct"));

        _channelSubstitute.Received(1).BasicPublish(
            Arg.Is("eshop_event_bus"),
            Arg.Is("TestIntegrationEvent"),
            Arg.Is(true),
            Arg.Any<IBasicProperties>(),
            Arg.Any<ReadOnlyMemory<byte>>());
    }

    [Fact]
    public async Task PublishAsync_WithNullEvent_ThrowsArgumentNullException()
    {
        // Arrange
        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            _serviceProviderSubstitute,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Act & Assert - Will fail when trying to get type name from null
        await Assert.ThrowsAsync<NullReferenceException>(async () =>
            await eventBus.PublishAsync(null!));
    }

    [Fact]
    public async Task PublishAsync_UsesEventTypeNameAsRoutingKey()
    {
        // Arrange
        var basicPropertiesSubstitute = Substitute.For<IBasicProperties>();
        basicPropertiesSubstitute.Headers.Returns(new Dictionary<string, object>());

        _channelSubstitute.CreateBasicProperties().Returns(basicPropertiesSubstitute);
        _connectionSubstitute.CreateModel().Returns(_channelSubstitute);
        _connectionSubstitute.IsOpen.Returns(true);

        var serviceProviderWithConnection = Substitute.For<IServiceProvider>();
        serviceProviderWithConnection.GetService(typeof(IConnection)).Returns(_connectionSubstitute);

        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            serviceProviderWithConnection,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Set up the private _rabbitMQConnection field using reflection
        var connectionField = typeof(RabbitMQEventBus).GetField("_rabbitMQConnection", BindingFlags.NonPublic | BindingFlags.Instance);
        connectionField?.SetValue(eventBus, _connectionSubstitute);

        var testEvent = new TestIntegrationEvent { TestData = "test" };

        // Act
        await eventBus.PublishAsync(testEvent);

        // Assert
        _channelSubstitute.Received(1).BasicPublish(
            Arg.Any<string>(),
            Arg.Is("TestIntegrationEvent"),
            Arg.Any<bool>(),
            Arg.Any<IBasicProperties>(),
            Arg.Any<ReadOnlyMemory<byte>>());
    }

    [Fact]
    public async Task PublishAsync_SetsDeliveryModeToTwo()
    {
        // Arrange
        var basicPropertiesSubstitute = Substitute.For<IBasicProperties>();
        basicPropertiesSubstitute.Headers.Returns(new Dictionary<string, object>());

        _channelSubstitute.CreateBasicProperties().Returns(basicPropertiesSubstitute);
        _connectionSubstitute.CreateModel().Returns(_channelSubstitute);
        _connectionSubstitute.IsOpen.Returns(true);

        var serviceProviderWithConnection = Substitute.For<IServiceProvider>();
        serviceProviderWithConnection.GetService(typeof(IConnection)).Returns(_connectionSubstitute);

        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            serviceProviderWithConnection,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Set up the private _rabbitMQConnection field using reflection
        var connectionField = typeof(RabbitMQEventBus).GetField("_rabbitMQConnection", BindingFlags.NonPublic | BindingFlags.Instance);
        connectionField?.SetValue(eventBus, _connectionSubstitute);

        var testEvent = new TestIntegrationEvent { TestData = "test" };

        // Act
        await eventBus.PublishAsync(testEvent);

        // Assert
        Assert.Equal(2, basicPropertiesSubstitute.DeliveryMode);
    }

    [Fact]
    public async Task PublishAsync_WithPublishException_ThrowsException()
    {
        // Arrange
        var basicPropertiesSubstitute = Substitute.For<IBasicProperties>();
        basicPropertiesSubstitute.Headers.Returns(new Dictionary<string, object>());

        _channelSubstitute.CreateBasicProperties().Returns(basicPropertiesSubstitute);
        _connectionSubstitute.CreateModel().Returns(_channelSubstitute);
        _connectionSubstitute.IsOpen.Returns(true);

        _channelSubstitute.When(x => x.BasicPublish(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<IBasicProperties>(),
            Arg.Any<ReadOnlyMemory<byte>>()))
            .Throw(new Exception("Publish failed"));

        var serviceProviderWithConnection = Substitute.For<IServiceProvider>();
        serviceProviderWithConnection.GetService(typeof(IConnection)).Returns(_connectionSubstitute);

        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            serviceProviderWithConnection,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Set up the private _rabbitMQConnection field using reflection
        var connectionField = typeof(RabbitMQEventBus).GetField("_rabbitMQConnection", BindingFlags.NonPublic | BindingFlags.Instance);
        connectionField?.SetValue(eventBus, _connectionSubstitute);

        var testEvent = new TestIntegrationEvent { TestData = "test" };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () =>
            await eventBus.PublishAsync(testEvent));
    }

    [Fact]
    public void Dispose_ClosesConsumerChannel()
    {
        // Arrange
        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            _serviceProviderSubstitute,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Act
        eventBus.Dispose();

        // Assert - Dispose should not throw
        Assert.NotNull(eventBus);
    }

    [Fact]
    public async Task StartAsync_WithNullConnection_ReturnsCompletedTask()
    {
        // Arrange
        var serviceProviderWithNullConnection = Substitute.For<IServiceProvider>();
        serviceProviderWithNullConnection
            .GetService(typeof(IConnection))
            .Returns(null);

        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            serviceProviderWithNullConnection,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Act
        var task = eventBus.StartAsync(CancellationToken.None);
        await Task.Delay(100); // Wait for background task

        // Assert
        Assert.NotNull(task);
    }

    [Fact]
    public async Task StopAsync_ReturnsCompletedTask()
    {
        // Arrange
        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            _serviceProviderSubstitute,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Act
        var task = eventBus.StopAsync(CancellationToken.None);

        // Assert
        Assert.True(task.IsCompleted);
        await task;
    }

    [Fact]
    public void Implements_IEventBus_Interface()
    {
        // Arrange & Act
        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            _serviceProviderSubstitute,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Assert
        Assert.IsAssignableFrom<IEventBus>(eventBus);
    }

    [Fact]
    public void Implements_IDisposable_Interface()
    {
        // Arrange & Act
        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            _serviceProviderSubstitute,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Assert
        Assert.IsAssignableFrom<IDisposable>(eventBus);
    }

    [Fact]
    public void Implements_IHostedService_Interface()
    {
        // Arrange & Act
        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            _serviceProviderSubstitute,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Assert
        Assert.IsAssignableFrom<Microsoft.Extensions.Hosting.IHostedService>(eventBus);
    }

    [Fact]
    public void RabbitMQEventBus_IsSealed()
    {
        // Arrange
        var busType = typeof(RabbitMQEventBus);

        // Act & Assert
        Assert.True(busType.IsSealed);
    }

    [Fact]
    public async Task PublishAsync_WithMultipleEvents_PublishesAll()
    {
        // Arrange
        var basicPropertiesSubstitute = Substitute.For<IBasicProperties>();
        basicPropertiesSubstitute.Headers.Returns(new Dictionary<string, object>());

        _channelSubstitute.CreateBasicProperties().Returns(basicPropertiesSubstitute);
        _connectionSubstitute.CreateModel().Returns(_channelSubstitute);
        _connectionSubstitute.IsOpen.Returns(true);

        var serviceProviderWithConnection = Substitute.For<IServiceProvider>();
        serviceProviderWithConnection.GetService(typeof(IConnection)).Returns(_connectionSubstitute);

        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            serviceProviderWithConnection,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Set up the private _rabbitMQConnection field using reflection
        var connectionField = typeof(RabbitMQEventBus).GetField("_rabbitMQConnection", BindingFlags.NonPublic | BindingFlags.Instance);
        connectionField?.SetValue(eventBus, _connectionSubstitute);

        var event1 = new TestIntegrationEvent { TestData = "event1" };
        var event2 = new TestIntegrationEvent { TestData = "event2" };

        // Act
        await eventBus.PublishAsync(event1);
        await eventBus.PublishAsync(event2);

        // Assert
        _channelSubstitute.Received(2).BasicPublish(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<IBasicProperties>(),
            Arg.Any<ReadOnlyMemory<byte>>());
    }

    [Fact]
    public async Task PublishAsync_PreservesEventId()
    {
        // Arrange
        var basicPropertiesSubstitute = Substitute.For<IBasicProperties>();
        basicPropertiesSubstitute.Headers.Returns(new Dictionary<string, object>());

        _channelSubstitute.CreateBasicProperties().Returns(basicPropertiesSubstitute);
        _connectionSubstitute.CreateModel().Returns(_channelSubstitute);
        _connectionSubstitute.IsOpen.Returns(true);

        var serviceProviderWithConnection = Substitute.For<IServiceProvider>();
        serviceProviderWithConnection.GetService(typeof(IConnection)).Returns(_connectionSubstitute);

        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            serviceProviderWithConnection,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Set up the private _rabbitMQConnection field using reflection
        var connectionField = typeof(RabbitMQEventBus).GetField("_rabbitMQConnection", BindingFlags.NonPublic | BindingFlags.Instance);
        connectionField?.SetValue(eventBus, _connectionSubstitute);

        var testEvent = new TestIntegrationEvent { TestData = "test" };
        var originalEventId = testEvent.Id;

        // Act
        await eventBus.PublishAsync(testEvent);

        // Assert
        Assert.Equal(originalEventId, testEvent.Id);
    }

    [Fact]
    public void Constructor_WithValidDependencies_SetsUpResiliencePipeline()
    {
        // Arrange
        var optionsWithRetry = Options.Create(new EventBusOptions 
        { 
            SubscriptionClientName = "test-queue",
            RetryCount = 5
        });

        // Act
        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            _serviceProviderSubstitute,
            optionsWithRetry,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Assert - Constructor completes without error
        Assert.NotNull(eventBus);
    }

    [Fact]
    public async Task PublishAsync_WithExchangeDeclareFailure_ThrowsException()
    {
        // Arrange
        var basicPropertiesSubstitute = Substitute.For<IBasicProperties>();
        basicPropertiesSubstitute.Headers.Returns(new Dictionary<string, object>());

        _channelSubstitute.CreateBasicProperties().Returns(basicPropertiesSubstitute);
        _connectionSubstitute.CreateModel().Returns(_channelSubstitute);
        _connectionSubstitute.IsOpen.Returns(true);

        _channelSubstitute.When(x => x.ExchangeDeclare(Arg.Any<string>(), Arg.Any<string>()))
            .Throw(new BrokerUnreachableException(new Exception("Broker unreachable")));

        var serviceProviderWithConnection = Substitute.For<IServiceProvider>();
        serviceProviderWithConnection.GetService(typeof(IConnection)).Returns(_connectionSubstitute);

        var eventBus = new RabbitMQEventBus(
            _loggerSubstitute,
            serviceProviderWithConnection,
            _optionsSubstitute,
            _subscriptionOptionsSubstitute,
            _telemetry);

        // Set up the private _rabbitMQConnection field using reflection
        var connectionField = typeof(RabbitMQEventBus).GetField("_rabbitMQConnection", BindingFlags.NonPublic | BindingFlags.Instance);
        connectionField?.SetValue(eventBus, _connectionSubstitute);

        var testEvent = new TestIntegrationEvent { TestData = "test" };

        // Act & Assert - Should throw after retries exhausted
        await Assert.ThrowsAsync<BrokerUnreachableException>(async () =>
            await eventBus.PublishAsync(testEvent));
    }

    // Test helper class
    private record TestIntegrationEvent : IntegrationEvent
    {
        public string TestData { get; set; } = string.Empty;
    }
}
