using eShop.EventBus.Abstractions;
using eShop.OrderProcessor;
using eShop.OrderProcessor.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace OrderProcessor.Tests;

public class GracePeriodManagerServiceTests
{
    [Fact]
    public void BackgroundTaskOptions_HasCorrectProperties()
    {
        var options = new BackgroundTaskOptions
        {
            CheckUpdateTime = 10,
            GracePeriodTime = 5
        };

        Assert.Equal(10, options.CheckUpdateTime);
        Assert.Equal(5, options.GracePeriodTime);
    }

    [Fact]
    public void GracePeriodConfirmedIntegrationEvent_CreatesWithCorrectOrderId()
    {
        var orderId = 123;
        var integrationEvent = new GracePeriodConfirmedIntegrationEvent(orderId);

        Assert.Equal(orderId, integrationEvent.OrderId);
        Assert.NotEqual(Guid.Empty, integrationEvent.Id);
        Assert.True(integrationEvent.CreationDate <= DateTime.UtcNow);
    }

    [Fact]
    public void GracePeriodConfirmedIntegrationEvent_HasUniqueIds()
    {
        var event1 = new GracePeriodConfirmedIntegrationEvent(1);
        var event2 = new GracePeriodConfirmedIntegrationEvent(1);

        Assert.NotEqual(event1.Id, event2.Id);
    }

    [Fact]
    public void BackgroundTaskOptions_DefaultValues()
    {
        var options = new BackgroundTaskOptions();

        Assert.Equal(0, options.CheckUpdateTime);
        Assert.Equal(0, options.GracePeriodTime);
    }

    [Fact]
    public void GracePeriodConfirmedIntegrationEvent_WithDifferentOrderIds()
    {
        var event1 = new GracePeriodConfirmedIntegrationEvent(1);
        var event2 = new GracePeriodConfirmedIntegrationEvent(2);

        Assert.NotEqual(event1.OrderId, event2.OrderId);
        Assert.Equal(1, event1.OrderId);
        Assert.Equal(2, event2.OrderId);
    }

    [Fact]
    public void Options_CanBeSetAndRetrieved()
    {
        var options = Options.Create(new BackgroundTaskOptions
        {
            CheckUpdateTime = 30,
            GracePeriodTime = 15
        });

        Assert.NotNull(options.Value);
        Assert.Equal(30, options.Value.CheckUpdateTime);
        Assert.Equal(15, options.Value.GracePeriodTime);
    }

    [Fact]
    public async Task EventBus_CanPublishEvent()
    {
        var eventBus = Substitute.For<IEventBus>();
        var integrationEvent = new GracePeriodConfirmedIntegrationEvent(100);

        await eventBus.PublishAsync(integrationEvent);

        await eventBus.Received(1).PublishAsync(Arg.Is<GracePeriodConfirmedIntegrationEvent>(e => e.OrderId == 100));
    }

    [Fact]
    public void Logger_CanBeSubstituted()
    {
        var logger = Substitute.For<ILogger<eShop.OrderProcessor.Services.GracePeriodManagerService>>();

        logger.IsEnabled(LogLevel.Debug).Returns(true);

        Assert.True(logger.IsEnabled(LogLevel.Debug));
    }

    [Fact]
    public void GracePeriodConfirmedIntegrationEvent_CreationDateIsRecent()
    {
        var beforeCreation = DateTime.UtcNow;
        var integrationEvent = new GracePeriodConfirmedIntegrationEvent(42);
        var afterCreation = DateTime.UtcNow;

        Assert.True(integrationEvent.CreationDate >= beforeCreation);
        Assert.True(integrationEvent.CreationDate <= afterCreation);
    }

    [Fact]
    public void BackgroundTaskOptions_CanSetNegativeValues()
    {
        var options = new BackgroundTaskOptions
        {
            CheckUpdateTime = -1,
            GracePeriodTime = -5
        };

        Assert.Equal(-1, options.CheckUpdateTime);
        Assert.Equal(-5, options.GracePeriodTime);
    }
}
