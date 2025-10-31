namespace copilot_sample.Api.Models.Dtos
{
    public class ProductAttributeDto
    {
        public int AttributeID { get; set; }
        public int ProductID { get; set; }
        public string AttributeName { get; set; } = null!;
        public string AttributeValue { get; set; } = null!;
    }

    public class AddProductAttributeDto
    {
        public int ProductID { get; set; }
        public string AttributeName { get; set; } = null!;
        public string AttributeValue { get; set; } = null!;
    }

    public class UpdateProductAttributeDto
    {
        public required string AttributeName { get; set; }
        public required string AttributeValue { get; set; }
    }
    
}
