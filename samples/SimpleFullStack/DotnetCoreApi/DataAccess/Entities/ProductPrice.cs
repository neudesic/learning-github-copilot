namespace copilot_sample.DataAccess.Entities
{
    public class ProductPrice
    {
        public int PriceID { get; set; }
        public int ProductID { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTill { get; set; }

        // Navigation property
        public Product? Product { get; set; }
    }
}
