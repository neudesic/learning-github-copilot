using eShop.Ordering.API.Application.Behaviors;
using eShop.Ordering.API.Application.IntegrationEvents;
using eShop.Ordering.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ordering.API.Tests.Application.Behaviors;

public class TransactionBehaviorTests
{
    [Fact]
    public void Constructor_ThrowsArgumentException_WhenDbContextIsNull()
    {
        // Arrange
        var mockIntegrationEventService = new Mock<IOrderingIntegrationEventService>();
        var mockLogger = new Mock<ILogger<TransactionBehavior<TestRequest, TestResponse>>>();
        
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TransactionBehavior<TestRequest, TestResponse>(
                null!,
                mockIntegrationEventService.Object,
                mockLogger.Object));
        
        Assert.Equal("OrderingContext", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenIntegrationEventServiceIsNull()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<OrderingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var mockDbContext = new OrderingContext(options, Mock.Of<IMediator>());
        var mockLogger = new Mock<ILogger<TransactionBehavior<TestRequest, TestResponse>>>();
        
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TransactionBehavior<TestRequest, TestResponse>(
                mockDbContext,
                null!,
                mockLogger.Object));
        
        Assert.Equal("orderingIntegrationEventService", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenLoggerIsNull()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<OrderingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var mockDbContext = new OrderingContext(options, Mock.Of<IMediator>());
        var mockIntegrationEventService = new Mock<IOrderingIntegrationEventService>();
        
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TransactionBehavior<TestRequest, TestResponse>(
                mockDbContext,
                mockIntegrationEventService.Object,
                null!));
        
        Assert.Equal("ILogger", exception.Message);
    }

    [Fact]
    public void Constructor_CreatesInstance_WhenAllDependenciesProvided()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<OrderingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var mockDbContext = new OrderingContext(options, Mock.Of<IMediator>());
        var mockIntegrationEventService = new Mock<IOrderingIntegrationEventService>();
        var mockLogger = new Mock<ILogger<TransactionBehavior<TestRequest, TestResponse>>>();
        
        // Act
        var behavior = new TransactionBehavior<TestRequest, TestResponse>(
            mockDbContext,
            mockIntegrationEventService.Object,
            mockLogger.Object);
        
        // Assert
        Assert.NotNull(behavior);
    }

    [Fact]
    public async Task Handle_CallsNextDelegate()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<OrderingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var mockDbContext = new OrderingContext(options, Mock.Of<IMediator>());
        var mockIntegrationEventService = new Mock<IOrderingIntegrationEventService>();
        mockIntegrationEventService.Setup(x => x.PublishEventsThroughEventBusAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        var mockLogger = new Mock<ILogger<TransactionBehavior<TestRequest, TestResponse>>>();
        
        var behavior = new TransactionBehavior<TestRequest, TestResponse>(
            mockDbContext,
            mockIntegrationEventService.Object,
            mockLogger.Object);
        
        var request = new TestRequest();
        var expectedResponse = new TestResponse { Success = true };
        var nextCalled = false;
        
        RequestHandlerDelegate<TestResponse> next = () =>
        {
            nextCalled = true;
            return Task.FromResult(expectedResponse);
        };

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal(expectedResponse, result);
    }

    [Fact]
    public async Task Handle_ReturnsResponseFromNextDelegate()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<OrderingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var mockDbContext = new OrderingContext(options, Mock.Of<IMediator>());
        var mockIntegrationEventService = new Mock<IOrderingIntegrationEventService>();
        mockIntegrationEventService.Setup(x => x.PublishEventsThroughEventBusAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        var mockLogger = new Mock<ILogger<TransactionBehavior<TestRequest, TestResponse>>>();
        
        var behavior = new TransactionBehavior<TestRequest, TestResponse>(
            mockDbContext,
            mockIntegrationEventService.Object,
            mockLogger.Object);
        
        var request = new TestRequest();
        var expectedResponse = new TestResponse { Success = true, Message = "Test Result" };
        
        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(expectedResponse);

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponse.Success, result.Success);
        Assert.Equal(expectedResponse.Message, result.Message);
    }

    [Fact]
    public async Task Handle_PublishesIntegrationEvents()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<OrderingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var mockDbContext = new OrderingContext(options, Mock.Of<IMediator>());
        var mockIntegrationEventService = new Mock<IOrderingIntegrationEventService>();
        mockIntegrationEventService.Setup(x => x.PublishEventsThroughEventBusAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        var mockLogger = new Mock<ILogger<TransactionBehavior<TestRequest, TestResponse>>>();
        
        var behavior = new TransactionBehavior<TestRequest, TestResponse>(
            mockDbContext,
            mockIntegrationEventService.Object,
            mockLogger.Object);
        
        var request = new TestRequest();
        var expectedResponse = new TestResponse { Success = true };
        
        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(expectedResponse);

        // Act
        await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        mockIntegrationEventService.Verify(
            x => x.PublishEventsThroughEventBusAsync(It.IsAny<Guid>()), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenExceptionOccurs_LogsErrorAndRethrows()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<OrderingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var mockDbContext = new OrderingContext(options, Mock.Of<IMediator>());
        var mockIntegrationEventService = new Mock<IOrderingIntegrationEventService>();
        var mockLogger = new Mock<ILogger<TransactionBehavior<TestRequest, TestResponse>>>();
        
        var behavior = new TransactionBehavior<TestRequest, TestResponse>(
            mockDbContext,
            mockIntegrationEventService.Object,
            mockLogger.Object);
        
        var request = new TestRequest();
        var expectedException = new InvalidOperationException("Test exception");
        
        RequestHandlerDelegate<TestResponse> next = () => throw expectedException;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await behavior.Handle(request, next, CancellationToken.None));
        
        Assert.Equal(expectedException, exception);
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }
}

public class TestRequest : IRequest<TestResponse>
{
}

public class TestResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}
