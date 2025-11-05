using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.Api.Models.Dtos;
using copilot_sample.Api.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class ProductServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);

        dbContext.Products.AddRange(new List<Product>
        {
            new Product { ProductID = 1, Name = "Laptop", SKU = "LAP-001", CategoryID = 1, Brand = "Dell", IsActive = true },
            new Product { ProductID = 2, Name = "Mouse", SKU = "MOU-001", CategoryID = 1, Brand = "Logitech", IsActive = true }
        });
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);

        // Act
        var result = await productService.GetProductsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Name == "Laptop");
        result.Should().Contain(p => p.Name == "Mouse");
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnEmptyList_WhenNoProducts()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var productService = new ProductService(dbContext);

        // Act
        var result = await productService.GetProductsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        dbContext.Products.Add(new Product { ProductID = 1, Name = "Laptop", SKU = "LAP-001", CategoryID = 1, Brand = "Dell", IsActive = true });
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);

        // Act
        var result = await productService.GetProductByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Laptop");
        result.SKU.Should().Be("LAP-001");
        result.Brand.Should().Be("Dell");
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var productService = new ProductService(dbContext);

        // Act
        var result = await productService.GetProductByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddProductAsync_ShouldAddProduct_WhenCategoryExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);
        var addProductDto = new AddProductDto
        {
            Name = "Laptop",
            Description = "High performance laptop",
            SKU = "LAP-001",
            CategoryID = 1,
            Brand = "Dell",
            IsActive = true
        };

        var initialCount = await dbContext.Products.CountAsync();

        // Act
        var result = await productService.AddProductAsync(addProductDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Laptop");
        result.SKU.Should().Be("LAP-001");
        (await dbContext.Products.CountAsync()).Should().Be(initialCount + 1);
    }

    [Fact]
    public async Task AddProductAsync_ShouldSetCreatedAtAndUpdatedAt()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);
        var beforeAdd = DateTime.UtcNow;

        var addProductDto = new AddProductDto
        {
            Name = "Laptop",
            Description = "High performance laptop",
            SKU = "LAP-001",
            CategoryID = 1,
            Brand = "Dell",
            IsActive = true
        };

        // Act
        var result = await productService.AddProductAsync(addProductDto);
        var addedProduct = await dbContext.Products.FindAsync(result.ProductID);

        // Assert
        addedProduct!.CreatedAt.Should().BeOnOrAfter(beforeAdd);
        addedProduct.UpdatedAt.Should().BeOnOrAfter(beforeAdd);
    }

    [Fact]
    public async Task AddProductAsync_ShouldAddMultipleProducts()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);

        var product1 = new AddProductDto { Name = "Laptop", SKU = "LAP-001", CategoryID = 1, Brand = "Dell", IsActive = true };
        var product2 = new AddProductDto { Name = "Mouse", SKU = "MOU-001", CategoryID = 1, Brand = "Logitech", IsActive = true };

        // Act
        await productService.AddProductAsync(product1);
        await productService.AddProductAsync(product2);

        // Assert
        (await dbContext.Products.CountAsync()).Should().Be(2);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateProduct_WhenProductExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        dbContext.Products.Add(new Product { ProductID = 1, Name = "Laptop", SKU = "LAP-001", CategoryID = 1, Brand = "Dell", IsActive = true });
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);
        var updateDto = new UpdateProductDto
        {
            Name = "Premium Laptop",
            SKU = "LAP-002",
            Brand = "HP",
            IsActive = true
        };

        // Act
        var result = await productService.UpdateProductAsync(1, updateDto);

        // Assert
        result.Should().BeTrue();
        var updatedProduct = await dbContext.Products.FindAsync(1);
        updatedProduct!.Name.Should().Be("Premium Laptop");
        updatedProduct.SKU.Should().Be("LAP-002");
        updatedProduct.Brand.Should().Be("HP");
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var productService = new ProductService(dbContext);

        var updateDto = new UpdateProductDto
        {
            Name = "Laptop"
        };

        // Act
        var result = await productService.UpdateProductAsync(999, updateDto);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldPreserveOldValuesWhenNullProvided()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        dbContext.Products.Add(new Product { ProductID = 1, Name = "Laptop", SKU = "LAP-001", CategoryID = 1, Brand = "Dell", IsActive = true });
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);
        var updateDto = new UpdateProductDto
        {
            Name = "Updated Laptop",
            SKU = null,
            Brand = null
        };

        // Act
        await productService.UpdateProductAsync(1, updateDto);

        // Assert
        var product = await dbContext.Products.FindAsync(1);
        product!.Name.Should().Be("Updated Laptop");
        product.SKU.Should().Be("LAP-001");
        product.Brand.Should().Be("Dell");
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateOnlyName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        dbContext.Products.Add(new Product { ProductID = 1, Name = "Laptop", SKU = "LAP-001", CategoryID = 1, Brand = "Dell", IsActive = true });
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);
        var updateDto = new UpdateProductDto { Name = "Premium Laptop" };

        // Act
        await productService.UpdateProductAsync(1, updateDto);

        // Assert
        var product = await dbContext.Products.FindAsync(1);
        product!.Name.Should().Be("Premium Laptop");
        product.SKU.Should().Be("LAP-001");
        product.Brand.Should().Be("Dell");
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateIsActive()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        dbContext.Products.Add(new Product { ProductID = 1, Name = "Laptop", SKU = "LAP-001", CategoryID = 1, Brand = "Dell", IsActive = true });
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);
        var updateDto = new UpdateProductDto { IsActive = false };

        // Act
        await productService.UpdateProductAsync(1, updateDto);

        // Assert
        var product = await dbContext.Products.FindAsync(1);
        product!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateCategoryId()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category1 = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        var category2 = new Category { CategoryID = 2, Name = "Books", Description = "Books" };
        dbContext.Categories.AddRange(category1, category2);
        dbContext.Products.Add(new Product { ProductID = 1, Name = "Laptop", SKU = "LAP-001", CategoryID = 1, Brand = "Dell", IsActive = true });
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);
        var updateDto = new UpdateProductDto { CategoryID = 2 };

        // Act
        await productService.UpdateProductAsync(1, updateDto);

        // Assert
        var product = await dbContext.Products.FindAsync(1);
        product!.CategoryID.Should().Be(2);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateUpdatedAtTimestamp()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        var originalProduct = new Product { ProductID = 1, Name = "Laptop", SKU = "LAP-001", CategoryID = 1, Brand = "Dell", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-1), UpdatedAt = DateTime.UtcNow.AddDays(-1) };
        dbContext.Products.Add(originalProduct);
        await dbContext.SaveChangesAsync();

        var originalUpdatedAt = originalProduct.UpdatedAt;
        var productService = new ProductService(dbContext);
        var updateDto = new UpdateProductDto { Name = "Updated Laptop" };

        // Act
        await System.Threading.Tasks.Task.Delay(100);
        await productService.UpdateProductAsync(1, updateDto);

        // Assert
        var product = await dbContext.Products.FindAsync(1);
        product!.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldDeleteProduct_WhenProductExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        dbContext.Products.Add(new Product { ProductID = 1, Name = "Laptop", SKU = "LAP-001", CategoryID = 1, Brand = "Dell", IsActive = true });
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);

        // Act
        var result = await productService.DeleteProductAsync(1);

        // Assert
        result.Should().BeTrue();
        var deletedProduct = await dbContext.Products.FindAsync(1);
        deletedProduct.Should().BeNull();
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var productService = new ProductService(dbContext);

        // Act
        var result = await productService.DeleteProductAsync(999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldDeleteOnlySpecificProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        dbContext.Products.AddRange(new List<Product>
        {
            new Product { ProductID = 1, Name = "Laptop", SKU = "LAP-001", CategoryID = 1, Brand = "Dell", IsActive = true },
            new Product { ProductID = 2, Name = "Mouse", SKU = "MOU-001", CategoryID = 1, Brand = "Logitech", IsActive = true }
        });
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);

        // Act
        await productService.DeleteProductAsync(1);

        // Assert
        (await dbContext.Products.CountAsync()).Should().Be(1);
        var remainingProduct = await dbContext.Products.FindAsync(2);
        remainingProduct.Should().NotBeNull();
        remainingProduct!.Name.Should().Be("Mouse");
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnProductsWithCategoryDto()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        dbContext.Products.Add(new Product { ProductID = 1, Name = "Laptop", SKU = "LAP-001", CategoryID = 1, Brand = "Dell", IsActive = true });
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);

        // Act
        var result = await productService.GetProductsAsync();

        // Assert
        result.Should().HaveCount(1);
        result[0].Category.Should().NotBeNull();
        result[0].Category!.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task AddProductAsync_ShouldReturnProductDtoWithoutCategory()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" };
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var productService = new ProductService(dbContext);
        var addProductDto = new AddProductDto
        {
            Name = "Laptop",
            Description = "High performance laptop",
            SKU = "LAP-001",
            CategoryID = 1,
            Brand = "Dell",
            IsActive = true
        };

        // Act
        var result = await productService.AddProductAsync(addProductDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Laptop");
        result.ProductID.Should().BeGreaterThan(0);
    }
}
