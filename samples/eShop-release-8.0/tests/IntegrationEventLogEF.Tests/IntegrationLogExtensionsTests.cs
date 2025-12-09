using eShop.EventBus.Events;
using eShop.IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;

namespace IntegrationEventLogEF.Tests;

public class IntegrationLogExtensionsTests
{
    private record TestIntegrationEvent : IntegrationEvent
    {
        public string TestProperty { get; set; } = "TestValue";
    }

    private class TestDbContext : DbContext
    {
        public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseIntegrationEventLogs();
        }
    }

    [Fact]
    public void UseIntegrationEventLogs_ConfiguresEntityMapping()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();

        // Act
        modelBuilder.UseIntegrationEventLogs();

        // Assert
        var model = modelBuilder.Model;
        var entityType = model.FindEntityType(typeof(IntegrationEventLogEntry));
        Assert.NotNull(entityType);
    }

    [Fact]
    public void UseIntegrationEventLogs_SetsTableName()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();

        // Act
        modelBuilder.UseIntegrationEventLogs();

        // Assert
        var model = modelBuilder.Model;
        var entityType = model.FindEntityType(typeof(IntegrationEventLogEntry));
        Assert.NotNull(entityType);
        Assert.Equal("IntegrationEventLog", entityType.GetTableName());
    }

    [Fact]
    public void UseIntegrationEventLogs_ConfiguresEventIdAsKey()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();

        // Act
        modelBuilder.UseIntegrationEventLogs();

        // Assert
        var model = modelBuilder.Model;
        var entityType = model.FindEntityType(typeof(IntegrationEventLogEntry));
        var primaryKey = entityType?.FindPrimaryKey();
        
        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal(nameof(IntegrationEventLogEntry.EventId), primaryKey.Properties[0].Name);
    }

    [Fact]
    public void UseIntegrationEventLogs_EventIdPrimaryKey_IsOfGuidType()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();

        // Act
        modelBuilder.UseIntegrationEventLogs();

        // Assert
        var model = modelBuilder.Model;
        var entityType = model.FindEntityType(typeof(IntegrationEventLogEntry));
        var primaryKey = entityType?.FindPrimaryKey();
        
        Assert.NotNull(primaryKey);
        Assert.Equal(typeof(Guid), primaryKey.Properties[0].ClrType);
    }

    [Fact]
    public void UseIntegrationEventLogs_CanBeCalledMultipleTimes()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();

        // Act
        modelBuilder.UseIntegrationEventLogs();
        modelBuilder.UseIntegrationEventLogs();

        // Assert
        var model = modelBuilder.Model;
        var entityType = model.FindEntityType(typeof(IntegrationEventLogEntry));
        Assert.NotNull(entityType);
        Assert.Equal("IntegrationEventLog", entityType.GetTableName());
    }

    [Fact]
    public void UseIntegrationEventLogs_ConfiguresEntity_WithoutThrowingException()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();

        // Act & Assert
        var exception = Record.Exception(() =>
        {
            modelBuilder.UseIntegrationEventLogs();
        });
        Assert.Null(exception);
    }

    [Fact]
    public void UseIntegrationEventLogs_WithValidModelBuilder_CreatesValidEntityConfiguration()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var options = optionsBuilder.Options;

        // Act
        using (var context = new TestDbContext(options))
        {
            var model = context.Model;
            var entityType = model.FindEntityType(typeof(IntegrationEventLogEntry));

            // Assert
            Assert.NotNull(entityType);
            Assert.Equal("IntegrationEventLog", entityType.GetTableName());
        }
    }

    [Fact]
    public void UseIntegrationEventLogs_PrimaryKeyConstraint_PreventsAddingDuplicateEventIds()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var options = optionsBuilder.Options;

        var testEvent = new TestIntegrationEvent();
        var transactionId = Guid.NewGuid();
        var entry1 = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act & Assert
        using (var context = new TestDbContext(options))
        {
            context.IntegrationEventLogs.Add(entry1);
            context.SaveChanges();

            // Verify we can't add another entry with the same EventId
            var entry2 = new IntegrationEventLogEntry(testEvent, transactionId);
            
            var exception = Record.Exception(() =>
            {
                context.IntegrationEventLogs.Add(entry2);
            });
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
    }

    [Fact]
    public void UseIntegrationEventLogs_AllowsDifferentEventIds()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var options = optionsBuilder.Options;

        var testEvent1 = new TestIntegrationEvent();
        var testEvent2 = new TestIntegrationEvent();
        var transactionId = Guid.NewGuid();
        var entry1 = new IntegrationEventLogEntry(testEvent1, transactionId);
        var entry2 = new IntegrationEventLogEntry(testEvent2, transactionId);

        // Act & Assert
        using (var context = new TestDbContext(options))
        {
            context.IntegrationEventLogs.Add(entry1);
            context.IntegrationEventLogs.Add(entry2);

            var exception = Record.Exception(() => context.SaveChanges());
            Assert.Null(exception);
        }
    }

    [Fact]
    public void UseIntegrationEventLogs_ConfiguredEntity_CanBeQueried()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var options = optionsBuilder.Options;

        var testEvent = new TestIntegrationEvent();
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act
        using (var context = new TestDbContext(options))
        {
            context.IntegrationEventLogs.Add(entry);
            context.SaveChanges();
        }

        // Assert
        using (var context = new TestDbContext(options))
        {
            var retrievedEntry = context.IntegrationEventLogs.Find(entry.EventId);
            Assert.NotNull(retrievedEntry);
            Assert.Equal(entry.EventId, retrievedEntry.EventId);
            Assert.Equal(entry.TransactionId, retrievedEntry.TransactionId);
        }
    }

    [Fact]
    public void UseIntegrationEventLogs_ConfiguredEntity_SupportsTracking()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var options = optionsBuilder.Options;

        var testEvent = new TestIntegrationEvent();
        var transactionId = Guid.NewGuid();
        var entry = new IntegrationEventLogEntry(testEvent, transactionId);

        // Act
        using (var context = new TestDbContext(options))
        {
            context.IntegrationEventLogs.Add(entry);
            context.SaveChanges();
        }

        // Update the entry
        using (var context = new TestDbContext(options))
        {
            var retrievedEntry = context.IntegrationEventLogs.Single(e => e.EventId == entry.EventId);
            retrievedEntry.State = EventStateEnum.Published;
            context.SaveChanges();
        }

        // Assert
        using (var context = new TestDbContext(options))
        {
            var updatedEntry = context.IntegrationEventLogs.Find(entry.EventId);
            Assert.NotNull(updatedEntry);
            Assert.Equal(EventStateEnum.Published, updatedEntry.State);
        }
    }

    [Fact]
    public void UseIntegrationEventLogs_TableName_IsNotChangedBetweenCalls()
    {
        // Arrange
        var modelBuilder1 = new ModelBuilder();
        var modelBuilder2 = new ModelBuilder();

        // Act
        modelBuilder1.UseIntegrationEventLogs();
        modelBuilder2.UseIntegrationEventLogs();

        // Assert
        var tableName1 = modelBuilder1.Model.FindEntityType(typeof(IntegrationEventLogEntry))?.GetTableName();
        var tableName2 = modelBuilder2.Model.FindEntityType(typeof(IntegrationEventLogEntry))?.GetTableName();

        Assert.Equal("IntegrationEventLog", tableName1);
        Assert.Equal("IntegrationEventLog", tableName2);
        Assert.Equal(tableName1, tableName2);
    }

    [Fact]
    public void UseIntegrationEventLogs_IsExtensionMethod()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();

        // Act
        modelBuilder.UseIntegrationEventLogs();

        // Assert - Extension method returns void, but this verifies it's callable
        Assert.NotNull(modelBuilder);
        Assert.NotNull(modelBuilder.Model);
    }

    [Fact]
    public void UseIntegrationEventLogs_EntityConfiguration_IsConsistent()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();
        modelBuilder.UseIntegrationEventLogs();
        var firstModel = modelBuilder.Model.FindEntityType(typeof(IntegrationEventLogEntry));

        var modelBuilder2 = new ModelBuilder();
        modelBuilder2.UseIntegrationEventLogs();
        var secondModel = modelBuilder2.Model.FindEntityType(typeof(IntegrationEventLogEntry));

        // Assert
        Assert.NotNull(firstModel);
        Assert.NotNull(secondModel);
        Assert.Equal(firstModel.GetTableName(), secondModel.GetTableName());
        Assert.Equal(firstModel.FindPrimaryKey()?.Properties.Count, secondModel.FindPrimaryKey()?.Properties.Count);
    }

    [Fact]
    public void UseIntegrationEventLogs_DoesNotAffectOtherEntities()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();
        modelBuilder.Entity<AnotherEntity>();

        // Act
        modelBuilder.UseIntegrationEventLogs();

        // Assert
        var integrationEventLogType = modelBuilder.Model.FindEntityType(typeof(IntegrationEventLogEntry));
        var anotherEntityType = modelBuilder.Model.FindEntityType(typeof(AnotherEntity));

        Assert.NotNull(integrationEventLogType);
        Assert.NotNull(anotherEntityType);
        Assert.Equal("IntegrationEventLog", integrationEventLogType.GetTableName());
    }

    // Helper class for testing that other entities are not affected
    private class AnotherEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
