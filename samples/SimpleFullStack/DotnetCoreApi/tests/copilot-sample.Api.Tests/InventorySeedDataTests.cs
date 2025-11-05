using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.DataAccess.SeedData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace copilot_sample.Test;

public class InventorySeedDataTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    #region GetCategories Tests

    [Fact]
    public void GetCategories_ShouldReturnFourCategories()
    {
        // Act
        var categories = InventorySeedData.GetCategories();

        // Assert
        categories.Should().HaveCount(4);
    }

    [Fact]
    public void GetCategories_ShouldContainElectronicsCategory()
    {
        // Act
        var categories = InventorySeedData.GetCategories();

        // Assert
        categories.Should().Contain(c => c.CategoryID == 1 && c.Name == "Electronics");
    }

    [Fact]
    public void GetCategories_ShouldHaveCorrectParentCategoryRelationships()
    {
        // Act
        var categories = InventorySeedData.GetCategories();
        var laptopCategory = categories.FirstOrDefault(c => c.Name == "Laptops");
        var smartphoneCategory = categories.FirstOrDefault(c => c.Name == "Smartphones");
        var accessoriesCategory = categories.FirstOrDefault(c => c.Name == "Accessories");

        // Assert
        laptopCategory.Should().NotBeNull();
        laptopCategory!.ParentCategoryID.Should().Be(1);
        smartphoneCategory.Should().NotBeNull();
        smartphoneCategory!.ParentCategoryID.Should().Be(1);
        accessoriesCategory.Should().NotBeNull();
        accessoriesCategory!.ParentCategoryID.Should().Be(1);
    }

    [Fact]
    public void GetCategories_ShouldHaveElectronicsAsRootCategory()
    {
        // Act
        var categories = InventorySeedData.GetCategories();
        var electronicsCategory = categories.FirstOrDefault(c => c.Name == "Electronics");

        // Assert
        electronicsCategory.Should().NotBeNull();
        electronicsCategory!.ParentCategoryID.Should().BeNull();
    }

    #endregion

    #region GetProducts Tests

    [Fact]
    public void GetProducts_ShouldReturnSevenProducts()
    {
        // Act
        var products = InventorySeedData.GetProducts();

        // Assert
        products.Should().HaveCount(7);
    }

    [Fact]
    public void GetProducts_ShouldContainUltraBookX1()
    {
        // Act
        var products = InventorySeedData.GetProducts();

        // Assert
        products.Should().Contain(p => p.ProductID == 1 && p.Name == "UltraBook X1" && p.SKU == "SKU-UBX1");
    }

    [Fact]
    public void GetProducts_ShouldHaveAllProductsAsActive()
    {
        // Act
        var products = InventorySeedData.GetProducts();

        // Assert
        products.Should().AllSatisfy(p => p.IsActive.Should().BeTrue());
    }

    [Fact]
    public void GetProducts_ShouldHaveUniqueSKUs()
    {
        // Act
        var products = InventorySeedData.GetProducts();
        var skus = products.Select(p => p.SKU).ToList();

        // Assert
        skus.Should().HaveSameCount(skus.Distinct());
    }

    [Fact]
    public void GetProducts_ShouldHaveCreatedAtAndUpdatedAtDates()
    {
        // Act
        var products = InventorySeedData.GetProducts();

        // Assert
        products.Should().AllSatisfy(p =>
        {
            p.CreatedAt.Should().NotBe(default(DateTime));
            p.UpdatedAt.Should().NotBe(default(DateTime));
        });
    }

    [Fact]
    public void GetProducts_ShouldHaveValidCategoryReferences()
    {
        // Act
        var products = InventorySeedData.GetProducts();
        var categories = InventorySeedData.GetCategories();
        var validCategoryIds = categories.Select(c => c.CategoryID).ToList();

        // Assert
        products.Should().AllSatisfy(p =>
        {
            validCategoryIds.Should().Contain(p.CategoryID);
        });
    }

    #endregion

    #region GetProductPrices Tests

    [Fact]
    public void GetProductPrices_ShouldReturnEightPrices()
    {
        // Act
        var prices = InventorySeedData.GetProductPrices();

        // Assert
        prices.Should().HaveCount(8);
    }

    [Fact]
    public void GetProductPrices_ShouldAllBeInUSD()
    {
        // Act
        var prices = InventorySeedData.GetProductPrices();

        // Assert
        prices.Should().AllSatisfy(p => p.CurrencyCode.Should().Be("USD"));
    }

    [Fact]
    public void GetProductPrices_ShouldHavePositivePrices()
    {
        // Act
        var prices = InventorySeedData.GetProductPrices();

        // Assert
        prices.Should().AllSatisfy(p => p.Price.Should().BeGreaterThan(0));
    }

    [Fact]
    public void GetProductPrices_ShouldHaveValidProductReferences()
    {
        // Act
        var prices = InventorySeedData.GetProductPrices();
        var products = InventorySeedData.GetProducts();
        var validProductIds = products.Select(p => p.ProductID).ToList();

        // Assert
        prices.Should().AllSatisfy(p =>
        {
            validProductIds.Should().Contain(p.ProductID);
        });
    }

    [Fact]
    public void GetProductPrices_ShouldHaveEffectiveFromDate()
    {
        // Act
        var prices = InventorySeedData.GetProductPrices();

        // Assert
        prices.Should().AllSatisfy(p => p.EffectiveFrom.Should().NotBe(default(DateTime)));
    }

    [Fact]
    public void GetProductPrices_Product1ShouldHaveActiveAndHistoricalPrices()
    {
        // Act
        var prices = InventorySeedData.GetProductPrices();
        var product1Prices = prices.Where(p => p.ProductID == 1).ToList();

        // Assert
        product1Prices.Should().HaveCount(2);
        product1Prices.Should().Contain(p => p.EffectiveTill == null);
        product1Prices.Should().Contain(p => p.EffectiveTill != null);
    }

    #endregion

    #region GetInventoryItems Tests

    [Fact]
    public void GetInventoryItems_ShouldReturnSevenItems()
    {
        // Act
        var inventory = InventorySeedData.GetInventoryItems();

        // Assert
        inventory.Should().HaveCount(7);
    }

    [Fact]
    public void GetInventoryItems_ShouldHaveNonNegativeQuantities()
    {
        // Act
        var inventory = InventorySeedData.GetInventoryItems();

        // Assert
        inventory.Should().AllSatisfy(i => i.Quantity.Should().BeGreaterThanOrEqualTo(0));
    }

    [Fact]
    public void GetInventoryItems_ShouldHaveValidProductReferences()
    {
        // Act
        var inventory = InventorySeedData.GetInventoryItems();
        var products = InventorySeedData.GetProducts();
        var validProductIds = products.Select(p => p.ProductID).ToList();

        // Assert
        inventory.Should().AllSatisfy(i =>
        {
            validProductIds.Should().Contain(i.ProductID);
        });
    }

    [Fact]
    public void GetInventoryItems_ShouldHaveLastUpdatedDate()
    {
        // Act
        var inventory = InventorySeedData.GetInventoryItems();

        // Assert
        inventory.Should().AllSatisfy(i => i.LastUpdated.Should().NotBe(default(DateTime)));
    }

    [Fact]
    public void GetInventoryItems_ShouldHaveQuantitiesBetweenTenAndTwoHundred()
    {
        // Act
        var inventory = InventorySeedData.GetInventoryItems();

        // Assert
        inventory.Should().AllSatisfy(i => 
        {
            i.Quantity.Should().BeGreaterThanOrEqualTo(10);
            i.Quantity.Should().BeLessThanOrEqualTo(200);
        });
    }

    #endregion

    #region GetProductAttributes Tests

    [Fact]
    public void GetProductAttributes_ShouldReturnTwentyOneAttributes()
    {
        // Act
        var attributes = InventorySeedData.GetProductAttributes();

        // Assert
        attributes.Should().HaveCount(21);
    }

    [Fact]
    public void GetProductAttributes_ShouldHaveValidProductReferences()
    {
        // Act
        var attributes = InventorySeedData.GetProductAttributes();
        var products = InventorySeedData.GetProducts();
        var validProductIds = products.Select(p => p.ProductID).ToList();

        // Assert
        attributes.Should().AllSatisfy(a =>
        {
            validProductIds.Should().Contain(a.ProductID);
        });
    }

    [Fact]
    public void GetProductAttributes_ShouldHaveNonEmptyAttributeNames()
    {
        // Act
        var attributes = InventorySeedData.GetProductAttributes();

        // Assert
        attributes.Should().AllSatisfy(a => a.AttributeName.Should().NotBeNullOrEmpty());
    }

    [Fact]
    public void GetProductAttributes_ShouldHaveNonEmptyAttributeValues()
    {
        // Act
        var attributes = InventorySeedData.GetProductAttributes();

        // Assert
        attributes.Should().AllSatisfy(a => a.AttributeValue.Should().NotBeNullOrEmpty());
    }

    [Fact]
    public void GetProductAttributes_Product1ShouldHaveProcessorRAMAndStorage()
    {
        // Act
        var attributes = InventorySeedData.GetProductAttributes();
        var product1Attributes = attributes.Where(a => a.ProductID == 1).ToList();

        // Assert
        product1Attributes.Should().HaveCount(3);
        product1Attributes.Should().Contain(a => a.AttributeName == "Processor");
        product1Attributes.Should().Contain(a => a.AttributeName == "RAM");
        product1Attributes.Should().Contain(a => a.AttributeName == "Storage");
    }

    #endregion

    #region GetProductReviews Tests

    [Fact]
    public void GetProductReviews_ShouldReturnThirteenReviews()
    {
        // Act
        var reviews = InventorySeedData.GetProductReviews();

        // Assert
        reviews.Should().HaveCount(13);
    }

    [Fact]
    public void GetProductReviews_ShouldHaveValidRatings()
    {
        // Act
        var reviews = InventorySeedData.GetProductReviews();

        // Assert
        reviews.Should().AllSatisfy(r => 
        {
            r.Rating.Should().BeGreaterThanOrEqualTo(1);
            r.Rating.Should().BeLessThanOrEqualTo(5);
        });
    }

    [Fact]
    public void GetProductReviews_ShouldHaveNonEmptyReviewerNames()
    {
        // Act
        var reviews = InventorySeedData.GetProductReviews();

        // Assert
        reviews.Should().AllSatisfy(r => r.ReviewerName.Should().NotBeNullOrEmpty());
    }

    [Fact]
    public void GetProductReviews_ShouldHaveValidProductReferences()
    {
        // Act
        var reviews = InventorySeedData.GetProductReviews();
        var products = InventorySeedData.GetProducts();
        var validProductIds = products.Select(p => p.ProductID).ToList();

        // Assert
        reviews.Should().AllSatisfy(r =>
        {
            validProductIds.Should().Contain(r.ProductID);
        });
    }

    [Fact]
    public void GetProductReviews_ShouldHaveReviewDates()
    {
        // Act
        var reviews = InventorySeedData.GetProductReviews();

        // Assert
        reviews.Should().AllSatisfy(r => r.ReviewDate.Should().NotBe(default(DateTime)));
    }

    [Fact]
    public void GetProductReviews_Product1ShouldHaveTwoReviews()
    {
        // Act
        var reviews = InventorySeedData.GetProductReviews();
        var product1Reviews = reviews.Where(r => r.ProductID == 1).ToList();

        // Assert
        product1Reviews.Should().HaveCount(2);
    }

    #endregion

    #region GetSeedDataCounts Tests

    [Fact]
    public void GetSeedDataCounts_ShouldReturnCorrectCounts()
    {
        // Act
        var counts = InventorySeedData.GetSeedDataCounts();

        // Assert
        counts.Categories.Should().Be(4);
        counts.Products.Should().Be(7);
        counts.Prices.Should().Be(8);
        counts.Inventory.Should().Be(7);
        counts.Attributes.Should().Be(21);
        counts.Reviews.Should().Be(13);
    }

    #endregion

    #region ApplyToContextAsync Tests

    [Fact]
    public async Task ApplyToContextAsync_ShouldSeedAllData()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();

        // Act
        await InventorySeedData.ApplyToContextAsync(dbContext);

        // Assert
        dbContext.Categories.Should().HaveCount(4);
        dbContext.Products.Should().HaveCount(7);
        dbContext.ProductPrices.Should().HaveCount(8);
        dbContext.Inventory.Should().HaveCount(7);
        dbContext.ProductAttributes.Should().HaveCount(21);
        dbContext.ProductReviews.Should().HaveCount(13);
    }

    [Fact]
    public async Task ApplyToContextAsync_ShouldNotSeedTwice()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        await InventorySeedData.ApplyToContextAsync(dbContext);
        var initialCategoryCount = dbContext.Categories.Count();

        // Act
        await InventorySeedData.ApplyToContextAsync(dbContext);

        // Assert - The count should remain the same, not double
        dbContext.Categories.Should().HaveCount(initialCategoryCount);
    }

    [Fact]
    public async Task ApplyToContextAsync_ShouldSeedCategoriesFirst()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();

        // Act
        await InventorySeedData.ApplyToContextAsync(dbContext);

        // Assert
        var categories = await dbContext.Categories.ToListAsync();
        categories.Should().HaveCount(4);
        categories.Should().Contain(c => c.Name == "Electronics");
    }

    [Fact]
    public async Task ApplyToContextAsync_ShouldSeedProductsWithValidCategories()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();

        // Act
        await InventorySeedData.ApplyToContextAsync(dbContext);

        // Assert
        var products = await dbContext.Products.ToListAsync();
        var categories = await dbContext.Categories.Select(c => c.CategoryID).ToListAsync();
        
        products.Should().AllSatisfy(p => categories.Should().Contain(p.CategoryID));
    }

    [Fact]
    public async Task ApplyToContextAsync_ShouldSeedInventoryWithValidProducts()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();

        // Act
        await InventorySeedData.ApplyToContextAsync(dbContext);

        // Assert
        var inventory = await dbContext.Inventory.ToListAsync();
        var productIds = await dbContext.Products.Select(p => p.ProductID).ToListAsync();
        
        inventory.Should().AllSatisfy(i => productIds.Should().Contain(i.ProductID));
    }

    #endregion
}
