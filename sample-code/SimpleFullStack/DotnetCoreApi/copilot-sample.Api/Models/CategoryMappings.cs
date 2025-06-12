using System.Linq.Expressions;
using copilot_sample.DataAccess.Entities;

namespace copilot_sample.Api.Models.Dtos
{
    public static class CategoryMappings
    {
        private static readonly Func<Category, CategoryDto> _compiledMapper = null!;

        static CategoryMappings()
        {
            _compiledMapper = MapCategory;
        }

        private static CategoryDto MapCategory(Category category)
        {
            return new CategoryDto
            {
                CategoryID = category.CategoryID,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryID = category.ParentCategoryID,
                SubCategories = category.SubCategories?.Select(sc => MapCategory(sc)).ToList()
            };
        }

        /// <summary>
        /// Expression to map a Category entity to a CategoryDto without subcategories.
        /// </summary>
        private static readonly Expression<Func<Category, CategoryDto>> ToCategoryDto = category => new CategoryDto
        {
            CategoryID = category.CategoryID,
            Name = category.Name,
            Description = category.Description,
            ParentCategoryID = category.ParentCategoryID,
            SubCategories = null // Subcategories will be mapped separately
        };

        /// <summary>
        /// Method to map subcategories after the main query is executed.
        /// </summary>
        public static CategoryDto MapWithSubcategories(Category category)
        {
            var dto = _compiledMapper(category);
            if (category.SubCategories != null)
            {
                dto.SubCategories = category.SubCategories.Select(MapWithSubcategories).ToList();
            }
            return dto;
        }
    }
}
