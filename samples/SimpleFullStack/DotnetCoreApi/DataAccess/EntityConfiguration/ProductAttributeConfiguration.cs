using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using copilot_sample.DataAccess.Entities;

namespace copilot_sample.DataAccess.EntityConfiguration
{
    public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            // Table mapping
            builder.ToTable("ProductAttributes");

            // Primary key
            builder.HasKey(pa => pa.AttributeID);

            // Column mappings
            builder.Property(pa => pa.AttributeID)
                .HasColumnName("AttributeID")
                .UseIdentityColumn();

            builder.Property(pa => pa.ProductID)
                .HasColumnName("ProductID")
                .IsRequired();

            builder.Property(pa => pa.AttributeName)
                .HasColumnName("AttributeName")
                .HasMaxLength(100);

            builder.Property(pa => pa.AttributeValue)
                .HasColumnName("AttributeValue")
                .HasMaxLength(255);

            // Relationships
            builder.HasOne(pa => pa.Product)
                .WithMany(p => p.ProductAttributes)
                .HasForeignKey(pa => pa.ProductID);
        }
    }
}
