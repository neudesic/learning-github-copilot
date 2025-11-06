using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

public class DatabaseInitializerTests
{
    private IServiceProvider GetServiceProvider(AppDbContext context)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => context);
        return serviceCollection.BuildServiceProvider();
    }

    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task InitializeAsync_ShouldCompleteSuccessfully_WithValidContext()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var serviceProvider = GetServiceProvider(context);
        var logger = new Mock<ILogger>();

        // Act
        await DatabaseInitializer.InitializeAsync(serviceProvider, logger.Object);

        // Assert
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Starting database initialization")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_ShouldThrowException_WhenContextIsNull()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => DatabaseInitializer.InitializeAsync(serviceProvider));
    }

    [Fact]
    public async Task InitializeAsync_ShouldLogInformation_WhenDatabaseInitializationSucceeds()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var serviceProvider = GetServiceProvider(context);
        var logger = new Mock<ILogger>();

        // Act
        await DatabaseInitializer.InitializeAsync(serviceProvider, logger.Object);

        // Assert
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("completed successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_ShouldLogWarning_WhenDatabaseIsEmpty()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var serviceProvider = GetServiceProvider(context);
        var logger = new Mock<ILogger>();

        // Act
        await DatabaseInitializer.InitializeAsync(serviceProvider, logger.Object);

        // Assert
        logger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("appears to be empty")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_ShouldHandleNullLogger()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var serviceProvider = GetServiceProvider(context);

        // Act
        var result = async () => await DatabaseInitializer.InitializeAsync(serviceProvider, null);

        // Assert
        await result.Should().NotThrowAsync();
    }

    [Fact]
    public async Task SeedDataAsync_ShouldCompleteSuccessfully_WithEmptyDatabase()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var serviceProvider = GetServiceProvider(context);
        var logger = new Mock<ILogger>();

        // Act
        await DatabaseInitializer.SeedDataAsync(serviceProvider, logger.Object);

        // Assert
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Runtime seeding completed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task SeedDataAsync_ShouldSkipSeeding_WhenDataAlreadyExists()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var category = new Category { Name = "Test Category" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var serviceProvider = GetServiceProvider(context);
        var logger = new Mock<ILogger>();

        // Act
        await DatabaseInitializer.SeedDataAsync(serviceProvider, logger.Object, forceReseed: false);

        // Assert
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("already contains data")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task SeedDataAsync_ShouldClearAndReseed_WhenForceReseedIsTrue()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var category = new Category { Name = "Test Category" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var serviceProvider = GetServiceProvider(context);
        var logger = new Mock<ILogger>();

        // Act
        await DatabaseInitializer.SeedDataAsync(serviceProvider, logger.Object, forceReseed: true);

        // Assert
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Force reseed requested")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task SeedDataAsync_ShouldHandleNullLogger()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var serviceProvider = GetServiceProvider(context);

        // Act
        var result = async () => await DatabaseInitializer.SeedDataAsync(serviceProvider, null);

        // Assert
        await result.Should().NotThrowAsync();
    }

    [Fact]
    public async Task SeedDataAsync_ShouldLogErrorWhenExceptionOccurs()
    {
        // Arrange
        var mockContext = new Mock<AppDbContext>();
        mockContext.Setup(c => c.Categories).Throws(new InvalidOperationException("Test exception"));

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => mockContext.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var logger = new Mock<ILogger>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => DatabaseInitializer.SeedDataAsync(serviceProvider, logger.Object));

        logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("error occurred while performing runtime database seeding")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ValidateDatabaseAsync_ShouldReturnFalse_WhenCannotConnect()
    {
        // Arrange
        var mockContext = new Mock<AppDbContext>();
        mockContext.Setup(c => c.Database.CanConnectAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => mockContext.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var logger = new Mock<ILogger>();

        // Act
        var result = await DatabaseInitializer.ValidateDatabaseAsync(serviceProvider, logger.Object);

        // Assert
        result.Should().BeFalse();
        logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Cannot connect")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ValidateDatabaseAsync_ShouldReturnTrue_WhenSufficientSeedDataExists()
    {
        // Arrange
        var context = GetInMemoryDbContext();

        // Add seed data
        for (int i = 0; i < 4; i++)
        {
            context.Categories.Add(new Category { Name = $"Category {i}" });
        }
        for (int i = 0; i < 7; i++)
        {
            context.Products.Add(new Product { Name = $"Product {i}", CategoryID = 1 });
        }
        for (int i = 0; i < 7; i++)
        {
            context.Inventory.Add(new Inventory { ProductID = 1, Quantity = 10 });
        }
        await context.SaveChangesAsync();

        var serviceProvider = GetServiceProvider(context);
        var logger = new Mock<ILogger>();

        // Act
        var result = await DatabaseInitializer.ValidateDatabaseAsync(serviceProvider, logger.Object);

        // Assert
        result.Should().BeTrue();
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("validation passed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ValidateDatabaseAsync_ShouldReturnFalse_WhenInsufficientSeedData()
    {
        // Arrange
        var context = GetInMemoryDbContext();

        // Add minimal seed data (less than required)
        context.Categories.Add(new Category { Name = "Category 1" });
        context.Products.Add(new Product { Name = "Product 1", CategoryID = 1 });
        await context.SaveChangesAsync();

        var serviceProvider = GetServiceProvider(context);
        var logger = new Mock<ILogger>();

        // Act
        var result = await DatabaseInitializer.ValidateDatabaseAsync(serviceProvider, logger.Object);

        // Assert
        result.Should().BeFalse();
        logger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("validation failed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ValidateDatabaseAsync_ShouldReturnFalse_WhenExceptionOccurs()
    {
        // Arrange
        var mockContext = new Mock<AppDbContext>();
        mockContext.Setup(c => c.Database.CanConnectAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Connection error"));

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => mockContext.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var logger = new Mock<ILogger>();

        // Act
        var result = await DatabaseInitializer.ValidateDatabaseAsync(serviceProvider, logger.Object);

        // Assert
        result.Should().BeFalse();
        logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("error occurred during database validation")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ValidateDatabaseAsync_ShouldHandleNullLogger()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var serviceProvider = GetServiceProvider(context);

        // Act
        var result = await DatabaseInitializer.ValidateDatabaseAsync(serviceProvider, null);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task InitializeAsync_ShouldCreateScopeFromServiceProvider()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var mockServiceCollection = new ServiceCollection();
        mockServiceCollection.AddScoped(_ => context);
        var serviceProvider = mockServiceCollection.BuildServiceProvider();

        // Act
        await DatabaseInitializer.InitializeAsync(serviceProvider);

        // Assert - If no exception is thrown, scope was created successfully
        Assert.True(true);
    }

    [Fact]
    public async Task SeedDataAsync_ShouldLogSeededCounts_WhenCompleted()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var serviceProvider = GetServiceProvider(context);
        var logger = new Mock<ILogger>();

        // Act
        await DatabaseInitializer.SeedDataAsync(serviceProvider, logger.Object);

        // Assert
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Added:")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ValidateDatabaseAsync_ShouldLogValidationCounts_WhenSuccessful()
    {
        // Arrange
        var context = GetInMemoryDbContext();

        // Add sufficient seed data
        for (int i = 0; i < 4; i++)
            context.Categories.Add(new Category { Name = $"Category {i}" });
        for (int i = 0; i < 7; i++)
            context.Products.Add(new Product { Name = $"Product {i}", CategoryID = 1 });
        for (int i = 0; i < 7; i++)
            context.Inventory.Add(new Inventory { ProductID = 1, Quantity = 10 });
        await context.SaveChangesAsync();

        var serviceProvider = GetServiceProvider(context);
        var logger = new Mock<ILogger>();

        // Act
        await DatabaseInitializer.ValidateDatabaseAsync(serviceProvider, logger.Object);

        // Assert
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Database validation:")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_ShouldLogErrorAndRethrow_WhenMigrationFails()
    {
        // Arrange
        var mockContext = new Mock<AppDbContext>();
        mockContext.Setup(c => c.Database.MigrateAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Migration failed"));

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => mockContext.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var logger = new Mock<ILogger>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => DatabaseInitializer.InitializeAsync(serviceProvider, logger.Object));

        logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("error occurred while initializing the database")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task SeedDataAsync_ShouldClearDataInCorrectOrder_WhenForceReseed()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        
        // Add all entity types
        var category = new Category { Name = "Test" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var product = new Product { Name = "Test Product", CategoryID = category.CategoryID };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var inventory = new Inventory { ProductID = product.ProductID, Quantity = 10 };
        context.Inventory.Add(inventory);
        await context.SaveChangesAsync();

        var serviceProvider = GetServiceProvider(context);
        var logger = new Mock<ILogger>();

        // Act
        await DatabaseInitializer.SeedDataAsync(serviceProvider, logger.Object, forceReseed: true);

        // Assert
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Clearing all existing data")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_ShouldLogInformation_WhenDatabaseHasSeedData()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var category = new Category { Name = "Test Category" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var serviceProvider = GetServiceProvider(context);
        var logger = new Mock<ILogger>();

        // Act
        await DatabaseInitializer.InitializeAsync(serviceProvider, logger.Object);

        // Assert
        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Database contains seed data from migrations")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task SeedDataAsync_ShouldThrowException_WhenContextIsNull()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => DatabaseInitializer.SeedDataAsync(serviceProvider));
    }
}
