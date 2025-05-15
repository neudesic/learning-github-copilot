using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using copoilot_sample.DataAccess.Entities;

namespace copoilot_sample.DataAccess.EntityConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Table mapping
            builder.ToTable("Products");

            // Primary key
            builder.HasKey(p => p.ProductID);

            // Column mappings
            builder.Property(p => p.ProductID)
                .HasColumnName("ProductID")
                .UseIdentityColumn();

            builder.Property(p => p.Name)
                .HasColumnName("Name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("Description")
                .HasColumnType("NVARCHAR(MAX)");

            builder.Property(p => p.SKU)
                .HasColumnName("SKU")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(p => p.SKU)
                .IsUnique();

            builder.Property(p => p.CategoryID)
                .HasColumnName("CategoryID")
                .IsRequired();

            builder.Property(p => p.Brand)
                .HasColumnName("Brand")
                .HasMaxLength(100);

            builder.Property(p => p.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true);

            // Relationships
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID);

            builder.HasMany(p => p.ProductPrices)
                .WithOne(pp => pp.Product)
                .HasForeignKey(pp => pp.ProductID);

            builder.HasOne(p => p.Inventory)
                .WithOne(i => i.Product)
                .HasForeignKey<Inventory>(i => i.ProductID);

            builder.HasMany(p => p.ProductAttributes)
                .WithOne(pa => pa.Product)
                .HasForeignKey(pa => pa.ProductID);

            builder.HasMany(p => p.ProductReviews)
                .WithOne(pr => pr.Product)
                .HasForeignKey(pr => pr.ProductID);
        }
    }
}
