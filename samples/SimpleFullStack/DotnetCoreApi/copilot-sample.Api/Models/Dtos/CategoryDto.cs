namespace copilot_sample.Api.Models.Dtos
{
    public class CategoryDto
    {
        public int CategoryID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? ParentCategoryID { get; set; }
        public List<CategoryDto>? SubCategories { get; set; }
    }
}
