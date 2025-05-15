namespace copoilot_sample.DataAccess.Entities
{
    public class ProductAttribute
    {
        public int AttributeID { get; set; }
        public int ProductID { get; set; }
        public required string AttributeName { get; set; }
        public required string AttributeValue { get; set; }

        // Navigation property
        public Product? Product { get; set; }
    }
}
