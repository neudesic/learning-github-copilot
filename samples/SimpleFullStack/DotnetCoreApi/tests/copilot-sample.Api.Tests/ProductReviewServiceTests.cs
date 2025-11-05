using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.Api.Models.Dtos;
using copilot_sample.Api.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class ProductReviewServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetProductReviewsAsync_ShouldReturnAllReviews()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        
        dbContext.ProductReviews.AddRange(new List<ProductReview>
        {
            new ProductReview { ReviewID = 1, ProductID = 1, ReviewerName = "John", Rating = 5, Comment = "Great!", ReviewDate = DateTime.UtcNow },
            new ProductReview { ReviewID = 2, ProductID = 1, ReviewerName = "Jane", Rating = 4, Comment = "Good", ReviewDate = DateTime.UtcNow }
        });
        await dbContext.SaveChangesAsync();

        var reviewService = new ProductReviewService(dbContext);

        // Act
        var result = await reviewService.GetProductReviewsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.ReviewerName == "John");
        result.Should().Contain(r => r.ReviewerName == "Jane");
    }

    [Fact]
    public async Task GetProductReviewByIdAsync_ShouldReturnReview_WhenReviewExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        dbContext.ProductReviews.Add(new ProductReview { ReviewID = 1, ProductID = 1, ReviewerName = "John", Rating = 5, Comment = "Great!", ReviewDate = DateTime.UtcNow });
        await dbContext.SaveChangesAsync();

        var reviewService = new ProductReviewService(dbContext);

        // Act
        var result = await reviewService.GetProductReviewByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.ReviewerName.Should().Be("John");
        result.Rating.Should().Be(5);
    }

    [Fact]
    public async Task GetProductReviewByIdAsync_ShouldReturnNull_WhenReviewDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var reviewService = new ProductReviewService(dbContext);

        // Act
        var result = await reviewService.GetProductReviewByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProductReviewsByProductIdAsync_ShouldReturnReviewsForProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product1 = new Product { ProductID = 1, Name = "Product 1", SKU = "P001", CategoryID = 1 };
        var product2 = new Product { ProductID = 2, Name = "Product 2", SKU = "P002", CategoryID = 1 };
        dbContext.Products.AddRange(product1, product2);
        
        dbContext.ProductReviews.AddRange(new List<ProductReview>
        {
            new ProductReview { ReviewID = 1, ProductID = 1, ReviewerName = "John", Rating = 5, ReviewDate = DateTime.UtcNow },
            new ProductReview { ReviewID = 2, ProductID = 1, ReviewerName = "Jane", Rating = 4, ReviewDate = DateTime.UtcNow },
            new ProductReview { ReviewID = 3, ProductID = 2, ReviewerName = "Bob", Rating = 3, ReviewDate = DateTime.UtcNow }
        });
        await dbContext.SaveChangesAsync();

        var reviewService = new ProductReviewService(dbContext);

        // Act
        var result = await reviewService.GetProductReviewsByProductIdAsync(1);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(r => r.ProductID.Should().Be(1));
    }

    [Fact]
    public async Task GetProductReviewsByProductIdAsync_ShouldReturnEmptyList_WhenNoReviewsForProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var reviewService = new ProductReviewService(dbContext);

        // Act
        var result = await reviewService.GetProductReviewsByProductIdAsync(999);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddProductReviewAsync_ShouldAddReview_WhenProductExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var reviewService = new ProductReviewService(dbContext);
        var addReviewDto = new AddProductReviewDto
        {
            ProductID = 1,
            ReviewerName = "John",
            Rating = 5,
            Comment = "Excellent product!"
        };

        // Act
        var result = await reviewService.AddProductReviewAsync(addReviewDto);

        // Assert
        result.Should().NotBeNull();
        result.ReviewerName.Should().Be("John");
        result.Rating.Should().Be(5);
        result.Comment.Should().Be("Excellent product!");
        (await dbContext.ProductReviews.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task AddProductReviewAsync_ShouldThrowException_WhenProductDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var reviewService = new ProductReviewService(dbContext);

        var addReviewDto = new AddProductReviewDto
        {
            ProductID = 999,
            ReviewerName = "John",
            Rating = 5,
            Comment = "Test"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => reviewService.AddProductReviewAsync(addReviewDto));
    }

    [Fact]
    public async Task AddProductReviewAsync_ShouldThrowException_WhenRatingBelowMinimum()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var reviewService = new ProductReviewService(dbContext);
        var addReviewDto = new AddProductReviewDto
        {
            ProductID = 1,
            ReviewerName = "John",
            Rating = 0,
            Comment = "Test"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => reviewService.AddProductReviewAsync(addReviewDto));
    }

    [Fact]
    public async Task AddProductReviewAsync_ShouldThrowException_WhenRatingAboveMaximum()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var reviewService = new ProductReviewService(dbContext);
        var addReviewDto = new AddProductReviewDto
        {
            ProductID = 1,
            ReviewerName = "John",
            Rating = 6,
            Comment = "Test"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => reviewService.AddProductReviewAsync(addReviewDto));
    }

    [Fact]
    public async Task UpdateProductReviewAsync_ShouldUpdateReview_WhenReviewExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        dbContext.ProductReviews.Add(new ProductReview { ReviewID = 1, ProductID = 1, ReviewerName = "John", Rating = 5, Comment = "Great!", ReviewDate = DateTime.UtcNow });
        await dbContext.SaveChangesAsync();

        var reviewService = new ProductReviewService(dbContext);
        var updateDto = new UpdateProductReviewDto
        {
            ReviewerName = "Jane",
            Rating = 4,
            Comment = "Good"
        };

        // Act
        var result = await reviewService.UpdateProductReviewAsync(1, updateDto);

        // Assert
        result.Should().BeTrue();
        var updatedReview = await dbContext.ProductReviews.FindAsync(1);
        updatedReview!.ReviewerName.Should().Be("Jane");
        updatedReview.Rating.Should().Be(4);
        updatedReview.Comment.Should().Be("Good");
    }

    [Fact]
    public async Task UpdateProductReviewAsync_ShouldReturnFalse_WhenReviewDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var reviewService = new ProductReviewService(dbContext);

        var updateDto = new UpdateProductReviewDto
        {
            ReviewerName = "Jane",
            Rating = 4,
            Comment = "Good"
        };

        // Act
        var result = await reviewService.UpdateProductReviewAsync(999, updateDto);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateProductReviewAsync_ShouldThrowException_WhenRatingInvalid()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        dbContext.ProductReviews.Add(new ProductReview { ReviewID = 1, ProductID = 1, ReviewerName = "John", Rating = 5, ReviewDate = DateTime.UtcNow });
        await dbContext.SaveChangesAsync();

        var reviewService = new ProductReviewService(dbContext);
        var updateDto = new UpdateProductReviewDto
        {
            Rating = 10
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => reviewService.UpdateProductReviewAsync(1, updateDto));
    }

    [Fact]
    public async Task UpdateProductReviewAsync_ShouldPartiallyUpdate_WhenOnlyRatingProvided()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        dbContext.ProductReviews.Add(new ProductReview { ReviewID = 1, ProductID = 1, ReviewerName = "John", Rating = 5, Comment = "Original", ReviewDate = DateTime.UtcNow });
        await dbContext.SaveChangesAsync();

        var reviewService = new ProductReviewService(dbContext);
        var updateDto = new UpdateProductReviewDto { Rating = 3 };

        // Act
        await reviewService.UpdateProductReviewAsync(1, updateDto);

        // Assert
        var review = await dbContext.ProductReviews.FindAsync(1);
        review!.Rating.Should().Be(3);
        review.ReviewerName.Should().Be("John");
        review.Comment.Should().Be("Original");
    }

    [Fact]
    public async Task DeleteProductReviewAsync_ShouldDeleteReview_WhenReviewExists()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var product = new Product { ProductID = 1, Name = "Test Product", SKU = "TEST-001", CategoryID = 1 };
        dbContext.Products.Add(product);
        dbContext.ProductReviews.Add(new ProductReview { ReviewID = 1, ProductID = 1, ReviewerName = "John", Rating = 5, ReviewDate = DateTime.UtcNow });
        await dbContext.SaveChangesAsync();

        var reviewService = new ProductReviewService(dbContext);

        // Act
        var result = await reviewService.DeleteProductReviewAsync(1);

        // Assert
        result.Should().BeTrue();
        var deletedReview = await dbContext.ProductReviews.FindAsync(1);
        deletedReview.Should().BeNull();
    }

    [Fact]
    public async Task DeleteProductReviewAsync_ShouldReturnFalse_WhenReviewDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var reviewService = new ProductReviewService(dbContext);

        // Act
        var result = await reviewService.DeleteProductReviewAsync(999);

        // Assert
        result.Should().BeFalse();
    }
}
