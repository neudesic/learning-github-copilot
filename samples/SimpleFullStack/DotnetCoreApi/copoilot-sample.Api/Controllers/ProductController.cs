using copoilot_sample.Api.Models.Dtos;
using copoilot_sample.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace copoilot_sample.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = $"Product with ID {id} not found." });
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDto addProductDto)
        {
            try
            {
                var product = await _productService.AddProductAsync(addProductDto);
                return CreatedAtAction(nameof(GetProductById), new { id = product.ProductID }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = "Invalid request.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            var success = await _productService.UpdateProductAsync(id, updateProductDto);
            if (!success)
            {
                return NotFound(new { Message = $"Product with ID {id} not found." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound(new { Message = $"Product with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
