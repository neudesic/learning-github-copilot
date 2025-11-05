using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.Api.Models.Dtos;
using copilot_sample.Api.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class ProductAttributeServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetProductAttributesAsync_ShouldReturnAllAttributes()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        
        dbContext.ProductAttributes.AddRange(new List<ProductAttribute>
        {
            new ProductAttribute { AttributeID = 1, ProductID = 1, AttributeName = "Color", AttributeValue = "Red" },
            new ProductAttribute { AttributeID = 2, ProductID = 1, AttributeName = "Size", AttributeValue = "Large" }
        });
        await dbContext.SaveChangesAsync();

        var attributeService = new ProductAttributeService(dbContext);

        // Act
        var result = await attributeService.GetProductAttributesAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(a => a.AttributeName == "Color");
        result.Should().Contain(a => a.AttributeName == "Size");
    }

    [Fact]
    public async Task GetProductAttributesAsync_ShouldReturnEmptyList_WhenNoAttributes()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var attributeService = new ProductAttributeService(dbContext);

        // Act
        var result = await attributeService.GetProductAttributesAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetProductAttributeByIdAsync_ShouldReturnAttribute_WhenAttributeExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(new ProductAttribute { AttributeID = 1, ProductID = 1, AttributeName = "Color", AttributeValue = "Red" });
        await dbContext.SaveChangesAsync();

        var attributeService = new ProductAttributeService(dbContext);

        // Act
        var result = await attributeService.GetProductAttributeByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.AttributeName.Should().Be("Color");
        result.AttributeValue.Should().Be("Red");
    }

    [Fact]
    public async Task GetProductAttributeByIdAsync_ShouldReturnNull_WhenAttributeDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var attributeService = new ProductAttributeService(dbContext);

        // Act
        var result = await attributeService.GetProductAttributeByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddProductAttributeAsync_ShouldAddAttribute_WhenProductExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var attributeService = new ProductAttributeService(dbContext);
        var addAttributeDto = new AddProductAttributeDto
        {
            ProductID = 1,
            AttributeName = "Color",
            AttributeValue = "Blue"
        };

        var initialCount = await dbContext.ProductAttributes.CountAsync();

        // Act
        var result = await attributeService.AddProductAttributeAsync(addAttributeDto);

        // Assert
        result.Should().NotBeNull();
        result.AttributeName.Should().Be("Color");
        result.AttributeValue.Should().Be("Blue");
        (await dbContext.ProductAttributes.CountAsync()).Should().Be(initialCount + 1);
    }

    [Fact]
    public async Task AddProductAttributeAsync_ShouldThrowException_WhenProductDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var attributeService = new ProductAttributeService(dbContext);

        var addAttributeDto = new AddProductAttributeDto
        {
            ProductID = 999,
            AttributeName = "Color",
            AttributeValue = "Blue"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => attributeService.AddProductAttributeAsync(addAttributeDto));
    }

    [Fact]
    public async Task AddProductAttributeAsync_ShouldAddMultipleAttributesToSameProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var attributeService = new ProductAttributeService(dbContext);

        var attribute1 = new AddProductAttributeDto { ProductID = 1, AttributeName = "Color", AttributeValue = "Red" };
        var attribute2 = new AddProductAttributeDto { ProductID = 1, AttributeName = "Size", AttributeValue = "Large" };

        // Act
        await attributeService.AddProductAttributeAsync(attribute1);
        await attributeService.AddProductAttributeAsync(attribute2);

        // Assert
        (await dbContext.ProductAttributes.CountAsync()).Should().Be(2);
    }

    [Fact]
    public async Task UpdateProductAttributeAsync_ShouldUpdateAttribute_WhenAttributeExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(new ProductAttribute { AttributeID = 1, ProductID = 1, AttributeName = "Color", AttributeValue = "Red" });
        await dbContext.SaveChangesAsync();

        var attributeService = new ProductAttributeService(dbContext);
        var updateDto = new UpdateProductAttributeDto
        {
            AttributeName = "Shade",
            AttributeValue = "Dark Red"
        };

        // Act
        var result = await attributeService.UpdateProductAttributeAsync(1, updateDto);

        // Assert
        result.Should().BeTrue();
        var updatedAttribute = await dbContext.ProductAttributes.FindAsync(1);
        updatedAttribute!.AttributeName.Should().Be("Shade");
        updatedAttribute.AttributeValue.Should().Be("Dark Red");
    }

    [Fact]
    public async Task UpdateProductAttributeAsync_ShouldReturnFalse_WhenAttributeDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var attributeService = new ProductAttributeService(dbContext);

        var updateDto = new UpdateProductAttributeDto
        {
            AttributeName = "Color",
            AttributeValue = "Blue"
        };

        // Act
        var result = await attributeService.UpdateProductAttributeAsync(999, updateDto);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateProductAttributeAsync_ShouldUpdateOnlyName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(new ProductAttribute { AttributeID = 1, ProductID = 1, AttributeName = "Color", AttributeValue = "Red" });
        await dbContext.SaveChangesAsync();

        var attributeService = new ProductAttributeService(dbContext);
        var updateDto = new UpdateProductAttributeDto
        {
            AttributeName = "NewColor",
            AttributeValue = "Red"
        };

        // Act
        await attributeService.UpdateProductAttributeAsync(1, updateDto);

        // Assert
        var attribute = await dbContext.ProductAttributes.FindAsync(1);
        attribute!.AttributeName.Should().Be("NewColor");
        attribute.AttributeValue.Should().Be("Red");
    }

    [Fact]
    public async Task UpdateProductAttributeAsync_ShouldUpdateOnlyValue()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(new ProductAttribute { AttributeID = 1, ProductID = 1, AttributeName = "Color", AttributeValue = "Red" });
        await dbContext.SaveChangesAsync();

        var attributeService = new ProductAttributeService(dbContext);
        var updateDto = new UpdateProductAttributeDto
        {
            AttributeName = "Color",
            AttributeValue = "Blue"
        };

        // Act
        await attributeService.UpdateProductAttributeAsync(1, updateDto);

        // Assert
        var attribute = await dbContext.ProductAttributes.FindAsync(1);
        attribute!.AttributeName.Should().Be("Color");
        attribute.AttributeValue.Should().Be("Blue");
    }

    [Fact]
    public async Task DeleteProductAttributeAsync_ShouldDeleteAttribute_WhenAttributeExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        dbContext.ProductAttributes.Add(new ProductAttribute { AttributeID = 1, ProductID = 1, AttributeName = "Color", AttributeValue = "Red" });
        await dbContext.SaveChangesAsync();

        var attributeService = new ProductAttributeService(dbContext);

        // Act
        var result = await attributeService.DeleteProductAttributeAsync(1);

        // Assert
        result.Should().BeTrue();
        var deletedAttribute = await dbContext.ProductAttributes.FindAsync(1);
        deletedAttribute.Should().BeNull();
    }

    [Fact]
    public async Task DeleteProductAttributeAsync_ShouldReturnFalse_WhenAttributeDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var attributeService = new ProductAttributeService(dbContext);

        // Act
        var result = await attributeService.DeleteProductAttributeAsync(999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteProductAttributeAsync_ShouldDeleteOnlySpecificAttribute()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        dbContext.ProductAttributes.AddRange(new List<ProductAttribute>
        {
            new ProductAttribute { AttributeID = 1, ProductID = 1, AttributeName = "Color", AttributeValue = "Red" },
            new ProductAttribute { AttributeID = 2, ProductID = 1, AttributeName = "Size", AttributeValue = "Large" }
        });
        await dbContext.SaveChangesAsync();

        var attributeService = new ProductAttributeService(dbContext);

        // Act
        await attributeService.DeleteProductAttributeAsync(1);

        // Assert
        (await dbContext.ProductAttributes.CountAsync()).Should().Be(1);
        var remainingAttribute = await dbContext.ProductAttributes.FindAsync(2);
        remainingAttribute.Should().NotBeNull();
        remainingAttribute!.AttributeName.Should().Be("Size");
    }
}
