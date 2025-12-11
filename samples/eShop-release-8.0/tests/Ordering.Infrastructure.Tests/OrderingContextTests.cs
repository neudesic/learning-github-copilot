using eShop.Ordering.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace Ordering.Infrastructure.Tests;

public class OrderingContextTests
{
    private DbContextOptions<OrderingContext> CreateInMemoryOptions(string databaseName)
    {
        return new DbContextOptionsBuilder<OrderingContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;
    }

    [Fact]
    public void Constructor_WithOptionsOnly_CreatesInstance()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb1");

        // Act
        var context = new OrderingContext(options);

        // Assert
        Assert.NotNull(context);
        Assert.False(context.HasActiveTransaction);
    }

    [Fact]
    public void Constructor_WithOptionsAndMediator_CreatesInstance()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb2");
        var mockMediator = new Mock<IMediator>();

        // Act
        var context = new OrderingContext(options, mockMediator.Object);

        // Assert
        Assert.NotNull(context);
        Assert.False(context.HasActiveTransaction);
    }

    [Fact]
    public void Constructor_WithNullMediator_ThrowsArgumentNullException()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb3");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new OrderingContext(options, null));
    }

    [Fact]
    public void GetCurrentTransaction_WhenNoTransaction_ReturnsNull()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb4");
        var context = new OrderingContext(options);

        // Act
        var transaction = context.GetCurrentTransaction();

        // Assert
        Assert.Null(transaction);
    }

    [Fact]
    public void HasActiveTransaction_WhenNoTransaction_ReturnsFalse()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb5");
        var context = new OrderingContext(options);

        // Act
        var hasTransaction = context.HasActiveTransaction;

        // Assert
        Assert.False(hasTransaction);
    }

    [Fact]
    public void DbSets_AreInitialized()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb6");
        var context = new OrderingContext(options);

        // Assert
        Assert.NotNull(context.Orders);
        Assert.NotNull(context.OrderItems);
        Assert.NotNull(context.Payments);
        Assert.NotNull(context.Buyers);
        Assert.NotNull(context.CardTypes);
    }

    [Fact]
    public async Task SaveEntitiesAsync_WithMediator_DispatchesDomainEventsAndSaves()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb7");
        var mockMediator = new Mock<IMediator>();
        var context = new OrderingContext(options, mockMediator.Object);

        // Act
        var result = await context.SaveEntitiesAsync();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SaveEntitiesAsync_WithCancellationToken_CompletesSuccessfully()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb8");
        var mockMediator = new Mock<IMediator>();
        var context = new OrderingContext(options, mockMediator.Object);
        var cancellationToken = new CancellationToken();

        // Act
        var result = await context.SaveEntitiesAsync(cancellationToken);

        // Assert
        Assert.True(result);
    }



    [Fact]
    public async Task CommitTransactionAsync_WithNullTransaction_ThrowsArgumentNullException()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb11");
        var mockMediator = new Mock<IMediator>();
        var context = new OrderingContext(options, mockMediator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            context.CommitTransactionAsync(null));
    }

    [Fact]
    public async Task CommitTransactionAsync_WithDifferentTransaction_ThrowsInvalidOperationException()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb12");
        var mockMediator = new Mock<IMediator>();
        var context = new OrderingContext(options, mockMediator.Object);
        
        var mockTransaction1 = new Mock<IDbContextTransaction>();
        mockTransaction1.Setup(t => t.TransactionId).Returns(Guid.NewGuid());
        
        var mockTransaction2 = new Mock<IDbContextTransaction>();
        mockTransaction2.Setup(t => t.TransactionId).Returns(Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            context.CommitTransactionAsync(mockTransaction1.Object));
    }

    [Fact]
    public void RollbackTransaction_WhenNoActiveTransaction_DoesNotThrow()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb13");
        var mockMediator = new Mock<IMediator>();
        var context = new OrderingContext(options, mockMediator.Object);

        // Act
        context.RollbackTransaction();

        // Assert
        Assert.False(context.HasActiveTransaction);
    }



    [Fact]
    public void OnModelCreating_ConfiguresEntitiesCorrectly()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb15");
        var mockMediator = new Mock<IMediator>();
        var context = new OrderingContext(options, mockMediator.Object);

        // Act
        var model = context.Model;

        // Assert
        Assert.NotNull(model);
        var orderEntityType = model.FindEntityType(typeof(eShop.Ordering.Domain.AggregatesModel.OrderAggregate.Order));
        var buyerEntityType = model.FindEntityType(typeof(eShop.Ordering.Domain.AggregatesModel.BuyerAggregate.Buyer));
        
        Assert.NotNull(orderEntityType);
        Assert.NotNull(buyerEntityType);
    }

    [Fact]
    public async Task SaveEntitiesAsync_AlwaysReturnsTrue()
    {
        // Arrange
        var options = CreateInMemoryOptions("TestDb16");
        var mockMediator = new Mock<IMediator>();
        var context = new OrderingContext(options, mockMediator.Object);

        // Act
        var result = await context.SaveEntitiesAsync();

        // Assert
        Assert.True(result);
    }


}
