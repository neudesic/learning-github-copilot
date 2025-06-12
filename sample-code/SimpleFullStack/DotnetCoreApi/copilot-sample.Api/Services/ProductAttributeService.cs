using copilot_sample.DataAccess;
using copilot_sample.DataAccess.Entities;
using copilot_sample.Api.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace copilot_sample.Api.Services
{
    public class ProductAttributeService
    {
        private readonly AppDbContext _dbContext;

        public ProductAttributeService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProductAttributeDto>> GetProductAttributesAsync()
        {
            var attributes = await _dbContext.ProductAttributes.ToListAsync();
            return attributes.Select(a => new ProductAttributeDto
            {
                AttributeID = a.AttributeID,
                ProductID = a.ProductID,
                AttributeName = a.AttributeName,
                AttributeValue = a.AttributeValue
            }).ToList();
        }

        public async Task<ProductAttributeDto?> GetProductAttributeByIdAsync(int id)
        {
            var attribute = await _dbContext.ProductAttributes.FindAsync(id);
            if (attribute == null) return null;

            return new ProductAttributeDto
            {
                AttributeID = attribute.AttributeID,
                ProductID = attribute.ProductID,
                AttributeName = attribute.AttributeName,
                AttributeValue = attribute.AttributeValue
            };
        }

        public async Task<ProductAttribute> AddProductAttributeAsync(AddProductAttributeDto addDto)
        {
            var product = await _dbContext.Products.FindAsync(addDto.ProductID);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {addDto.ProductID} does not exist.");
            }

            var attribute = new ProductAttribute
            {
                ProductID = addDto.ProductID,
                AttributeName = addDto.AttributeName,
                AttributeValue = addDto.AttributeValue
            };

            _dbContext.ProductAttributes.Add(attribute);
            await _dbContext.SaveChangesAsync();

            return attribute;
        }

        public async Task<bool> UpdateProductAttributeAsync(int id, UpdateProductAttributeDto updateDto)
        {
            var attribute = await _dbContext.ProductAttributes.FindAsync(id);
            if (attribute == null) return false;

            attribute.AttributeName = updateDto.AttributeName;
            attribute.AttributeValue = updateDto.AttributeValue;

            _dbContext.ProductAttributes.Update(attribute);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAttributeAsync(int id)
        {
            var attribute = await _dbContext.ProductAttributes.FindAsync(id);
            if (attribute == null) return false;

            _dbContext.ProductAttributes.Remove(attribute);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
