using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using copoilot_sample.DataAccess.Entities;

namespace copoilot_sample.DataAccess.EntityConfiguration
{
    public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {
            // Table mapping
            builder.ToTable("ProductReviews");

            // Primary key
            builder.HasKey(pr => pr.ReviewID);

            // Column mappings
            builder.Property(pr => pr.ReviewID)
                .HasColumnName("ReviewID")
                .UseIdentityColumn();

            builder.Property(pr => pr.ProductID)
                .HasColumnName("ProductID")
                .IsRequired();

            builder.Property(pr => pr.ReviewerName)
                .HasColumnName("ReviewerName")
                .HasMaxLength(100);

            builder.Property(pr => pr.Rating)
                .HasColumnName("Rating")
                .IsRequired();

            // Add check constraint for Rating
            builder.HasCheckConstraint("CK_ProductReviews_Rating", "Rating BETWEEN 1 AND 5");

            builder.Property(pr => pr.Comment)
                .HasColumnName("Comment")
                .HasColumnType("NVARCHAR(MAX)");

            builder.Property(pr => pr.ReviewDate)
                .HasColumnName("ReviewDate")
                .HasDefaultValueSql("GETDATE()");

            // Relationships
            builder.HasOne(pr => pr.Product)
                .WithMany(p => p.ProductReviews)
                .HasForeignKey(pr => pr.ProductID);
        }
    }
}
