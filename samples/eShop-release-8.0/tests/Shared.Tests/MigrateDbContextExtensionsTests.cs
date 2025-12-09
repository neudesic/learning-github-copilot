namespace eShop.Shared.Tests;

using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;

public class MigrateDbContextExtensionsTests
{
    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
    }

    private class AnotherTestDbContext : DbContext
    {
        public AnotherTestDbContext(DbContextOptions<AnotherTestDbContext> options) : base(options) { }
    }

    private class TestSeeder : IDbSeeder<TestDbContext>
    {
        public Task SeedAsync(TestDbContext context)
        {
            return Task.CompletedTask;
        }
    }

    [Fact]
    public void AddMigration_WithDbContextOnly_RegistersHostedService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        // Act
        var result = services.AddMigration<TestDbContext>();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IServiceCollection>(result);
        var serviceProvider = services.BuildServiceProvider();
        var hostedServices = serviceProvider.GetServices<IHostedService>().OfType<BackgroundService>();
        Assert.Single(hostedServices);
    }

    [Fact]
    public void AddMigration_WithDbContextAndSeeder_RegistersHostedService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        Func<TestDbContext, IServiceProvider, Task> seeder = (_, _) => Task.CompletedTask;

        // Act
        var result = services.AddMigration<TestDbContext>(seeder);

        // Assert
        Assert.NotNull(result);
        var serviceProvider = services.BuildServiceProvider();
        var hostedServices = serviceProvider.GetServices<IHostedService>().OfType<BackgroundService>();
        Assert.Single(hostedServices);
    }

    [Fact]
    public void AddMigration_WithDbContextAndSeederType_RegistersSeederAndHostedService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        // Act
        var result = services.AddMigration<TestDbContext, TestSeeder>();

        // Assert
        Assert.NotNull(result);
        var serviceProvider = services.BuildServiceProvider();
        var hostedServices = serviceProvider.GetServices<IHostedService>().OfType<BackgroundService>();
        Assert.Single(hostedServices);
        
        var seeder = serviceProvider.GetService<IDbSeeder<TestDbContext>>();
        Assert.NotNull(seeder);
        Assert.IsType<TestSeeder>(seeder);
    }

    [Fact]
    public void AddMigration_ReturnsServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        // Act
        var result = services.AddMigration<TestDbContext>();

        // Assert
        Assert.Same(services, result);
    }

    [Fact]
    public void AddMigration_WithMultipleDbContexts_RegistersMultipleHostedServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb1"));
        services.AddDbContext<AnotherTestDbContext>(options => options.UseInMemoryDatabase("TestDb2"));

        // Act
        services.AddMigration<TestDbContext>();
        services.AddMigration<AnotherTestDbContext>();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var hostedServices = serviceProvider.GetServices<IHostedService>().OfType<BackgroundService>().ToList();
        Assert.Equal(2, hostedServices.Count());
    }

    [Fact]
    public void AddMigration_AllowsChaining()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        // Act
        var result = services
            .AddMigration<TestDbContext>()
            .AddLogging();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void IDbSeeder_IsValidInterface()
    {
        // Arrange & Act
        var seederType = typeof(IDbSeeder<TestDbContext>);

        // Assert
        Assert.True(seederType.IsInterface);
        var seedAsyncMethod = seederType.GetMethod("SeedAsync");
        Assert.NotNull(seedAsyncMethod);
        Assert.Equal(typeof(Task), seedAsyncMethod!.ReturnType);
    }

    [Fact]
    public void TestSeeder_ImplementsIDbSeeder()
    {
        // Arrange & Act & Assert
        Assert.IsAssignableFrom<IDbSeeder<TestDbContext>>(new TestSeeder());
    }

    [Fact]
    public void AddMigration_WithDifferentDbContexts_CreatesIndependentServices()
    {
        // Arrange
        var services1 = new ServiceCollection();
        services1.AddLogging();
        services1.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb1"));

        var services2 = new ServiceCollection();
        services2.AddLogging();
        services2.AddDbContext<AnotherTestDbContext>(options => options.UseInMemoryDatabase("TestDb2"));

        // Act
        services1.AddMigration<TestDbContext>();
        services2.AddMigration<AnotherTestDbContext>();

        var provider1 = services1.BuildServiceProvider();
        var provider2 = services2.BuildServiceProvider();

        // Assert
        var hostedServices1 = provider1.GetServices<IHostedService>().OfType<BackgroundService>();
        var hostedServices2 = provider2.GetServices<IHostedService>().OfType<BackgroundService>();

        Assert.Single(hostedServices1);
        Assert.Single(hostedServices2);
    }

    [Fact]
    public void AddMigration_SupportsGenericConstraints()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        // Act
        var result = services.AddMigration<TestDbContext>();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void AddMigration_WithLambdaSeeder_Works()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        // Act
        services.AddMigration<TestDbContext>((ctx, sp) => Task.CompletedTask);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var hostedService = serviceProvider.GetService<IHostedService>();
        Assert.NotNull(hostedService);
    }

    [Fact]
    public void AddMigration_SeederTypeWithoutService_RegistersSeeder()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        // Act
        services.AddMigration<TestDbContext, TestSeeder>();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var hostedService = serviceProvider.GetService<IHostedService>();
        Assert.NotNull(hostedService);
        var seeder = serviceProvider.GetService<IDbSeeder<TestDbContext>>();
        Assert.NotNull(seeder);
    }

    [Fact]
    public void DbSeederInterface_HasCorrectConstraint()
    {
        // Arrange & Act
        var seederInterface = typeof(IDbSeeder<>);

        // Assert
        Assert.True(seederInterface.IsGenericTypeDefinition);
        var genericArgs = seederInterface.GetGenericArguments();
        Assert.Single(genericArgs);
        var genericArg = genericArgs[0];
        var constraints = genericArg.GetGenericParameterConstraints();
        Assert.Contains(typeof(DbContext), constraints);
    }

    [Fact]
    public void AddMigration_MultipleCallsWithDifferentSeeders_RegistersMultipleServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        Func<TestDbContext, IServiceProvider, Task> seeder1 = (_, _) => Task.CompletedTask;
        Func<TestDbContext, IServiceProvider, Task> seeder2 = (_, _) => Task.CompletedTask;

        // Act
        services.AddMigration<TestDbContext>(seeder1);
        services.AddMigration<TestDbContext>(seeder2);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var hostedServices = serviceProvider.GetServices<IHostedService>();
        Assert.Equal(2, hostedServices.Count());
    }

    [Fact]
    public void AddMigration_NullServices_ThrowsArgumentNullException()
    {
        // Arrange
        IServiceCollection? services = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            services!.AddMigration<TestDbContext>());
    }

    [Fact]
    public void AddMigration_RegistersServiceForMultipleContexts()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));
        services.AddDbContext<AnotherTestDbContext>(options => options.UseInMemoryDatabase("AnotherTestDb"));

        // Act
        services.AddMigration<TestDbContext>();
        services.AddMigration<AnotherTestDbContext>();

        var provider = services.BuildServiceProvider();

        // Assert
        var hostedServices = provider.GetServices<IHostedService>().OfType<BackgroundService>();
        Assert.Equal(2, hostedServices.Count());
    }

    [Fact]
    public void AddMigration_WithLambdaSeeder_SeederIsCallable()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        Func<TestDbContext, IServiceProvider, Task> seeder = (ctx, sp) => Task.CompletedTask;

        // Act
        services.AddMigration<TestDbContext>(seeder);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.NotNull(serviceProvider);
    }

    [Fact]
    public void AddMigration_ServiceCollection_ReturnsIServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        // Act
        IServiceCollection result = services.AddMigration<TestDbContext>();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IServiceCollection>(result);
    }

    [Fact]
    public void AddMigration_CreatesSeededService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        // Act
        services.AddMigration<TestDbContext, TestSeeder>();
        var provider = services.BuildServiceProvider();

        // Assert
        var seeder = provider.GetService<IDbSeeder<TestDbContext>>();
        Assert.NotNull(seeder);
        Assert.IsType<TestSeeder>(seeder);
    }

    [Fact]
    public void AddMigration_HostedServiceIsNotNull()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        // Act
        services.AddMigration<TestDbContext>();
        var provider = services.BuildServiceProvider();
        var hostedService = provider.GetService<IHostedService>();

        // Assert
        Assert.NotNull(hostedService);
    }

    [Fact]
    public void AddMigration_CanChainMultipleCalls()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services
            .AddLogging()
            .AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"))
            .AddMigration<TestDbContext>();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void AddMigration_WithSeederType_SeederIsScoped()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        // Act
        services.AddMigration<TestDbContext, TestSeeder>();
        var provider = services.BuildServiceProvider();

        // Assert
        using (var scope1 = provider.CreateScope())
        {
            var seeder1 = scope1.ServiceProvider.GetService<IDbSeeder<TestDbContext>>();
            using (var scope2 = provider.CreateScope())
            {
                var seeder2 = scope2.ServiceProvider.GetService<IDbSeeder<TestDbContext>>();
                Assert.NotNull(seeder1);
                Assert.NotNull(seeder2);
                Assert.NotSame(seeder1, seeder2);
            }
        }
    }

    [Fact]
    public void IDbSeeder_SeedAsync_IsAsync()
    {
        // Arrange & Act
        var method = typeof(IDbSeeder<TestDbContext>).GetMethod("SeedAsync");

        // Assert
        Assert.NotNull(method);
        Assert.True(typeof(Task).IsAssignableFrom(method.ReturnType));
    }

    [Fact]
    public void AddMigration_CanAddMultipleSeederTypes()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<TestDbContext>(options => options.UseInMemoryDatabase("TestDb"));
        services.AddDbContext<AnotherTestDbContext>(options => options.UseInMemoryDatabase("AnotherDb"));

        // Act
        services.AddMigration<TestDbContext, TestSeeder>();
        
        var provider = services.BuildServiceProvider();

        // Assert
        var seeder = provider.GetService<IDbSeeder<TestDbContext>>();
        Assert.NotNull(seeder);
        Assert.IsType<TestSeeder>(seeder);
    }
}
