using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using copoilot_sample.DataAccess.Entities;

namespace copoilot_sample.DataAccess.EntityConfiguration
{
    public class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPrice>
    {
        public void Configure(EntityTypeBuilder<ProductPrice> builder)
        {
            // Table mapping
            builder.ToTable("ProductPrices");

            // Primary key
            builder.HasKey(pp => pp.PriceID);

            // Column mappings
            builder.Property(pp => pp.PriceID)
                .HasColumnName("PriceID")
                .UseIdentityColumn();

            builder.Property(pp => pp.ProductID)
                .HasColumnName("ProductID")
                .IsRequired();

            builder.Property(pp => pp.Price)
                .HasColumnName("Price")
                .HasColumnType("DECIMAL(18, 2)")
                .IsRequired();

            builder.Property(pp => pp.CurrencyCode)
                .HasColumnName("CurrencyCode")
                .HasMaxLength(3)
                .HasDefaultValue("USD");

            builder.Property(pp => pp.EffectiveFrom)
                .HasColumnName("EffectiveFrom")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(pp => pp.EffectiveTill)
                .HasColumnName("EffectiveTill");

            // Relationships
            builder.HasOne(pp => pp.Product)
                .WithMany(p => p.ProductPrices)
                .HasForeignKey(pp => pp.ProductID);
        }
    }
}
