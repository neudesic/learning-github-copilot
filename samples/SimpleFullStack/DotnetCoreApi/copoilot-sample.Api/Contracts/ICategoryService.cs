using copoilot_sample.Api.Models.Dtos;

namespace copoilot_sample.Api.Services
{
    /// <summary>
    /// Interface for managing categories in the application.
    /// Provides methods for CRUD operations on categories.
    /// </summary>
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto?> AddCategoryAsync(AddCategoryDto addCategoryDto);
        Task<bool> UpdateCategoryDescriptionAsync(int id, UpdateCategoryDescriptionDto updateDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
