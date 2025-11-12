---
applyTo: 'tests/**/*.cs'
---

# Unit Test Instructions for .NET 8 Inventory API

## General Testing Principles

### Test Framework & Libraries
- **xUnit**: Primary testing framework (v2.9.3+)
- **Moq**: Mocking framework (v4.20.72+) for creating test doubles
- **FluentAssertions**: Assertion library (v8.2.0+) for readable assertions
- **Microsoft.EntityFrameworkCore.InMemory**: In-memory database provider for testing

### Naming Conventions
- Test class names: `{ClassUnderTest}Tests` (e.g., `ProductServiceTests`, `CategoryControllerTests`)
- Test method names: `{MethodName}_{Scenario}_{ExpectedBehavior}` (e.g., `GetProduct_WithValidId_ReturnsProduct`)
- Use descriptive names that explain the test purpose without reading the code

### Test Structure (AAA Pattern)
Always organize tests using the Arrange-Act-Assert pattern:

```csharp
[Fact]
public async Task MethodName_Scenario_ExpectedBehavior()
{
    // Arrange - Set up test data, mocks, and dependencies
    var mockRepository = new Mock<IRepository>();
    var service = new Service(mockRepository.Object);
    
    // Act - Execute the method being tested
    var result = await service.MethodAsync();
    
    // Assert - Verify the expected outcome
    result.Should().NotBeNull();
}
```

## Entity and Data Testing

### Product Entity Requirements
- **ALWAYS** include the required `SKU` property when creating Product instances
- Products require: `Name`, `SKU`, and `CategoryID` at minimum
- Example:
```csharp
var product = new Product 
{ 
    Name = "Test Product", 
    SKU = "TEST-SKU-001", 
    CategoryID = 1 
};
```

### In-Memory Database Testing
- Use `DbContextOptionsBuilder<AppDbContext>` with `.UseInMemoryDatabase()`
- Use unique database names per test: `Guid.NewGuid().ToString()`
- Call `SaveChangesAsync()` after adding entities to ensure they're tracked
- For tests requiring multiple entities, save after each logical group

```csharp
private AppDbContext GetInMemoryDbContext()
{
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;
    
    return new AppDbContext(options);
}
```

### Database Initialization Testing
- InMemory databases use `EnsureCreatedAsync()` instead of `MigrateAsync()`
- Don't mock `DatabaseFacade` - it's not mockable (no parameterless constructor)
- Don't mock DbContext properties (`Categories`, `Products`, etc.) - they're not virtual
- Use real InMemory contexts with disposed contexts to simulate failures:

```csharp
// To simulate connection/database failures
var context = GetInMemoryDbContext();
await context.DisposeAsync();
// Now using this context will throw exceptions
```

## Mocking Best Practices

### What to Mock
- **Services**: Mock service interfaces (`IProductService`, `ICategoryService`)
- **Repositories**: Mock repository patterns when not using EF Core directly
- **External dependencies**: HTTP clients, third-party APIs, file systems
- **Loggers**: Mock `ILogger` to verify logging behavior

### What NOT to Mock
- **Entity Framework DbContext**: Use InMemory provider instead
- **DatabaseFacade**: Not mockable - use real InMemory database
- **Value objects**: Use real instances
- **DTOs/Models**: Use real instances

### Logger Mocking Pattern
```csharp
var logger = new Mock<ILogger>();

// Verify logging
logger.Verify(
    x => x.Log(
        LogLevel.Information,
        It.IsAny<EventId>(),
        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("expected message")),
        It.IsAny<Exception>(),
        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
    Times.Once);
```

### Service Mocking Pattern
```csharp
var mockService = new Mock<IProductService>();
mockService
    .Setup(s => s.GetProductByIdAsync(It.IsAny<int>()))
    .ReturnsAsync(new ProductDto { ProductID = 1, Name = "Test" });
```

## Controller Testing

### Setup Pattern
```csharp
private readonly Mock<IProductService> _mockProductService;
private readonly ProductController _controller;

public ProductControllerTests()
{
    _mockProductService = new Mock<IProductService>();
    _controller = new ProductController(_mockProductService.Object);
}
```

### Testing Action Results
```csharp
// Test successful response
var result = await _controller.GetProduct(1);
result.Should().BeOfType<OkObjectResult>();
var okResult = result as OkObjectResult;
okResult.Value.Should().BeOfType<ProductDto>();

// Test not found response
var result = await _controller.GetProduct(999);
result.Should().BeOfType<NotFoundResult>();

// Test bad request
var result = await _controller.CreateProduct(null);
result.Should().BeOfType<BadRequestObjectResult>();
```

## Service Testing

### Async Method Testing
- Always use `async Task` for test methods testing async code
- Use `await` when calling async methods
- Test both success and failure paths

### Exception Handling
```csharp
[Fact]
public async Task Method_WithInvalidData_ThrowsException()
{
    // Arrange
    var service = new Service();
    
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(
        () => service.MethodAsync(null));
}
```

### FluentAssertions Usage
```csharp
// Collections
result.Should().NotBeNull();
result.Should().HaveCount(5);
result.Should().Contain(x => x.Name == "Test");

// Strings
name.Should().Be("Expected");
name.Should().Contain("part");
name.Should().StartWith("prefix");

// Booleans
isValid.Should().BeTrue();
isValid.Should().BeFalse();

// Numbers
count.Should().BeGreaterThan(0);
count.Should().BeLessThanOrEqualTo(100);

// Objects
product.Should().NotBeNull();
product.Should().BeEquivalentTo(expectedProduct);
```

## Data Access Layer Testing

### Testing Entity Configurations
- Verify required properties are enforced
- Test foreign key relationships
- Verify cascade delete behaviors
- Check index configurations

### Testing Seed Data
- Verify seed data is applied correctly
- Test data relationships are valid
- Ensure referential integrity

### Repository Pattern Testing
```csharp
[Fact]
public async Task GetById_WithValidId_ReturnsEntity()
{
    // Arrange
    using var context = GetInMemoryDbContext();
    var repository = new Repository(context);
    var entity = new Entity { Id = 1, Name = "Test" };
    context.Entities.Add(entity);
    await context.SaveChangesAsync();
    
    // Act
    var result = await repository.GetByIdAsync(1);
    
    // Assert
    result.Should().NotBeNull();
    result.Name.Should().Be("Test");
}
```

## Test Coverage Goals

### Minimum Coverage Requirements
- **Controllers**: 80%+ coverage
- **Services**: 90%+ coverage
- **Data Access**: 85%+ coverage
- **Overall Project**: 85%+ coverage

### What to Test
1. **Happy paths**: Normal, expected use cases
2. **Edge cases**: Boundary conditions, empty collections, null values
3. **Error cases**: Invalid input, exceptions, error handling
4. **Business logic**: All conditional branches and calculations
5. **Validation**: Input validation and business rules

### What NOT to Test (Generally)
- Third-party library code
- Framework code (EF Core, ASP.NET Core internals)
- Auto-generated code
- Simple property getters/setters without logic

## Common Test Scenarios

### Testing CRUD Operations
```csharp
[Fact]
public async Task Create_ValidEntity_ReturnsCreatedEntity()
{
    // Arrange
    var context = GetInMemoryDbContext();
    var service = new Service(context);
    var dto = new CreateDto { Name = "New Item" };
    
    // Act
    var result = await service.CreateAsync(dto);
    
    // Assert
    result.Should().NotBeNull();
    result.Name.Should().Be("New Item");
    var saved = await context.Entities.FindAsync(result.Id);
    saved.Should().NotBeNull();
}

[Fact]
public async Task Update_ExistingEntity_UpdatesSuccessfully()
{
    // Arrange
    var context = GetInMemoryDbContext();
    var entity = new Entity { Id = 1, Name = "Original" };
    context.Entities.Add(entity);
    await context.SaveChangesAsync();
    
    var service = new Service(context);
    var updateDto = new UpdateDto { Name = "Updated" };
    
    // Act
    await service.UpdateAsync(1, updateDto);
    
    // Assert
    var updated = await context.Entities.FindAsync(1);
    updated.Name.Should().Be("Updated");
}

[Fact]
public async Task Delete_ExistingEntity_RemovesFromDatabase()
{
    // Arrange
    var context = GetInMemoryDbContext();
    var entity = new Entity { Id = 1 };
    context.Entities.Add(entity);
    await context.SaveChangesAsync();
    
    var service = new Service(context);
    
    // Act
    await service.DeleteAsync(1);
    
    // Assert
    var deleted = await context.Entities.FindAsync(1);
    deleted.Should().BeNull();
}
```

### Testing Navigation Properties
```csharp
[Fact]
public async Task GetProduct_IncludesCategory()
{
    // Arrange
    var context = GetInMemoryDbContext();
    var category = new Category { CategoryID = 1, Name = "Electronics" };
    var product = new Product 
    { 
        ProductID = 1, 
        Name = "Laptop", 
        SKU = "LAP-001",
        CategoryID = 1,
        Category = category 
    };
    context.Categories.Add(category);
    context.Products.Add(product);
    await context.SaveChangesAsync();
    
    // Act
    var result = await context.Products
        .Include(p => p.Category)
        .FirstAsync(p => p.ProductID == 1);
    
    // Assert
    result.Category.Should().NotBeNull();
    result.Category.Name.Should().Be("Electronics");
}
```

## Performance Testing Considerations

- Use `[Theory]` with `[InlineData]` for parameterized tests
- Dispose of contexts and resources properly
- Keep tests fast (< 1 second each ideally)
- Avoid Thread.Sleep() - use async waiting patterns

## Test Organization

### File Structure
```
tests/
├── copilot-sample.Api.Tests/
│   ├── Controllers/
│   │   ├── ProductControllerTests.cs
│   │   └── CategoryControllerTests.cs
│   └── Services/
│       ├── ProductServiceTests.cs
│       └── CategoryServiceTests.cs
└── copilot-sample.DataAccess.Tests/
    ├── Entities/
    ├── EntityConfiguration/
    └── DatabaseInitializerTests.cs
```

### Test Class Organization
1. Private fields for shared test dependencies
2. Constructor for common setup
3. Helper methods (at bottom of class)
4. Test methods grouped by functionality
5. Use `#region` sparingly, prefer clear method grouping

## Continuous Integration

- All tests must pass before merging
- Run tests on every commit
- Generate and track code coverage reports
- Use `dotnet test` for running all tests
- Use `dotnet test --filter` for running specific tests

## Example: Complete Test Class

```csharp
using Xunit;
using Moq;
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
    public async Task GetProductById_WithValidId_ReturnsProduct()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var product = new Product 
        { 
            ProductID = 1, 
            Name = "Test Product",
            SKU = "TEST-001",
            CategoryID = 1
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        // Act
        var result = await service.GetProductByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Product");
        result.SKU.Should().Be("TEST-001");
    }

    [Fact]
    public async Task GetProductById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new ProductService(context);

        // Act
        var result = await service.GetProductByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetProductById_WithInvalidId_ThrowsArgumentException(int invalidId)
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new ProductService(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => service.GetProductByIdAsync(invalidId));
    }
}
```

## Remember
- Write tests first when doing TDD
- Keep tests simple and focused (one assertion concept per test)
- Make tests independent and isolated
- Use meaningful test data that represents real scenarios
- Regularly review and refactor tests
- Run tests frequently during development
- Update tests when requirements change
- Document complex test scenarios with comments 