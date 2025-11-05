using copilot_sample.Api.Models.Dtos;

namespace copilot_sample.Api.Services
{
    /// <summary>
    /// Interface for managing product reviews in the application.
    /// Provides methods for CRUD operations on product reviews.
    /// </summary>
    public interface IProductReviewService
    {
        Task<List<ProductReviewDto>> GetProductReviewsAsync();
        Task<ProductReviewDto?> GetProductReviewByIdAsync(int id);
        Task<List<ProductReviewDto>> GetProductReviewsByProductIdAsync(int productId);
        Task<ProductReviewDto> AddProductReviewAsync(AddProductReviewDto addProductReviewDto);
        Task<bool> UpdateProductReviewAsync(int id, UpdateProductReviewDto updateDto);
        Task<bool> DeleteProductReviewAsync(int id);
    }
}
