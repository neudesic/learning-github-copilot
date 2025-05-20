using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.Api.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace copilot_sample.Api.Services
{
    /// <summary>
    /// Service for managing categories in the application.
    /// Provides methods for CRUD operations on categories.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context to interact with the database.</param>
        public CategoryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            return await _dbContext.Categories
                .Include(c => c.SubCategories)
                .Select(i => CategoryMappings.MapWithSubcategories(i))
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _dbContext.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return null;
            }

            // Use the mapping expression to map the category to a CategoryDto
            return CategoryMappings.MapWithSubcategories(category);
        }

        public async Task<CategoryDto?> AddCategoryAsync(AddCategoryDto addCategoryDto)
        {
            var existingCategory = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == addCategoryDto.Name.ToLower());

            if (existingCategory != null)
            {
                return null;
            }

            var category = new Category
            {
                Name = addCategoryDto.Name,
                Description = addCategoryDto.Description,
                ParentCategoryID = addCategoryDto.ParentCategoryID
            };

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            // Use the mapping expression to map the newly created category to a CategoryDto
            return CategoryMappings.MapWithSubcategories(category);
        }

        public async Task<bool> UpdateCategoryDescriptionAsync(int id, UpdateCategoryDescriptionDto updateDto)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }

            category.Description = updateDto.Description;
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
