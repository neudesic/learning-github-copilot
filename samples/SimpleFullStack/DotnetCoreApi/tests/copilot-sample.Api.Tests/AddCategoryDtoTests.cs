using copilot_sample.Api.Models.Dtos;
using FluentAssertions;

public class AddCategoryDtoTests
{
    [Fact]
    public void AddCategoryDto_ShouldInitializeWithDefaultValues()
    {
        // Act
        var addCategoryDto = new AddCategoryDto();

        // Assert
        addCategoryDto.Name.Should().BeNull();
        addCategoryDto.Description.Should().BeNull();
        addCategoryDto.ParentCategoryID.Should().BeNull();
    }

    [Fact]
    public void AddCategoryDto_ShouldSetAndGetName()
    {
        // Arrange
        var addCategoryDto = new AddCategoryDto();
        var expectedName = "Electronics";

        // Act
        addCategoryDto.Name = expectedName;

        // Assert
        addCategoryDto.Name.Should().Be(expectedName);
    }

    [Fact]
    public void AddCategoryDto_ShouldSetAndGetDescription()
    {
        // Arrange
        var addCategoryDto = new AddCategoryDto();
        var expectedDescription = "Electronic devices and gadgets";

        // Act
        addCategoryDto.Description = expectedDescription;

        // Assert
        addCategoryDto.Description.Should().Be(expectedDescription);
    }

    [Fact]
    public void AddCategoryDto_ShouldSetAndGetParentCategoryID()
    {
        // Arrange
        var addCategoryDto = new AddCategoryDto();
        var expectedParentId = 5;

        // Act
        addCategoryDto.ParentCategoryID = expectedParentId;

        // Assert
        addCategoryDto.ParentCategoryID.Should().Be(expectedParentId);
    }

    [Fact]
    public void AddCategoryDto_ShouldInitializeWithAllProperties()
    {
        // Act
        var addCategoryDto = new AddCategoryDto
        {
            Name = "Main Category",
            Description = "A main category",
            ParentCategoryID = 10
        };

        // Assert
        addCategoryDto.Name.Should().Be("Main Category");
        addCategoryDto.Description.Should().Be("A main category");
        addCategoryDto.ParentCategoryID.Should().Be(10);
    }

    [Fact]
    public void AddCategoryDto_ShouldHandleNullDescription()
    {
        // Act
        var addCategoryDto = new AddCategoryDto
        {
            Name = "Category",
            Description = null
        };

        // Assert
        addCategoryDto.Description.Should().BeNull();
    }

    [Fact]
    public void AddCategoryDto_ShouldHandleNullParentCategoryID()
    {
        // Act
        var addCategoryDto = new AddCategoryDto
        {
            Name = "Root Category",
            ParentCategoryID = null
        };

        // Assert
        addCategoryDto.ParentCategoryID.Should().BeNull();
    }

    [Fact]
    public void AddCategoryDto_ShouldAllowPropertyModification()
    {
        // Arrange
        var addCategoryDto = new AddCategoryDto
        {
            Name = "Original Name",
            Description = "Original Description",
            ParentCategoryID = 1
        };

        // Act
        addCategoryDto.Name = "Modified Name";
        addCategoryDto.Description = "Modified Description";
        addCategoryDto.ParentCategoryID = 2;

        // Assert
        addCategoryDto.Name.Should().Be("Modified Name");
        addCategoryDto.Description.Should().Be("Modified Description");
        addCategoryDto.ParentCategoryID.Should().Be(2);
    }

    [Fact]
    public void AddCategoryDto_ShouldPreserveDataIntegrity()
    {
        // Arrange
        var addCategoryDto = new AddCategoryDto
        {
            Name = "Category 100",
            Description = "Description for category 100",
            ParentCategoryID = 50
        };

        // Act - No changes made
        var retrievedName = addCategoryDto.Name;
        var retrievedDescription = addCategoryDto.Description;
        var retrievedParentId = addCategoryDto.ParentCategoryID;

        // Assert
        retrievedName.Should().Be("Category 100");
        retrievedDescription.Should().Be("Description for category 100");
        retrievedParentId.Should().Be(50);
    }

    [Fact]
    public void AddCategoryDto_ShouldHandleSpecialCharactersInName()
    {
        // Arrange
        var specialName = "Category & Sub-Category (Test) [2024]";

        // Act
        var addCategoryDto = new AddCategoryDto
        {
            Name = specialName
        };

        // Assert
        addCategoryDto.Name.Should().Be(specialName);
    }

    [Fact]
    public void AddCategoryDto_ShouldHandleSpecialCharactersInDescription()
    {
        // Arrange
        var specialDescription = "Description with special chars: @#$%^&*(){}[]|\\:;\"'<>,.?/";

        // Act
        var addCategoryDto = new AddCategoryDto
        {
            Name = "Test",
            Description = specialDescription
        };

        // Assert
        addCategoryDto.Description.Should().Be(specialDescription);
    }

    [Fact]
    public void AddCategoryDto_ShouldHandleLongStrings()
    {
        // Arrange
        var longName = new string('A', 1000);
        var longDescription = new string('B', 5000);

        // Act
        var addCategoryDto = new AddCategoryDto
        {
            Name = longName,
            Description = longDescription
        };

        // Assert
        addCategoryDto.Name.Length.Should().Be(1000);
        addCategoryDto.Description.Length.Should().Be(5000);
    }

    [Fact]
    public void AddCategoryDto_ShouldHandleUnicodeCharacters()
    {
        // Arrange
        var unicodeName = "Категория 🎯 (测试)";
        var unicodeDescription = "Descripción con caracteres acentuados: àáâãäå";

        // Act
        var addCategoryDto = new AddCategoryDto
        {
            Name = unicodeName,
            Description = unicodeDescription
        };

        // Assert
        addCategoryDto.Name.Should().Be(unicodeName);
        addCategoryDto.Description.Should().Be(unicodeDescription);
    }

    [Fact]
    public void AddCategoryDto_ShouldHandleWhitespaceInStrings()
    {
        // Arrange
        var nameWithWhitespace = "  Category with spaces  ";
        var descriptionWithNewlines = "Line 1\nLine 2\nLine 3";

        // Act
        var addCategoryDto = new AddCategoryDto
        {
            Name = nameWithWhitespace,
            Description = descriptionWithNewlines
        };

        // Assert
        addCategoryDto.Name.Should().Be(nameWithWhitespace);
        addCategoryDto.Description.Should().Be(descriptionWithNewlines);
    }

    [Fact]
    public void AddCategoryDto_ShouldSupportZeroParentCategoryID()
    {
        // Act
        var addCategoryDto = new AddCategoryDto
        {
            Name = "Test",
            ParentCategoryID = 0
        };

        // Assert
        addCategoryDto.ParentCategoryID.Should().Be(0);
    }

    [Fact]
    public void AddCategoryDto_ShouldSupportNegativeParentCategoryID()
    {
        // Act
        var addCategoryDto = new AddCategoryDto
        {
            Name = "Test",
            ParentCategoryID = -1
        };

        // Assert
        addCategoryDto.ParentCategoryID.Should().Be(-1);
    }

    [Fact]
    public void AddCategoryDto_ShouldSupportLargeParentCategoryID()
    {
        // Arrange
        var largeId = int.MaxValue;

        // Act
        var addCategoryDto = new AddCategoryDto
        {
            Name = "Test",
            ParentCategoryID = largeId
        };

        // Assert
        addCategoryDto.ParentCategoryID.Should().Be(largeId);
    }

    [Fact]
    public void AddCategoryDto_ShouldCreateIndependentInstances()
    {
        // Arrange
        var addCategoryDto1 = new AddCategoryDto { Name = "Category 1", Description = "Description 1" };
        var addCategoryDto2 = new AddCategoryDto { Name = "Category 2", Description = "Description 2" };

        // Act & Assert
        addCategoryDto1.Name.Should().Be("Category 1");
        addCategoryDto2.Name.Should().Be("Category 2");
        addCategoryDto1.Description.Should().Be("Description 1");
        addCategoryDto2.Description.Should().Be("Description 2");
    }

    [Fact]
    public void AddCategoryDto_ShouldHandleEmptyStrings()
    {
        // Act
        var addCategoryDto = new AddCategoryDto
        {
            Name = "",
            Description = ""
        };

        // Assert
        addCategoryDto.Name.Should().Be("");
        addCategoryDto.Description.Should().Be("");
    }

    [Fact]
    public void AddCategoryDto_ShouldAllowNullToNonNullTransition()
    {
        // Arrange
        var addCategoryDto = new AddCategoryDto
        {
            Name = "Test",
            Description = null,
            ParentCategoryID = null
        };

        // Act
        addCategoryDto.Description = "New Description";
        addCategoryDto.ParentCategoryID = 10;

        // Assert
        addCategoryDto.Description.Should().Be("New Description");
        addCategoryDto.ParentCategoryID.Should().Be(10);
    }

    [Fact]
    public void AddCategoryDto_ShouldAllowNonNullToNullTransition()
    {
        // Arrange
        var addCategoryDto = new AddCategoryDto
        {
            Name = "Test",
            Description = "Some Description",
            ParentCategoryID = 10
        };

        // Act
        addCategoryDto.Description = null;
        addCategoryDto.ParentCategoryID = null;

        // Assert
        addCategoryDto.Description.Should().BeNull();
        addCategoryDto.ParentCategoryID.Should().BeNull();
    }
}

public class UpdateCategoryDescriptionDtoTests
{
    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldInitializeWithDefaultValues()
    {
        // Act
        var updateDto = new UpdateCategoryDescriptionDto();

        // Assert
        updateDto.Description.Should().BeNull();
    }

    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldSetAndGetDescription()
    {
        // Arrange
        var updateDto = new UpdateCategoryDescriptionDto();
        var expectedDescription = "Updated description";

        // Act
        updateDto.Description = expectedDescription;

        // Assert
        updateDto.Description.Should().Be(expectedDescription);
    }

    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldHandleNullDescription()
    {
        // Act
        var updateDto = new UpdateCategoryDescriptionDto
        {
            Description = null
        };

        // Assert
        updateDto.Description.Should().BeNull();
    }

    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldAllowPropertyModification()
    {
        // Arrange
        var updateDto = new UpdateCategoryDescriptionDto
        {
            Description = "Original Description"
        };

        // Act
        updateDto.Description = "Modified Description";

        // Assert
        updateDto.Description.Should().Be("Modified Description");
    }

    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldHandleSpecialCharacters()
    {
        // Arrange
        var specialDescription = "Description with special chars: @#$%^&*(){}[]|\\:;\"'<>,.?/";

        // Act
        var updateDto = new UpdateCategoryDescriptionDto
        {
            Description = specialDescription
        };

        // Assert
        updateDto.Description.Should().Be(specialDescription);
    }

    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldHandleLongStrings()
    {
        // Arrange
        var longDescription = new string('B', 5000);

        // Act
        var updateDto = new UpdateCategoryDescriptionDto
        {
            Description = longDescription
        };

        // Assert
        updateDto.Description.Length.Should().Be(5000);
    }

    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldHandleUnicodeCharacters()
    {
        // Arrange
        var unicodeDescription = "Descripción con caracteres acentuados: àáâãäå 测试 🎯";

        // Act
        var updateDto = new UpdateCategoryDescriptionDto
        {
            Description = unicodeDescription
        };

        // Assert
        updateDto.Description.Should().Be(unicodeDescription);
    }

    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldHandleWhitespaceInStrings()
    {
        // Arrange
        var descriptionWithNewlines = "Line 1\nLine 2\nLine 3";

        // Act
        var updateDto = new UpdateCategoryDescriptionDto
        {
            Description = descriptionWithNewlines
        };

        // Assert
        updateDto.Description.Should().Be(descriptionWithNewlines);
    }

    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldHandleEmptyString()
    {
        // Act
        var updateDto = new UpdateCategoryDescriptionDto
        {
            Description = ""
        };

        // Assert
        updateDto.Description.Should().Be("");
    }

    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldAllowNullToNonNullTransition()
    {
        // Arrange
        var updateDto = new UpdateCategoryDescriptionDto
        {
            Description = null
        };

        // Act
        updateDto.Description = "New Description";

        // Assert
        updateDto.Description.Should().Be("New Description");
    }

    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldAllowNonNullToNullTransition()
    {
        // Arrange
        var updateDto = new UpdateCategoryDescriptionDto
        {
            Description = "Some Description"
        };

        // Act
        updateDto.Description = null;

        // Assert
        updateDto.Description.Should().BeNull();
    }

    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldCreateIndependentInstances()
    {
        // Arrange
        var updateDto1 = new UpdateCategoryDescriptionDto { Description = "Description 1" };
        var updateDto2 = new UpdateCategoryDescriptionDto { Description = "Description 2" };

        // Act & Assert
        updateDto1.Description.Should().Be("Description 1");
        updateDto2.Description.Should().Be("Description 2");
    }

    [Fact]
    public void UpdateCategoryDescriptionDto_ShouldPreserveDataIntegrity()
    {
        // Arrange
        var updateDto = new UpdateCategoryDescriptionDto
        {
            Description = "Original Description"
        };

        // Act - No changes made
        var retrievedDescription = updateDto.Description;

        // Assert
        retrievedDescription.Should().Be("Original Description");
    }
}
