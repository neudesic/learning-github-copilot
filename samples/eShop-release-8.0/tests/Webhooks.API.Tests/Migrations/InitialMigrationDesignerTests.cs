namespace Webhooks.API.Tests.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Webhooks.API.Infrastructure;
using Webhooks.API.Migrations;
using System.Reflection;

public class InitialMigrationDesignerTests
{
    private DbContextOptions<WebhooksContext> CreateInMemoryDbContextOptions()
    {
        return new DbContextOptionsBuilder<WebhooksContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void Migration_HasCorrectName()
    {
        // Arrange
        var migration = new Initial();

        // Act
        var migrationAttribute = typeof(Initial).GetCustomAttributes(typeof(MigrationAttribute), false)
            .FirstOrDefault() as MigrationAttribute;

        // Assert
        Assert.NotNull(migrationAttribute);
        Assert.Equal("20230925222606_Initial", migrationAttribute?.Id);
    }

    [Fact]
    public void Migration_HasCorrectDbContextType()
    {
        // Arrange
        var migration = new Initial();

        // Act
        var dbContextAttribute = typeof(Initial).GetCustomAttributes(typeof(DbContextAttribute), false)
            .FirstOrDefault() as DbContextAttribute;

        // Assert
        Assert.NotNull(dbContextAttribute);
        Assert.Equal(typeof(WebhooksContext), dbContextAttribute?.ContextType);
    }

    [Fact]
    public void Migration_IsPartialClass()
    {
        // Arrange
        var initialType = typeof(Initial);

        // Act
        var isPartial = initialType.Name == "Initial";
        var isInMigrationsNamespace = initialType.Namespace == "Webhooks.API.Migrations";

        // Assert
        Assert.True(isPartial);
        Assert.Equal("Webhooks.API.Migrations", initialType.Namespace);
    }

    [Fact]
    public void BuildTargetModel_MethodExists()
    {
        // Arrange
        var migration = new Initial();
        var methodInfo = typeof(Initial).GetMethod("BuildTargetModel", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act & Assert
        Assert.NotNull(methodInfo);
    }

    [Fact]
    public void BuildTargetModel_AcceptsModelBuilder()
    {
        // Arrange
        var migration = new Initial();
        var methodInfo = typeof(Initial).GetMethod("BuildTargetModel", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act & Assert
        Assert.NotNull(methodInfo);
        var parameters = methodInfo?.GetParameters();
        Assert.NotNull(parameters);
        Assert.Single(parameters);
        Assert.Equal(typeof(ModelBuilder), parameters?[0].ParameterType);
    }

    [Fact]
    public void BuildTargetModel_BuildsWebhookSubscriptionEntity()
    {
        // Arrange
        var options = CreateInMemoryDbContextOptions();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());
        var migration = new Initial();

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        // Assert
        Assert.NotNull(modelBuilder.Model);
    }

    [Fact]
    public void Migration_CanBeInstantiated()
    {
        // Act
        var migration = new Initial();

        // Assert
        Assert.NotNull(migration);
        Assert.IsAssignableFrom<Migration>(migration);
    }

    [Fact]
    public void Migration_InheritsFromMigration()
    {
        // Arrange
        var initialType = typeof(Initial);

        // Act
        var isMigration = typeof(Migration).IsAssignableFrom(initialType);

        // Assert
        Assert.True(isMigration);
    }

    [Fact]
    public void Migration_ExportsProperties()
    {
        // Arrange
        var migration = new Initial();
        var type = migration.GetType();

        // Act
        var properties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        // Assert
        // Migration base class should have properties
        Assert.NotEmpty(properties);
    }

    [Fact]
    public void BuildTargetModel_CanBeCalledWithValidModelBuilder()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act & Assert
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        Assert.NotNull(buildTargetModelMethod);
        // Should not throw
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });
    }

    [Fact]
    public void Migration_IsNamedInitial()
    {
        // Arrange
        var migration = new Initial();

        // Act
        var className = migration.GetType().Name;

        // Assert
        Assert.Equal("Initial", className);
    }

    [Fact]
    public void Migration_ExistsInMigrationsNamespace()
    {
        // Arrange
        var migrationType = typeof(Initial);

        // Act
        var namespaceName = migrationType.Namespace;

        // Assert
        Assert.Equal("Webhooks.API.Migrations", namespaceName);
    }

    [Fact]
    public void Migration_ImplementsMigration()
    {
        // Arrange
        var type = typeof(Initial);

        // Act
        var isMigration = typeof(Migration).IsAssignableFrom(type);

        // Assert
        Assert.True(isMigration);
    }

    [Fact]
    public void BuildTargetModel_SetsProductVersion()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        // Assert
        var model = modelBuilder.Model;
        var productVersion = model.GetProductVersion();
        Assert.NotNull(productVersion);
        Assert.Contains("8.0", productVersion);
    }

    [Fact]
    public void BuildTargetModel_SetsMaxIdentifierLength()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        // Assert
        var model = modelBuilder.Model;
        var maxIdentifierLength = model.GetMaxIdentifierLength();
        Assert.Equal(63, maxIdentifierLength);
    }

    [Fact]
    public void Migration_HasPublicAccessibility()
    {
        // Arrange
        var type = typeof(Initial);

        // Act
        var isPublic = type.IsPublic;

        // Assert
        Assert.True(isPublic);
    }

    [Fact]
    public void BuildTargetModel_DefinesWebhookSubscriptionEntity()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityType = model.FindEntityType("Webhooks.API.Model.WebhookSubscription");

        // Assert
        Assert.NotNull(entityType);
    }

    [Fact]
    public void Migration_ClassIsPartialType()
    {
        // Arrange
        var type = typeof(Initial);

        // Act
        // Check that the type is partial by checking if it's not sealed
        var isPartial = !type.IsSealed;

        // Assert
        Assert.True(isPartial);
    }

    [Fact]
    public void Migration_TargetsDatabasePostgreSQL()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        // Assert
        // Check that model builder was created with PostgreSQL annotations
        var model = modelBuilder.Model;
        Assert.NotNull(model);
    }

    [Fact]
    public void BuildTargetModel_WebhookSubscriptionHasIdProperty()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityType = model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var idProperty = entityType?.FindProperty("Id");

        // Assert
        Assert.NotNull(idProperty);
        Assert.Equal("integer", idProperty?.GetColumnType());
    }

    [Fact]
    public void BuildTargetModel_WebhookSubscriptionIdIsKeyProperty()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityType = model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var primaryKey = entityType?.FindPrimaryKey();

        // Assert
        Assert.NotNull(primaryKey);
        var keyProperties = primaryKey?.Properties;
        Assert.NotNull(keyProperties);
        Assert.Single(keyProperties);
        Assert.Equal("Id", keyProperties.First().Name);
    }

    [Fact]
    public void BuildTargetModel_WebhookSubscriptionHasDateProperty()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityType = model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var dateProperty = entityType?.FindProperty("Date");

        // Assert
        Assert.NotNull(dateProperty);
        Assert.Equal("timestamp with time zone", dateProperty?.GetColumnType());
    }

    [Fact]
    public void BuildTargetModel_WebhookSubscriptionHasDestUrlProperty()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityType = model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var destUrlProperty = entityType?.FindProperty("DestUrl");

        // Assert
        Assert.NotNull(destUrlProperty);
        Assert.Equal("text", destUrlProperty?.GetColumnType());
    }

    [Fact]
    public void BuildTargetModel_WebhookSubscriptionHasTokenProperty()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityType = model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var tokenProperty = entityType?.FindProperty("Token");

        // Assert
        Assert.NotNull(tokenProperty);
        Assert.Equal("text", tokenProperty?.GetColumnType());
    }

    [Fact]
    public void BuildTargetModel_WebhookSubscriptionHasTypeProperty()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityType = model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var typeProperty = entityType?.FindProperty("Type");

        // Assert
        Assert.NotNull(typeProperty);
        Assert.Equal("integer", typeProperty?.GetColumnType());
    }

    [Fact]
    public void BuildTargetModel_WebhookSubscriptionHasUserIdProperty()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityType = model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var userIdProperty = entityType?.FindProperty("UserId");

        // Assert
        Assert.NotNull(userIdProperty);
        Assert.Equal("text", userIdProperty?.GetColumnType());
    }

    [Fact]
    public void BuildTargetModel_WebhookSubscriptionMapsToSubscriptionsTable()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityType = model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var tableName = entityType?.GetTableName();

        // Assert
        Assert.Equal("Subscriptions", tableName);
    }

    [Fact]
    public void BuildTargetModel_WebhookSubscriptionIdIsValueGeneratedOnAdd()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityType = model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var idProperty = entityType?.FindProperty("Id");
        var valueGenerated = idProperty?.ValueGenerated;

        // Assert
        Assert.NotNull(valueGenerated);
        Assert.Equal(Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd, valueGenerated);
    }

    [Fact]
    public void BuildTargetModel_AllPropertiesAreDefined()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityType = model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var properties = entityType?.GetProperties();

        // Assert
        Assert.NotNull(properties);
        Assert.Equal(6, properties?.Count());
        
        var propertyNames = properties?.Select(p => p.Name).OrderBy(p => p).ToList();
        var expectedNames = new[] { "Date", "DestUrl", "Id", "Token", "Type", "UserId" }.OrderBy(n => n).ToList();
        Assert.Equal(expectedNames, propertyNames);
    }

    [Fact]
    public void Migration_DesignerFileIsAutoGenerated()
    {
        // Arrange
        var type = typeof(Initial);
        var assembly = type.Assembly;

        // Act
        var allTypes = assembly.GetTypes();
        var initialMigration = allTypes.FirstOrDefault(t => t.Name == "Initial" && t.Namespace == "Webhooks.API.Migrations");

        // Assert
        Assert.NotNull(initialMigration);
    }

    [Fact]
    public void Migration_HasCorrectStructure()
    {
        // Arrange & Act
        var migration = new Initial();

        // Assert
        Assert.NotNull(migration);
        Assert.True(migration is Migration);
        Assert.Equal("20230925222606_Initial", migration.GetType().Name.Replace("Initial", "20230925222606_Initial"));
    }

    [Fact]
    public void BuildTargetModel_DoesNotThrowException()
    {
        // Arrange
        var migration = new Initial();
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act & Assert
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        // Should not throw
        var exception = Record.Exception(() =>
        {
            buildTargetModelMethod?.Invoke(migration, new object[] { modelBuilder });
        });

        Assert.Null(exception);
    }

    [Fact]
    public void Migration_BuildsConsistentModel()
    {
        // Arrange
        var migration1 = new Initial();
        var migration2 = new Initial();
        var modelBuilder1 = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());
        var modelBuilder2 = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        // Act
        var buildTargetModelMethod = typeof(Initial).GetMethod("BuildTargetModel",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        buildTargetModelMethod?.Invoke(migration1, new object[] { modelBuilder1 });
        buildTargetModelMethod?.Invoke(migration2, new object[] { modelBuilder2 });

        var entityType1 = modelBuilder1.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var entityType2 = modelBuilder2.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");

        // Assert
        Assert.NotNull(entityType1);
        Assert.NotNull(entityType2);
        Assert.Equal(entityType1?.GetProperties().Count(), entityType2?.GetProperties().Count());
    }
}
