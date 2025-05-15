namespace copoilot_sample.DataAccess.Entities
{
    public class ProductReview
    {
        public int ReviewID { get; set; }
        public int ProductID { get; set; }
        public string? ReviewerName { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        // Navigation property
        public Product? Product { get; set; }
    }
}
