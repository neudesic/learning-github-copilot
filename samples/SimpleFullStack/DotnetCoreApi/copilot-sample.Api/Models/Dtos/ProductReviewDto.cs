namespace copilot_sample.Api.Models.Dtos
{
    public class ProductReviewDto
    {
        public int ReviewID { get; set; }
        public int ProductID { get; set; }
        public string? ReviewerName { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }

    public class AddProductReviewDto
    {
        public int ProductID { get; set; }
        public string? ReviewerName { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }

    public class UpdateProductReviewDto
    {
        public string? ReviewerName { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
    }
}
