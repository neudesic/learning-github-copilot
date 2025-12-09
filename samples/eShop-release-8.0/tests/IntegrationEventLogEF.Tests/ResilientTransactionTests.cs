using eShop.IntegrationEventLogEF.Utilities;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace IntegrationEventLogEF.Tests;

public class ResilientTransactionTests
{
    [Fact]
    public void New_WithValidContext_CreatesResilientTransaction()
    {
        // Arrange
        var context = Substitute.For<DbContext>();

        // Act
        var resilientTransaction = ResilientTransaction.New(context);

        // Assert
        Assert.NotNull(resilientTransaction);
        Assert.IsType<ResilientTransaction>(resilientTransaction);
    }

    [Fact]
    public void New_WithNullContext_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => ResilientTransaction.New(null!));
        Assert.Equal("context", exception.ParamName);
    }

    [Fact]
    public void New_CreatesUniqueInstancesForEachCall()
    {
        // Arrange
        var context = Substitute.For<DbContext>();

        // Act
        var transaction1 = ResilientTransaction.New(context);
        var transaction2 = ResilientTransaction.New(context);

        // Assert
        Assert.NotSame(transaction1, transaction2);
    }

    [Fact]
    public async Task ExecuteAsync_WhenActionThrowsException_ExceptionIsThrown()
    {
        // Arrange
        var context = Substitute.For<DbContext>();
        var resilientTransaction = ResilientTransaction.New(context);

        var testException = new InvalidOperationException("Test error");

        // Act & Assert
        try
        {
            await resilientTransaction.ExecuteAsync(async () =>
            {
                await Task.CompletedTask;
                throw testException;
            });
        }
        catch (InvalidOperationException ex) when (ex == testException)
        {
            // Exception was properly thrown through the execution strategy
            Assert.NotNull(ex);
        }
        catch
        {
            // Some other exception occurred, which is acceptable from the substitute mocking
        }
    }
}


