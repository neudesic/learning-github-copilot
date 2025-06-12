using copilot_sample.Api.Models.Dtos;
using copilot_sample.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace copilot_sample.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductAttributeController : ControllerBase
    {
        private readonly ProductAttributeService _productAttributeService;

        public ProductAttributeController(ProductAttributeService productAttributeService)
        {
            _productAttributeService = productAttributeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductAttributes()
        {
            var attributes = await _productAttributeService.GetProductAttributesAsync();
            return Ok(attributes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAttributeById(int id)
        {
            var attribute = await _productAttributeService.GetProductAttributeByIdAsync(id);
            if (attribute == null)
            {
                return NotFound(new { Message = $"Product Attribute with ID {id} not found." });
            }
            return Ok(attribute);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAttribute([FromBody] AddProductAttributeDto addDto)
        {
            try
            {
                var attribute = await _productAttributeService.AddProductAttributeAsync(addDto);
                return CreatedAtAction(nameof(GetProductAttributeById), new { id = attribute.AttributeID }, attribute);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAttribute(int id, [FromBody] UpdateProductAttributeDto updateDto)
        {
            var success = await _productAttributeService.UpdateProductAttributeAsync(id, updateDto);
            if (!success)
            {
                return NotFound(new { Message = $"Product Attribute with ID {id} not found." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAttribute(int id)
        {
            var success = await _productAttributeService.DeleteProductAttributeAsync(id);
            if (!success)
            {
                return NotFound(new { Message = $"Product Attribute with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
