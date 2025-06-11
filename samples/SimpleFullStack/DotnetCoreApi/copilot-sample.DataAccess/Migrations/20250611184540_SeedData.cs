using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace copilotsample.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "Description", "Name", "ParentCategoryID" },
                values: new object[,]
                {
                    { 1, "Electronic gadgets and devices", "Electronics", null },
                    { 2, "Portable computers", "Laptops", 1 },
                    { 3, "Mobile phones and accessories", "Smartphones", 1 },
                    { 4, "Electronics accessories", "Accessories", 1 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "Brand", "CategoryID", "CreatedAt", "Description", "IsActive", "Name", "SKU", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "TechBrand", 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lightweight business laptop with high performance", true, "UltraBook X1", "SKU-UBX1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "GamePro", 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "High-end gaming laptop with RGB lighting", true, "Gaming Beast Z9", "SKU-GBZ9", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "SmartTech", 3, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest smartphone with AI-powered camera", true, "Galaxy X10", "SKU-GX10", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "SoundWave", 4, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wireless earphones with noise cancellation", true, "EarPods Pro", "SKU-EPP", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "ThinkCorp", 2, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Durable business laptop with excellent battery life", true, "ThinkMate Pro 14", "SKU-TMP14", new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "PixelTech", 3, new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compact smartphone with high-resolution camera", true, "PixelCam A2", "SKU-PXA2", new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "ChargeX", 4, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Portable power bank with fast charging support", true, "PowerCharge 10000", "SKU-PC10000", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "InventoryID", "LastUpdated", "ProductID", "Quantity" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 25 },
                    { 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 10 },
                    { 3, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 50 },
                    { 4, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 100 },
                    { 5, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 30 },
                    { 6, new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 45 },
                    { 7, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 200 }
                });

            migrationBuilder.InsertData(
                table: "ProductAttributes",
                columns: new[] { "AttributeID", "AttributeName", "AttributeValue", "ProductID" },
                values: new object[,]
                {
                    { 1, "Processor", "Intel Core i7", 1 },
                    { 2, "RAM", "16GB", 1 },
                    { 3, "Storage", "512GB SSD", 1 },
                    { 4, "Processor", "AMD Ryzen 9", 2 },
                    { 5, "RAM", "32GB", 2 },
                    { 6, "Graphics Card", "NVIDIA RTX 4080", 2 },
                    { 7, "Display", "6.5-inch OLED", 3 },
                    { 8, "Battery", "4000mAh", 3 },
                    { 9, "Camera", "108MP", 3 },
                    { 10, "Connectivity", "Bluetooth 5.2", 4 },
                    { 11, "Noise Cancellation", "Active", 4 },
                    { 12, "Battery Life", "8 hours", 4 },
                    { 13, "Processor", "Intel i5", 5 },
                    { 14, "RAM", "8GB", 5 },
                    { 15, "Weight", "1.3kg", 5 },
                    { 16, "Camera", "64MP", 6 },
                    { 17, "Storage", "128GB", 6 },
                    { 18, "Display", "6.1-inch AMOLED", 6 },
                    { 19, "Capacity", "10000mAh", 7 },
                    { 20, "USB Ports", "2", 7 },
                    { 21, "Fast Charging", "Yes", 7 }
                });

            migrationBuilder.InsertData(
                table: "ProductPrices",
                columns: new[] { "PriceID", "CurrencyCode", "EffectiveFrom", "EffectiveTill", "Price", "ProductID" },
                values: new object[,]
                {
                    { 1, "USD", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1299.99m, 1 },
                    { 2, "USD", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1199.99m, 1 },
                    { 3, "USD", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1999.00m, 2 },
                    { 4, "USD", new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 899.50m, 3 },
                    { 5, "USD", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 199.99m, 4 },
                    { 6, "USD", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 999.99m, 5 },
                    { 7, "USD", new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 649.00m, 6 },
                    { 8, "USD", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 49.99m, 7 }
                });

            migrationBuilder.InsertData(
                table: "ProductReviews",
                columns: new[] { "ReviewID", "Comment", "ProductID", "Rating", "ReviewDate", "ReviewerName" },
                values: new object[,]
                {
                    { 1, "Absolutely love this laptop! Fast and sleek.", 1, 5, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alice Johnson" },
                    { 2, "Good performance but gets a bit warm.", 1, 4, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bob Smith" },
                    { 3, "A beast for gaming. Smooth experience!", 2, 5, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tommy Lee" },
                    { 4, "Amazing camera and display! Worth every penny.", 3, 5, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Carlos Vega" },
                    { 5, "Battery life could be better.", 3, 3, new DateTime(2024, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diana Lee" },
                    { 6, "Very comfortable fit and great sound.", 4, 4, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Emma Stone" },
                    { 7, "Connection drops sometimes.", 4, 2, new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "John Doe" },
                    { 8, "Reliable laptop, solid build and decent performance.", 5, 4, new DateTime(2024, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harvey Dent" },
                    { 9, "Good for light work, but a bit slow for multitasking.", 5, 3, new DateTime(2024, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rachel Green" },
                    { 10, "Compact yet powerful. Great value for the price.", 6, 5, new DateTime(2024, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bruce Banner" },
                    { 11, "Excellent camera quality and battery.", 6, 4, new DateTime(2024, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Natasha Romanoff" },
                    { 12, "Lasts all day. Perfect for travel.", 7, 5, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Steve Rogers" },
                    { 13, "Charges fast, but gets warm during use.", 7, 3, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tony Stark" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "InventoryID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "InventoryID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "InventoryID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "InventoryID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "InventoryID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "InventoryID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "InventoryID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductAttributes",
                keyColumn: "AttributeID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProductPrices",
                keyColumn: "PriceID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductPrices",
                keyColumn: "PriceID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductPrices",
                keyColumn: "PriceID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductPrices",
                keyColumn: "PriceID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductPrices",
                keyColumn: "PriceID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductPrices",
                keyColumn: "PriceID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductPrices",
                keyColumn: "PriceID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductPrices",
                keyColumn: "PriceID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "ReviewID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 1);
        }
    }
}
