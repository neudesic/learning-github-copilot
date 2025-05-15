namespace copoilot_sample.DataAccess.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? ParentCategoryID { get; set; }

        // Navigation properties
        public Category? ParentCategory { get; set; }
        public ICollection<Category>? SubCategories { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
