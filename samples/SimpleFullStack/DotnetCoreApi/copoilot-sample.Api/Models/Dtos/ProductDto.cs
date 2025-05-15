namespace copoilot_sample.Api.Models.Dtos
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string SKU { get; set; } = null!;
        public int CategoryID { get; set; }
        public string? Brand { get; set; }
        public bool IsActive { get; set; }

        public CategoryDto Category { get; set; }
    }

    public class AddProductDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string SKU { get; set; } = null!;
        public int CategoryID { get; set; }
        public string? Brand { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? SKU { get; set; }
        public int? CategoryID { get; set; }
        public string? Brand { get; set; }
        public bool? IsActive { get; set; }
    }


}
