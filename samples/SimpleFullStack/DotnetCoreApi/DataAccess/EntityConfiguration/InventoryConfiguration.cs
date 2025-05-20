using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using copilot_sample.DataAccess.Entities;

namespace copilot_sample.DataAccess.EntityConfiguration
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            // Table mapping
            builder.ToTable("Inventory");

            // Primary key
            builder.HasKey(i => i.InventoryID);

            // Column mappings
            builder.Property(i => i.InventoryID)
                .HasColumnName("InventoryID")
                .UseIdentityColumn();

            builder.Property(i => i.ProductID)
                .HasColumnName("ProductID")
                .IsRequired();

            builder.Property(i => i.Quantity)
                .HasColumnName("Quantity")
                .IsRequired();

            builder.Property(i => i.LastUpdated)
                .HasColumnName("LastUpdated")
                .HasDefaultValueSql("GETDATE()");

            // Relationships
            builder.HasOne(i => i.Product)
                .WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(i => i.ProductID);
        }
    }
}
