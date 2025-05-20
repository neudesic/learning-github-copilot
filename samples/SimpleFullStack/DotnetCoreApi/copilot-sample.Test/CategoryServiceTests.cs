using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.Api.Models.Dtos;
using copilot_sample.Api.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class CategoryServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetCategoriesAsync_ShouldReturnAllCategories()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        dbContext.Categories.AddRange(new List<Category>
        {
            new Category { CategoryID = 100, Name = "Electronics", Description = "Electronic items" },
            new Category { CategoryID = 200, Name = "Books", Description = "Books and novels" }
        });
        await dbContext.SaveChangesAsync();
        var total = dbContext.Categories.Count();

        var categoryService = new CategoryService(dbContext);

        // Act
        var result = await categoryService.GetCategoriesAsync();

        // Assert
        result.Should().HaveCount(total);
        result.Should().Contain(c => c.Name == "Electronics");
        result.Should().Contain(c => c.Name == "Books");
    }

    [Fact]
    public async Task GetCategoryByIdAsync_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        dbContext.Categories.Add(new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic items" });
        await dbContext.SaveChangesAsync();

        var categoryService = new CategoryService(dbContext);

        // Act
        var result = await categoryService.GetCategoryByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task GetCategoryByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        
        var categoryService = new CategoryService(dbContext);

        // Act
        var result = await categoryService.GetCategoryByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddCategoryAsync_ShouldAddCategory_WhenCategoryDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var categoryService = new CategoryService(dbContext);

        var newCategory = new AddCategoryDto
        {
            Name = "Electronics Apps",
            Description = "Electronic items"
        };
        var totalCount = dbContext.Categories.Count();

        // Act
        var result = await categoryService.AddCategoryAsync(newCategory);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Electronics Apps");
        (await dbContext.Categories.CountAsync()).Should().Be(totalCount +1);
    }

    [Fact]
    public async Task AddCategoryAsync_ShouldReturnNull_WhenCategoryWithSameNameExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        dbContext.Categories.Add(new Category { CategoryID = 101, Name = "Electronics101", Description = "Electronic items" });
        await dbContext.SaveChangesAsync();

        var categoryService = new CategoryService(dbContext);

        var newCategory = new AddCategoryDto
        {
            Name = "Electronics101",
            Description = "Duplicate category"
        };

        // Act
        var result = await categoryService.AddCategoryAsync(newCategory);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateCategoryDescriptionAsync_ShouldUpdateDescription_WhenCategoryExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        dbContext.Categories.Add(new Category { CategoryID = 12, Name = "Electronics12", Description = "Old description" });
        await dbContext.SaveChangesAsync();

        var categoryService = new CategoryService(dbContext);

        var updateDto = new UpdateCategoryDescriptionDto
        {
            Description = "Updated description"
        };

        // Act
        var result = await categoryService.UpdateCategoryDescriptionAsync(12, updateDto);

        // Assert
        result.Should().BeTrue();
        var updatedCategory = await dbContext.Categories.FindAsync(12);
        updatedCategory!.Description.Should().Be("Updated description");
    }

    [Fact]
    public async Task UpdateCategoryDescriptionAsync_ShouldReturnFalse_WhenCategoryDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var categoryService = new CategoryService(dbContext);

        var updateDto = new UpdateCategoryDescriptionDto
        {
            Description = "Updated description"
        };

        // Act
        var result = await categoryService.UpdateCategoryDescriptionAsync(13, updateDto);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteCategoryAsync_ShouldDeleteCategory_WhenCategoryExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        dbContext.Categories.Add(new Category { CategoryID = 11, Name = "Electronics11", Description = "Electronic items" });
        await dbContext.SaveChangesAsync();

        var categoryService = new CategoryService(dbContext);

        // Act
        var result = await categoryService.DeleteCategoryAsync(11);

        // Assert
        result.Should().BeTrue();
        (await dbContext.Categories.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task DeleteCategoryAsync_ShouldReturnFalse_WhenCategoryDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var categoryService = new CategoryService(dbContext);

        // Act
        var result = await categoryService.DeleteCategoryAsync(11);

        // Assert
        result.Should().BeFalse();
    }
}
