using copilot_sample.Api.Models.Dtos;
using copilot_sample.DataAccess.Entities;

namespace copilot_sample.Api.Services
{
    public interface IProductAttributeService
    {
        Task<List<ProductAttributeDto>> GetProductAttributesAsync();
        Task<ProductAttributeDto?> GetProductAttributeByIdAsync(int id);
        Task<ProductAttribute> AddProductAttributeAsync(AddProductAttributeDto addDto);
        Task<bool> UpdateProductAttributeAsync(int id, UpdateProductAttributeDto updateDto);
        Task<bool> DeleteProductAttributeAsync(int id);
    }
}
