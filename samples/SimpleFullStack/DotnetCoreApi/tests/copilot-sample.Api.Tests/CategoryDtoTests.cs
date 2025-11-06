using copilot_sample.Api.Models.Dtos;
using FluentAssertions;

public class CategoryDtoTests
{
    [Fact]
    public void CategoryDto_ShouldInitializeWithDefaultValues()
    {
        // Act
        var categoryDto = new CategoryDto();

        // Assert
        categoryDto.CategoryID.Should().Be(0);
        categoryDto.Name.Should().BeNull();
        categoryDto.Description.Should().BeNull();
        categoryDto.ParentCategoryID.Should().BeNull();
        categoryDto.SubCategories.Should().BeNull();
    }

    [Fact]
    public void CategoryDto_ShouldSetAndGetCategoryID()
    {
        // Arrange
        var categoryDto = new CategoryDto();
        var expectedId = 42;

        // Act
        categoryDto.CategoryID = expectedId;

        // Assert
        categoryDto.CategoryID.Should().Be(expectedId);
    }

    [Fact]
    public void CategoryDto_ShouldSetAndGetName()
    {
        // Arrange
        var categoryDto = new CategoryDto();
        var expectedName = "Electronics";

        // Act
        categoryDto.Name = expectedName;

        // Assert
        categoryDto.Name.Should().Be(expectedName);
    }

    [Fact]
    public void CategoryDto_ShouldSetAndGetDescription()
    {
        // Arrange
        var categoryDto = new CategoryDto();
        var expectedDescription = "Electronic devices and gadgets";

        // Act
        categoryDto.Description = expectedDescription;

        // Assert
        categoryDto.Description.Should().Be(expectedDescription);
    }

    [Fact]
    public void CategoryDto_ShouldSetAndGetParentCategoryID()
    {
        // Arrange
        var categoryDto = new CategoryDto();
        var expectedParentId = 5;

        // Act
        categoryDto.ParentCategoryID = expectedParentId;

        // Assert
        categoryDto.ParentCategoryID.Should().Be(expectedParentId);
    }

    [Fact]
    public void CategoryDto_ShouldSetAndGetSubCategories()
    {
        // Arrange
        var categoryDto = new CategoryDto();
        var subCategories = new List<CategoryDto>
        {
            new CategoryDto { CategoryID = 2, Name = "Laptops" },
            new CategoryDto { CategoryID = 3, Name = "Desktops" }
        };

        // Act
        categoryDto.SubCategories = subCategories;

        // Assert
        categoryDto.SubCategories.Should().NotBeNull();
        categoryDto.SubCategories.Should().HaveCount(2);
        categoryDto.SubCategories[0].Name.Should().Be("Laptops");
        categoryDto.SubCategories[1].Name.Should().Be("Desktops");
    }

    [Fact]
    public void CategoryDto_ShouldInitializeWithAllProperties()
    {
        // Arrange
        var subCategories = new List<CategoryDto>
        {
            new CategoryDto { CategoryID = 2, Name = "Subcategory 1" }
        };

        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = "Main Category",
            Description = "A main category",
            ParentCategoryID = null,
            SubCategories = subCategories
        };

        // Assert
        categoryDto.CategoryID.Should().Be(1);
        categoryDto.Name.Should().Be("Main Category");
        categoryDto.Description.Should().Be("A main category");
        categoryDto.ParentCategoryID.Should().BeNull();
        categoryDto.SubCategories.Should().HaveCount(1);
    }

    [Fact]
    public void CategoryDto_ShouldHandleNullDescription()
    {
        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = "Category",
            Description = null
        };

        // Assert
        categoryDto.Description.Should().BeNull();
    }

    [Fact]
    public void CategoryDto_ShouldHandleNullParentCategoryID()
    {
        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = "Root Category",
            ParentCategoryID = null
        };

        // Assert
        categoryDto.ParentCategoryID.Should().BeNull();
    }

    [Fact]
    public void CategoryDto_ShouldHandleNullSubCategories()
    {
        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = "Category",
            SubCategories = null
        };

        // Assert
        categoryDto.SubCategories.Should().BeNull();
    }

    [Fact]
    public void CategoryDto_ShouldHandleEmptySubCategoriesList()
    {
        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = "Category",
            SubCategories = new List<CategoryDto>()
        };

        // Assert
        categoryDto.SubCategories.Should().NotBeNull();
        categoryDto.SubCategories.Should().HaveCount(0);
    }

    [Fact]
    public void CategoryDto_ShouldSupportMultipleSubCategories()
    {
        // Arrange
        var subCategories = new List<CategoryDto>
        {
            new CategoryDto { CategoryID = 2, Name = "Sub1", Description = "Description 1" },
            new CategoryDto { CategoryID = 3, Name = "Sub2", Description = "Description 2" },
            new CategoryDto { CategoryID = 4, Name = "Sub3", Description = "Description 3" }
        };

        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = "Parent Category",
            SubCategories = subCategories
        };

        // Assert
        categoryDto.SubCategories.Should().HaveCount(3);
        categoryDto.SubCategories[0].CategoryID.Should().Be(2);
        categoryDto.SubCategories[1].CategoryID.Should().Be(3);
        categoryDto.SubCategories[2].CategoryID.Should().Be(4);
    }

    [Fact]
    public void CategoryDto_ShouldSupportNestedSubCategories()
    {
        // Arrange
        var nestedSubCategories = new List<CategoryDto>
        {
            new CategoryDto { CategoryID = 3, Name = "Nested Sub1" }
        };

        var subCategories = new List<CategoryDto>
        {
            new CategoryDto { CategoryID = 2, Name = "Sub1", SubCategories = nestedSubCategories }
        };

        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = "Parent",
            SubCategories = subCategories
        };

        // Act & Assert
        categoryDto.SubCategories.Should().HaveCount(1);
        categoryDto.SubCategories![0].SubCategories.Should().HaveCount(1);
        categoryDto.SubCategories[0].SubCategories![0].CategoryID.Should().Be(3);
        categoryDto.SubCategories[0].SubCategories[0].Name.Should().Be("Nested Sub1");
    }

    [Fact]
    public void CategoryDto_ShouldAllowPropertyModification()
    {
        // Arrange
        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = "Original Name",
            Description = "Original Description"
        };

        // Act
        categoryDto.Name = "Modified Name";
        categoryDto.Description = "Modified Description";

        // Assert
        categoryDto.Name.Should().Be("Modified Name");
        categoryDto.Description.Should().Be("Modified Description");
    }

    [Fact]
    public void CategoryDto_ShouldPreserveDataIntegrity()
    {
        // Arrange
        var categoryDto = new CategoryDto
        {
            CategoryID = 100,
            Name = "Category 100",
            Description = "Description for category 100",
            ParentCategoryID = 50,
            SubCategories = new List<CategoryDto>
            {
                new CategoryDto { CategoryID = 101, Name = "Sub Category" }
            }
        };

        // Act - No changes made
        var retrievedId = categoryDto.CategoryID;
        var retrievedName = categoryDto.Name;
        var retrievedDescription = categoryDto.Description;
        var retrievedParentId = categoryDto.ParentCategoryID;
        var retrievedSubCount = categoryDto.SubCategories?.Count ?? 0;

        // Assert
        retrievedId.Should().Be(100);
        retrievedName.Should().Be("Category 100");
        retrievedDescription.Should().Be("Description for category 100");
        retrievedParentId.Should().Be(50);
        retrievedSubCount.Should().Be(1);
    }

    [Fact]
    public void CategoryDto_ShouldHandleSpecialCharactersInName()
    {
        // Arrange
        var specialName = "Category & Sub-Category (Test) [2024]";

        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = specialName
        };

        // Assert
        categoryDto.Name.Should().Be(specialName);
    }

    [Fact]
    public void CategoryDto_ShouldHandleSpecialCharactersInDescription()
    {
        // Arrange
        var specialDescription = "Description with special chars: @#$%^&*(){}[]|\\:;\"'<>,.?/";

        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = "Test",
            Description = specialDescription
        };

        // Assert
        categoryDto.Description.Should().Be(specialDescription);
    }

    [Fact]
    public void CategoryDto_ShouldHandleLongStrings()
    {
        // Arrange
        var longName = new string('A', 1000);
        var longDescription = new string('B', 5000);

        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = longName,
            Description = longDescription
        };

        // Assert
        categoryDto.Name.Length.Should().Be(1000);
        categoryDto.Description.Length.Should().Be(5000);
    }

    [Fact]
    public void CategoryDto_ShouldHandleUnicodeCharacters()
    {
        // Arrange
        var unicodeName = "Категория 🎯 (测试)";
        var unicodeDescription = "Descripción con caracteres acentuados: àáâãäå";

        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = unicodeName,
            Description = unicodeDescription
        };

        // Assert
        categoryDto.Name.Should().Be(unicodeName);
        categoryDto.Description.Should().Be(unicodeDescription);
    }

    [Fact]
    public void CategoryDto_ShouldHandleWhitespaceInStrings()
    {
        // Arrange
        var nameWithWhitespace = "  Category with spaces  ";
        var descriptionWithNewlines = "Line 1\nLine 2\nLine 3";

        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = 1,
            Name = nameWithWhitespace,
            Description = descriptionWithNewlines
        };

        // Assert
        categoryDto.Name.Should().Be(nameWithWhitespace);
        categoryDto.Description.Should().Be(descriptionWithNewlines);
    }

    [Fact]
    public void CategoryDto_ShouldSupportZeroCategoryID()
    {
        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = 0,
            Name = "Test"
        };

        // Assert
        categoryDto.CategoryID.Should().Be(0);
    }

    [Fact]
    public void CategoryDto_ShouldSupportNegativeCategoryID()
    {
        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = -1,
            Name = "Test"
        };

        // Assert
        categoryDto.CategoryID.Should().Be(-1);
    }

    [Fact]
    public void CategoryDto_ShouldSupportLargeCategoryID()
    {
        // Arrange
        var largeId = int.MaxValue;

        // Act
        var categoryDto = new CategoryDto
        {
            CategoryID = largeId,
            Name = "Test"
        };

        // Assert
        categoryDto.CategoryID.Should().Be(largeId);
    }

    [Fact]
    public void CategoryDto_ShouldCreateIndependentInstances()
    {
        // Arrange
        var categoryDto1 = new CategoryDto { CategoryID = 1, Name = "Category 1" };
        var categoryDto2 = new CategoryDto { CategoryID = 2, Name = "Category 2" };

        // Act & Assert
        categoryDto1.CategoryID.Should().Be(1);
        categoryDto2.CategoryID.Should().Be(2);
        categoryDto1.Name.Should().Be("Category 1");
        categoryDto2.Name.Should().Be("Category 2");
    }
}
