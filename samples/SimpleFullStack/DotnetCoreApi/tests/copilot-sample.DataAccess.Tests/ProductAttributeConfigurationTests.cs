using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.DataAccess.EntityConfiguration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

public class ProductAttributeConfigurationTests
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
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));

        // Act & Assert
        entityType?.GetTableName().Should().Be("ProductAttributes");
    }

    [Fact]
    public void Configure_ShouldHavePrimaryKey_OnAttributeID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var primaryKey = entityType?.FindPrimaryKey();

        // Act & Assert
        primaryKey.Should().NotBeNull();
        primaryKey?.Properties.Should().HaveCount(1);
        primaryKey?.Properties[0].Name.Should().Be("AttributeID");
    }

    [Fact]
    public void Configure_ShouldMapAttributeIDColumn_WithValueGeneratedOnAdd()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var attributeIdProperty = entityType?.FindProperty("AttributeID");

        // Act & Assert
        attributeIdProperty?.GetColumnName().Should().Be("AttributeID");
        attributeIdProperty?.ValueGenerated.Should().Be(ValueGenerated.OnAdd);
    }

    [Fact]
    public void Configure_ShouldMapProductIDColumn_AsRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var productIdProperty = entityType?.FindProperty("ProductID");

        // Act & Assert
        productIdProperty?.GetColumnName().Should().Be("ProductID");
        productIdProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldMapAttributeNameColumn_WithMaxLength100()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var attributeNameProperty = entityType?.FindProperty("AttributeName");

        // Act & Assert
        attributeNameProperty?.GetColumnName().Should().Be("AttributeName");
        attributeNameProperty?.GetMaxLength().Should().Be(100);
    }

    [Fact]
    public void Configure_ShouldMapAttributeValueColumn_WithMaxLength255()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var attributeValueProperty = entityType?.FindProperty("AttributeValue");

        // Act & Assert
        attributeValueProperty?.GetColumnName().Should().Be("AttributeValue");
        attributeValueProperty?.GetMaxLength().Should().Be(255);
    }

    [Fact]
    public void Configure_ShouldHaveProductRelationship()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
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
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var productForeignKey = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "ProductID");
        productForeignKey.Should().NotBeNull();
    }

    [Fact]
    public async Task Configure_ShouldAllowValidProductAttribute_WithAllRequiredFields()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-001",
            CategoryID = 1
        };

        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "Color",
            AttributeValue = "Red"
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        savedAttribute.Should().NotBeNull();
        savedAttribute?.AttributeName.Should().Be("Color");
        savedAttribute?.AttributeValue.Should().Be("Red");
    }

    [Fact]
    public async Task Configure_ShouldAutoGenerateAttributeID_OnAdd()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-002",
            CategoryID = 1
        };

        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "Size",
            AttributeValue = "Large"
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);
        await dbContext.SaveChangesAsync();

        // Act & Assert
        productAttribute.AttributeID.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Configure_ShouldAllowAttributeNameWithMaxLength100()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-003",
            CategoryID = 1
        };

        var longAttributeName = new string('A', 100);
        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = longAttributeName,
            AttributeValue = "Value"
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        savedAttribute.Should().NotBeNull();
        savedAttribute?.AttributeName.Length.Should().Be(100);
    }

    [Fact]
    public async Task Configure_ShouldAllowAttributeValueWithMaxLength255()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-004",
            CategoryID = 1
        };

        var longAttributeValue = new string('V', 255);
        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "LongValue",
            AttributeValue = longAttributeValue
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        savedAttribute.Should().NotBeNull();
        savedAttribute?.AttributeValue.Length.Should().Be(255);
    }

    [Fact]
    public async Task Configure_ShouldAllowMultipleAttributesPerProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Multi-Attribute Product",
            SKU = "TEST-005",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var attributes = new List<ProductAttribute>
        {
            new ProductAttribute { ProductID = product.ProductID, AttributeName = "Color", AttributeValue = "Blue" },
            new ProductAttribute { ProductID = product.ProductID, AttributeName = "Size", AttributeValue = "Medium" },
            new ProductAttribute { ProductID = product.ProductID, AttributeName = "Material", AttributeValue = "Cotton" }
        };

        dbContext.ProductAttributes.AddRange(attributes);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedAttributes = await dbContext.ProductAttributes
            .Where(pa => pa.ProductID == product.ProductID)
            .ToListAsync();
        savedAttributes.Should().HaveCount(3);
    }

    [Fact]
    public async Task Configure_ShouldLoadProductNavigation_WithProductAttribute()
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

        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "Weight",
            AttributeValue = "500g"
        };

        dbContext.ProductAttributes.Add(productAttribute);
        await dbContext.SaveChangesAsync();

        // Act
        var loadedAttribute = await dbContext.ProductAttributes
            .Include(pa => pa.Product)
            .FirstOrDefaultAsync(pa => pa.AttributeID == productAttribute.AttributeID);

        // Assert
        loadedAttribute.Should().NotBeNull();
        loadedAttribute?.Product.Should().NotBeNull();
        loadedAttribute?.Product?.Name.Should().Be("Test Product");
    }

    [Fact]
    public async Task Configure_ShouldAllowUpdateProductAttribute()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-007",
            CategoryID = 1
        };

        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "Color",
            AttributeValue = "Red"
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);
        await dbContext.SaveChangesAsync();

        // Act
        var savedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        savedAttribute!.AttributeValue = "Blue";
        await dbContext.SaveChangesAsync();

        // Assert
        var updatedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        updatedAttribute.Should().NotBeNull();
        updatedAttribute?.AttributeValue.Should().Be("Blue");
    }

    [Fact]
    public async Task Configure_ShouldAllowDeleteProductAttribute()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-008",
            CategoryID = 1
        };

        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "Color",
            AttributeValue = "Red"
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);
        await dbContext.SaveChangesAsync();

        // Act
        dbContext.ProductAttributes.Remove(productAttribute);
        await dbContext.SaveChangesAsync();

        // Assert
        var deletedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        deletedAttribute.Should().BeNull();
    }

    [Fact]
    public void Configure_ShouldNotAllowNullAttributeName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var attributeNameProperty = entityType?.FindProperty("AttributeName");

        // Act & Assert
        attributeNameProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldNotAllowNullAttributeValue()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var attributeValueProperty = entityType?.FindProperty("AttributeValue");

        // Act & Assert
        attributeValueProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldNotAllowNullProductID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var productIdProperty = entityType?.FindProperty("ProductID");

        // Act & Assert
        productIdProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public async Task Configure_ShouldAllowAttributeNameWithSpecialCharacters()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-009",
            CategoryID = 1
        };

        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "Attr@#$%-Name",
            AttributeValue = "Value"
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        savedAttribute.Should().NotBeNull();
        savedAttribute?.AttributeName.Should().Be("Attr@#$%-Name");
    }

    [Fact]
    public async Task Configure_ShouldAllowAttributeValueWithSpecialCharacters()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-010",
            CategoryID = 1
        };

        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "SpecialValue",
            AttributeValue = "Value@#$%-123"
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        savedAttribute.Should().NotBeNull();
        savedAttribute?.AttributeValue.Should().Be("Value@#$%-123");
    }

    [Fact]
    public async Task Configure_ShouldAllowAttributeNameWithWhitespace()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-011",
            CategoryID = 1
        };

        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "Attribute Name With Spaces",
            AttributeValue = "Value"
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        savedAttribute.Should().NotBeNull();
        savedAttribute?.AttributeName.Should().Be("Attribute Name With Spaces");
    }

    [Fact]
    public async Task Configure_ShouldAllowAttributeValueWithWhitespace()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-012",
            CategoryID = 1
        };

        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "Attribute",
            AttributeValue = "Value With Multiple Spaces"
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        savedAttribute.Should().NotBeNull();
        savedAttribute?.AttributeValue.Should().Be("Value With Multiple Spaces");
    }

    [Fact]
    public async Task Configure_ShouldLoadProductAttributesNavigation_FromProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-013",
            CategoryID = 1
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var attributes = new List<ProductAttribute>
        {
            new ProductAttribute { ProductID = product.ProductID, AttributeName = "Color", AttributeValue = "Red" },
            new ProductAttribute { ProductID = product.ProductID, AttributeName = "Size", AttributeValue = "Large" }
        };

        dbContext.ProductAttributes.AddRange(attributes);
        await dbContext.SaveChangesAsync();

        // Act
        var loadedProduct = await dbContext.Products
            .Include(p => p.ProductAttributes)
            .FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

        // Assert
        loadedProduct.Should().NotBeNull();
        loadedProduct?.ProductAttributes.Should().HaveCount(2);
    }

    [Fact]
    public async Task Configure_ShouldAllowMultipleProductsWithAttributes()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product1 = new Product { Name = "Product 1", SKU = "SKU-001", CategoryID = 1 };
        var product2 = new Product { Name = "Product 2", SKU = "SKU-002", CategoryID = 1 };

        var attributes = new List<ProductAttribute>
        {
            new ProductAttribute { ProductID = product1.ProductID, AttributeName = "Color", AttributeValue = "Red" },
            new ProductAttribute { ProductID = product2.ProductID, AttributeName = "Color", AttributeValue = "Blue" }
        };

        dbContext.Products.AddRange(product1, product2);
        dbContext.ProductAttributes.AddRange(attributes);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var allAttributes = await dbContext.ProductAttributes.ToListAsync();
        allAttributes.Should().HaveCount(2);
    }

    [Fact]
    public async Task Configure_ShouldAllowEmptyStringForAttributeValue()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-014",
            CategoryID = 1
        };

        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "OptionalAttr",
            AttributeValue = ""
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        savedAttribute.Should().NotBeNull();
        savedAttribute?.AttributeValue.Should().Be("");
    }

    [Fact]
    public async Task Configure_ShouldAllowNumericAttributeValue()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-015",
            CategoryID = 1
        };

        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "Quantity",
            AttributeValue = "12345"
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        savedAttribute.Should().NotBeNull();
        savedAttribute?.AttributeValue.Should().Be("12345");
    }

    [Fact]
    public async Task Configure_ShouldAllowUnicodeCharactersInAttributeName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product
        {
            Name = "Test Product",
            SKU = "TEST-016",
            CategoryID = 1
        };

        var productAttribute = new ProductAttribute
        {
            ProductID = product.ProductID,
            AttributeName = "颜色", // Color in Chinese
            AttributeValue = "红色" // Red in Chinese
        };

        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(productAttribute);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedAttribute = await dbContext.ProductAttributes.FindAsync(productAttribute.AttributeID);
        savedAttribute.Should().NotBeNull();
        savedAttribute?.AttributeName.Should().Be("颜色");
        savedAttribute?.AttributeValue.Should().Be("红色");
    }
}
