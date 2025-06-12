using copilot_sample.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace copilot_sample.DataAccess.SeedData
{
    /// <summary>
    /// Contains all seed data definitions for the inventory system.
    /// This class centralizes seed data management and can be used by both migrations and runtime seeding.
    /// </summary>
    public static class InventorySeedData
    {
        /// <summary>
        /// Applies all seed data to the given ModelBuilder.
        /// Used during migration generation via AppDbContext.OnModelCreating.
        /// </summary>
        /// <param name="modelBuilder">The ModelBuilder to configure</param>
        public static void ApplyToModelBuilder(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(GetCategories());
            
            // Seed Products
            modelBuilder.Entity<Product>().HasData(GetProducts());
            
            // Seed Product Prices
            modelBuilder.Entity<ProductPrice>().HasData(GetProductPrices());
            
            // Seed Inventory
            modelBuilder.Entity<Inventory>().HasData(GetInventoryItems());
            
            // Seed Product Attributes
            modelBuilder.Entity<ProductAttribute>().HasData(GetProductAttributes());
            
            // Seed Product Reviews
            modelBuilder.Entity<ProductReview>().HasData(GetProductReviews());
        }

        /// <summary>
        /// Applies seed data directly to a database context.
        /// Used for runtime seeding when migrations are not available.
        /// </summary>
        /// <param name="context">The database context</param>
        public static async Task ApplyToContextAsync(AppDbContext context)
        {
            // Check if data already exists
            if (await context.Categories.AnyAsync())
            {
                return; // Data already exists, skip seeding
            }

            // Add seed data
            await context.Categories.AddRangeAsync(GetCategories());
            await context.SaveChangesAsync();

            await context.Products.AddRangeAsync(GetProducts());
            await context.SaveChangesAsync();

            await context.ProductPrices.AddRangeAsync(GetProductPrices());
            await context.SaveChangesAsync();

            await context.Inventory.AddRangeAsync(GetInventoryItems());
            await context.SaveChangesAsync();

            await context.ProductAttributes.AddRangeAsync(GetProductAttributes());
            await context.SaveChangesAsync();

            await context.ProductReviews.AddRangeAsync(GetProductReviews());
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the seed data for categories
        /// </summary>
        public static Category[] GetCategories()
        {
            return new[]
            {
                new Category { CategoryID = 1, Name = "Electronics", Description = "Electronic gadgets and devices", ParentCategoryID = null },
                new Category { CategoryID = 2, Name = "Laptops", Description = "Portable computers", ParentCategoryID = 1 },
                new Category { CategoryID = 3, Name = "Smartphones", Description = "Mobile phones and accessories", ParentCategoryID = 1 },
                new Category { CategoryID = 4, Name = "Accessories", Description = "Electronics accessories", ParentCategoryID = 1 }
            };
        }

        /// <summary>
        /// Gets the seed data for products
        /// </summary>
        public static Product[] GetProducts()
        {
            return new[]
            {
                new Product 
                { 
                    ProductID = 1, 
                    Name = "UltraBook X1", 
                    Description = "Lightweight business laptop with high performance", 
                    SKU = "SKU-UBX1", 
                    CategoryID = 2, 
                    Brand = "TechBrand",
                    CreatedAt = new DateTime(2024, 1, 1),
                    UpdatedAt = new DateTime(2024, 1, 1),
                    IsActive = true
                },
                new Product 
                { 
                    ProductID = 2, 
                    Name = "Gaming Beast Z9", 
                    Description = "High-end gaming laptop with RGB lighting", 
                    SKU = "SKU-GBZ9", 
                    CategoryID = 2, 
                    Brand = "GamePro",
                    CreatedAt = new DateTime(2024, 1, 15),
                    UpdatedAt = new DateTime(2024, 1, 15),
                    IsActive = true
                },
                new Product 
                { 
                    ProductID = 3, 
                    Name = "Galaxy X10", 
                    Description = "Latest smartphone with AI-powered camera", 
                    SKU = "SKU-GX10", 
                    CategoryID = 3, 
                    Brand = "SmartTech",
                    CreatedAt = new DateTime(2024, 3, 15),
                    UpdatedAt = new DateTime(2024, 3, 15),
                    IsActive = true
                },
                new Product 
                { 
                    ProductID = 4, 
                    Name = "EarPods Pro", 
                    Description = "Wireless earphones with noise cancellation", 
                    SKU = "SKU-EPP", 
                    CategoryID = 4, 
                    Brand = "SoundWave",
                    CreatedAt = new DateTime(2024, 2, 1),
                    UpdatedAt = new DateTime(2024, 2, 1),
                    IsActive = true
                },
                new Product 
                { 
                    ProductID = 5, 
                    Name = "ThinkMate Pro 14", 
                    Description = "Durable business laptop with excellent battery life", 
                    SKU = "SKU-TMP14", 
                    CategoryID = 2, 
                    Brand = "ThinkCorp",
                    CreatedAt = new DateTime(2024, 4, 1),
                    UpdatedAt = new DateTime(2024, 4, 1),
                    IsActive = true
                },
                new Product 
                { 
                    ProductID = 6, 
                    Name = "PixelCam A2", 
                    Description = "Compact smartphone with high-resolution camera", 
                    SKU = "SKU-PXA2", 
                    CategoryID = 3, 
                    Brand = "PixelTech",
                    CreatedAt = new DateTime(2024, 4, 15),
                    UpdatedAt = new DateTime(2024, 4, 15),
                    IsActive = true
                },
                new Product 
                { 
                    ProductID = 7, 
                    Name = "PowerCharge 10000", 
                    Description = "Portable power bank with fast charging support", 
                    SKU = "SKU-PC10000", 
                    CategoryID = 4, 
                    Brand = "ChargeX",
                    CreatedAt = new DateTime(2024, 3, 1),
                    UpdatedAt = new DateTime(2024, 3, 1),
                    IsActive = true
                }
            };
        }

        /// <summary>
        /// Gets the seed data for product prices
        /// </summary>
        public static ProductPrice[] GetProductPrices()
        {
            return new[]
            {
                new ProductPrice { PriceID = 1, ProductID = 1, Price = 1299.99m, CurrencyCode = "USD", EffectiveFrom = new DateTime(2025, 1, 1), EffectiveTill = null },
                new ProductPrice { PriceID = 2, ProductID = 1, Price = 1199.99m, CurrencyCode = "USD", EffectiveFrom = new DateTime(2024, 10, 1), EffectiveTill = new DateTime(2024, 12, 31) },
                new ProductPrice { PriceID = 3, ProductID = 2, Price = 1999.00m, CurrencyCode = "USD", EffectiveFrom = new DateTime(2025, 1, 15), EffectiveTill = null },
                new ProductPrice { PriceID = 4, ProductID = 3, Price = 899.50m, CurrencyCode = "USD", EffectiveFrom = new DateTime(2025, 3, 15), EffectiveTill = null },
                new ProductPrice { PriceID = 5, ProductID = 4, Price = 199.99m, CurrencyCode = "USD", EffectiveFrom = new DateTime(2025, 2, 1), EffectiveTill = null },
                new ProductPrice { PriceID = 6, ProductID = 5, Price = 999.99m, CurrencyCode = "USD", EffectiveFrom = new DateTime(2025, 4, 1), EffectiveTill = null },
                new ProductPrice { PriceID = 7, ProductID = 6, Price = 649.00m, CurrencyCode = "USD", EffectiveFrom = new DateTime(2025, 4, 15), EffectiveTill = null },
                new ProductPrice { PriceID = 8, ProductID = 7, Price = 49.99m, CurrencyCode = "USD", EffectiveFrom = new DateTime(2025, 3, 1), EffectiveTill = null }
            };
        }

        /// <summary>
        /// Gets the seed data for inventory items
        /// </summary>
        public static Inventory[] GetInventoryItems()
        {
            return new[]
            {
                new Inventory { InventoryID = 1, ProductID = 1, Quantity = 25, LastUpdated = new DateTime(2024, 1, 1) },
                new Inventory { InventoryID = 2, ProductID = 2, Quantity = 10, LastUpdated = new DateTime(2024, 1, 15) },
                new Inventory { InventoryID = 3, ProductID = 3, Quantity = 50, LastUpdated = new DateTime(2024, 3, 15) },
                new Inventory { InventoryID = 4, ProductID = 4, Quantity = 100, LastUpdated = new DateTime(2024, 2, 1) },
                new Inventory { InventoryID = 5, ProductID = 5, Quantity = 30, LastUpdated = new DateTime(2024, 4, 1) },
                new Inventory { InventoryID = 6, ProductID = 6, Quantity = 45, LastUpdated = new DateTime(2024, 4, 15) },
                new Inventory { InventoryID = 7, ProductID = 7, Quantity = 200, LastUpdated = new DateTime(2024, 3, 1) }
            };
        }

        /// <summary>
        /// Gets the seed data for product attributes
        /// </summary>
        public static ProductAttribute[] GetProductAttributes()
        {
            return new[]
            {
                // UltraBook X1 attributes
                new ProductAttribute { AttributeID = 1, ProductID = 1, AttributeName = "Processor", AttributeValue = "Intel Core i7" },
                new ProductAttribute { AttributeID = 2, ProductID = 1, AttributeName = "RAM", AttributeValue = "16GB" },
                new ProductAttribute { AttributeID = 3, ProductID = 1, AttributeName = "Storage", AttributeValue = "512GB SSD" },
                // Gaming Beast Z9 attributes
                new ProductAttribute { AttributeID = 4, ProductID = 2, AttributeName = "Processor", AttributeValue = "AMD Ryzen 9" },
                new ProductAttribute { AttributeID = 5, ProductID = 2, AttributeName = "RAM", AttributeValue = "32GB" },
                new ProductAttribute { AttributeID = 6, ProductID = 2, AttributeName = "Graphics Card", AttributeValue = "NVIDIA RTX 4080" },
                // Galaxy X10 attributes
                new ProductAttribute { AttributeID = 7, ProductID = 3, AttributeName = "Display", AttributeValue = "6.5-inch OLED" },
                new ProductAttribute { AttributeID = 8, ProductID = 3, AttributeName = "Battery", AttributeValue = "4000mAh" },
                new ProductAttribute { AttributeID = 9, ProductID = 3, AttributeName = "Camera", AttributeValue = "108MP" },
                // EarPods Pro attributes
                new ProductAttribute { AttributeID = 10, ProductID = 4, AttributeName = "Connectivity", AttributeValue = "Bluetooth 5.2" },
                new ProductAttribute { AttributeID = 11, ProductID = 4, AttributeName = "Noise Cancellation", AttributeValue = "Active" },
                new ProductAttribute { AttributeID = 12, ProductID = 4, AttributeName = "Battery Life", AttributeValue = "8 hours" },
                // ThinkMate Pro 14 attributes
                new ProductAttribute { AttributeID = 13, ProductID = 5, AttributeName = "Processor", AttributeValue = "Intel i5" },
                new ProductAttribute { AttributeID = 14, ProductID = 5, AttributeName = "RAM", AttributeValue = "8GB" },
                new ProductAttribute { AttributeID = 15, ProductID = 5, AttributeName = "Weight", AttributeValue = "1.3kg" },
                // PixelCam A2 attributes
                new ProductAttribute { AttributeID = 16, ProductID = 6, AttributeName = "Camera", AttributeValue = "64MP" },
                new ProductAttribute { AttributeID = 17, ProductID = 6, AttributeName = "Storage", AttributeValue = "128GB" },
                new ProductAttribute { AttributeID = 18, ProductID = 6, AttributeName = "Display", AttributeValue = "6.1-inch AMOLED" },
                // PowerCharge 10000 attributes
                new ProductAttribute { AttributeID = 19, ProductID = 7, AttributeName = "Capacity", AttributeValue = "10000mAh" },
                new ProductAttribute { AttributeID = 20, ProductID = 7, AttributeName = "USB Ports", AttributeValue = "2" },
                new ProductAttribute { AttributeID = 21, ProductID = 7, AttributeName = "Fast Charging", AttributeValue = "Yes" }
            };
        }

        /// <summary>
        /// Gets the seed data for product reviews
        /// </summary>
        public static ProductReview[] GetProductReviews()
        {
            return new[]
            {
                // UltraBook X1 reviews
                new ProductReview { ReviewID = 1, ProductID = 1, ReviewerName = "Alice Johnson", Rating = 5, Comment = "Absolutely love this laptop! Fast and sleek.", ReviewDate = new DateTime(2024, 1, 15) },
                new ProductReview { ReviewID = 2, ProductID = 1, ReviewerName = "Bob Smith", Rating = 4, Comment = "Good performance but gets a bit warm.", ReviewDate = new DateTime(2024, 1, 20) },
                // Gaming Beast Z9 reviews
                new ProductReview { ReviewID = 3, ProductID = 2, ReviewerName = "Tommy Lee", Rating = 5, Comment = "A beast for gaming. Smooth experience!", ReviewDate = new DateTime(2024, 2, 1) },
                // Galaxy X10 reviews
                new ProductReview { ReviewID = 4, ProductID = 3, ReviewerName = "Carlos Vega", Rating = 5, Comment = "Amazing camera and display! Worth every penny.", ReviewDate = new DateTime(2024, 3, 20) },
                new ProductReview { ReviewID = 5, ProductID = 3, ReviewerName = "Diana Lee", Rating = 3, Comment = "Battery life could be better.", ReviewDate = new DateTime(2024, 3, 25) },
                // EarPods Pro reviews
                new ProductReview { ReviewID = 6, ProductID = 4, ReviewerName = "Emma Stone", Rating = 4, Comment = "Very comfortable fit and great sound.", ReviewDate = new DateTime(2024, 2, 10) },
                new ProductReview { ReviewID = 7, ProductID = 4, ReviewerName = "John Doe", Rating = 2, Comment = "Connection drops sometimes.", ReviewDate = new DateTime(2024, 2, 15) },
                // ThinkMate Pro 14 reviews
                new ProductReview { ReviewID = 8, ProductID = 5, ReviewerName = "Harvey Dent", Rating = 4, Comment = "Reliable laptop, solid build and decent performance.", ReviewDate = new DateTime(2024, 4, 10) },
                new ProductReview { ReviewID = 9, ProductID = 5, ReviewerName = "Rachel Green", Rating = 3, Comment = "Good for light work, but a bit slow for multitasking.", ReviewDate = new DateTime(2024, 4, 12) },
                // PixelCam A2 reviews
                new ProductReview { ReviewID = 10, ProductID = 6, ReviewerName = "Bruce Banner", Rating = 5, Comment = "Compact yet powerful. Great value for the price.", ReviewDate = new DateTime(2024, 4, 20) },
                new ProductReview { ReviewID = 11, ProductID = 6, ReviewerName = "Natasha Romanoff", Rating = 4, Comment = "Excellent camera quality and battery.", ReviewDate = new DateTime(2024, 4, 22) },
                // PowerCharge 10000 reviews
                new ProductReview { ReviewID = 12, ProductID = 7, ReviewerName = "Steve Rogers", Rating = 5, Comment = "Lasts all day. Perfect for travel.", ReviewDate = new DateTime(2024, 3, 10) },
                new ProductReview { ReviewID = 13, ProductID = 7, ReviewerName = "Tony Stark", Rating = 3, Comment = "Charges fast, but gets warm during use.", ReviewDate = new DateTime(2024, 3, 15) }
            };
        }

        /// <summary>
        /// Gets summary information about the seed data
        /// </summary>
        public static (int Categories, int Products, int Prices, int Inventory, int Attributes, int Reviews) GetSeedDataCounts()
        {
            return (
                Categories: GetCategories().Length,
                Products: GetProducts().Length,
                Prices: GetProductPrices().Length,
                Inventory: GetInventoryItems().Length,
                Attributes: GetProductAttributes().Length,
                Reviews: GetProductReviews().Length
            );
        }
    }
}
