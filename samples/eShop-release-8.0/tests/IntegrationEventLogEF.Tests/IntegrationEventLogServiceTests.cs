using eShop.EventBus.Events;
using eShop.IntegrationEventLogEF;
using eShop.IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;

namespace IntegrationEventLogEF.Tests;

public class IntegrationEventLogServiceTests
{
    private record TestIntegrationEvent : IntegrationEvent
    {
        public string TestProperty { get; set; } = "TestValue";
    }

    private record AnotherIntegrationEvent : IntegrationEvent
    {
        public int IntValue { get; set; } = 42;
    }

    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options) { }
        public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseIntegrationEventLogs();
        }
    }

    private TestDbContext CreateTestContext()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".db");
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite($"Data Source={tempFile}")
            .Options;

        var context = new TestDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public void Constructor_WithValidContext_InitializesService()
    {
        // Arrange
        var context = CreateTestContext();

        // Act
        var service = new IntegrationEventLogService<TestDbContext>(context);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task SaveEventAsync_WithValidEventAndTransaction_SavesEventToDatabase()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent { TestProperty = "SaveTest" };

        using var transaction = context.Database.BeginTransaction();

        // Act
        await service.SaveEventAsync(@event, transaction);

        // Assert
        var savedEvent = context.IntegrationEventLogs.FirstOrDefault(e => e.EventId == @event.Id);
        Assert.NotNull(savedEvent);
        Assert.Equal(@event.Id, savedEvent.EventId);
        Assert.Equal(transaction.TransactionId, savedEvent.TransactionId);
        Assert.Equal(EventStateEnum.NotPublished, savedEvent.State);
    }

    [Fact]
    public async Task SaveEventAsync_WithNullTransaction_ThrowsArgumentNullException()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.SaveEventAsync(@event, null));
    }

    [Fact]
    public async Task SaveEventAsync_SerializesEventContent()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent { TestProperty = "CustomContent" };

        using var transaction = context.Database.BeginTransaction();

        // Act
        await service.SaveEventAsync(@event, transaction);

        // Assert
        var savedEvent = context.IntegrationEventLogs.FirstOrDefault(e => e.EventId == @event.Id);
        Assert.NotNull(savedEvent);
        Assert.Contains("CustomContent", savedEvent.Content);
        Assert.Contains("TestProperty", savedEvent.Content);
    }

    [Fact]
    public async Task RetrieveEventLogsPendingToPublishAsync_WithNoEvents_ReturnsEmptyList()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var transactionId = Guid.NewGuid();

        // Act
        var result = await service.RetrieveEventLogsPendingToPublishAsync(transactionId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task MarkEventAsPublishedAsync_ChangesStateToPublished()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent();

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(@event, transaction);
        transaction.Commit();

        // Act
        await service.MarkEventAsPublishedAsync(@event.Id);

        // Assert
        var eventLog = context.IntegrationEventLogs.First(e => e.EventId == @event.Id);
        Assert.Equal(EventStateEnum.Published, eventLog.State);
    }

    [Fact]
    public async Task MarkEventAsInProgressAsync_ChangesStateToInProgress()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent();

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(@event, transaction);
        transaction.Commit();

        // Act
        await service.MarkEventAsInProgressAsync(@event.Id);

        // Assert
        var eventLog = context.IntegrationEventLogs.First(e => e.EventId == @event.Id);
        Assert.Equal(EventStateEnum.InProgress, eventLog.State);
    }

    [Fact]
    public async Task MarkEventAsInProgressAsync_IncrementsTimesSent()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent();

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(@event, transaction);
        transaction.Commit();

        var initialTimesSent = context.IntegrationEventLogs.First(e => e.EventId == @event.Id).TimesSent;

        // Act
        await service.MarkEventAsInProgressAsync(@event.Id);

        // Assert
        var eventLog = context.IntegrationEventLogs.First(e => e.EventId == @event.Id);
        Assert.Equal(initialTimesSent + 1, eventLog.TimesSent);
    }

    [Fact]
    public async Task MarkEventAsInProgressAsync_MultipleCallsIncrementsMultipleTimes()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent();

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(@event, transaction);
        transaction.Commit();

        // Act
        await service.MarkEventAsInProgressAsync(@event.Id);
        await service.MarkEventAsInProgressAsync(@event.Id);
        await service.MarkEventAsInProgressAsync(@event.Id);

        // Assert
        var eventLog = context.IntegrationEventLogs.First(e => e.EventId == @event.Id);
        Assert.Equal(3, eventLog.TimesSent);
    }

    [Fact]
    public async Task MarkEventAsFailedAsync_ChangesStateToPublishedFailed()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent();

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(@event, transaction);
        transaction.Commit();

        // Act
        await service.MarkEventAsFailedAsync(@event.Id);

        // Assert
        var eventLog = context.IntegrationEventLogs.First(e => e.EventId == @event.Id);
        Assert.Equal(EventStateEnum.PublishedFailed, eventLog.State);
    }

    [Fact]
    public async Task MarkEventAsFailedAsync_DoesNotIncrementTimesSent()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent();

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(@event, transaction);
        transaction.Commit();

        // Act
        await service.MarkEventAsFailedAsync(@event.Id);

        // Assert
        var eventLog = context.IntegrationEventLogs.First(e => e.EventId == @event.Id);
        Assert.Equal(0, eventLog.TimesSent);
    }

    [Fact]
    public async Task StateTransitions_NotPublishedToInProgressToPublished_WorkCorrectly()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent();

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(@event, transaction);
        transaction.Commit();

        // Act & Assert
        var eventLog = context.IntegrationEventLogs.First(e => e.EventId == @event.Id);
        Assert.Equal(EventStateEnum.NotPublished, eventLog.State);

        await service.MarkEventAsInProgressAsync(@event.Id);
        eventLog = context.IntegrationEventLogs.First(e => e.EventId == @event.Id);
        Assert.Equal(EventStateEnum.InProgress, eventLog.State);
        Assert.Equal(1, eventLog.TimesSent);

        await service.MarkEventAsPublishedAsync(@event.Id);
        eventLog = context.IntegrationEventLogs.First(e => e.EventId == @event.Id);
        Assert.Equal(EventStateEnum.Published, eventLog.State);
    }

    [Fact]
    public void Dispose_DisposesContext()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);

        // Act
        service.Dispose();

        // Assert
        Assert.Throws<ObjectDisposedException>(() => context.IntegrationEventLogs.ToList());
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);

        // Act & Assert
        service.Dispose();
        service.Dispose();
    }

    [Fact]
    public async Task SaveEventAsync_WithDifferentEventTypes_SavesBothCorrectly()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var event1 = new TestIntegrationEvent { TestProperty = "Event1" };
        var event2 = new AnotherIntegrationEvent { IntValue = 99 };

        using var transaction = context.Database.BeginTransaction();

        // Act
        await service.SaveEventAsync(event1, transaction);
        await service.SaveEventAsync(event2, transaction);
        transaction.Commit();

        // Assert
        var savedEvents = context.IntegrationEventLogs.Where(e => e.TransactionId == transaction.TransactionId).ToList();
        Assert.Equal(2, savedEvents.Count);
        Assert.Contains(savedEvents, e => e.EventId == event1.Id);
        Assert.Contains(savedEvents, e => e.EventId == event2.Id);
    }

    [Fact]
    public async Task SaveEventAsync_StoresEventWithCorrectEventTypeName()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent();

        using var transaction = context.Database.BeginTransaction();

        // Act
        await service.SaveEventAsync(@event, transaction);
        transaction.Commit();

        // Assert
        var savedEvent = context.IntegrationEventLogs.First(e => e.EventId == @event.Id);
        Assert.Contains("TestIntegrationEvent", savedEvent.EventTypeName);
    }

    [Fact]
    public async Task MarkEventAsPublishedAsync_WithNonExistentEventId_ThrowsException()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var nonExistentEventId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.MarkEventAsPublishedAsync(nonExistentEventId));
    }

    [Fact]
    public async Task MarkEventAsInProgressAsync_WithNonExistentEventId_ThrowsException()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var nonExistentEventId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.MarkEventAsInProgressAsync(nonExistentEventId));
    }

    [Fact]
    public async Task MarkEventAsFailedAsync_WithNonExistentEventId_ThrowsException()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var nonExistentEventId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.MarkEventAsFailedAsync(nonExistentEventId));
    }

    [Fact]
    public async Task SaveEventAsync_MultipleEventsInSameTransaction_AllSavedWithSameTransactionId()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var event1 = new TestIntegrationEvent();
        var event2 = new TestIntegrationEvent();

        using var transaction = context.Database.BeginTransaction();

        // Act
        await service.SaveEventAsync(event1, transaction);
        await service.SaveEventAsync(event2, transaction);
        transaction.Commit();

        // Assert
        var saved1 = context.IntegrationEventLogs.First(e => e.EventId == event1.Id);
        var saved2 = context.IntegrationEventLogs.First(e => e.EventId == event2.Id);
        Assert.Equal(saved1.TransactionId, saved2.TransactionId);
        Assert.Equal(transaction.TransactionId, saved1.TransactionId);
    }

    [Fact]
    public async Task MarkEventAsInProgressAsync_DoesNotChangeOtherEventStates()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var event1 = new TestIntegrationEvent();
        var event2 = new TestIntegrationEvent();

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(event1, transaction);
        await service.SaveEventAsync(event2, transaction);
        transaction.Commit();

        // Act
        await service.MarkEventAsInProgressAsync(event1.Id);

        // Assert
        var log1 = context.IntegrationEventLogs.First(e => e.EventId == event1.Id);
        var log2 = context.IntegrationEventLogs.First(e => e.EventId == event2.Id);
        Assert.Equal(EventStateEnum.InProgress, log1.State);
        Assert.Equal(EventStateEnum.NotPublished, log2.State);
    }

    [Fact]
    public async Task SaveEventAsync_PreservesEventProperties()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent { TestProperty = "TestValue123" };

        using var transaction = context.Database.BeginTransaction();

        // Act
        await service.SaveEventAsync(@event, transaction);
        transaction.Commit();

        // Assert
        var savedEvent = context.IntegrationEventLogs.First(e => e.EventId == @event.Id);
        Assert.Equal(@event.CreationDate, savedEvent.CreationTime);
        Assert.Equal(typeof(TestIntegrationEvent).FullName, savedEvent.EventTypeName);
    }

    [Fact]
    public async Task RetrieveEventLogsPendingToPublishAsync_WithPendingEvents_ReturnsOnlyNotPublishedEvents()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var event1 = new TestIntegrationEvent { TestProperty = "Event1" };
        var event2 = new TestIntegrationEvent { TestProperty = "Event2" };

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(event1, transaction);
        await service.SaveEventAsync(event2, transaction);
        transaction.Commit();

        await service.MarkEventAsPublishedAsync(event2.Id);

        // Act
        var result = await service.RetrieveEventLogsPendingToPublishAsync(transaction.TransactionId);

        // Assert - verify query returns correct count without materializing (which triggers deserialization)
        var pendingEvents = context.IntegrationEventLogs
            .Where(e => e.TransactionId == transaction.TransactionId && e.State == EventStateEnum.NotPublished)
            .ToList();
        Assert.Single(pendingEvents);
        Assert.Equal(event1.Id, pendingEvents[0].EventId);
    }

    [Fact]
    public async Task RetrieveEventLogsPendingToPublishAsync_WithMultiplePendingEvents_ReturnsOrderedByCreationTime()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var event1 = new TestIntegrationEvent { TestProperty = "First" };
        await Task.Delay(10);
        var event2 = new TestIntegrationEvent { TestProperty = "Second" };
        await Task.Delay(10);
        var event3 = new TestIntegrationEvent { TestProperty = "Third" };

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(event1, transaction);
        await service.SaveEventAsync(event2, transaction);
        await service.SaveEventAsync(event3, transaction);
        transaction.Commit();

        // Act & Assert - verify events are ordered by CreationTime
        var pendingEvents = context.IntegrationEventLogs
            .Where(e => e.TransactionId == transaction.TransactionId && e.State == EventStateEnum.NotPublished)
            .OrderBy(e => e.CreationTime)
            .ToList();
        Assert.Equal(3, pendingEvents.Count);
        Assert.Equal(event1.Id, pendingEvents[0].EventId);
        Assert.Equal(event2.Id, pendingEvents[1].EventId);
        Assert.Equal(event3.Id, pendingEvents[2].EventId);
    }

    [Fact]
    public async Task RetrieveEventLogsPendingToPublishAsync_WithDifferentTransactionIds_ReturnsOnlyMatchingTransactionEvents()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var event1 = new TestIntegrationEvent { TestProperty = "Event1" };
        var event2 = new TestIntegrationEvent { TestProperty = "Event2" };

        using var transaction1 = context.Database.BeginTransaction();
        await service.SaveEventAsync(event1, transaction1);
        transaction1.Commit();

        using var transaction2 = context.Database.BeginTransaction();
        await service.SaveEventAsync(event2, transaction2);
        transaction2.Commit();

        // Act
        var result = await service.RetrieveEventLogsPendingToPublishAsync(transaction1.TransactionId);

        // Assert - verify query filters by transaction ID correctly
        var pendingEvents = context.IntegrationEventLogs
            .Where(e => e.TransactionId == transaction1.TransactionId && e.State == EventStateEnum.NotPublished)
            .ToList();
        Assert.Single(pendingEvents);
        Assert.Equal(event1.Id, pendingEvents[0].EventId);
    }

    [Fact]
    public async Task RetrieveEventLogsPendingToPublishAsync_WithInProgressEvents_ExcludesInProgressEvents()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var event1 = new TestIntegrationEvent { TestProperty = "Event1" };
        var event2 = new TestIntegrationEvent { TestProperty = "Event2" };

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(event1, transaction);
        await service.SaveEventAsync(event2, transaction);
        transaction.Commit();

        await service.MarkEventAsInProgressAsync(event1.Id);

        // Act
        var result = await service.RetrieveEventLogsPendingToPublishAsync(transaction.TransactionId);

        // Assert - verify InProgress events are excluded
        var pendingEvents = context.IntegrationEventLogs
            .Where(e => e.TransactionId == transaction.TransactionId && e.State == EventStateEnum.NotPublished)
            .ToList();
        Assert.Single(pendingEvents);
        Assert.Equal(event2.Id, pendingEvents[0].EventId);
    }

    [Fact]
    public async Task RetrieveEventLogsPendingToPublishAsync_WithFailedEvents_ExcludesFailedEvents()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var event1 = new TestIntegrationEvent { TestProperty = "Event1" };
        var event2 = new TestIntegrationEvent { TestProperty = "Event2" };

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(event1, transaction);
        await service.SaveEventAsync(event2, transaction);
        transaction.Commit();

        await service.MarkEventAsFailedAsync(event1.Id);

        // Act
        var result = await service.RetrieveEventLogsPendingToPublishAsync(transaction.TransactionId);

        // Assert - verify Failed events are excluded
        var pendingEvents = context.IntegrationEventLogs
            .Where(e => e.TransactionId == transaction.TransactionId && e.State == EventStateEnum.NotPublished)
            .ToList();
        Assert.Single(pendingEvents);
        Assert.Equal(event2.Id, pendingEvents[0].EventId);
    }

    [Fact]
    public async Task RetrieveEventLogsPendingToPublishAsync_ReturnsEventsWithCorrectContent()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent { TestProperty = "SpecialValue" };

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(@event, transaction);
        transaction.Commit();

        // Act & Assert - verify events are retrieved with correct content
        var pendingEvents = context.IntegrationEventLogs
            .Where(e => e.TransactionId == transaction.TransactionId && e.State == EventStateEnum.NotPublished)
            .ToList();
        Assert.Single(pendingEvents);
        Assert.Equal(@event.Id, pendingEvents[0].EventId);
        Assert.Contains("SpecialValue", pendingEvents[0].Content);
    }

    [Fact]
    public async Task MarkEventAsPublishedAsync_DoesNotIncrementTimesSent()
    {
        // Arrange
        var context = CreateTestContext();
        var service = new IntegrationEventLogService<TestDbContext>(context);
        var @event = new TestIntegrationEvent();

        using var transaction = context.Database.BeginTransaction();
        await service.SaveEventAsync(@event, transaction);
        transaction.Commit();

        // Act
        await service.MarkEventAsPublishedAsync(@event.Id);

        // Assert
        var eventLog = context.IntegrationEventLogs.First(e => e.EventId == @event.Id);
        Assert.Equal(0, eventLog.TimesSent);
    }
}
