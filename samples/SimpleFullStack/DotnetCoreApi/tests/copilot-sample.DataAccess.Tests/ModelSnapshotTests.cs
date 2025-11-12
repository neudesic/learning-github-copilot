using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

public class ModelSnapshotTests
{
    private AppDbContext GetInMemoryDbContext(bool createDatabase = false)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        if (createDatabase)
        {
            context.Database.EnsureCreated();
        }
        return context;
    }

    #region Category Configuration Tests

    [Fact]
    public void ModelSnapshot_Category_HasCorrectTableName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));

        // Act & Assert
        entityType?.GetTableName().Should().Be("Categories");
    }

    [Fact]
    public void ModelSnapshot_Category_HasPrimaryKeyOnCategoryID()
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
    public void ModelSnapshot_Category_CategoryIDIsValueGeneratedOnAdd()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var categoryIdProperty = entityType?.FindProperty("CategoryID");

        // Act & Assert
        categoryIdProperty?.ValueGenerated.Should().Be(ValueGenerated.OnAdd);
    }

    [Fact]
    public void ModelSnapshot_Category_NameIsRequired_WithMaxLength100()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var nameProperty = entityType?.FindProperty("Name");

        // Act & Assert
        nameProperty?.IsNullable.Should().BeFalse();
        nameProperty?.GetMaxLength().Should().Be(100);
    }

    [Fact]
    public void ModelSnapshot_Category_DescriptionIsOptional_WithMaxLength500()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var descriptionProperty = entityType?.FindProperty("Description");

        // Act & Assert
        descriptionProperty?.IsNullable.Should().BeTrue();
        descriptionProperty?.GetMaxLength().Should().Be(500);
    }

    [Fact]
    public void ModelSnapshot_Category_ParentCategoryIDIsOptional()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var parentCategoryIdProperty = entityType?.FindProperty("ParentCategoryID");

        // Act & Assert
        parentCategoryIdProperty?.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ModelSnapshot_Category_ParentCategoryForeignKeyConfigured()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var parentCategoryFk = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "ParentCategoryID");
        parentCategoryFk.Should().NotBeNull();
    }

    [Fact]
    public void ModelSnapshot_Category_HasSelfReferencingRelationship()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var parentCategoryFk = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "ParentCategoryID");
        parentCategoryFk.Should().NotBeNull();
        parentCategoryFk?.DeleteBehavior.Should().Be(DeleteBehavior.Restrict);
    }

    [Fact]
    public void ModelSnapshot_Category_HasProductsNavigation()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var productsNavigation = entityType?.FindNavigation("Products");

        // Act & Assert
        productsNavigation.Should().NotBeNull();
        productsNavigation?.TargetEntityType.Name.Should().Contain("Product");
    }

    [Fact]
    public void ModelSnapshot_Category_HasSubCategoriesNavigation()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Category));
        var subCategoriesNavigation = entityType?.FindNavigation("SubCategories");

        // Act & Assert
        subCategoriesNavigation.Should().NotBeNull();
        subCategoriesNavigation?.TargetEntityType.Name.Should().Contain("Category");
    }

    #endregion

    #region Product Configuration Tests

    [Fact]
    public void ModelSnapshot_Product_HasCorrectTableName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));

        // Act & Assert
        entityType?.GetTableName().Should().Be("Products");
    }

    [Fact]
    public void ModelSnapshot_Product_HasPrimaryKeyOnProductID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var primaryKey = entityType?.FindPrimaryKey();

        // Act & Assert
        primaryKey.Should().NotBeNull();
        primaryKey?.Properties.Should().HaveCount(1);
        primaryKey?.Properties[0].Name.Should().Be("ProductID");
    }

    [Fact]
    public void ModelSnapshot_Product_SKUIsUnique()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var indexes = entityType?.GetIndexes();

        // Act & Assert
        indexes.Should().NotBeEmpty();
        var skuIndex = indexes?.FirstOrDefault(i => i.Properties[0].Name == "SKU");
        skuIndex.Should().NotBeNull();
        skuIndex?.IsUnique.Should().BeTrue();
    }

    [Fact]
    public void ModelSnapshot_Product_HasCategoryIDForeignKeyConfigured()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var categoryFk = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "CategoryID");
        categoryFk.Should().NotBeNull();
    }

    [Fact]
    public void ModelSnapshot_Product_SKUIsRequired_WithMaxLength100()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var skuProperty = entityType?.FindProperty("SKU");

        // Act & Assert
        skuProperty?.IsNullable.Should().BeFalse();
        skuProperty?.GetMaxLength().Should().Be(100);
    }

    [Fact]
    public void ModelSnapshot_Product_NameIsRequired_WithMaxLength200()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var nameProperty = entityType?.FindProperty("Name");

        // Act & Assert
        nameProperty?.IsNullable.Should().BeFalse();
        nameProperty?.GetMaxLength().Should().Be(200);
    }

    [Fact]
    public void ModelSnapshot_Product_DescriptionIsOptional()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var descriptionProperty = entityType?.FindProperty("Description");

        // Act & Assert
        descriptionProperty?.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ModelSnapshot_Product_BrandIsOptional_WithMaxLength100()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var brandProperty = entityType?.FindProperty("Brand");

        // Act & Assert
        brandProperty?.IsNullable.Should().BeTrue();
        brandProperty?.GetMaxLength().Should().Be(100);
    }

    [Fact]
    public void ModelSnapshot_Product_CategoryIDIsRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var categoryIdProperty = entityType?.FindProperty("CategoryID");

        // Act & Assert
        categoryIdProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void ModelSnapshot_Product_IsActiveHasDefaultValueTrue()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var isActiveProperty = entityType?.FindProperty("IsActive");

        // Act & Assert
        isActiveProperty?.GetDefaultValue().Should().Be(true);
    }

    [Fact]
    public void ModelSnapshot_Product_CreatedAtHasDefaultValueSql()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var createdAtProperty = entityType?.FindProperty("CreatedAt");

        // Act & Assert
        createdAtProperty?.GetDefaultValueSql().Should().Be("datetime('now')");
    }

    [Fact]
    public void ModelSnapshot_Product_UpdatedAtHasDefaultValueSql()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var updatedAtProperty = entityType?.FindProperty("UpdatedAt");

        // Act & Assert
        updatedAtProperty?.GetDefaultValueSql().Should().Be("datetime('now')");
    }

    [Fact]
    public void ModelSnapshot_Product_HasCategoryForeignKey()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var categoryFk = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "CategoryID");
        categoryFk.Should().NotBeNull();
        categoryFk?.DeleteBehavior.Should().Be(DeleteBehavior.Cascade);
    }

    [Fact]
    public void ModelSnapshot_Product_HasCategoryNavigation()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var categoryNavigation = entityType?.FindNavigation("Category");

        // Act & Assert
        categoryNavigation.Should().NotBeNull();
        categoryNavigation?.TargetEntityType.Name.Should().Contain("Category");
    }

    [Fact]
    public void ModelSnapshot_Product_HasInventoryNavigation()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var inventoryNavigation = entityType?.FindNavigation("Inventory");

        // Act & Assert
        inventoryNavigation.Should().NotBeNull();
    }

    [Fact]
    public void ModelSnapshot_Product_HasProductAttributesNavigation()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var attributesNavigation = entityType?.FindNavigation("ProductAttributes");

        // Act & Assert
        attributesNavigation.Should().NotBeNull();
        attributesNavigation?.TargetEntityType.Name.Should().Contain("ProductAttribute");
    }

    [Fact]
    public void ModelSnapshot_Product_HasProductPricesNavigation()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var pricesNavigation = entityType?.FindNavigation("ProductPrices");

        // Act & Assert
        pricesNavigation.Should().NotBeNull();
        pricesNavigation?.TargetEntityType.Name.Should().Contain("ProductPrice");
    }

    [Fact]
    public void ModelSnapshot_Product_HasProductReviewsNavigation()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Product));
        var reviewsNavigation = entityType?.FindNavigation("ProductReviews");

        // Act & Assert
        reviewsNavigation.Should().NotBeNull();
        reviewsNavigation?.TargetEntityType.Name.Should().Contain("ProductReview");
    }

    #endregion

    #region Inventory Configuration Tests

    [Fact]
    public void ModelSnapshot_Inventory_HasCorrectTableName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));

        // Act & Assert
        entityType?.GetTableName().Should().Be("Inventory");
    }

    [Fact]
    public void ModelSnapshot_Inventory_HasPrimaryKeyOnInventoryID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var primaryKey = entityType?.FindPrimaryKey();

        // Act & Assert
        primaryKey.Should().NotBeNull();
        primaryKey?.Properties.Should().HaveCount(1);
        primaryKey?.Properties[0].Name.Should().Be("InventoryID");
    }

    [Fact]
    public void ModelSnapshot_Inventory_ProductIDHasUniqueIndexConfiguration()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));

        // Act & Assert
        entityType.Should().NotBeNull();
        var primaryKey = entityType?.FindPrimaryKey();
        primaryKey.Should().NotBeNull();
    }

    [Fact]
    public void ModelSnapshot_Inventory_QuantityIsRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var quantityProperty = entityType?.FindProperty("Quantity");

        // Act & Assert
        quantityProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void ModelSnapshot_Inventory_LastUpdatedHasDefaultValueSql()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var lastUpdatedProperty = entityType?.FindProperty("LastUpdated");

        // Act & Assert
        lastUpdatedProperty?.GetDefaultValueSql().Should().Be("datetime('now')");
    }

    [Fact]
    public void ModelSnapshot_Inventory_HasProductForeignKey()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var productFk = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "ProductID");
        productFk.Should().NotBeNull();
        productFk?.DeleteBehavior.Should().Be(DeleteBehavior.Cascade);
    }

    [Fact]
    public void ModelSnapshot_Inventory_HasProductNavigation()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var productNavigation = entityType?.FindNavigation("Product");

        // Act & Assert
        productNavigation.Should().NotBeNull();
        productNavigation?.TargetEntityType.Name.Should().Contain("Product");
    }

    [Fact]
    public void ModelSnapshot_Inventory_OneToOneWithProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(Inventory));
        var productNavigation = entityType?.FindNavigation("Product");

        // Act & Assert
        productNavigation?.IsCollection.Should().BeFalse();
    }

    #endregion

    #region ProductAttribute Configuration Tests

    [Fact]
    public void ModelSnapshot_ProductAttribute_HasCorrectTableName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));

        // Act & Assert
        entityType?.GetTableName().Should().Be("ProductAttributes");
    }

    [Fact]
    public void ModelSnapshot_ProductAttribute_HasPrimaryKeyOnAttributeID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var primaryKey = entityType?.FindPrimaryKey();

        // Act & Assert
        primaryKey.Should().NotBeNull();
        primaryKey?.Properties.Should().HaveCount(1);
        primaryKey?.Properties[0].Name.Should().Be("AttributeID");
    }

    [Fact]
    public void ModelSnapshot_ProductAttribute_AttributeNameIsRequired_WithMaxLength100()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var attributeNameProperty = entityType?.FindProperty("AttributeName");

        // Act & Assert
        attributeNameProperty?.IsNullable.Should().BeFalse();
        attributeNameProperty?.GetMaxLength().Should().Be(100);
    }

    [Fact]
    public void ModelSnapshot_ProductAttribute_AttributeValueIsRequired_WithMaxLength255()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var attributeValueProperty = entityType?.FindProperty("AttributeValue");

        // Act & Assert
        attributeValueProperty?.IsNullable.Should().BeFalse();
        attributeValueProperty?.GetMaxLength().Should().Be(255);
    }

    [Fact]
    public void ModelSnapshot_ProductAttribute_ProductIDIsRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var productIdProperty = entityType?.FindProperty("ProductID");

        // Act & Assert
        productIdProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void ModelSnapshot_ProductAttribute_HasProductForeignKey()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var productFk = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "ProductID");
        productFk.Should().NotBeNull();
        productFk?.DeleteBehavior.Should().Be(DeleteBehavior.Cascade);
    }

    [Fact]
    public void ModelSnapshot_ProductAttribute_HasProductNavigation()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var productNavigation = entityType?.FindNavigation("Product");

        // Act & Assert
        productNavigation.Should().NotBeNull();
        productNavigation?.TargetEntityType.Name.Should().Contain("Product");
    }

    [Fact]
    public void ModelSnapshot_ProductAttribute_HasProductIDForeignKeyConfigured()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductAttribute));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var productFk = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "ProductID");
        productFk.Should().NotBeNull();
    }

    #endregion

    #region ProductPrice Configuration Tests

    [Fact]
    public void ModelSnapshot_ProductPrice_HasCorrectTableName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));

        // Act & Assert
        entityType?.GetTableName().Should().Be("ProductPrices");
    }

    [Fact]
    public void ModelSnapshot_ProductPrice_HasPrimaryKeyOnPriceID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var primaryKey = entityType?.FindPrimaryKey();

        // Act & Assert
        primaryKey.Should().NotBeNull();
        primaryKey?.Properties.Should().HaveCount(1);
        primaryKey?.Properties[0].Name.Should().Be("PriceID");
    }

    [Fact]
    public void ModelSnapshot_ProductPrice_PriceHasPrecision18Scale2()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var priceProperty = entityType?.FindProperty("Price");

        // Act & Assert
        priceProperty?.GetPrecision().Should().Be(18);
        priceProperty?.GetScale().Should().Be(2);
    }

    [Fact]
    public void ModelSnapshot_ProductPrice_CurrencyCodeIsRequired_WithDefaultValue()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var currencyCodeProperty = entityType?.FindProperty("CurrencyCode");

        // Act & Assert
        currencyCodeProperty?.IsNullable.Should().BeFalse();
        currencyCodeProperty?.GetMaxLength().Should().Be(3);
        currencyCodeProperty?.GetDefaultValue().Should().Be("USD");
    }

    [Fact]
    public void ModelSnapshot_ProductPrice_EffectiveFromHasDefaultValueSql()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var effectiveFromProperty = entityType?.FindProperty("EffectiveFrom");

        // Act & Assert
        effectiveFromProperty?.GetDefaultValueSql().Should().Be("datetime('now')");
    }

    [Fact]
    public void ModelSnapshot_ProductPrice_EffectiveTillIsOptional()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var effectiveTillProperty = entityType?.FindProperty("EffectiveTill");

        // Act & Assert
        effectiveTillProperty?.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ModelSnapshot_ProductPrice_ProductIDIsRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var productIdProperty = entityType?.FindProperty("ProductID");

        // Act & Assert
        productIdProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void ModelSnapshot_ProductPrice_HasProductForeignKey()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var productFk = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "ProductID");
        productFk.Should().NotBeNull();
        productFk?.DeleteBehavior.Should().Be(DeleteBehavior.Cascade);
    }

    [Fact]
    public void ModelSnapshot_ProductPrice_HasProductNavigation()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var productNavigation = entityType?.FindNavigation("Product");

        // Act & Assert
        productNavigation.Should().NotBeNull();
        productNavigation?.TargetEntityType.Name.Should().Contain("Product");
    }

    [Fact]
    public void ModelSnapshot_ProductPrice_HasProductIDForeignKeyConfigured()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductPrice));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var productFk = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "ProductID");
        productFk.Should().NotBeNull();
    }

    #endregion

    #region ProductReview Configuration Tests

    [Fact]
    public void ModelSnapshot_ProductReview_HasCorrectTableName()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductReview));

        // Act & Assert
        entityType?.GetTableName().Should().Be("ProductReviews");
    }

    [Fact]
    public void ModelSnapshot_ProductReview_HasPrimaryKeyOnReviewID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductReview));
        var primaryKey = entityType?.FindPrimaryKey();

        // Act & Assert
        primaryKey.Should().NotBeNull();
        primaryKey?.Properties.Should().HaveCount(1);
        primaryKey?.Properties[0].Name.Should().Be("ReviewID");
    }

    [Fact]
    public void ModelSnapshot_ProductReview_RatingIsRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductReview));
        var ratingProperty = entityType?.FindProperty("Rating");

        // Act & Assert
        ratingProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void ModelSnapshot_ProductReview_CommentIsOptional()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductReview));
        var commentProperty = entityType?.FindProperty("Comment");

        // Act & Assert
        commentProperty?.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ModelSnapshot_ProductReview_ReviewerNameIsOptional_WithMaxLength100()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductReview));
        var reviewerNameProperty = entityType?.FindProperty("ReviewerName");

        // Act & Assert
        reviewerNameProperty?.IsNullable.Should().BeTrue();
        reviewerNameProperty?.GetMaxLength().Should().Be(100);
    }

    [Fact]
    public void ModelSnapshot_ProductReview_ReviewDateHasDefaultValueSql()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductReview));
        var reviewDateProperty = entityType?.FindProperty("ReviewDate");

        // Act & Assert
        reviewDateProperty?.GetDefaultValueSql().Should().Be("datetime('now')");
    }

    [Fact]
    public void ModelSnapshot_ProductReview_ProductIDIsRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductReview));
        var productIdProperty = entityType?.FindProperty("ProductID");

        // Act & Assert
        productIdProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void ModelSnapshot_ProductReview_HasProductForeignKey()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductReview));
        var foreignKeys = entityType?.GetForeignKeys();

        // Act & Assert
        foreignKeys.Should().NotBeEmpty();
        var productFk = foreignKeys?.FirstOrDefault(fk => fk.Properties[0].Name == "ProductID");
        productFk.Should().NotBeNull();
        productFk?.DeleteBehavior.Should().Be(DeleteBehavior.Cascade);
    }

    [Fact]
    public void ModelSnapshot_ProductReview_HasProductNavigation()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductReview));
        var productNavigation = entityType?.FindNavigation("Product");

        // Act & Assert
        productNavigation.Should().NotBeNull();
        productNavigation?.TargetEntityType.Name.Should().Contain("Product");
    }

    [Fact]
    public void ModelSnapshot_ProductReview_RatingPropertyIsRequired()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductReview));
        var ratingProperty = entityType?.FindProperty("Rating");

        // Act & Assert
        ratingProperty.Should().NotBeNull();
        ratingProperty?.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void ModelSnapshot_ProductReview_HasIndexConfigurationOnProductID()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var entityType = dbContext.Model.FindEntityType(typeof(ProductReview));

        // Act & Assert
        entityType.Should().NotBeNull();
    }

    #endregion

    #region Seed Data Verification Tests

    [Fact]
    public async Task ModelSnapshot_SeedData_HasFourCategories()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var categories = await dbContext.Categories.ToListAsync();

        // Assert
        categories.Should().HaveCount(4);
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_CategoryWithIDOneIsElectronics()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var category = await dbContext.Categories.FindAsync(1);

        // Assert
        category.Should().NotBeNull();
        category?.Name.Should().Be("Electronics");
        category?.Description.Should().Be("Electronic gadgets and devices");
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_CategoryWithIDTwoIsLaptopsWithParent()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var category = await dbContext.Categories.FindAsync(2);

        // Assert
        category.Should().NotBeNull();
        category?.Name.Should().Be("Laptops");
        category?.ParentCategoryID.Should().Be(1);
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_HasSevenProducts()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var products = await dbContext.Products.ToListAsync();

        // Assert
        products.Should().HaveCount(7);
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_ProductWithIDOneHasCorrectDetails()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var product = await dbContext.Products.FindAsync(1);

        // Assert
        product.Should().NotBeNull();
        product?.Name.Should().Be("UltraBook X1");
        product?.SKU.Should().Be("SKU-UBX1");
        product?.Brand.Should().Be("TechBrand");
        product?.CategoryID.Should().Be(2);
        product?.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_AllProductsAreActive()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var products = await dbContext.Products.ToListAsync();

        // Assert
        products.Should().AllSatisfy(p => p.IsActive.Should().BeTrue());
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_AllProductsHaveUniqueSKUs()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var products = await dbContext.Products.ToListAsync();
        var skus = products.Select(p => p.SKU).ToList();

        // Assert
        skus.Should().HaveCount(skus.Distinct().Count());
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_HasSevenInventoryRecords()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var inventoryRecords = await dbContext.Inventory.ToListAsync();

        // Assert
        inventoryRecords.Should().HaveCount(7);
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_InventoryForProductOneHasCorrectQuantity()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var inventory = await dbContext.Inventory.FirstOrDefaultAsync(i => i.ProductID == 1);

        // Assert
        inventory.Should().NotBeNull();
        inventory?.Quantity.Should().Be(25);
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_HasTwentyOneProductAttributes()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var attributes = await dbContext.ProductAttributes.ToListAsync();

        // Assert
        attributes.Should().HaveCount(21);
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_ProductAttributesLinkedToProducts()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var attributes = await dbContext.ProductAttributes
            .Include(pa => pa.Product)
            .ToListAsync();

        // Assert
        attributes.Should().AllSatisfy(a => a.Product.Should().NotBeNull());
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_HasEightProductPrices()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var prices = await dbContext.ProductPrices.ToListAsync();

        // Assert
        prices.Should().HaveCount(8);
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_AllProductPricesHaveUSDCurrency()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var prices = await dbContext.ProductPrices.ToListAsync();

        // Assert
        prices.Should().AllSatisfy(p => p.CurrencyCode.Should().Be("USD"));
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_ProductPriceForProductOneIsCorrect()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var price = await dbContext.ProductPrices.FirstOrDefaultAsync(p => p.ProductID == 1 && p.PriceID == 1);

        // Assert
        price.Should().NotBeNull();
        price?.Price.Should().Be(1299.99m);
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_HasThirteenProductReviews()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var reviews = await dbContext.ProductReviews.ToListAsync();

        // Assert
        reviews.Should().HaveCount(13);
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_AllReviewRatingsAreBetweenOneAndFive()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var reviews = await dbContext.ProductReviews.ToListAsync();

        // Assert
        reviews.Should().AllSatisfy(r => 
        {
            r.Rating.Should().BeGreaterThanOrEqualTo(1);
            r.Rating.Should().BeLessThanOrEqualTo(5);
        });
    }

    [Fact]
    public async Task ModelSnapshot_SeedData_ReviewForProductOneIsCorrect()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var review = await dbContext.ProductReviews.FindAsync(1);

        // Assert
        review.Should().NotBeNull();
        review?.ProductID.Should().Be(1);
        review?.Rating.Should().Be(5);
        review?.ReviewerName.Should().Be("Alice Johnson");
    }

    #endregion

    #region Relationship Validation Tests

    [Fact]
    public async Task ModelSnapshot_Relationships_ProductBelongsToCategory()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var product = await dbContext.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductID == 1);

        // Assert
        product.Should().NotBeNull();
        product?.Category.Should().NotBeNull();
        product?.Category?.CategoryID.Should().Be(2);
    }

    [Fact]
    public async Task ModelSnapshot_Relationships_CategoryWithProductsNavigationWorks()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var category = await dbContext.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.CategoryID == 2);

        // Assert
        category.Should().NotBeNull();
        category?.Products.Should().NotBeNull();
    }

    [Fact]
    public async Task ModelSnapshot_Relationships_SubCategoriesNavigationWorks()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var category = await dbContext.Categories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.CategoryID == 1);

        // Assert
        category.Should().NotBeNull();
        category?.SubCategories.Should().NotBeNull();
    }

    [Fact]
    public async Task ModelSnapshot_Relationships_InventoryBelongsToProduct()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var inventory = await dbContext.Inventory
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.InventoryID == 1);

        // Assert
        inventory.Should().NotBeNull();
        inventory?.Product.Should().NotBeNull();
        inventory?.Product?.ProductID.Should().Be(1);
    }

    [Fact]
    public async Task ModelSnapshot_Relationships_ProductHasOneInventory()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var product = await dbContext.Products
            .Include(p => p.Inventory)
            .FirstOrDefaultAsync(p => p.ProductID == 1);

        // Assert
        product.Should().NotBeNull();
        product?.Inventory.Should().NotBeNull();
    }

    [Fact]
    public async Task ModelSnapshot_Relationships_ProductHasMultipleAttributes()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var product = await dbContext.Products
            .Include(p => p.ProductAttributes)
            .FirstOrDefaultAsync(p => p.ProductID == 1);

        // Assert
        product.Should().NotBeNull();
        product?.ProductAttributes.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ModelSnapshot_Relationships_ProductHasMultiplePrices()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var product = await dbContext.Products
            .Include(p => p.ProductPrices)
            .FirstOrDefaultAsync(p => p.ProductID == 1);

        // Assert
        product.Should().NotBeNull();
        product?.ProductPrices.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ModelSnapshot_Relationships_ProductHasMultipleReviews()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext(createDatabase: true);

        // Act
        var product = await dbContext.Products
            .Include(p => p.ProductReviews)
            .FirstOrDefaultAsync(p => p.ProductID == 1);

        // Assert
        product.Should().NotBeNull();
        product?.ProductReviews.Should().NotBeEmpty();
    }

    #endregion
}
