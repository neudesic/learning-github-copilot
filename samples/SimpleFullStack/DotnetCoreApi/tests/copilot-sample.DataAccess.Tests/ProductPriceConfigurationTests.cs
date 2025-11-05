using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.DataAccess.EntityConfiguration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

public class ProductPriceConfigurationTests
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
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));

        // Act & Assert
        entityType?.GetTableName().Should().Be("ProductPrices");
    }

    [Fact]
    public void Configure_ShouldHavePrimaryKey_OnPriceID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var primaryKey = entityType?.FindPrimaryKey();

        // Act & Assert
        primaryKey.Should().NotBeNull();
        primaryKey?.Properties.Should().HaveCount(1);
        primaryKey?.Properties[0].Name.Should().Be("PriceID");
    }

    [Fact]
    public void Configure_ShouldMapPriceIDColumn_WithValueGeneratedOnAdd()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var priceIdProperty = entityType?.FindProperty("PriceID");

        // Act & Assert
        priceIdProperty?.GetColumnName().Should().Be("PriceID");
        priceIdProperty?.ValueGenerated.Should().Be(ValueGenerated.OnAdd);
    }

    [Fact]
    public void Configure_ShouldMapProductIDColumn_AsRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var productIdProperty = entityType?.FindProperty("ProductID");

        // Act & Assert
        productIdProperty?.GetColumnName().Should().Be("ProductID");
        productIdProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldMapPriceColumn_WithPrecision18_2()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var priceProperty = entityType?.FindProperty("Price");

        // Act & Assert
        priceProperty?.GetColumnName().Should().Be("Price");
        var precision = priceProperty?.GetPrecision();
        var scale = priceProperty?.GetScale();
        precision.Should().Be(18);
        scale.Should().Be(2);
        priceProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldMapCurrencyCodeColumn_WithMaxLength3()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var currencyCodeProperty = entityType?.FindProperty("CurrencyCode");

        // Act & Assert
        currencyCodeProperty?.GetColumnName().Should().Be("CurrencyCode");
        currencyCodeProperty?.GetMaxLength().Should().Be(3);
    }

    [Fact]
    public void Configure_ShouldMapCurrencyCodeColumn_WithDefaultValue_USD()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var currencyCodeProperty = entityType?.FindProperty("CurrencyCode");

        // Act & Assert
        currencyCodeProperty?.GetDefaultValue().Should().Be("USD");
    }

    [Fact]
    public void Configure_ShouldMapEffectiveFromColumn_WithDefaultValueSql()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var effectiveFromProperty = entityType?.FindProperty("EffectiveFrom");

        // Act & Assert
        effectiveFromProperty?.GetColumnName().Should().Be("EffectiveFrom");
        effectiveFromProperty?.GetDefaultValueSql().Should().Be("datetime('now')");
    }

    [Fact]
    public void Configure_ShouldMapEffectiveTillColumn_AsNullable()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var effectiveTillProperty = entityType?.FindProperty("EffectiveTill");

        // Act & Assert
        effectiveTillProperty?.GetColumnName().Should().Be("EffectiveTill");
        effectiveTillProperty?.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void Configure_ShouldHaveForeignKeyRelationship_ThroughProductID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var productForeignKey = foreignKeys?.FirstOrDefault(fk => fk.PrincipalEntityType?.Name.EndsWith("Product") ?? false);
        productForeignKey.Should().NotBeNull();
        productForeignKey?.Properties.Should().HaveCount(1);
        productForeignKey?.Properties[0].Name.Should().Be("ProductID");
    }

    [Fact]
    public void Configure_ShouldHaveNavigationProperty_ToProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var navigationProperty = entityType?.FindNavigation("Product");

        // Act & Assert
        navigationProperty.Should().NotBeNull();
        navigationProperty?.TargetEntityType.Name.Should().Contain("Product");
    }

    [Fact]
    public async Task Configure_ShouldAllowValidProductPrice_WithAllRequiredFields()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var productPrice = new ProductPrice
        {
            ProductID = 1,
            Price = 99.99m,
            CurrencyCode = "USD",
            EffectiveFrom = DateTime.UtcNow,
            EffectiveTill = null
        };

        dbContext.ProductPrices.Add(productPrice);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedPrice = await dbContext.ProductPrices.FindAsync(productPrice.PriceID);
        savedPrice.Should().NotBeNull();
        savedPrice?.Price.Should().Be(99.99m);
        savedPrice?.CurrencyCode.Should().Be("USD");
    }

    [Fact]
    public async Task Configure_ShouldAutoGeneratePriceID_OnAdd()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var productPrice = new ProductPrice
        {
            ProductID = 1,
            Price = 49.50m,
            CurrencyCode = "EUR",
            EffectiveFrom = DateTime.UtcNow
        };

        dbContext.ProductPrices.Add(productPrice);
        await dbContext.SaveChangesAsync();

        // Act & Assert
        productPrice.PriceID.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Configure_ShouldUseDefaultCurrencyCode_WhenNotSpecified()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var productPrice = new ProductPrice
        {
            ProductID = 1,
            Price = 29.99m,
            EffectiveFrom = DateTime.UtcNow
        };

        dbContext.ProductPrices.Add(productPrice);
        await dbContext.SaveChangesAsync();

        // Act & Assert
        var savedPrice = await dbContext.ProductPrices.FindAsync(productPrice.PriceID);
        savedPrice.Should().NotBeNull();
        savedPrice?.CurrencyCode.Should().Be("USD");
    }

    [Fact]
    public async Task Configure_ShouldAllowNullEffectiveTill()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var productPrice = new ProductPrice
        {
            ProductID = 1,
            Price = 59.99m,
            CurrencyCode = "GBP",
            EffectiveFrom = DateTime.UtcNow,
            EffectiveTill = null
        };

        dbContext.ProductPrices.Add(productPrice);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedPrice = await dbContext.ProductPrices.FindAsync(productPrice.PriceID);
        savedPrice.Should().NotBeNull();
        savedPrice?.EffectiveTill.Should().BeNull();
    }

    [Fact]
    public async Task Configure_ShouldAllowMultiplePrices_ForSameProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var prices = new List<ProductPrice>
        {
            new ProductPrice { ProductID = 1, Price = 99.99m, CurrencyCode = "USD", EffectiveFrom = DateTime.UtcNow },
            new ProductPrice { ProductID = 1, Price = 89.99m, CurrencyCode = "EUR", EffectiveFrom = DateTime.UtcNow.AddDays(1) },
            new ProductPrice { ProductID = 1, Price = 79.99m, CurrencyCode = "GBP", EffectiveFrom = DateTime.UtcNow.AddDays(2) }
        };

        dbContext.ProductPrices.AddRange(prices);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedPrices = await dbContext.ProductPrices.Where(pp => pp.ProductID == 1).ToListAsync();
        savedPrices.Should().HaveCount(3);
    }

    [Fact]
    public async Task Configure_ShouldLoadProductNavigation_WithPrices()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        dbContext.ProductPrices.AddRange(new List<ProductPrice>
        {
            new ProductPrice { ProductID = 1, Price = 100m, CurrencyCode = "USD", EffectiveFrom = DateTime.UtcNow },
            new ProductPrice { ProductID = 1, Price = 90m, CurrencyCode = "EUR", EffectiveFrom = DateTime.UtcNow }
        });
        await dbContext.SaveChangesAsync();

        // Act
        var loadedProduct = await dbContext.Products
            .Include(p => p.ProductPrices)
            .FirstOrDefaultAsync(p => p.ProductID == 1);

        // Assert
        loadedProduct.Should().NotBeNull();
        loadedProduct?.ProductPrices.Should().HaveCount(2);
    }

    [Fact]
    public async Task Configure_ShouldEnforceMaxLength_OnCurrencyCode()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var currencyCodeProperty = entityType?.FindProperty("CurrencyCode");

        // Act & Assert
        currencyCodeProperty?.GetMaxLength().Should().Be(3);
    }

    [Fact]
    public async Task Configure_ShouldNotEnforceForeignKeyConstraint_InMemory()
    {
        // Arrange - In-memory database doesn't enforce FK constraints by default
        var dbContext = GetInMemoryDbContext();
        var price = new ProductPrice
        {
            PriceID = 1,
            ProductID = 999,
            Price = 50m,
            CurrencyCode = "USD",
            EffectiveFrom = DateTime.UtcNow
        };

        dbContext.ProductPrices.Add(price);

        // Act & Assert - In-memory allows orphaned prices
        await dbContext.SaveChangesAsync();
        var savedPrice = await dbContext.ProductPrices.FindAsync(1);
        savedPrice.Should().NotBeNull();
    }

    [Fact]
    public async Task Configure_ShouldHandleLargeDecimalPrice_WithPrecision18_2()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var largePrice = new ProductPrice
        {
            ProductID = 1,
            Price = 9999999999999999.99m,
            CurrencyCode = "USD",
            EffectiveFrom = DateTime.UtcNow
        };

        dbContext.ProductPrices.Add(largePrice);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedPrice = await dbContext.ProductPrices.FindAsync(largePrice.PriceID);
        savedPrice.Should().NotBeNull();
        savedPrice?.Price.Should().Be(9999999999999999.99m);
    }

    [Fact]
    public async Task Configure_ShouldSupportDifferentCurrencyCodes()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var currencies = new[] { "USD", "EUR", "GBP", "JPY", "AUD" };
        var prices = currencies.Select((code, index) => new ProductPrice
        {
            ProductID = 1,
            Price = 100m,
            CurrencyCode = code,
            EffectiveFrom = DateTime.UtcNow.AddDays(index)
        }).ToList();

        dbContext.ProductPrices.AddRange(prices);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedPrices = await dbContext.ProductPrices.Where(pp => pp.ProductID == 1).ToListAsync();
        savedPrices.Should().HaveCount(5);
        savedPrices.Select(p => p.CurrencyCode).Should().Contain(currencies);
    }

    [Fact]
    public async Task Configure_ShouldHandleDateRanges_EffectiveFromAndTill()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var startDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = new DateTime(2024, 12, 31, 23, 59, 59, DateTimeKind.Utc);

        var productPrice = new ProductPrice
        {
            ProductID = 1,
            Price = 75.50m,
            CurrencyCode = "USD",
            EffectiveFrom = startDate,
            EffectiveTill = endDate
        };

        dbContext.ProductPrices.Add(productPrice);
        await dbContext.SaveChangesAsync();

        // Act
        var savedPrice = await dbContext.ProductPrices.FindAsync(productPrice.PriceID);

        // Assert
        savedPrice.Should().NotBeNull();
        savedPrice?.EffectiveFrom.Should().Be(startDate);
        savedPrice?.EffectiveTill.Should().Be(endDate);
    }
}
