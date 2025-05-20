namespace copilot_sample.Api.Models.Dtos
{
    public class AddCategoryDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? ParentCategoryID { get; set; }
    }
    public class UpdateCategoryDescriptionDto
    {
        public string? Description { get; set; }
    }
}
