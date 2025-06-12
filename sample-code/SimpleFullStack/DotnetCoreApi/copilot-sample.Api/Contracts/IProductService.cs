using copilot_sample.Api.Models.Dtos;

namespace copilot_sample.Api.Services
{
    /// <summary>
    /// Interface for managing products in the application.
    /// Provides methods for CRUD operations on products.
    /// </summary>
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> AddProductAsync(AddProductDto addProductDto);
        Task<bool> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);
    }
}
