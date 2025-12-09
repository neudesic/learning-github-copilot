using eShop.EventBus.Abstractions;
using eShop.EventBus.Events;

namespace EventBus.Tests;

public record TestIntegrationEvent : IntegrationEvent
{
    public string? TestData { get; set; } = "TestData";
}

public class TestEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
{
    public bool HandleCalled { get; private set; }
    public TestIntegrationEvent? ReceivedEvent { get; private set; }
    public int CallCount { get; private set; }

    public Task Handle(TestIntegrationEvent @event)
    {
        HandleCalled = true;
        ReceivedEvent = @event;
        CallCount++;
        return Task.CompletedTask;
    }
}

public class ThrowingEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
{
    public Task Handle(TestIntegrationEvent @event)
    {
        throw new InvalidOperationException("Handler intentionally throws");
    }
}

public class AsyncEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
{
    public int CallCount { get; private set; }

    public async Task Handle(TestIntegrationEvent @event)
    {
        CallCount++;
        await Task.Delay(10);
    }
}

public class IIntegrationEventHandlerTests
{
    [Fact]
    public async Task Handle_WithValidEvent_ShouldExecuteHandler()
    {
        // Arrange
        var handler = new TestEventHandler();
        var testEvent = new TestIntegrationEvent { TestData = "Custom Data" };

        // Act
        await handler.Handle(testEvent);

        // Assert
        Assert.True(handler.HandleCalled);
        Assert.NotNull(handler.ReceivedEvent);
        Assert.Equal("Custom Data", handler.ReceivedEvent.TestData);
    }

    [Fact]
    public async Task Handle_ShouldPassEventDataCorrectly()
    {
        // Arrange
        var handler = new TestEventHandler();
        var testEvent = new TestIntegrationEvent { TestData = "Important Data" };

        // Act
        await handler.Handle(testEvent);

        // Assert
        Assert.NotNull(handler.ReceivedEvent);
        Assert.Equal(testEvent.TestData, handler.ReceivedEvent.TestData);
        Assert.Equal(testEvent.Id, handler.ReceivedEvent.Id);
    }

    [Fact]
    public async Task Handle_WithMultipleCalls_ShouldIncrementCallCount()
    {
        // Arrange
        var handler = new TestEventHandler();
        var event1 = new TestIntegrationEvent();
        var event2 = new TestIntegrationEvent();

        // Act
        await handler.Handle(event1);
        await handler.Handle(event2);

        // Assert
        Assert.Equal(2, handler.CallCount);
    }

    [Fact]
    public async Task Handle_ShouldReturnTask()
    {
        // Arrange
        var handler = new TestEventHandler();
        var testEvent = new TestIntegrationEvent();

        // Act
        var task = handler.Handle(testEvent);

        // Assert
        Assert.NotNull(task);
        await task;
    }

    [Fact]
    public async Task Handle_ShouldPreserveEventId()
    {
        // Arrange
        var handler = new TestEventHandler();
        var testEvent = new TestIntegrationEvent();
        var originalId = testEvent.Id;

        // Act
        await handler.Handle(testEvent);

        // Assert
        Assert.NotNull(handler.ReceivedEvent);
        Assert.Equal(originalId, handler.ReceivedEvent.Id);
    }

    [Fact]
    public async Task Handle_ShouldPreserveEventCreationDate()
    {
        // Arrange
        var handler = new TestEventHandler();
        var testEvent = new TestIntegrationEvent();
        var originalCreationDate = testEvent.CreationDate;

        // Act
        await handler.Handle(testEvent);

        // Assert
        Assert.NotNull(handler.ReceivedEvent);
        Assert.Equal(originalCreationDate, handler.ReceivedEvent.CreationDate);
    }

    [Fact]
    public async Task IIntegrationEventHandler_ExplicitImplementation_ShouldCastEventCorrectly()
    {
        // Arrange
        IIntegrationEventHandler handler = new TestEventHandler();
        var testEvent = new TestIntegrationEvent { TestData = "Cast Test" };

        // Act
        await handler.Handle(testEvent);

        // Assert
        var castedHandler = (TestEventHandler)handler;
        Assert.NotNull(castedHandler.ReceivedEvent);
        Assert.Equal("Cast Test", castedHandler.ReceivedEvent.TestData);
    }

    [Fact]
    public void GenericHandler_CanBeAssignedToBaseInterface()
    {
        // Arrange
        IIntegrationEventHandler baseHandler = new TestEventHandler();

        // Act & Assert
        Assert.NotNull(baseHandler);
        Assert.IsAssignableFrom<IIntegrationEventHandler>(baseHandler);
    }

    [Fact]
    public async Task Handle_WithDifferentEventTypes_ShouldHandleIndependently()
    {
        // Arrange
        var handler1 = new TestEventHandler();
        var handler2 = new TestEventHandler();
        var event1 = new TestIntegrationEvent { TestData = "Event 1" };
        var event2 = new TestIntegrationEvent { TestData = "Event 2" };

        // Act
        await handler1.Handle(event1);
        await handler2.Handle(event2);

        // Assert
        Assert.NotNull(handler1.ReceivedEvent);
        Assert.NotNull(handler2.ReceivedEvent);
        Assert.Equal("Event 1", handler1.ReceivedEvent.TestData);
        Assert.Equal("Event 2", handler2.ReceivedEvent.TestData);
        Assert.NotEqual(handler1.ReceivedEvent.TestData, handler2.ReceivedEvent.TestData);
    }

    [Fact]
    public async Task AsyncHandler_ShouldCompleteAsynchronously()
    {
        // Arrange
        var handler = new AsyncEventHandler();
        var testEvent = new TestIntegrationEvent();

        // Act
        var task = handler.Handle(testEvent);
        await task;

        // Assert
        Assert.True(task.IsCompleted);
        Assert.Equal(1, handler.CallCount);
    }

    [Fact]
    public async Task AsyncHandler_WithMultipleCalls_ShouldHandleSequentially()
    {
        // Arrange
        var handler = new AsyncEventHandler();
        var event1 = new TestIntegrationEvent();
        var event2 = new TestIntegrationEvent();

        // Act
        await handler.Handle(event1);
        await handler.Handle(event2);

        // Assert
        Assert.Equal(2, handler.CallCount);
    }

    [Fact]
    public async Task ThrowingHandler_ShouldThrowException()
    {
        // Arrange
        var handler = new ThrowingEventHandler();
        var testEvent = new TestIntegrationEvent();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await handler.Handle(testEvent)
        );
        Assert.NotNull(exception);
        Assert.Contains("intentionally throws", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldNotModifyOriginalEvent()
    {
        // Arrange
        var handler = new TestEventHandler();
        var originalTestData = "Original Data";
        var testEvent = new TestIntegrationEvent { TestData = originalTestData };
        var originalId = testEvent.Id;
        var originalCreationDate = testEvent.CreationDate;

        // Act
        await handler.Handle(testEvent);

        // Assert
        Assert.Equal(originalTestData, testEvent.TestData);
        Assert.Equal(originalId, testEvent.Id);
        Assert.Equal(originalCreationDate, testEvent.CreationDate);
    }

    [Fact]
    public async Task Handle_WithNullableProperties_ShouldHandleCorrectly()
    {
        // Arrange
        var handler = new TestEventHandler();
        var testEvent = new TestIntegrationEvent { TestData = null };

        // Act
        await handler.Handle(testEvent);

        // Assert
        Assert.NotNull(handler.ReceivedEvent);
        Assert.Null(handler.ReceivedEvent.TestData);
    }

    [Fact]
    public async Task MultipleHandlers_ShouldOperateIndependently()
    {
        // Arrange
        var handler1 = new TestEventHandler();
        var handler2 = new TestEventHandler();
        var handler3 = new TestEventHandler();
        var testEvent = new TestIntegrationEvent();

        // Act
        await handler1.Handle(testEvent);
        await handler2.Handle(testEvent);
        await handler3.Handle(testEvent);

        // Assert
        Assert.Equal(1, handler1.CallCount);
        Assert.Equal(1, handler2.CallCount);
        Assert.Equal(1, handler3.CallCount);
    }

    [Fact]
    public async Task Handle_ShouldBeAwaitableMultipleTimes()
    {
        // Arrange
        var handler = new TestEventHandler();
        var testEvent = new TestIntegrationEvent();

        // Act
        var task1 = handler.Handle(testEvent);
        var task2 = handler.Handle(new TestIntegrationEvent());

        await task1;
        await task2;

        // Assert
        Assert.Equal(2, handler.CallCount);
        Assert.True(task1.IsCompleted);
        Assert.True(task2.IsCompleted);
    }

    [Fact]
    public async Task IntegrationEventHandler_WithInheritedEvent_ShouldWork()
    {
        // Arrange
        var handler = new TestEventHandler();
        var derivedEvent = new TestIntegrationEvent { TestData = "Derived" };

        // Act
        await handler.Handle(derivedEvent);

        // Assert
        Assert.NotNull(handler.ReceivedEvent);
        Assert.Equal("Derived", handler.ReceivedEvent.TestData);
    }

    [Theory]
    [InlineData("Event1")]
    [InlineData("Event2")]
    [InlineData("EventWithLongName")]
    public async Task Handle_WithVariousEventData_ShouldPreserveData(string testData)
    {
        // Arrange
        var handler = new TestEventHandler();
        var testEvent = new TestIntegrationEvent { TestData = testData };

        // Act
        await handler.Handle(testEvent);

        // Assert
        Assert.NotNull(handler.ReceivedEvent);
        Assert.Equal(testData, handler.ReceivedEvent.TestData);
    }

    [Fact]
    public async Task Handle_ShouldNotBlockCaller()
    {
        // Arrange
        var handler = new AsyncEventHandler();
        var testEvent = new TestIntegrationEvent();
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var task = handler.Handle(testEvent);
        stopwatch.Stop();

        // Assert - calling Handle should return immediately without waiting
        Assert.True(stopwatch.ElapsedMilliseconds < 1000);

        // Wait for actual completion
        await task;
        Assert.Equal(1, handler.CallCount);
    }

    [Fact]
    public async Task Handle_ImplementationExplicitly_ShouldDelegate()
    {
        // Arrange
        var handler = new TestEventHandler();
        IIntegrationEventHandler baseHandler = handler;
        var testEvent = new TestIntegrationEvent { TestData = "Explicit Test" };

        // Act
        await baseHandler.Handle(testEvent);

        // Assert
        Assert.NotNull(handler.ReceivedEvent);
        Assert.Equal("Explicit Test", handler.ReceivedEvent.TestData);
    }

    [Fact]
    public async Task Handle_WithEventId_ShouldMaintainIdentity()
    {
        // Arrange
        var handler = new TestEventHandler();
        var testEvent = new TestIntegrationEvent();
        var originalId = testEvent.Id;

        // Act
        await handler.Handle(testEvent);

        // Assert
        Assert.NotNull(handler.ReceivedEvent);
        Assert.Equal(originalId, handler.ReceivedEvent.Id);
    }

    [Fact]
    public void Handler_Interface_ShouldSupportCovariance()
    {
        // Arrange
        var handler = new TestEventHandler();
        IIntegrationEventHandler interfaceHandler = handler;

        // Act & Assert
        Assert.NotNull(interfaceHandler);
        Assert.IsAssignableFrom<IIntegrationEventHandler>(handler);
    }

    [Fact]
    public async Task Handle_ShouldAllowAsyncCompletion()
    {
        // Arrange
        var handler = new AsyncEventHandler();
        var testEvent = new TestIntegrationEvent();

        // Act
        await handler.Handle(testEvent);

        // Assert
        Assert.Equal(1, handler.CallCount);
    }

    [Fact]
    public async Task Handle_WithMultipleAsyncHandlers_ShouldExecuteIndependently()
    {
        // Arrange
        var handler1 = new AsyncEventHandler();
        var handler2 = new AsyncEventHandler();
        var testEvent = new TestIntegrationEvent();

        // Act
        var task1 = handler1.Handle(testEvent);
        var task2 = handler2.Handle(testEvent);

        await Task.WhenAll(task1, task2);

        // Assert
        Assert.Equal(1, handler1.CallCount);
        Assert.Equal(1, handler2.CallCount);
    }

    [Fact]
    public async Task Handle_ShouldPreserveEventProperties()
    {
        // Arrange
        var handler = new TestEventHandler();
        var testEvent = new TestIntegrationEvent { TestData = "Test Property" };
        var originalId = testEvent.Id;
        var originalCreationDate = testEvent.CreationDate;

        // Act
        await handler.Handle(testEvent);

        // Assert
        Assert.NotNull(handler.ReceivedEvent);
        Assert.Equal(originalId, handler.ReceivedEvent.Id);
        Assert.Equal(originalCreationDate, handler.ReceivedEvent.CreationDate);
        Assert.Equal("Test Property", handler.ReceivedEvent.TestData);
    }

    [Fact]
    public void IIntegrationEventHandler_BaseInterface_ShouldBeImplementable()
    {
        // Arrange & Act
        var handler = new TestEventHandler() as IIntegrationEventHandler;

        // Assert
        Assert.NotNull(handler);
        Assert.IsAssignableFrom<IIntegrationEventHandler>(handler);
    }

    [Fact]
    public async Task Handle_ShouldAllowMultipleAsyncCalls()
    {
        // Arrange
        var handler = new AsyncEventHandler();
        var event1 = new TestIntegrationEvent();
        var event2 = new TestIntegrationEvent();
        var event3 = new TestIntegrationEvent();

        // Act
        var task1 = handler.Handle(event1);
        var task2 = handler.Handle(event2);
        var task3 = handler.Handle(event3);

        await Task.WhenAll(task1, task2, task3);

        // Assert
        Assert.Equal(3, handler.CallCount);
    }
}
