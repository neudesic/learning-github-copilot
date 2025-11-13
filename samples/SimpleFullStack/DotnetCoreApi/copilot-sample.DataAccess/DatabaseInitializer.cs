using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using copilot_sample.DataAccess.SeedData;

namespace copilot_sample.DataAccess
{
    /// <summary>
    /// Provides database initialization and seeding functionality.
    /// Handles database creation, migration application, and optional runtime seeding.
    /// </summary>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Initializes the database by ensuring it exists and applying pending migrations.
        /// This method should be called during application startup.
        /// </summary>
        /// <param name="serviceProvider">The service provider to resolve dependencies</param>
        /// <param name="logger">Optional logger for tracking initialization progress</param>
        public static async Task InitializeAsync(IServiceProvider serviceProvider, ILogger? logger = null)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                logger?.LogInformation("Starting database initialization...");

                // Ensure database is created and apply pending migrations
                // For InMemory databases, use EnsureCreated; for real databases, use Migrate
                var isInMemory = context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";
                if (isInMemory)
                {
                    await context.Database.EnsureCreatedAsync();
                }
                else
                {
                    await context.Database.MigrateAsync();
                }

                logger?.LogInformation("Database initialization completed successfully. All migrations have been applied.");

                // Check if data exists (seed data is handled by migrations via AppDbContext.SeedData)
                var hasData = await context.Categories.AnyAsync();
                if (hasData)
                {
                    logger?.LogInformation("Database contains seed data from migrations.");
                }
                else
                {
                    logger?.LogWarning("Database appears to be empty. Seed data should be applied via migrations.");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }
        /// <summary>
        /// Alternative method for runtime seeding if needed (not recommended for production).
        /// Use this only if you need to add data that's not part of migrations.
        /// </summary>
        /// <param name="serviceProvider">The service provider to resolve dependencies</param>
        /// <param name="logger">Optional logger for tracking seeding progress</param>
        /// <param name="forceReseed">If true, will clear existing data and reseed</param>
        public static async Task SeedDataAsync(IServiceProvider serviceProvider, ILogger? logger = null, bool forceReseed = false)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Check if seeding is needed
                var hasData = await context.Categories.AnyAsync();
                if (hasData && !forceReseed)
                {
                    logger?.LogInformation("Database already contains data. Skipping runtime seeding.");
                    return;
                }

                if (forceReseed && hasData)
                {
                    logger?.LogInformation("Force reseed requested. Clearing existing data...");
                    await ClearExistingDataAsync(context, logger);
                }

                logger?.LogInformation("Starting runtime database seeding...");

                // Apply seed data using the centralized seed data class
                await InventorySeedData.ApplyToContextAsync(context);

                // Log seeding results
                var counts = InventorySeedData.GetSeedDataCounts();
                logger?.LogInformation($"Runtime seeding completed successfully. Added: {counts.Categories} categories, {counts.Products} products, {counts.Prices} prices, {counts.Inventory} inventory items, {counts.Attributes} attributes, {counts.Reviews} reviews.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "An error occurred while performing runtime database seeding.");
                throw;
            }
        }

        /// <summary>
        /// Clears all existing data from the database (in reverse dependency order).
        /// WARNING: This will delete all data in the database!
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="logger">Optional logger</param>
        private static async Task ClearExistingDataAsync(AppDbContext context, ILogger? logger = null)
        {
            logger?.LogWarning("Clearing all existing data from database...");

            // Delete in reverse dependency order to avoid foreign key constraints
            context.ProductReviews.RemoveRange(context.ProductReviews);
            context.ProductAttributes.RemoveRange(context.ProductAttributes);
            context.ProductPrices.RemoveRange(context.ProductPrices);
            context.Inventory.RemoveRange(context.Inventory);
            context.Products.RemoveRange(context.Products);
            context.Categories.RemoveRange(context.Categories);

            await context.SaveChangesAsync();
            logger?.LogInformation("All existing data cleared from database.");
        }

        /// <summary>
        /// Validates that the database schema and seed data are properly configured.
        /// </summary>
        /// <param name="serviceProvider">The service provider to resolve dependencies</param>
        /// <param name="logger">Optional logger for validation messages</param>
        /// <returns>True if validation passes, false otherwise</returns>
        public static async Task<bool> ValidateDatabaseAsync(IServiceProvider serviceProvider, ILogger? logger = null)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Check if database can be accessed
                var canConnect = await context.Database.CanConnectAsync();
                if (!canConnect)
                {
                    logger?.LogError("Cannot connect to the database.");
                    return false;
                }

                // For in-memory databases, skip seed data validation as seeding happens later
                var isInMemory = context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";
                if (isInMemory)
                {
                    logger?.LogInformation("In-memory database validation passed (seed data validation skipped).");
                    return true;
                }

                // Check if required tables exist with data
                var categoriesCount = await context.Categories.CountAsync();
                var productsCount = await context.Products.CountAsync();
                var inventoryCount = await context.Inventory.CountAsync();

                logger?.LogInformation($"Database validation: Categories={categoriesCount}, Products={productsCount}, Inventory={inventoryCount}");

                // Basic validation - should have seed data
                if (categoriesCount >= 4 && productsCount >= 7 && inventoryCount >= 7)
                {
                    logger?.LogInformation("Database validation passed.");
                    return true;
                }
                else
                {
                    logger?.LogWarning("Database validation failed. Expected seed data not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "An error occurred during database validation.");
                return false;
            }
        }
    }
}