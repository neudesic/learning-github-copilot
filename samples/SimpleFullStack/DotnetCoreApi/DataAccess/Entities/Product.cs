namespace copilot_sample.DataAccess.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string SKU { get; set; } = null!;
        public int CategoryID { get; set; }
        public string? Brand { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public Category? Category { get; set; }
        public ICollection<ProductPrice>? ProductPrices { get; set; }
        public Inventory? Inventory { get; set; }
        public ICollection<ProductAttribute>? ProductAttributes { get; set; }
        public ICollection<ProductReview>? ProductReviews { get; set; }
    }
}
