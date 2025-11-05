using copilot_sample.DataAccess.Entities;
using copilot_sample.Api.Models.Dtos;
using FluentAssertions;

public class CategoryMappingsTests
{
    [Fact]
    public void MapCategory_ShouldMapSimpleCategory_WithoutSubcategories()
    {
        // Arrange
        var category = new Category
        {
            CategoryID = 1,
            Name = "Electronics",
            Description = "Electronic items",
            ParentCategoryID = null,
            SubCategories = null
        };

        // Act
        var result = CategoryMappings.MapWithSubcategories(category);

        // Assert
        result.Should().NotBeNull();
        result.CategoryID.Should().Be(1);
        result.Name.Should().Be("Electronics");
        result.Description.Should().Be("Electronic items");
        result.ParentCategoryID.Should().BeNull();
        result.SubCategories.Should().BeNull();
    }

    [Fact]
    public void MapCategory_ShouldMapCategoryWithSubcategories()
    {
        // Arrange
        var subcategory = new Category
        {
            CategoryID = 2,
            Name = "Laptops",
            Description = "Laptop computers",
            ParentCategoryID = 1,
            SubCategories = null
        };

        var category = new Category
        {
            CategoryID = 1,
            Name = "Electronics",
            Description = "Electronic items",
            ParentCategoryID = null,
            SubCategories = new List<Category> { subcategory }
        };

        // Act
        var result = CategoryMappings.MapWithSubcategories(category);

        // Assert
        result.Should().NotBeNull();
        result.CategoryID.Should().Be(1);
        result.Name.Should().Be("Electronics");
        result.SubCategories.Should().NotBeNull();
        result.SubCategories.Should().HaveCount(1);
        result.SubCategories[0].CategoryID.Should().Be(2);
        result.SubCategories[0].Name.Should().Be("Laptops");
        result.SubCategories[0].Description.Should().Be("Laptop computers");
    }

    [Fact]
    public void MapCategory_ShouldMapNestedSubcategories()
    {
        // Arrange
        var nestedSubcategory = new Category
        {
            CategoryID = 3,
            Name = "Gaming Laptops",
            Description = "High-performance gaming laptops",
            ParentCategoryID = 2,
            SubCategories = null
        };

        var subcategory = new Category
        {
            CategoryID = 2,
            Name = "Laptops",
            Description = "Laptop computers",
            ParentCategoryID = 1,
            SubCategories = new List<Category> { nestedSubcategory }
        };

        var category = new Category
        {
            CategoryID = 1,
            Name = "Electronics",
            Description = "Electronic items",
            ParentCategoryID = null,
            SubCategories = new List<Category> { subcategory }
        };

        // Act
        var result = CategoryMappings.MapWithSubcategories(category);

        // Assert
        result.Should().NotBeNull();
        result.SubCategories.Should().HaveCount(1);
        result.SubCategories![0].SubCategories.Should().HaveCount(1);
        result.SubCategories[0].SubCategories![0].CategoryID.Should().Be(3);
        result.SubCategories[0].SubCategories[0].Name.Should().Be("Gaming Laptops");
    }

    [Fact]
    public void MapCategory_ShouldMapMultipleSubcategories()
    {
        // Arrange
        var subcategory1 = new Category
        {
            CategoryID = 2,
            Name = "Laptops",
            Description = "Laptop computers",
            ParentCategoryID = 1,
            SubCategories = null
        };

        var subcategory2 = new Category
        {
            CategoryID = 3,
            Name = "Desktops",
            Description = "Desktop computers",
            ParentCategoryID = 1,
            SubCategories = null
        };

        var category = new Category
        {
            CategoryID = 1,
            Name = "Electronics",
            Description = "Electronic items",
            ParentCategoryID = null,
            SubCategories = new List<Category> { subcategory1, subcategory2 }
        };

        // Act
        var result = CategoryMappings.MapWithSubcategories(category);

        // Assert
        result.SubCategories.Should().HaveCount(2);
        result.SubCategories[0].Name.Should().Be("Laptops");
        result.SubCategories[1].Name.Should().Be("Desktops");
    }

    [Fact]
    public void MapCategory_ShouldHandleNullSubcategoriesCollection()
    {
        // Arrange
        var category = new Category
        {
            CategoryID = 1,
            Name = "Electronics",
            Description = "Electronic items",
            ParentCategoryID = null,
            SubCategories = null
        };

        // Act
        var result = CategoryMappings.MapWithSubcategories(category);

        // Assert
        result.SubCategories.Should().BeNull();
    }

    [Fact]
    public void MapCategory_ShouldHandleEmptySubcategoriesCollection()
    {
        // Arrange
        var category = new Category
        {
            CategoryID = 1,
            Name = "Electronics",
            Description = "Electronic items",
            ParentCategoryID = null,
            SubCategories = new List<Category>()
        };

        // Act
        var result = CategoryMappings.MapWithSubcategories(category);

        // Assert
        result.SubCategories.Should().NotBeNull();
        result.SubCategories.Should().HaveCount(0);
    }

    [Fact]
    public void MapCategory_ShouldPreserveNullDescription()
    {
        // Arrange
        var category = new Category
        {
            CategoryID = 1,
            Name = "Electronics",
            Description = null,
            ParentCategoryID = null,
            SubCategories = null
        };

        // Act
        var result = CategoryMappings.MapWithSubcategories(category);

        // Assert
        result.Description.Should().BeNull();
    }

    [Fact]
    public void MapCategory_ShouldMapCategoryWithParentCategoryID()
    {
        // Arrange
        var category = new Category
        {
            CategoryID = 2,
            Name = "Laptops",
            Description = "Laptop computers",
            ParentCategoryID = 1,
            SubCategories = null
        };

        // Act
        var result = CategoryMappings.MapWithSubcategories(category);

        // Assert
        result.ParentCategoryID.Should().Be(1);
    }

    [Fact]
    public void MapCategory_ShouldMaintainCategoryIntegrity()
    {
        // Arrange
        var category = new Category
        {
            CategoryID = 100,
            Name = "Test Category",
            Description = "Test Description",
            ParentCategoryID = 50,
            SubCategories = null
        };

        // Act
        var result = CategoryMappings.MapWithSubcategories(category);

        // Assert
        result.CategoryID.Should().Be(100);
        result.Name.Should().Be("Test Category");
        result.Description.Should().Be("Test Description");
        result.ParentCategoryID.Should().Be(50);
    }

    [Fact]
    public void MapCategory_ShouldMapComplexHierarchy()
    {
        // Arrange - Create a multi-level hierarchy
        var level3 = new Category
        {
            CategoryID = 4,
            Name = "Laptop Bags",
            Description = "Bags for laptops",
            ParentCategoryID = 2,
            SubCategories = null
        };

        var level2 = new Category
        {
            CategoryID = 2,
            Name = "Laptops",
            Description = "Laptop computers",
            ParentCategoryID = 1,
            SubCategories = new List<Category> { level3 }
        };

        var level1 = new Category
        {
            CategoryID = 1,
            Name = "Electronics",
            Description = "Electronic items",
            ParentCategoryID = null,
            SubCategories = new List<Category> { level2 }
        };

        // Act
        var result = CategoryMappings.MapWithSubcategories(level1);

        // Assert
        result.CategoryID.Should().Be(1);
        result.SubCategories.Should().HaveCount(1);
        result.SubCategories![0].CategoryID.Should().Be(2);
        result.SubCategories[0].SubCategories.Should().HaveCount(1);
        result.SubCategories[0].SubCategories![0].CategoryID.Should().Be(4);
        result.SubCategories[0].SubCategories[0].Name.Should().Be("Laptop Bags");
    }
}
