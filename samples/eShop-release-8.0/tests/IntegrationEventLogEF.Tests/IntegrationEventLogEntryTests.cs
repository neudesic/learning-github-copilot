using eShop.EventBus.Events;
using eShop.IntegrationEventLogEF;
using System.Text.Json;

namespace IntegrationEventLogEF.Tests;

public class IntegrationEventLogEntryTests
{
    private record TestEvent : IntegrationEvent
    {
        public string TestProperty { get; set; } = "TestValue";
    }

    private record AnotherTestEvent : IntegrationEvent
    {
        public int IntValue { get; set; } = 42;
        public string StringValue { get; set; } = "AnotherValue";
    }

    [Fact]
    public void Constructor_WithValidEventAndTransactionId_InitializesPropertiesCorrectly()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId = Guid.NewGuid();

        // Act
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Assert
        Assert.Equal(testEvent.Id, entry.EventId);
        Assert.Equal(testEvent.CreationDate, entry.CreationTime);
        Assert.Equal(typeof(TestEvent).FullName, entry.EventTypeName);
        Assert.Equal(EventStateEnum.NotPublished, entry.State);
        Assert.Equal(0, entry.TimesSent);
        Assert.Equal(transactionId, entry.TransactionId);
        Assert.NotNull(entry.Content);
        Assert.NotEmpty(entry.Content);
    }

    [Fact]
    public void Constructor_SerializesEventContentCorrectly()
    {
        // Arrange
        var testEvent = new TestEvent { TestProperty = "CustomValue" };
        var transactionId = Guid.NewGuid();

        // Act
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Assert
        Assert.Contains("TestProperty", entry.Content);
        Assert.Contains("CustomValue", entry.Content);
        Assert.Contains(testEvent.Id.ToString(), entry.Content);
    }

    [Fact]
    public void Constructor_WithDifferentEvents_SerializesDifferentContent()
    {
        // Arrange
        var testEvent1 = new TestEvent();
        var testEvent2 = new AnotherTestEvent();
        var transactionId = Guid.NewGuid();

        // Act
        var entry1 = new IntegrationEventLogEntry(testEvent1, transactionId);
        var entry2 = new IntegrationEventLogEntry(testEvent2, transactionId);

        // Assert
        Assert.NotEqual(entry1.Content, entry2.Content);
        Assert.Contains("TestProperty", entry1.Content);
        Assert.Contains("IntValue", entry2.Content);
    }

    [Fact]
    public void EventTypeShortName_ReturnsLastPartOfFullName()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act
        var shortName = entry.EventTypeShortName;

        // Assert
        Assert.EndsWith("TestEvent", shortName);
    }

    [Fact]
    public void EventTypeShortName_WithComplexNamespace_ReturnsOnlyClassName()
    {
        // Arrange
        var testEvent = new AnotherTestEvent();
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act
        var shortName = entry.EventTypeShortName;

        // Assert
        Assert.EndsWith("AnotherTestEvent", shortName);
    }

    [Fact]
    public void DeserializeJsonContent_WithValidJson_PopulatesIntegrationEvent()
    {
        // Arrange
        var testEvent = new TestEvent { TestProperty = "DeserializeTest" };
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act
        var result = entry.DeserializeJsonContent(typeof(TestEvent));
        var deserializedEvent = result.IntegrationEvent as TestEvent;

        // Assert
        Assert.NotNull(result);
        Assert.Same(result, entry); // Verify fluent return
        Assert.NotNull(deserializedEvent);
        Assert.Equal(testEvent.Id, deserializedEvent.Id);
        Assert.Equal("DeserializeTest", deserializedEvent.TestProperty);
    }

    [Fact]
    public void DeserializeJsonContent_WithAnotherEvent_DeserializesCorrectly()
    {
        // Arrange
        var testEvent = new AnotherTestEvent { IntValue = 99, StringValue = "Test" };
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act
        var result = entry.DeserializeJsonContent(typeof(AnotherTestEvent));
        var deserializedEvent = result.IntegrationEvent as AnotherTestEvent;

        // Assert
        Assert.NotNull(deserializedEvent);
        Assert.Equal(99, deserializedEvent.IntValue);
        Assert.Equal("Test", deserializedEvent.StringValue);
    }

    [Fact]
    public void DeserializeJsonContent_SupportsFluentChaining()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act
        var result = entry
            .DeserializeJsonContent(typeof(TestEvent))
            .DeserializeJsonContent(typeof(TestEvent));

        // Assert
        Assert.NotNull(result.IntegrationEvent);
    }

    [Fact]
    public void State_CanBeChangedAfterConstruction()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act
        entry.State = EventStateEnum.InProgress;

        // Assert
        Assert.Equal(EventStateEnum.InProgress, entry.State);
    }

    [Fact]
    public void State_CanTransitionThroughAllStates()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act & Assert
        Assert.Equal(EventStateEnum.NotPublished, entry.State);

        entry.State = EventStateEnum.InProgress;
        Assert.Equal(EventStateEnum.InProgress, entry.State);

        entry.State = EventStateEnum.Published;
        Assert.Equal(EventStateEnum.Published, entry.State);

        entry.State = EventStateEnum.PublishedFailed;
        Assert.Equal(EventStateEnum.PublishedFailed, entry.State);
    }

    [Fact]
    public void TimesSent_CanBeIncremented()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act
        entry.TimesSent = 1;
        entry.TimesSent += 1;
        entry.TimesSent += 1;

        // Assert
        Assert.Equal(3, entry.TimesSent);
    }

    [Fact]
    public void Constructor_WithDifferentTransactionIds_StoresCorrectTransactionId()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId1 = Guid.NewGuid();
        var transactionId2 = Guid.NewGuid();

        // Act
        var entry1 = new IntegrationEventLogEntry(testEvent, transactionId1);
        var entry2 = new IntegrationEventLogEntry(testEvent, transactionId2);

        // Assert
        Assert.Equal(transactionId1, entry1.TransactionId);
        Assert.Equal(transactionId2, entry2.TransactionId);
        Assert.NotEqual(entry1.TransactionId, entry2.TransactionId);
    }

    [Fact]
    public void Constructor_PreservesEventCreationDate()
    {
        // Arrange
        var testEvent = new TestEvent();
        var expectedCreationDate = testEvent.CreationDate;
        var transactionId = Guid.NewGuid();

        // Act
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Assert
        Assert.Equal(expectedCreationDate, entry.CreationTime);
    }

    [Fact]
    public void DeserializeJsonContent_WithCaseInsensitiveJson_DeserializesSuccessfully()
    {
        // Arrange
        var testEvent = new TestEvent { TestProperty = "CaseTest" };
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act
        var result = entry.DeserializeJsonContent(typeof(TestEvent));

        // Assert
        Assert.NotNull(result.IntegrationEvent);
        var deserializedEvent = result.IntegrationEvent as TestEvent;
        Assert.NotNull(deserializedEvent);
    }

    [Fact]
    public void Content_IsNotEmptyAfterConstruction()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId = Guid.NewGuid();

        // Act
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Assert
        Assert.NotNull(entry.Content);
        Assert.NotEmpty(entry.Content);
        Assert.True(entry.Content.Length > 0);
    }

    [Fact]
    public void Content_IsValidJson()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act & Assert
        var exception = Record.Exception(() =>
        {
            JsonSerializer.Deserialize<TestEvent>(entry.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        });
        Assert.Null(exception);
    }

    [Fact]
    public void EventTypeShortName_WithEmptyFullName_HandlesEdgeCase()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act
        var shortName = entry.EventTypeShortName;

        // Assert
        Assert.NotNull(shortName);
        Assert.NotEmpty(shortName);
    }

    [Fact]
    public void EventId_IsSetFromEventIdInConstructor()
    {
        // Arrange
        var testEvent = new TestEvent();
        var expectedEventId = testEvent.Id;
        var transactionId = Guid.NewGuid();

        // Act
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Assert
        Assert.Equal(expectedEventId, entry.EventId);
    }

    [Fact]
    public void MultipleEntries_WithSameEvent_HaveSameEventId()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId1 = Guid.NewGuid();
        var transactionId2 = Guid.NewGuid();

        // Act
        var entry1 = new IntegrationEventLogEntry(testEvent, transactionId1);
        var entry2 = new IntegrationEventLogEntry(testEvent, transactionId2);

        // Assert
        Assert.Equal(entry1.EventId, entry2.EventId);
    }

    [Fact]
    public void EventTypeName_IsRequired_Property()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId = Guid.NewGuid();

        // Act
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Assert
        Assert.NotNull(entry.EventTypeName);
        Assert.NotEmpty(entry.EventTypeName);
        Assert.Equal(typeof(TestEvent).FullName, entry.EventTypeName);
    }

    [Fact]
    public void DeserializeJsonContent_ReturnsSameInstance()
    {
        // Arrange
        var testEvent = new TestEvent();
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act
        var result = entry.DeserializeJsonContent(typeof(TestEvent));

        // Assert
        Assert.Same(entry, result);
    }
}
