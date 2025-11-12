using copilot_sample.Api.Controllers;
using copilot_sample.Api.Models.Dtos;
using copilot_sample.Api.Services;
using copilot_sample.DataAccess.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace copilot_sample.Api.Tests.Controllers
{
    public class ProductAttributeControllerTests
    {
        private ProductAttributeController CreateController(Mock<IProductAttributeService>? mockService = null)
        {
            mockService ??= new Mock<IProductAttributeService>();
            return new ProductAttributeController(mockService.Object);
        }

        #region GetProductAttributes Tests

        [Fact]
        public async Task GetProductAttributes_ReturnsOkResult_WithAllAttributes()
        {
            // Arrange
            var attributes = new List<ProductAttributeDto>
            {
                new ProductAttributeDto { AttributeID = 1, ProductID = 1, AttributeName = "Color", AttributeValue = "Red" },
                new ProductAttributeDto { AttributeID = 2, ProductID = 1, AttributeName = "Size", AttributeValue = "Large" }
            };

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.GetProductAttributesAsync())
                .ReturnsAsync(attributes);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.GetProductAttributes();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(attributes);
            mockService.Verify(s => s.GetProductAttributesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProductAttributes_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            var emptyAttributes = new List<ProductAttributeDto>();

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.GetProductAttributesAsync())
                .ReturnsAsync(emptyAttributes);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.GetProductAttributes();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(emptyAttributes);
            mockService.Verify(s => s.GetProductAttributesAsync(), Times.Once);
        }

        #endregion

        #region GetProductAttributeById Tests

        [Fact]
        public async Task GetProductAttributeById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var attributeId = 1;
            var attribute = new ProductAttributeDto
            {
                AttributeID = 1,
                ProductID = 1,
                AttributeName = "Color",
                AttributeValue = "Blue"
            };

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.GetProductAttributeByIdAsync(attributeId))
                .ReturnsAsync(attribute);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.GetProductAttributeById(attributeId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(attribute);
            mockService.Verify(s => s.GetProductAttributeByIdAsync(attributeId), Times.Once);
        }

        [Fact]
        public async Task GetProductAttributeById_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var attributeId = 999;

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.GetProductAttributeByIdAsync(attributeId))
                .ReturnsAsync((ProductAttributeDto?)null);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.GetProductAttributeById(attributeId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult!.Value.Should().BeEquivalentTo(new { Message = $"Product Attribute with ID {attributeId} not found." });
            mockService.Verify(s => s.GetProductAttributeByIdAsync(attributeId), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetProductAttributeById_WithInvalidId_ReturnsNotFound(int invalidId)
        {
            // Arrange
            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.GetProductAttributeByIdAsync(invalidId))
                .ReturnsAsync((ProductAttributeDto?)null);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.GetProductAttributeById(invalidId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        #endregion

        #region AddProductAttribute Tests

        [Fact]
        public async Task AddProductAttribute_WithValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var addDto = new AddProductAttributeDto
            {
                ProductID = 1,
                AttributeName = "Material",
                AttributeValue = "Cotton"
            };

            var createdAttribute = new ProductAttribute
            {
                AttributeID = 1,
                ProductID = addDto.ProductID,
                AttributeName = addDto.AttributeName,
                AttributeValue = addDto.AttributeValue
            };

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.AddProductAttributeAsync(It.IsAny<AddProductAttributeDto>()))
                .ReturnsAsync(createdAttribute);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.AddProductAttribute(addDto);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult!.ActionName.Should().Be(nameof(ProductAttributeController.GetProductAttributeById));
            createdResult.RouteValues!["id"].Should().Be(createdAttribute.AttributeID);
            createdResult.Value.Should().BeEquivalentTo(createdAttribute);
            mockService.Verify(s => s.AddProductAttributeAsync(addDto), Times.Once);
        }

        [Fact]
        public async Task AddProductAttribute_WhenServiceThrowsArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var addDto = new AddProductAttributeDto
            {
                ProductID = 999,
                AttributeName = "Color",
                AttributeValue = "Green"
            };

            var exceptionMessage = $"Product with ID {addDto.ProductID} does not exist.";

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.AddProductAttributeAsync(It.IsAny<AddProductAttributeDto>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            var controller = CreateController(mockService);

            // Act
            var result = await controller.AddProductAttribute(addDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult!.Value.Should().BeEquivalentTo(new { Message = exceptionMessage });
            mockService.Verify(s => s.AddProductAttributeAsync(addDto), Times.Once);
        }

        [Fact]
        public async Task AddProductAttribute_WithMultipleAttributes_ReturnsCreatedAtActionForEach()
        {
            // Arrange
            var addDto1 = new AddProductAttributeDto { ProductID = 1, AttributeName = "Color", AttributeValue = "Red" };
            var addDto2 = new AddProductAttributeDto { ProductID = 1, AttributeName = "Size", AttributeValue = "Small" };

            var createdAttribute1 = new ProductAttribute
            {
                AttributeID = 1,
                ProductID = addDto1.ProductID,
                AttributeName = addDto1.AttributeName,
                AttributeValue = addDto1.AttributeValue
            };

            var createdAttribute2 = new ProductAttribute
            {
                AttributeID = 2,
                ProductID = addDto2.ProductID,
                AttributeName = addDto2.AttributeName,
                AttributeValue = addDto2.AttributeValue
            };

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.AddProductAttributeAsync(addDto1))
                .ReturnsAsync(createdAttribute1);

            mockService
                .Setup(s => s.AddProductAttributeAsync(addDto2))
                .ReturnsAsync(createdAttribute2);

            var controller = CreateController(mockService);

            // Act
            var result1 = await controller.AddProductAttribute(addDto1);
            var result2 = await controller.AddProductAttribute(addDto2);

            // Assert
            result1.Should().BeOfType<CreatedAtActionResult>();
            result2.Should().BeOfType<CreatedAtActionResult>();

            var createdResult1 = result1 as CreatedAtActionResult;
            var createdResult2 = result2 as CreatedAtActionResult;

            createdResult1!.RouteValues!["id"].Should().Be(1);
            createdResult2!.RouteValues!["id"].Should().Be(2);

            mockService.Verify(s => s.AddProductAttributeAsync(addDto1), Times.Once);
            mockService.Verify(s => s.AddProductAttributeAsync(addDto2), Times.Once);
        }

        #endregion

        #region UpdateProductAttribute Tests

        [Fact]
        public async Task UpdateProductAttribute_WithValidData_ReturnsNoContent()
        {
            // Arrange
            var attributeId = 1;
            var updateDto = new UpdateProductAttributeDto
            {
                AttributeName = "UpdatedColor",
                AttributeValue = "Yellow"
            };

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.UpdateProductAttributeAsync(attributeId, updateDto))
                .ReturnsAsync(true);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.UpdateProductAttribute(attributeId, updateDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            mockService.Verify(s => s.UpdateProductAttributeAsync(attributeId, updateDto), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAttribute_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var attributeId = 999;
            var updateDto = new UpdateProductAttributeDto
            {
                AttributeName = "Color",
                AttributeValue = "Purple"
            };

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.UpdateProductAttributeAsync(attributeId, updateDto))
                .ReturnsAsync(false);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.UpdateProductAttribute(attributeId, updateDto);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult!.Value.Should().BeEquivalentTo(new { Message = $"Product Attribute with ID {attributeId} not found." });
            mockService.Verify(s => s.UpdateProductAttributeAsync(attributeId, updateDto), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task UpdateProductAttribute_WithInvalidId_ReturnsNotFound(int invalidId)
        {
            // Arrange
            var updateDto = new UpdateProductAttributeDto
            {
                AttributeName = "Size",
                AttributeValue = "Medium"
            };

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.UpdateProductAttributeAsync(invalidId, updateDto))
                .ReturnsAsync(false);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.UpdateProductAttribute(invalidId, updateDto);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UpdateProductAttribute_UpdatesAttributeName_ReturnsNoContent()
        {
            // Arrange
            var attributeId = 1;
            var updateDto = new UpdateProductAttributeDto
            {
                AttributeName = "NewName",
                AttributeValue = "Value"
            };

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.UpdateProductAttributeAsync(attributeId, updateDto))
                .ReturnsAsync(true);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.UpdateProductAttribute(attributeId, updateDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateProductAttribute_UpdatesAttributeValue_ReturnsNoContent()
        {
            // Arrange
            var attributeId = 1;
            var updateDto = new UpdateProductAttributeDto
            {
                AttributeName = "Name",
                AttributeValue = "NewValue"
            };

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.UpdateProductAttributeAsync(attributeId, updateDto))
                .ReturnsAsync(true);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.UpdateProductAttribute(attributeId, updateDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        #endregion

        #region DeleteProductAttribute Tests

        [Fact]
        public async Task DeleteProductAttribute_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var attributeId = 1;

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.DeleteProductAttributeAsync(attributeId))
                .ReturnsAsync(true);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.DeleteProductAttribute(attributeId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            mockService.Verify(s => s.DeleteProductAttributeAsync(attributeId), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAttribute_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var attributeId = 999;

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.DeleteProductAttributeAsync(attributeId))
                .ReturnsAsync(false);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.DeleteProductAttribute(attributeId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult!.Value.Should().BeEquivalentTo(new { Message = $"Product Attribute with ID {attributeId} not found." });
            mockService.Verify(s => s.DeleteProductAttributeAsync(attributeId), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteProductAttribute_WithInvalidId_ReturnsNotFound(int invalidId)
        {
            // Arrange
            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.DeleteProductAttributeAsync(invalidId))
                .ReturnsAsync(false);

            var controller = CreateController(mockService);

            // Act
            var result = await controller.DeleteProductAttribute(invalidId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task DeleteProductAttribute_AfterDeletion_ReturnsNoContent()
        {
            // Arrange
            var attributeId = 1;

            var mockService = new Mock<IProductAttributeService>();
            mockService
                .Setup(s => s.DeleteProductAttributeAsync(attributeId))
                .ReturnsAsync(true);

            var controller = CreateController(mockService);

            // Act - Delete the attribute
            var result = await controller.DeleteProductAttribute(attributeId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        #endregion
    }
}
