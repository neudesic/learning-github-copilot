using copilot_sample.Api.Models.Dtos;
using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace copilot_sample.Api.Services
{
    /// <summary>
    /// Service for managing products in the application.
    /// Provides methods for CRUD operations on products.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context to interact with the database.</param>
        public ProductService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var products = await _dbContext.Products.ToListAsync();
            return products.Select(p => new ProductDto
            {
                ProductID = p.ProductID,
                Name = p.Name,
                Description = p.Description,
                SKU = p.SKU,
                CategoryID = p.CategoryID,
                Brand = p.Brand,
                IsActive = p.IsActive,
                Category = new CategoryDto
                {
                    CategoryID = p.Category.CategoryID,
                    Name = p.Category.Name,
                    Description = p.Category.Description,

                }
            }).ToList();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                CategoryID = product.CategoryID,
                Brand = product.Brand,
                IsActive = product.IsActive
            };
        }

        public async Task<ProductDto> AddProductAsync(AddProductDto addProductDto)
        {
            var product = new Product
            {
                Name = addProductDto.Name,
                Description = addProductDto.Description,
                SKU = addProductDto.SKU,
                CategoryID = addProductDto.CategoryID,
                Brand = addProductDto.Brand,
                IsActive = addProductDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return new ProductDto
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                CategoryID = product.CategoryID,
                Brand = product.Brand,
                IsActive = product.IsActive
            };
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null) return false;

            product.Name = updateProductDto.Name ?? product.Name;
            product.Description = updateProductDto.Description ?? product.Description;
            product.SKU = updateProductDto.SKU ?? product.SKU;
            product.CategoryID = updateProductDto.CategoryID ?? product.CategoryID;
            product.Brand = updateProductDto.Brand ?? product.Brand;
            product.IsActive = updateProductDto.IsActive ?? product.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null) return false;

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
