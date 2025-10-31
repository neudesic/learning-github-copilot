using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using copilot_sample.DataAccess.Entities;

namespace copilot_sample.DataAccess.EntityConfiguration
{
    public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {            // Table mapping with check constraint
            builder.ToTable("ProductReviews", t => t.HasCheckConstraint("CK_ProductReviews_Rating", "Rating BETWEEN 1 AND 5"));

            // Primary key
            builder.HasKey(pr => pr.ReviewID);

            // Column mappings
            builder.Property(pr => pr.ReviewID)
                .HasColumnName("ReviewID")
                .ValueGeneratedOnAdd();

            builder.Property(pr => pr.ProductID)
                .HasColumnName("ProductID")
                .IsRequired();

            builder.Property(pr => pr.ReviewerName)
                .HasColumnName("ReviewerName")
                .HasMaxLength(100);            builder.Property(pr => pr.Rating)
                .HasColumnName("Rating")
                .IsRequired();            builder.Property(pr => pr.Comment)
                .HasColumnName("Comment");

            builder.Property(pr => pr.ReviewDate)
                .HasColumnName("ReviewDate")
                .HasDefaultValueSql("datetime('now')");

            // Relationships
            builder.HasOne(pr => pr.Product)
                .WithMany(p => p.ProductReviews)
                .HasForeignKey(pr => pr.ProductID);
        }
    }
}
