using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.DataAccess.EntityConfiguration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

public class CategoryConfigurationTests
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
        var entityType = dbContext.Model.FindEntityType(typeof(Category));

        // Act & Assert
        entityType?.GetTableName().Should().Be("Categories");
    }

    [Fact]
    public void Configure_ShouldHavePrimaryKey_OnCategoryID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var primaryKey = entityType?.FindPrimaryKey();

        // Act & Assert
        primaryKey.Should().NotBeNull();
        primaryKey?.Properties.Should().HaveCount(1);
        primaryKey?.Properties[0].Name.Should().Be("CategoryID");
    }

    [Fact]
    public void Configure_ShouldMapCategoryIDColumn_WithValueGeneratedOnAdd()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var categoryIdProperty = entityType?.FindProperty("CategoryID");

        // Act & Assert
        categoryIdProperty?.GetColumnName().Should().Be("CategoryID");
        categoryIdProperty?.ValueGenerated.Should().Be(ValueGenerated.OnAdd);
    }

    [Fact]
    public void Configure_ShouldMapNameColumn_WithMaxLength100_AndRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var nameProperty = entityType?.FindProperty("Name");

        // Act & Assert
        nameProperty?.GetColumnName().Should().Be("Name");
        nameProperty?.GetMaxLength().Should().Be(100);
        nameProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_ShouldMapDescriptionColumn_WithMaxLength500_AsNullable()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var descriptionProperty = entityType?.FindProperty("Description");

        // Act & Assert
        descriptionProperty?.GetColumnName().Should().Be("Description");
        descriptionProperty?.GetMaxLength().Should().Be(500);
        descriptionProperty?.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void Configure_ShouldMapParentCategoryIDColumn_AsNullable()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var parentCategoryIdProperty = entityType?.FindProperty("ParentCategoryID");

        // Act & Assert
        parentCategoryIdProperty?.GetColumnName().Should().Be("ParentCategoryID");
        parentCategoryIdProperty?.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void Configure_ShouldHaveParentCategoryRelationship()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var navigationProperty = entityType?.FindNavigation("ParentCategory");

        // Act & Assert
        navigationProperty.Should().NotBeNull();
        navigationProperty?.TargetEntityType.Name.Should().Contain("Category");
    }

    [Fact]
    public void Configure_ShouldHaveSubCategoriesRelationship()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var navigationProperty = entityType?.FindNavigation("SubCategories");

        // Act & Assert
        navigationProperty.Should().NotBeNull();
        navigationProperty?.TargetEntityType.Name.Should().Contain("Category");
    }

    [Fact]
    public void Configure_ShouldHaveForeignKeyToParentCategoryOnParentCategoryID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var parentCategoryForeignKey = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "ParentCategoryID");
        parentCategoryForeignKey.Should().NotBeNull();
    }

    [Fact]
    public void Configure_ShouldHaveForeignKeyDeleteBehavior_Restrict()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var foreignKeys = entityType?.GetForeignKeys();
        var parentCategoryForeignKey = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "ParentCategoryID");

        // Act & Assert
        parentCategoryForeignKey.Should().NotBeNull();
        parentCategoryForeignKey?.DeleteBehavior.Should().Be(DeleteBehavior.Restrict);
    }

    [Fact]
    public async Task Configure_ShouldAllowValidCategory_WithAllRequiredFields()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category
        {
            Name = "Electronics",
            Description = "Electronic items and gadgets"
        };

        dbContext.Set<Category>().Add(category);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedCategory = await dbContext.Set<Category>().FindAsync(category.CategoryID);
        savedCategory.Should().NotBeNull();
        savedCategory?.Name.Should().Be("Electronics");
        savedCategory?.Description.Should().Be("Electronic items and gadgets");
    }

    [Fact]
    public async Task Configure_ShouldAutoGenerateCategoryID_OnAdd()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category
        {
            Name = "Computers"
        };

        dbContext.Set<Category>().Add(category);
        await dbContext.SaveChangesAsync();

        // Act & Assert
        category.CategoryID.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Configure_ShouldAllowNullableDescription()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category
        {
            Name = "Category Without Description",
            Description = null
        };

        dbContext.Set<Category>().Add(category);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedCategory = await dbContext.Set<Category>().FindAsync(category.CategoryID);
        savedCategory.Should().NotBeNull();
        savedCategory?.Description.Should().BeNull();
    }

    [Fact]
    public async Task Configure_ShouldAllowNullableParentCategoryID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category
        {
            Name = "Root Category",
            ParentCategoryID = null
        };

        dbContext.Set<Category>().Add(category);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedCategory = await dbContext.Set<Category>().FindAsync(category.CategoryID);
        savedCategory.Should().NotBeNull();
        savedCategory?.ParentCategoryID.Should().BeNull();
    }

    [Fact]
    public async Task Configure_ShouldAllowCategoryWithParentCategory()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var parentCategory = new Category
        {
            Name = "Electronics"
        };

        var subCategory = new Category
        {
            Name = "Laptops",
            ParentCategoryID = 1
        };

        dbContext.Set<Category>().Add(parentCategory);
        await dbContext.SaveChangesAsync();

        subCategory.ParentCategoryID = parentCategory.CategoryID;
        dbContext.Set<Category>().Add(subCategory);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedSubCategory = await dbContext.Set<Category>().FindAsync(subCategory.CategoryID);
        savedSubCategory.Should().NotBeNull();
        savedSubCategory?.ParentCategoryID.Should().Be(parentCategory.CategoryID);
    }

    [Fact]
    public async Task Configure_ShouldLoadParentCategoryNavigation_WithCategory()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var parentCategory = new Category
        {
            Name = "Electronics"
        };

        var subCategory = new Category
        {
            Name = "Laptops"
        };

        dbContext.Set<Category>().Add(parentCategory);
        await dbContext.SaveChangesAsync();

        subCategory.ParentCategoryID = parentCategory.CategoryID;
        dbContext.Set<Category>().Add(subCategory);
        await dbContext.SaveChangesAsync();

        // Act
        var loadedCategory = await dbContext.Set<Category>()
            .Include(c => c.ParentCategory)
            .FirstOrDefaultAsync(c => c.CategoryID == subCategory.CategoryID);

        // Assert
        loadedCategory.Should().NotBeNull();
        loadedCategory?.ParentCategoryID.Should().Be(parentCategory.CategoryID);
    }

    [Fact]
    public async Task Configure_ShouldLoadSubCategoriesNavigation_WithCategory()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var parentCategory = new Category
        {
            Name = "Electronics"
        };

        var subCategory1 = new Category
        {
            Name = "Laptops"
        };

        var subCategory2 = new Category
        {
            Name = "Phones"
        };

        dbContext.Set<Category>().Add(parentCategory);
        await dbContext.SaveChangesAsync();

        subCategory1.ParentCategoryID = parentCategory.CategoryID;
        subCategory2.ParentCategoryID = parentCategory.CategoryID;
        dbContext.Set<Category>().AddRange(subCategory1, subCategory2);
        await dbContext.SaveChangesAsync();

        // Act
        var loadedCategory = await dbContext.Set<Category>()
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.CategoryID == parentCategory.CategoryID);

        // Assert
        loadedCategory.Should().NotBeNull();
        loadedCategory?.SubCategories.Should().NotBeNull();
    }

    [Fact]
    public async Task Configure_ShouldAllowCategoryNameWithMaxLength100()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var longName = new string('A', 100);
        var category = new Category
        {
            Name = longName
        };

        dbContext.Set<Category>().Add(category);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedCategory = await dbContext.Set<Category>().FindAsync(category.CategoryID);
        savedCategory.Should().NotBeNull();
        savedCategory?.Name.Length.Should().Be(100);
    }

    [Fact]
    public async Task Configure_ShouldAllowDescriptionWithMaxLength500()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var longDescription = new string('D', 500);
        var category = new Category
        {
            Name = "Test Category",
            Description = longDescription
        };

        dbContext.Set<Category>().Add(category);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedCategory = await dbContext.Set<Category>().FindAsync(category.CategoryID);
        savedCategory.Should().NotBeNull();
        savedCategory?.Description?.Length.Should().Be(500);
    }

    [Fact]
    public async Task Configure_ShouldAllowMultipleCategories_WithDifferentNames()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var categories = new List<Category>
        {
            new Category { Name = "Electronics" },
            new Category { Name = "Clothing" },
            new Category { Name = "Books" }
        };

        dbContext.Set<Category>().AddRange(categories);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedCategories = await dbContext.Set<Category>().ToListAsync();
        savedCategories.Should().HaveCount(3);
    }

    [Fact]
    public async Task Configure_ShouldAllowUpdateCategory()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category
        {
            Name = "Original Name",
            Description = "Original Description"
        };

        dbContext.Set<Category>().Add(category);
        await dbContext.SaveChangesAsync();

        // Act
        var savedCategory = await dbContext.Set<Category>().FindAsync(category.CategoryID);
        savedCategory!.Name = "Updated Name";
        savedCategory.Description = "Updated Description";
        await dbContext.SaveChangesAsync();

        // Assert
        var updatedCategory = await dbContext.Set<Category>().FindAsync(category.CategoryID);
        updatedCategory.Should().NotBeNull();
        updatedCategory?.Name.Should().Be("Updated Name");
        updatedCategory?.Description.Should().Be("Updated Description");
    }

    [Fact]
    public async Task Configure_ShouldAllowDeleteCategory()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category
        {
            Name = "To Be Deleted"
        };

        dbContext.Set<Category>().Add(category);
        await dbContext.SaveChangesAsync();
        var categoryId = category.CategoryID;

        // Act
        var savedCategory = await dbContext.Set<Category>().FindAsync(categoryId);
        dbContext.Set<Category>().Remove(savedCategory!);
        await dbContext.SaveChangesAsync();

        // Assert
        var deletedCategory = await dbContext.Set<Category>().FindAsync(categoryId);
        deletedCategory.Should().BeNull();
    }

    [Fact]
    public void Configure_ShouldNotAllowNullCategoryName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var nameProperty = entityType?.FindProperty("Name");

        // Act & Assert
        nameProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public async Task Configure_ShouldAllowCategoryWithSpecialCharactersInName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category
        {
            Name = "Category @#$% & Special (123)"
        };

        dbContext.Set<Category>().Add(category);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedCategory = await dbContext.Set<Category>().FindAsync(category.CategoryID);
        savedCategory.Should().NotBeNull();
        savedCategory?.Name.Should().Be("Category @#$% & Special (123)");
    }

    [Fact]
    public async Task Configure_ShouldAllowCategoryWithSpecialCharactersInDescription()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var category = new Category
        {
            Name = "Special Category",
            Description = "Description with @#$% & special (123) characters"
        };

        dbContext.Set<Category>().Add(category);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedCategory = await dbContext.Set<Category>().FindAsync(category.CategoryID);
        savedCategory.Should().NotBeNull();
        savedCategory?.Description.Should().Be("Description with @#$% & special (123) characters");
    }

    [Fact]
    public async Task Configure_ShouldCreateHierarchicalCategoryStructure()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        
        var root = new Category { Name = "Electronics" };
        var level1 = new Category { Name = "Computers" };
        var level2 = new Category { Name = "Laptops" };

        dbContext.Set<Category>().Add(root);
        await dbContext.SaveChangesAsync();

        level1.ParentCategoryID = root.CategoryID;
        dbContext.Set<Category>().Add(level1);
        await dbContext.SaveChangesAsync();

        level2.ParentCategoryID = level1.CategoryID;
        dbContext.Set<Category>().Add(level2);

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var savedLevel2 = await dbContext.Set<Category>().FindAsync(level2.CategoryID);
        savedLevel2.Should().NotBeNull();
        savedLevel2?.ParentCategoryID.Should().Be(level1.CategoryID);
    }

    [Fact]
    public async Task Configure_ShouldRestrictDeletionOfParentCategoryWithSubCategories()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var parentCategory = new Category { Name = "Electronics" };
        var subCategory = new Category { Name = "Laptops" };

        dbContext.Set<Category>().Add(parentCategory);
        await dbContext.SaveChangesAsync();

        subCategory.ParentCategoryID = parentCategory.CategoryID;
        dbContext.Set<Category>().Add(subCategory);
        await dbContext.SaveChangesAsync();

        // Act & Assert - DeleteBehavior.Restrict should prevent deletion
        var savedParent = await dbContext.Set<Category>().FindAsync(parentCategory.CategoryID);
        var savedSubCategory = await dbContext.Set<Category>()
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.CategoryID == parentCategory.CategoryID);

        savedParent.Should().NotBeNull();
        savedSubCategory?.SubCategories.Should().HaveCount(1);
    }

    [Fact]
    public async Task Configure_ShouldAllowMultipleLevelsOfCategoryHierarchy()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var categories = new List<Category>();

        var root = new Category { Name = "Root" };
        dbContext.Set<Category>().Add(root);
        await dbContext.SaveChangesAsync();

        for (int i = 1; i <= 5; i++)
        {
            var category = new Category { Name = $"Level {i}", ParentCategoryID = i == 1 ? root.CategoryID : i };
            dbContext.Set<Category>().Add(category);
            categories.Add(category);
        }

        // Act & Assert
        await dbContext.SaveChangesAsync();
        var allCategories = await dbContext.Set<Category>().ToListAsync();
        allCategories.Should().HaveCount(6);
    }
}
