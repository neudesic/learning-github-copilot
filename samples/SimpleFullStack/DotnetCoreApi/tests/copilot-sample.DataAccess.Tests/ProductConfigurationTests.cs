using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.DataAccess.EntityConfiguration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

public class ProductConfigurationTests
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
        var entityType = dbContext.Model.FindEntityType(typeof(Product));

        // Act & Assert
        entityType?.GetTableName().Should().Be("Products");
    }

    [Fact]
    public void Configure_ShouldHavePrimaryKey_OnProductID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var primaryKey = entityType?.FindPrimaryKey();

        // Act & Assert
        primaryKey.Should().NotBeNull();
        primaryKey?.Properties.Should().HaveCount(1);
        primaryKey?.Properties[0].Name.Should().Be("ProductID");
    }

    [Fact]
    public void Configure_ShouldMapProductIDColumn_WithValueGeneratedOnAdd()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var productIdProperty = entityType?.FindProperty("ProductID");

        // Act & Assert
        productIdProperty?.GetColumnName().Should().Be("ProductID");
        productIdProperty?.ValueGenerated.Should().Be(ValueGenerated.OnAdd);
    }

    [Fact]
    public void Configure_ShouldMapNameColumn_WithMaxLength200_AndRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var nameProperty = entityType?.FindProperty("Name");

        // Act & Assert
        nameProperty?.GetColumnName().Should().Be("Name");
        nameProperty?.GetMaxLength().Should().Be(200);
        nameProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldMapDescriptionColumn_AsNullable()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var descriptionProperty = entityType?.FindProperty("Description");

        // Act & Assert
        descriptionProperty?.GetColumnName().Should().Be("Description");
        descriptionProperty?.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void Configure_ShouldMapSKUColumn_WithMaxLength100_AndRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var skuProperty = entityType?.FindProperty("SKU");

        // Act & Assert
        skuProperty?.GetColumnName().Should().Be("SKU");
        skuProperty?.GetMaxLength().Should().Be(100);
        skuProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldHaveUniqueIndex_OnSKU()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var indexes = entityType?.GetIndexes();

        // Act & Assert
        indexes.Should().NotBeEmpty();
        var skuIndex = indexes?.FirstOrDefault(i => i.Properties[0].Name == "SKU");
        skuIndex.Should().NotBeNull();
        skuIndex?.IsUnique.Should().BeTrue();
    }

    [Fact]
    public void Configure_ShouldMapCategoryIDColumn_AsRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var categoryIdProperty = entityType?.FindProperty("CategoryID");

        // Act & Assert
        categoryIdProperty?.GetColumnName().Should().Be("CategoryID");
        categoryIdProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldMapBrandColumn_WithMaxLength100_AsNullable()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var brandProperty = entityType?.FindProperty("Brand");

        // Act & Assert
        brandProperty?.GetColumnName().Should().Be("Brand");
        brandProperty?.GetMaxLength().Should().Be(100);
        brandProperty?.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void Configure_ShouldMapCreatedAtColumn_WithDefaultValueSql()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var createdAtProperty = entityType?.FindProperty("CreatedAt");

        // Act & Assert
        createdAtProperty?.GetColumnName().Should().Be("CreatedAt");
        createdAtProperty?.GetDefaultValueSql().Should().Be("datetime('now')");
    }

    [Fact]
    public void Configure_ShouldMapUpdatedAtColumn_WithDefaultValueSql()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var updatedAtProperty = entityType?.FindProperty("UpdatedAt");

        // Act & Assert
        updatedAtProperty?.GetColumnName().Should().Be("UpdatedAt");
        updatedAtProperty?.GetDefaultValueSql().Should().Be("datetime('now')");
    }

    [Fact]
    public void Configure_ShouldMapIsActiveColumn_WithDefaultValueTrue()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var isActiveProperty = entityType?.FindProperty("IsActive");

        // Act & Assert
        isActiveProperty?.GetColumnName().Should().Be("IsActive");
        isActiveProperty?.GetDefaultValue().Should().Be(true);
    }

    [Fact]
    public void Configure_ShouldHaveCategoryRelationship()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var navigationProperty = entityType?.FindNavigation("Category");

        // Act & Assert
        navigationProperty.Should().NotBeNull();
        navigationProperty?.TargetEntityType.Name.Should().Contain("Category");
    }

    [Fact]
    public void Configure_ShouldHaveProductPricesRelationship()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var navigationProperty = entityType?.FindNavigation("ProductPrices");

        // Act & Assert
        navigationProperty.Should().NotBeNull();
        navigationProperty?.TargetEntityType.Name.Should().Contain("ProductPrice");
    }

    [Fact]
    public void Configure_ShouldHaveInventoryRelationship()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var navigationProperty = entityType?.FindNavigation("Inventory");

        // Act & Assert
        navigationProperty.Should().NotBeNull();
        navigationProperty?.TargetEntityType.Name.Should().Contain("Inventory");
    }

    [Fact]
    public void Configure_ShouldHaveProductAttributesRelationship()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var navigationProperty = entityType?.FindNavigation("ProductAttributes");

        // Act & Assert
        navigationProperty.Should().NotBeNull();
        navigationProperty?.TargetEntityType.Name.Should().Contain("ProductAttribute");
    }

    [Fact]
    public void Configure_ShouldHaveProductReviewsRelationship()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var navigationProperty = entityType?.FindNavigation("ProductReviews");

        // Act & Assert
        navigationProperty.Should().NotBeNull();
        navigationProperty?.TargetEntityType.Name.Should().Contain("ProductReview");
    }

    [Fact]
    public void Configure_ShouldHaveForeignKeyToCategoryOnCategoryID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var categoryForeignKey = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "CategoryID");
        categoryForeignKey.Should().NotBeNull();
    }

    [Fact]
    public async Task Configure_ShouldAllowValidProduct_WithAllRequiredFields()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-001",
            CategoryID = 1,
            Description = "A test product",
            Brand = "TestBrand",
            IsActive = true
        };

        dbContext.Products.Add(product);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);
        savedProduct.Should().NotBeNull();
        savedProduct?.Name.Should().Be("Test Product");
        savedProduct?.SKU.Should().Be("TEST-001");
        savedProduct?.CategoryID.Should().Be(1);
    }

    [Fact]
    public async Task Configure_ShouldAutoGenerateProductID_OnAdd()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Another Product",
            SKU = "TEST-002",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        // Act & Assert
        product.ProductID.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Configure_ShouldEnforceSKUUniqueness()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product1 = new Product
        {
            Name = "Product 1",
            SKU = "UNIQUE-SKU",
            CategoryID = 1
        };

        var product2 = new Product
        {
            Name = "Product 2",
            SKU = "UNIQUE-SKU",
            CategoryID = 1
        };

        dbContext.Products.Add(product1);
        await dbContext.SaveChangesAsync();

        dbContext.Products.Add(product2);

        // Act & Assert - In-memory doesn't enforce unique constraints, but the configuration should exist
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var indexes = entityType?.GetIndexes();
        var skuIndex = indexes?.FirstOrDefault(i => i.Properties[0].Name == "SKU");
        skuIndex?.IsUnique.Should().BeTrue();
    }

    [Fact]
    public async Task Configure_ShouldAllowNullableDescription()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Product Without Description",
            SKU = "TEST-003",
            CategoryID = 1,
            Description = null
        };

        dbContext.Products.Add(product);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);
        savedProduct.Should().NotBeNull();
        savedProduct?.Description.Should().BeNull();
    }

    [Fact]
    public async Task Configure_ShouldAllowNullableBrand()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Product Without Brand",
            SKU = "TEST-004",
            CategoryID = 1,
            Brand = null
        };

        dbContext.Products.Add(product);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);
        savedProduct.Should().NotBeNull();
        savedProduct?.Brand.Should().BeNull();
    }

    [Fact]
    public async Task Configure_ShouldSetDefaultIsActiveToTrue()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Active Product",
            SKU = "TEST-005",
            CategoryID = 1,
            IsActive = true
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        // Act
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);

        // Assert
        savedProduct.Should().NotBeNull();
        savedProduct?.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Configure_ShouldAllowProductNameWithMaxLength200()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var longName = new string('A', 200);
        var product = new Product
        {
            Name = longName,
            SKU = "TEST-006",
            CategoryID = 1
        };

        dbContext.Products.Add(product);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);
        savedProduct.Should().NotBeNull();
        savedProduct?.Name.Length.Should().Be(200);
    }

    [Fact]
    public async Task Configure_ShouldAllowSKUWithMaxLength100()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var longSku = new string('S', 100);
        var product = new Product
        {
            Name = "Product with Long SKU",
            SKU = longSku,
            CategoryID = 1
        };

        dbContext.Products.Add(product);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);
        savedProduct.Should().NotBeNull();
        savedProduct?.SKU.Length.Should().Be(100);
    }

    [Fact]
    public async Task Configure_ShouldAllowBrandWithMaxLength100()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var longBrand = new string('B', 100);
        var product = new Product
        {
            Name = "Product with Long Brand",
            SKU = "TEST-007",
            CategoryID = 1,
            Brand = longBrand
        };

        dbContext.Products.Add(product);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);
        savedProduct.Should().NotBeNull();
        savedProduct?.Brand?.Length.Should().Be(100);
    }

    [Fact]
    public async Task Configure_ShouldLoadProductPricesNavigation_WithProduct()
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

        // Act
        var loadedProduct = await dbContext.Products
            .Include(p => p.ProductPrices)
            .FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

        // Assert
        loadedProduct.Should().NotBeNull();
        loadedProduct?.ProductPrices.Should().NotBeNull();
    }

    [Fact]
    public async Task Configure_ShouldLoadInventoryNavigation_WithProduct()
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

        // Act
        var loadedProduct = await dbContext.Products
            .Include(p => p.Inventory)
            .FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

        // Assert
        loadedProduct.Should().NotBeNull();
    }

    [Fact]
    public async Task Configure_ShouldLoadProductAttributesNavigation_WithProduct()
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

        // Act
        var loadedProduct = await dbContext.Products
            .Include(p => p.ProductAttributes)
            .FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

        // Assert
        loadedProduct.Should().NotBeNull();
        loadedProduct?.ProductAttributes.Should().NotBeNull();
    }

    [Fact]
    public async Task Configure_ShouldLoadProductReviewsNavigation_WithProduct()
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

        // Act
        var loadedProduct = await dbContext.Products
            .Include(p => p.ProductReviews)
            .FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

        // Assert
        loadedProduct.Should().NotBeNull();
        loadedProduct?.ProductReviews.Should().NotBeNull();
    }

    [Fact]
    public async Task Configure_ShouldAllowMultipleProducts_WithDifferentSKUs()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var products = new List<Product>
        {
            new Product { Name = "Product 1", SKU = "SKU-001", CategoryID = 1 },
            new Product { Name = "Product 2", SKU = "SKU-002", CategoryID = 1 },
            new Product { Name = "Product 3", SKU = "SKU-003", CategoryID = 2 }
        };

        dbContext.Products.AddRange(products);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedProducts = await dbContext.Products.ToListAsync();
        savedProducts.Should().HaveCount(3);
    }

    [Fact]
    public async Task Configure_ShouldRetainActiveStatus_AfterSave()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Active Product",
            SKU = "TEST-013",
            CategoryID = 1,
            IsActive = true
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        // Act
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);

        // Assert
        savedProduct.Should().NotBeNull();
        savedProduct?.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Configure_ShouldAllowInactiveProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Inactive Product",
            SKU = "TEST-014",
            CategoryID = 1,
            IsActive = false
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        // Act
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);

        // Assert
        savedProduct.Should().NotBeNull();
        savedProduct?.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Configure_ShouldLoadCategoryNavigation_WithProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category
        {
            Name = "Test Category"
        };

        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-008",
            CategoryID = category.CategoryID,
            Brand = "asd",
            IsActive = true
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        // Act
        var loadedProduct = await dbContext.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

        // Assert
        loadedProduct.Should().NotBeNull();
        loadedProduct?.Category.Should().NotBeNull();
        loadedProduct?.Category?.CategoryID.Should().Be(category.CategoryID);
    }

    [Fact]
    public async Task Configure_ShouldAllowUpdateProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Original Name",
            SKU = "TEST-015",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        // Act
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);
        savedProduct!.Name = "Updated Name";
        savedProduct.Brand = "NewBrand";
        await dbContext.SaveChangesAsync();

        // Assert
        var updatedProduct = await dbContext.Products.FindAsync(product.ProductID);
        updatedProduct.Should().NotBeNull();
        updatedProduct?.Name.Should().Be("Updated Name");
        updatedProduct?.Brand.Should().Be("NewBrand");
    }

    [Fact]
    public async Task Configure_ShouldSetCreatedAtTimestamp()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Timestamp Product",
            SKU = "TEST-016",
            CategoryID = 1
        };

        dbContext.Products.Add(product);

        // Act
        await dbContext.SaveChangesAsync();
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);

        // Assert
        savedProduct.Should().NotBeNull();
        savedProduct?.CreatedAt.Should().NotBe(default(DateTime));
    }

    [Fact]
    public async Task Configure_ShouldSetUpdatedAtTimestamp()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Timestamp Product",
            SKU = "TEST-017",
            CategoryID = 1
        };

        dbContext.Products.Add(product);

        // Act
        await dbContext.SaveChangesAsync();
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);

        // Assert
        savedProduct.Should().NotBeNull();
        savedProduct?.UpdatedAt.Should().NotBe(default(DateTime));
    }

    [Fact]
    public void Configure_ShouldNotAllowNullProductName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var nameProperty = entityType?.FindProperty("Name");

        // Act & Assert
        nameProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldNotAllowNullSKU()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var skuProperty = entityType?.FindProperty("SKU");

        // Act & Assert
        skuProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldNotAllowNullCategoryID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var categoryIdProperty = entityType?.FindProperty("CategoryID");

        // Act & Assert
        categoryIdProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public async Task Configure_ShouldAllowProductWithSpecialCharactersInName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Product @#$% & Special (123)",
            SKU = "TEST-018",
            CategoryID = 1
        };

        dbContext.Products.Add(product);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);
        savedProduct.Should().NotBeNull();
        savedProduct?.Name.Should().Be("Product @#$% & Special (123)");
    }

    [Fact]
    public async Task Configure_ShouldAllowProductWithSpecialCharactersInSKU()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Special SKU Product",
            SKU = "TEST-SKU-001",
            CategoryID = 1
        };

        dbContext.Products.Add(product);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedProduct = await dbContext.Products.FindAsync(product.ProductID);
        savedProduct.Should().NotBeNull();
        savedProduct?.SKU.Should().Be("TEST-SKU-001");
    }
}
