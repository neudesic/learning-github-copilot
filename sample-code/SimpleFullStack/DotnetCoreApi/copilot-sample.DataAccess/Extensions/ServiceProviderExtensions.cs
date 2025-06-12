using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace copilot_sample.DataAccess.Extensions
{
    /// <summary>
    /// Extension methods for IServiceProvider to facilitate database operations.
    /// These extensions make it easier to initialize and seed the database from the API layer.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Initializes the database with migrations and validates the setup.
        /// This is the recommended method for production environments.
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>True if initialization and validation succeed, false otherwise</returns>
        public static async Task<bool> InitializeDatabaseAsync(this IServiceProvider serviceProvider, ILogger? logger = null)
        {
            try
            {
                // Initialize database (apply migrations)
                await DatabaseInitializer.InitializeAsync(serviceProvider, logger);
                
                // Validate database setup
                var isValid = await DatabaseInitializer.ValidateDatabaseAsync(serviceProvider, logger);
                if (!isValid)
                {
                    logger?.LogWarning("Database validation failed after initialization.");
                    return false;
                }
                
                logger?.LogInformation("Database initialization and validation completed successfully.");
                return true;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to initialize and validate database.");
                return false;
            }
        }

        /// <summary>
        /// Seeds the database with sample data if needed.
        /// This method should only be used for development/testing environments.
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="logger">Optional logger</param>
        /// <param name="forceReseed">If true, clears existing data and reseeds</param>
        /// <returns>True if seeding succeeds or is skipped, false otherwise</returns>
        public static async Task<bool> SeedDatabaseAsync(this IServiceProvider serviceProvider, ILogger? logger = null, bool forceReseed = false)
        {
            try
            {
                await DatabaseInitializer.SeedDataAsync(serviceProvider, logger, forceReseed);
                return true;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to seed database.");
                return false;
            }
        }

        /// <summary>
        /// Performs a complete database setup: initialization, validation, and optional seeding.
        /// This is a convenience method that combines multiple operations.
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="logger">Optional logger</param>
        /// <param name="seedInDevelopment">If true, seeds data in development environment</param>
        /// <param name="forceReseed">If true and seeding is enabled, clears existing data first</param>
        /// <returns>True if all operations succeed, false otherwise</returns>
        public static async Task<bool> SetupDatabaseAsync(this IServiceProvider serviceProvider, ILogger? logger = null, bool seedInDevelopment = true, bool forceReseed = false)
        {
            try
            {
                // Step 1: Initialize database
                var initSuccess = await serviceProvider.InitializeDatabaseAsync(logger);
                if (!initSuccess)
                {
                    return false;
                }

                // Step 2: Seed data if in development and requested
                if (seedInDevelopment)
                {
                    var seedSuccess = await serviceProvider.SeedDatabaseAsync(logger, forceReseed);
                    if (!seedSuccess)
                    {
                        logger?.LogWarning("Database seeding failed, but initialization was successful.");
                        return true; // Don't fail entirely if seeding fails
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to setup database.");
                return false;
            }
        }
    }
}
