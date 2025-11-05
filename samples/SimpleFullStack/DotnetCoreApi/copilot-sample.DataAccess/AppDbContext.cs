using System.Threading;
using Microsoft.EntityFrameworkCore;
using copilot_sample.DataAccess.Entities;
using copilot_sample.DataAccess.EntityConfiguration;
using copilot_sample.DataAccess.SeedData;

namespace copilot_sample.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Define DbSet properties for your entities
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductPriceConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductAttributeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductReviewConfiguration());

            // Seed data
            SeedData(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<Product>())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Property(p => p.CreatedAt).CurrentValue == default)
                    {
                        entry.Property(p => p.CreatedAt).CurrentValue = now;
                    }

                    entry.Property(p => p.UpdatedAt).CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(p => p.UpdatedAt).CurrentValue = now;
                }
            }
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Use centralized seed data from the SeedData folder
            InventorySeedData.ApplyToModelBuilder(modelBuilder);
        }
    }
}
