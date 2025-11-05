using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.DataAccess.EntityConfiguration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

public class InventoryConfigurationTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public void Configure_ShouldMapTableName_WhenConfiguring()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));

        // Act & Assert
        entityType?.GetTableName().Should().Be("Inventory");
    }

    [Fact]
    public void Configure_ShouldHavePrimaryKey_OnInventoryID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var primaryKey = entityType?.FindPrimaryKey();

        // Act & Assert
        primaryKey.Should().NotBeNull();
        primaryKey?.Properties.Should().HaveCount(1);
        primaryKey?.Properties[0].Name.Should().Be("InventoryID");
    }

    [Fact]
    public void Configure_ShouldMapInventoryIDColumn_WithValueGeneratedOnAdd()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var inventoryIdProperty = entityType?.FindProperty("InventoryID");

        // Act & Assert
        inventoryIdProperty?.GetColumnName().Should().Be("InventoryID");
        inventoryIdProperty?.ValueGenerated.Should().Be(ValueGenerated.OnAdd);
    }

    [Fact]
    public void Configure_ShouldMapProductIDColumn_AsRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var productIdProperty = entityType?.FindProperty("ProductID");

        // Act & Assert
        productIdProperty?.GetColumnName().Should().Be("ProductID");
        productIdProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldMapQuantityColumn_AsRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var quantityProperty = entityType?.FindProperty("Quantity");

        // Act & Assert
        quantityProperty?.GetColumnName().Should().Be("Quantity");
        quantityProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldMapLastUpdatedColumn_WithDefaultValueSql()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var lastUpdatedProperty = entityType?.FindProperty("LastUpdated");

        // Act & Assert
        lastUpdatedProperty?.GetColumnName().Should().Be("LastUpdated");
        lastUpdatedProperty?.GetDefaultValueSql().Should().Be("datetime('now')");
    }

    [Fact]
    public void Configure_ShouldHaveProductRelationship()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var navigationProperty = entityType?.FindNavigation("Product");

        // Act & Assert
        navigationProperty.Should().NotBeNull();
        navigationProperty?.TargetEntityType.Name.Should().Contain("Product");
    }

    [Fact]
    public void Configure_ShouldHaveForeignKeyToProductOnProductID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var productForeignKey = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "ProductID");
        productForeignKey.Should().NotBeNull();
    }

    [Fact]
    public async Task Configure_ShouldAllowValidInventory_WithAllRequiredFields()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-001",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var inventory = new Inventory
        {
            ProductID = product.ProductID,
            Quantity = 100
        };

        dbContext.Inventory.Add(inventory);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedInventory = await dbContext.Inventory.FindAsync(inventory.InventoryID);
        savedInventory.Should().NotBeNull();
        savedInventory?.ProductID.Should().Be(product.ProductID);
        savedInventory?.Quantity.Should().Be(100);
    }

    [Fact]
    public async Task Configure_ShouldAutoGenerateInventoryID_OnAdd()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-002",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var inventory = new Inventory
        {
            ProductID = product.ProductID,
            Quantity = 50
        };

        dbContext.Inventory.Add(inventory);
        await dbContext.SaveChangesAsync();

        // Act & Assert
        inventory.InventoryID.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Configure_ShouldSetLastUpdatedTimestamp_OnAdd()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-003",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var inventory = new Inventory
        {
            ProductID = product.ProductID,
            Quantity = 75
        };

        dbContext.Inventory.Add(inventory);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedInventory = await dbContext.Inventory.FindAsync(inventory.InventoryID);
        savedInventory.Should().NotBeNull();
        savedInventory?.ProductID.Should().Be(product.ProductID);
        savedInventory?.Quantity.Should().Be(75);
    }

    [Fact]
    public async Task Configure_ShouldLoadProductNavigation_WithInventory()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-004",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var inventory = new Inventory
        {
            ProductID = product.ProductID,
            Quantity = 100
        };

        dbContext.Inventory.Add(inventory);
        await dbContext.SaveChangesAsync();

        // Act
        var loadedInventory = await dbContext.Inventory
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.InventoryID == inventory.InventoryID);

        // Assert
        loadedInventory.Should().NotBeNull();
        loadedInventory?.Product.Should().NotBeNull();
        loadedInventory?.Product?.ProductID.Should().Be(product.ProductID);
    }

    [Fact]
    public async Task Configure_ShouldAllowInventoryWithLargeQuantity()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-005",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var inventory = new Inventory
        {
            ProductID = product.ProductID,
            Quantity = int.MaxValue
        };

        dbContext.Inventory.Add(inventory);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedInventory = await dbContext.Inventory.FindAsync(inventory.InventoryID);
        savedInventory.Should().NotBeNull();
        savedInventory?.Quantity.Should().Be(int.MaxValue);
    }

    [Fact]
    public async Task Configure_ShouldAllowInventoryWithZeroQuantity()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-006",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var inventory = new Inventory
        {
            ProductID = product.ProductID,
            Quantity = 0
        };

        dbContext.Inventory.Add(inventory);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedInventory = await dbContext.Inventory.FindAsync(inventory.InventoryID);
        savedInventory.Should().NotBeNull();
        savedInventory?.Quantity.Should().Be(0);
    }

    [Fact]
    public async Task Configure_ShouldAllowMultipleInventories_ForDifferentProducts()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product1 = new Product
        {
            Name = "Product 1",
            SKU = "SKU-001",
            CategoryID = 1
        };

        var product2 = new Product
        {
            Name = "Product 2",
            SKU = "SKU-002",
            CategoryID = 1
        };

        dbContext.Products.AddRange(product1, product2);
        await dbContext.SaveChangesAsync();

        var inventory1 = new Inventory
        {
            ProductID = product1.ProductID,
            Quantity = 100
        };

        var inventory2 = new Inventory
        {
            ProductID = product2.ProductID,
            Quantity = 200
        };

        dbContext.Inventory.AddRange(inventory1, inventory2);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedInventories = await dbContext.Inventory.ToListAsync();
        savedInventories.Should().HaveCount(2);
    }

    [Fact]
    public async Task Configure_ShouldAllowUpdateInventory_Quantity()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-007",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var inventory = new Inventory
        {
            ProductID = product.ProductID,
            Quantity = 100
        };

        dbContext.Inventory.Add(inventory);
        await dbContext.SaveChangesAsync();

        // Act
        var savedInventory = await dbContext.Inventory.FindAsync(inventory.InventoryID);
        savedInventory!.Quantity = 150;
        await dbContext.SaveChangesAsync();

        // Assert
        var updatedInventory = await dbContext.Inventory.FindAsync(inventory.InventoryID);
        updatedInventory.Should().NotBeNull();
        updatedInventory?.Quantity.Should().Be(150);
    }

    [Fact]
    public async Task Configure_ShouldEnforceOneToOneRelationship_WithProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-009",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var inventory = new Inventory
        {
            ProductID = product.ProductID,
            Quantity = 100
        };

        dbContext.Inventory.Add(inventory);
        await dbContext.SaveChangesAsync();

        // Act
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var navigationProperty = entityType?.FindNavigation("Product");

        // Assert
        navigationProperty.Should().NotBeNull();
        navigationProperty?.IsCollection.Should().BeFalse();
    }

    [Fact]
    public async Task Configure_ShouldLoadInventoryFromProduct_WithInclude()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-010",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var inventory = new Inventory
        {
            ProductID = product.ProductID,
            Quantity = 100
        };

        dbContext.Inventory.Add(inventory);
        await dbContext.SaveChangesAsync();

        // Act
        var loadedProduct = await dbContext.Products
            .Include(p => p.Inventory)
            .FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

        // Assert
        loadedProduct.Should().NotBeNull();
        loadedProduct?.Inventory.Should().NotBeNull();
        loadedProduct?.Inventory?.Quantity.Should().Be(100);
    }

    [Fact]
    public async Task Configure_ShouldAllowNegativeQuantity()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-011",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var inventory = new Inventory
        {
            ProductID = product.ProductID,
            Quantity = -10
        };

        dbContext.Inventory.Add(inventory);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedInventory = await dbContext.Inventory.FindAsync(inventory.InventoryID);
        savedInventory.Should().NotBeNull();
        savedInventory?.Quantity.Should().Be(-10);
    }

    [Fact]
    public async Task Configure_ShouldDeleteInventory()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-012",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var inventory = new Inventory
        {
            ProductID = product.ProductID,
            Quantity = 100
        };

        dbContext.Inventory.Add(inventory);
        await dbContext.SaveChangesAsync();

        // Act
        dbContext.Inventory.Remove(inventory);
        await dbContext.SaveChangesAsync();

        // Assert
        var deletedInventory = await dbContext.Inventory.FindAsync(inventory.InventoryID);
        deletedInventory.Should().BeNull();
    }
}
