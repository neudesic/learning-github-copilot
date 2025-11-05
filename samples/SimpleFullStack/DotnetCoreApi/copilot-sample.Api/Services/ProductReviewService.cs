using copilot_sample.Api.Models.Dtos;
using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace copilot_sample.Api.Services
{
    /// <summary>
    /// Service for managing product reviews in the application.
    /// Provides methods for CRUD operations on product reviews.
    /// </summary>
    public class ProductReviewService : IProductReviewService
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductReviewService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context to interact with the database.</param>
        public ProductReviewService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProductReviewDto>> GetProductReviewsAsync()
        {
            var reviews = await _dbContext.ProductReviews.ToListAsync();
            return reviews.Select(r => new ProductReviewDto
            {
                ReviewID = r.ReviewID,
                ProductID = r.ProductID,
                ReviewerName = r.ReviewerName,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate
            }).ToList();
        }

        public async Task<ProductReviewDto?> GetProductReviewByIdAsync(int id)
        {
            var review = await _dbContext.ProductReviews.FindAsync(id);
            if (review == null) return null;

            return new ProductReviewDto
            {
                ReviewID = review.ReviewID,
                ProductID = review.ProductID,
                ReviewerName = review.ReviewerName,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            };
        }

        public async Task<List<ProductReviewDto>> GetProductReviewsByProductIdAsync(int productId)
        {
            var reviews = await _dbContext.ProductReviews
                .Where(r => r.ProductID == productId)
                .ToListAsync();

            return reviews.Select(r => new ProductReviewDto
            {
                ReviewID = r.ReviewID,
                ProductID = r.ProductID,
                ReviewerName = r.ReviewerName,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate
            }).ToList();
        }

        public async Task<ProductReviewDto> AddProductReviewAsync(AddProductReviewDto addProductReviewDto)
        {
            var productExists = await _dbContext.Products.FindAsync(addProductReviewDto.ProductID);
            if (productExists == null)
                throw new ArgumentException($"Product with ID {addProductReviewDto.ProductID} does not exist");

            if (addProductReviewDto.Rating < 1 || addProductReviewDto.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5");

            var review = new ProductReview
            {
                ProductID = addProductReviewDto.ProductID,
                ReviewerName = addProductReviewDto.ReviewerName,
                Rating = addProductReviewDto.Rating,
                Comment = addProductReviewDto.Comment,
                ReviewDate = DateTime.UtcNow
            };

            _dbContext.ProductReviews.Add(review);
            await _dbContext.SaveChangesAsync();

            return new ProductReviewDto
            {
                ReviewID = review.ReviewID,
                ProductID = review.ProductID,
                ReviewerName = review.ReviewerName,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            };
        }

        public async Task<bool> UpdateProductReviewAsync(int id, UpdateProductReviewDto updateProductReviewDto)
        {
            var review = await _dbContext.ProductReviews.FindAsync(id);
            if (review == null) return false;

            if (updateProductReviewDto.ReviewerName != null)
                review.ReviewerName = updateProductReviewDto.ReviewerName;

            if (updateProductReviewDto.Rating.HasValue)
            {
                if (updateProductReviewDto.Rating < 1 || updateProductReviewDto.Rating > 5)
                    throw new ArgumentException("Rating must be between 1 and 5");
                review.Rating = updateProductReviewDto.Rating.Value;
            }

            if (updateProductReviewDto.Comment != null)
                review.Comment = updateProductReviewDto.Comment;

            _dbContext.ProductReviews.Update(review);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductReviewAsync(int id)
        {
            var review = await _dbContext.ProductReviews.FindAsync(id);
            if (review == null) return false;

            _dbContext.ProductReviews.Remove(review);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
