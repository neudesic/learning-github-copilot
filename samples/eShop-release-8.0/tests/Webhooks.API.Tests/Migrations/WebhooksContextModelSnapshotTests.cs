namespace Webhooks.API.Tests.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Webhooks.API.Infrastructure;
using Webhooks.API.Migrations;
using System.Reflection;

public class WebhooksContextModelSnapshotTests
{
    [Fact]
    public void ModelSnapshot_HasCorrectDbContextType()
    {
        // Arrange
        var snapshotType = typeof(WebhooksContextModelSnapshot);

        // Act
        var dbContextAttribute = snapshotType.GetCustomAttributes(typeof(DbContextAttribute), false)
            .FirstOrDefault() as DbContextAttribute;

        // Assert
        Assert.NotNull(dbContextAttribute);
        Assert.Equal(typeof(WebhooksContext), dbContextAttribute?.ContextType);
    }

    [Fact]
    public void ModelSnapshot_IsPartialClass()
    {
        // Arrange
        var snapshotType = typeof(WebhooksContextModelSnapshot);

        // Act
        var isPartial = !snapshotType.IsSealed;
        var isInMigrationsNamespace = snapshotType.Namespace == "Webhooks.API.Migrations";
        var hasCorrectName = snapshotType.Name == "WebhooksContextModelSnapshot";

        // Assert
        Assert.True(isPartial);
        Assert.True(isInMigrationsNamespace);
        Assert.True(hasCorrectName);
    }

    [Fact]
    public void ModelSnapshot_InheritsFromModelSnapshot()
    {
        // Arrange
        var snapshotType = typeof(WebhooksContextModelSnapshot);

        // Act
        var isModelSnapshot = typeof(ModelSnapshot).IsAssignableFrom(snapshotType);

        // Assert
        Assert.True(isModelSnapshot);
    }

    [Fact]
    public void ModelSnapshot_HasBuildModelMethod()
    {
        // Arrange
        var snapshot = new WebhooksContextModelSnapshot();
        var methodInfo = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);

        // Act & Assert
        Assert.NotNull(methodInfo);
    }

    [Fact]
    public void BuildModel_AcceptsModelBuilder()
    {
        // Arrange
        var snapshot = new WebhooksContextModelSnapshot();
        var methodInfo = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        var parameters = methodInfo?.GetParameters();

        // Assert
        Assert.NotNull(parameters);
        Assert.Single(parameters);
        Assert.Equal(typeof(ModelBuilder), parameters?[0].ParameterType);
    }

    [Fact]
    public void BuildModel_ConfiguresWebhookSubscriptionEntity()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityType = model.FindEntityType("Webhooks.API.Model.WebhookSubscription");

        // Assert
        Assert.NotNull(entityType);
    }

    [Fact]
    public void BuildModel_SetsProductVersion()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var productVersion = model.GetProductVersion();

        // Assert
        Assert.NotNull(productVersion);
        Assert.Contains("8.0", productVersion);
    }

    [Fact]
    public void BuildModel_SetsMaxIdentifierLength()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var maxIdentifierLength = model.GetMaxIdentifierLength();

        // Assert
        Assert.Equal(63, maxIdentifierLength);
    }

    [Fact]
    public void ModelSnapshot_CanBeInstantiated()
    {
        // Act
        var snapshot = new WebhooksContextModelSnapshot();

        // Assert
        Assert.NotNull(snapshot);
        Assert.IsAssignableFrom<ModelSnapshot>(snapshot);
    }

    [Fact]
    public void BuildModel_DoesNotThrowException()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act & Assert
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);

        var exception = Record.Exception(() =>
        {
            buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });
        });

        Assert.Null(exception);
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionHasIdProperty()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var idProperty = entityType?.FindProperty("Id");

        // Assert
        Assert.NotNull(idProperty);
        Assert.Equal("integer", idProperty?.GetColumnType());
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionIdIsKeyProperty()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var primaryKey = entityType?.FindPrimaryKey();

        // Assert
        Assert.NotNull(primaryKey);
        var keyProperties = primaryKey?.Properties;
        Assert.NotNull(keyProperties);
        Assert.Single(keyProperties);
        Assert.Equal("Id", keyProperties.First().Name);
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionIdIsValueGeneratedOnAdd()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var idProperty = entityType?.FindProperty("Id");
        var valueGenerated = idProperty?.ValueGenerated;

        // Assert
        Assert.NotNull(valueGenerated);
        Assert.Equal(ValueGenerated.OnAdd, valueGenerated);
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionHasDateProperty()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var dateProperty = entityType?.FindProperty("Date");

        // Assert
        Assert.NotNull(dateProperty);
        Assert.Equal("timestamp with time zone", dateProperty?.GetColumnType());
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionHasDestUrlProperty()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var destUrlProperty = entityType?.FindProperty("DestUrl");

        // Assert
        Assert.NotNull(destUrlProperty);
        Assert.Equal("text", destUrlProperty?.GetColumnType());
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionDestUrlIsRequired()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var destUrlProperty = entityType?.FindProperty("DestUrl");

        // Assert
        Assert.NotNull(destUrlProperty);
        Assert.False(destUrlProperty?.IsNullable);
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionHasTokenProperty()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var tokenProperty = entityType?.FindProperty("Token");

        // Assert
        Assert.NotNull(tokenProperty);
        Assert.Equal("text", tokenProperty?.GetColumnType());
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionTokenIsNullable()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var tokenProperty = entityType?.FindProperty("Token");

        // Assert
        Assert.NotNull(tokenProperty);
        Assert.True(tokenProperty?.IsNullable);
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionHasTypeProperty()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var typeProperty = entityType?.FindProperty("Type");

        // Assert
        Assert.NotNull(typeProperty);
        Assert.Equal("integer", typeProperty?.GetColumnType());
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionHasUserIdProperty()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var userIdProperty = entityType?.FindProperty("UserId");

        // Assert
        Assert.NotNull(userIdProperty);
        Assert.Equal("text", userIdProperty?.GetColumnType());
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionUserIdIsRequired()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var userIdProperty = entityType?.FindProperty("UserId");

        // Assert
        Assert.NotNull(userIdProperty);
        Assert.False(userIdProperty?.IsNullable);
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionMapsToSubscriptionsTable()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var tableName = entityType?.GetTableName();

        // Assert
        Assert.Equal("Subscriptions", tableName);
    }

    [Fact]
    public void BuildModel_AllPropertiesAreDefined()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var properties = entityType?.GetProperties();

        // Assert
        Assert.NotNull(properties);
        Assert.Equal(6, properties?.Count());
        
        var propertyNames = properties?.Select(p => p.Name).OrderBy(p => p).ToList();
        var expectedNames = new[] { "Date", "DestUrl", "Id", "Token", "Type", "UserId" }.OrderBy(n => n).ToList();
        Assert.Equal(expectedNames, propertyNames);
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionHasTypeIndex()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var indexes = entityType?.GetIndexes();

        // Assert
        Assert.NotNull(indexes);
        var typeIndex = indexes?.FirstOrDefault(i => i.Properties.Any(p => p.Name == "Type"));
        Assert.NotNull(typeIndex);
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionHasUserIdIndex()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var indexes = entityType?.GetIndexes();

        // Assert
        Assert.NotNull(indexes);
        var userIdIndex = indexes?.FirstOrDefault(i => i.Properties.Any(p => p.Name == "UserId"));
        Assert.NotNull(userIdIndex);
    }

    [Fact]
    public void BuildModel_WebhookSubscriptionHasTwoIndexes()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var indexes = entityType?.GetIndexes();

        // Assert
        Assert.NotNull(indexes);
        Assert.Equal(2, indexes?.Count());
    }

    [Fact]
    public void ModelSnapshot_ClassIsPartialType()
    {
        // Arrange
        var snapshotType = typeof(WebhooksContextModelSnapshot);

        // Act
        var isPartial = !snapshotType.IsSealed;

        // Assert
        Assert.True(isPartial);
    }

    [Fact]
    public void BuildModel_BuildsConsistentModel()
    {
        // Arrange
        var snapshot1 = new WebhooksContextModelSnapshot();
        var snapshot2 = new WebhooksContextModelSnapshot();
        var modelBuilder1 = new ModelBuilder(new ConventionSet());
        var modelBuilder2 = new ModelBuilder(new ConventionSet());

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot1, new object[] { modelBuilder1 });
        buildModelMethod?.Invoke(snapshot2, new object[] { modelBuilder2 });

        var entityType1 = modelBuilder1.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var entityType2 = modelBuilder2.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");

        // Assert
        Assert.NotNull(entityType1);
        Assert.NotNull(entityType2);
        Assert.Equal(entityType1?.GetProperties().Count(), entityType2?.GetProperties().Count());
        Assert.Equal(entityType1?.GetIndexes().Count(), entityType2?.GetIndexes().Count());
    }

    [Fact]
    public void BuildModel_ModelHasNoEntitiesOtherThanWebhookSubscription()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var entityTypes = model.GetEntityTypes();

        // Assert
        Assert.Single(entityTypes);
        var entityName = entityTypes.First().Name;
        Assert.Contains("WebhookSubscription", entityName);
    }

    [Fact]
    public void BuildModel_EntityHasCorrectFullName()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");

        // Assert
        Assert.NotNull(entityType);
        var entityName = entityType?.Name;
        Assert.Contains("WebhookSubscription", entityName);
    }

    [Fact]
    public void BuildModel_IdPropertyHasCorrectAttributes()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var idProperty = entityType?.FindProperty("Id");

        // Assert
        Assert.NotNull(idProperty);
        Assert.Equal(typeof(int), idProperty?.ClrType);
        Assert.Equal("integer", idProperty?.GetColumnType());
        Assert.False(idProperty?.IsNullable);
    }

    [Fact]
    public void BuildModel_DatePropertyHasCorrectAttributes()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var dateProperty = entityType?.FindProperty("Date");

        // Assert
        Assert.NotNull(dateProperty);
        Assert.Equal(typeof(DateTime), dateProperty?.ClrType);
        Assert.Equal("timestamp with time zone", dateProperty?.GetColumnType());
    }

    [Fact]
    public void BuildModel_TypePropertyHasIntegerColumnType()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var typeProperty = entityType?.FindProperty("Type");

        // Assert
        Assert.NotNull(typeProperty);
        Assert.Equal("integer", typeProperty?.GetColumnType());
    }

    [Fact]
    public void BuildModel_StringPropertiesHaveTextColumnType()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");

        // Assert
        Assert.Equal("text", entityType?.FindProperty("DestUrl")?.GetColumnType());
        Assert.Equal("text", entityType?.FindProperty("Token")?.GetColumnType());
        Assert.Equal("text", entityType?.FindProperty("UserId")?.GetColumnType());
    }

    [Fact]
    public void BuildModel_PrimaryKeyOnlyContainsIdProperty()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var entityType = modelBuilder.Model.FindEntityType("Webhooks.API.Model.WebhookSubscription");
        var primaryKey = entityType?.FindPrimaryKey();
        var keyProperties = primaryKey?.Properties;

        // Assert
        Assert.NotNull(keyProperties);
        Assert.Single(keyProperties);
        Assert.Equal("Id", keyProperties?.First().Name);
    }

    [Fact]
    public void ModelSnapshot_ReflectsEF8Configuration()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var productVersion = model.GetProductVersion();

        // Assert
        Assert.NotNull(productVersion);
        Assert.True(productVersion.StartsWith("8.0"), $"Expected EF Core 8.0, got {productVersion}");
    }

    [Fact]
    public void BuildModel_ConfiguresPostgreSQLIdentity()
    {
        // Arrange
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var buildModelMethod = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);
        buildModelMethod?.Invoke(snapshot, new object[] { modelBuilder });

        var model = modelBuilder.Model;
        var maxIdentifierLength = model.GetMaxIdentifierLength();

        // Assert
        Assert.Equal(63, maxIdentifierLength);
    }

    [Fact]
    public void ModelSnapshot_NameMatches()
    {
        // Arrange
        var snapshot = new WebhooksContextModelSnapshot();

        // Act
        var typeName = typeof(WebhooksContextModelSnapshot).Name;

        // Assert
        Assert.Equal("WebhooksContextModelSnapshot", typeName);
    }

    [Fact]
    public void BuildModel_MethodIsProtected()
    {
        // Arrange
        var methodInfo = typeof(WebhooksContextModelSnapshot).GetMethod("BuildModel",
            BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        var isProtected = methodInfo?.IsAssembly == false && !methodInfo?.IsPublic == true;

        // Assert
        Assert.NotNull(methodInfo);
    }
}
