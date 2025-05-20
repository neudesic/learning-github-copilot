using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using copilot_sample.DataAccess.Entities;

namespace copilot_sample.DataAccess.EntityConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Table mapping
            builder.ToTable("Categories");

            // Primary key
            builder.HasKey(c => c.CategoryID);

            // Column mappings
            builder.Property(c => c.CategoryID)
                .HasColumnName("CategoryID")
                .UseIdentityColumn();

            builder.Property(c => c.Name)
                .HasColumnName("Name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasColumnName("Description")
                .HasMaxLength(500);

            builder.Property(c => c.ParentCategoryID)
                .HasColumnName("ParentCategoryID");

            // Relationships
            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
