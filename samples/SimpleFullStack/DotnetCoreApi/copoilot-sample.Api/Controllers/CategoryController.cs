using copoilot_sample.Api.Models.Dtos;
using copoilot_sample.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace copoilot_sample.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = $"Category with ID {id} not found." });
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDto addCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _categoryService.AddCategoryAsync(addCategoryDto);
            if (category == null)
            {
                return Conflict(new { Message = $"A category with {addCategoryDto.Name} already exists." });
            }
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryID }, category);
        }

        [HttpPatch("{id}/description")]
        public async Task<IActionResult> UpdateCategoryDescription(int id, [FromBody] UpdateCategoryDescriptionDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _categoryService.UpdateCategoryDescriptionAsync(id, updateDto);
            if (!success)
            {
                return NotFound(new { Message = $"Category with ID {id} not found." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            if (!success)
            {
                return NotFound(new { Message = $"Category with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
