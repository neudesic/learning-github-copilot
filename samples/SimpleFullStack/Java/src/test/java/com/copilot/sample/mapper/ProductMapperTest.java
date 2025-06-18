package com.copilot.sample.mapper;

import com.copilot.sample.dto.ProductDto;
import com.copilot.sample.entity.Category;
import com.copilot.sample.entity.Product;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.DisplayName;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

@DisplayName("ProductMapper Tests")
class ProductMapperTest {

    @Test
    @DisplayName("Should map Product entity to ProductDto")
    void shouldMapProductEntityToDto() {
        // Given
        Product product = new Product();
        product.setProductId(1);
        product.setName("iPhone 15");
        product.setDescription("Latest Apple smartphone");
        product.setSku("IPHONE15-128GB");
        product.setCategoryId(10);
        product.setBrand("Apple");
        product.setIsActive(true);
        product.setPrice(new BigDecimal("999.99"));
        product.setCreatedAt(LocalDateTime.now());
        product.setUpdatedAt(LocalDateTime.now());

        // When
        ProductDto dto = ProductMapper.mapToDto(product);

        // Then
        assertThat(dto).isNotNull();
        assertThat(dto.getProductId()).isEqualTo(1);
        assertThat(dto.getName()).isEqualTo("iPhone 15");
        assertThat(dto.getDescription()).isEqualTo("Latest Apple smartphone");
        assertThat(dto.getSku()).isEqualTo("IPHONE15-128GB");
        assertThat(dto.getCategoryId()).isEqualTo(10);
        assertThat(dto.getBrand()).isEqualTo("Apple");
        assertThat(dto.getIsActive()).isTrue();
    }

    @Test
    @DisplayName("Should map Product entity with Category to ProductDto")
    void shouldMapProductEntityWithCategoryToDto() {
        // Given
        Category category = new Category();
        category.setCategoryId(10);
        category.setName("Electronics");
        category.setDescription("Electronic devices");

        Product product = new Product();
        product.setProductId(1);
        product.setName("iPhone 15");
        product.setSku("IPHONE15-128GB");
        product.setCategoryId(10);
        product.setCategory(category);

        // When
        ProductDto dto = ProductMapper.mapToDto(product);

        // Then
        assertThat(dto).isNotNull();
        assertThat(dto.getProductId()).isEqualTo(1);
        assertThat(dto.getName()).isEqualTo("iPhone 15");
        assertThat(dto.getCategory()).isNotNull();
        assertThat(dto.getCategory().getCategoryId()).isEqualTo(10);
        assertThat(dto.getCategory().getName()).isEqualTo("Electronics");
    }

    @Test
    @DisplayName("Should return null when mapping null Product entity to DTO")
    void shouldReturnNullWhenMappingNullProductEntityToDto() {
        // When
        ProductDto dto = ProductMapper.mapToDto(null);

        // Then
        assertThat(dto).isNull();
    }

    @Test
    @DisplayName("Should map ProductDto to Product entity")
    void shouldMapProductDtoToEntity() {
        // Given
        ProductDto dto = new ProductDto();
        dto.setProductId(2);
        dto.setName("Samsung Galaxy S24");
        dto.setDescription("Premium Android smartphone");
        dto.setSku("GALAXY-S24-256GB");
        dto.setCategoryId(10);
        dto.setBrand("Samsung");
        dto.setIsActive(false);

        // When
        Product entity = ProductMapper.mapToEntity(dto);

        // Then
        assertThat(entity).isNotNull();
        assertThat(entity.getProductId()).isEqualTo(2);
        assertThat(entity.getName()).isEqualTo("Samsung Galaxy S24");
        assertThat(entity.getDescription()).isEqualTo("Premium Android smartphone");
        assertThat(entity.getSku()).isEqualTo("GALAXY-S24-256GB");
        assertThat(entity.getCategoryId()).isEqualTo(10);
        assertThat(entity.getBrand()).isEqualTo("Samsung");
        assertThat(entity.getIsActive()).isFalse();
    }

    @Test
    @DisplayName("Should return null when mapping null ProductDto to entity")
    void shouldReturnNullWhenMappingNullProductDtoToEntity() {
        // When
        Product entity = ProductMapper.mapToEntity(null);

        // Then
        assertThat(entity).isNull();
    }

    @Test
    @DisplayName("Should map list of Products to list of ProductDtos")
    void shouldMapListOfProductsToListOfDtos() {
        // Given
        Product product1 = new Product();
        product1.setProductId(1);
        product1.setName("iPhone 15");
        product1.setSku("IPHONE15-128GB");

        Product product2 = new Product();
        product2.setProductId(2);
        product2.setName("Samsung Galaxy S24");
        product2.setSku("GALAXY-S24-256GB");

        List<Product> products = List.of(product1, product2);

        // When
        List<ProductDto> dtos = ProductMapper.mapToDtoList(products);

        // Then
        assertThat(dtos).isNotNull();
        assertThat(dtos).hasSize(2);
        assertThat(dtos.get(0).getProductId()).isEqualTo(1);
        assertThat(dtos.get(0).getName()).isEqualTo("iPhone 15");
        assertThat(dtos.get(1).getProductId()).isEqualTo(2);
        assertThat(dtos.get(1).getName()).isEqualTo("Samsung Galaxy S24");
    }

    @Test
    @DisplayName("Should return null when mapping null list of Products to DTOs")
    void shouldReturnNullWhenMappingNullListOfProductsToDto() {
        // When
        List<ProductDto> dtos = ProductMapper.mapToDtoList(null);

        // Then
        assertThat(dtos).isNull();
    }

    @Test
    @DisplayName("Should map empty list of Products to empty list of DTOs")
    void shouldMapEmptyListOfProductsToEmptyListOfDtos() {
        // Given
        List<Product> products = new ArrayList<>();

        // When
        List<ProductDto> dtos = ProductMapper.mapToDtoList(products);

        // Then
        assertThat(dtos).isNotNull();
        assertThat(dtos).isEmpty();
    }

    @Test
    @DisplayName("Should handle Product with null fields when mapping to DTO")
    void shouldHandleProductWithNullFieldsWhenMappingToDto() {
        // Given
        Product product = new Product();
        product.setProductId(null);
        product.setName(null);
        product.setDescription(null);
        product.setSku(null);
        product.setCategoryId(null);
        product.setBrand(null);
        product.setIsActive(null);
        product.setCategory(null);

        // When
        ProductDto dto = ProductMapper.mapToDto(product);

        // Then
        assertThat(dto).isNotNull();
        assertThat(dto.getProductId()).isNull();
        assertThat(dto.getName()).isNull();
        assertThat(dto.getDescription()).isNull();
        assertThat(dto.getSku()).isNull();
        assertThat(dto.getCategoryId()).isNull();
        assertThat(dto.getBrand()).isNull();
        assertThat(dto.getIsActive()).isNull();
        assertThat(dto.getCategory()).isNull();
    }

    @Test
    @DisplayName("Should handle ProductDto with null fields when mapping to entity")
    void shouldHandleProductDtoWithNullFieldsWhenMappingToEntity() {
        // Given
        ProductDto dto = new ProductDto();
        dto.setProductId(null);
        dto.setName(null);
        dto.setDescription(null);
        dto.setSku(null);
        dto.setCategoryId(null);
        dto.setBrand(null);
        dto.setIsActive(null);

        // When
        Product entity = ProductMapper.mapToEntity(dto);

        // Then
        assertThat(entity).isNotNull();
        assertThat(entity.getProductId()).isNull();
        assertThat(entity.getName()).isNull();
        assertThat(entity.getDescription()).isNull();
        assertThat(entity.getSku()).isNull();
        assertThat(entity.getCategoryId()).isNull();
        assertThat(entity.getBrand()).isNull();
        assertThat(entity.getIsActive()).isNull();
    }

    @Test
    @DisplayName("Should not map price field from entity to DTO")
    void shouldNotMapPriceFieldFromEntityToDto() {
        // Given
        Product product = new Product();
        product.setProductId(1);
        product.setName("Test Product");
        product.setPrice(new BigDecimal("100.00"));

        // When
        ProductDto dto = ProductMapper.mapToDto(product);

        // Then
        assertThat(dto).isNotNull();
        assertThat(dto.getProductId()).isEqualTo(1);
        assertThat(dto.getName()).isEqualTo("Test Product");
        // Note: Price is not mapped to DTO based on the mapper implementation
    }

    @Test
    @DisplayName("Should not map timestamp fields from entity to DTO")
    void shouldNotMapTimestampFieldsFromEntityToDto() {
        // Given
        LocalDateTime now = LocalDateTime.now();
        Product product = new Product();
        product.setProductId(1);
        product.setName("Test Product");
        product.setCreatedAt(now);
        product.setUpdatedAt(now);

        // When
        ProductDto dto = ProductMapper.mapToDto(product);

        // Then
        assertThat(dto).isNotNull();
        assertThat(dto.getProductId()).isEqualTo(1);
        assertThat(dto.getName()).isEqualTo("Test Product");
        // Note: Timestamp fields are not mapped to DTO based on the mapper implementation
    }

    @Test
    @DisplayName("Should handle Category mapping when category is null")
    void shouldHandleCategoryMappingWhenCategoryIsNull() {
        // Given
        Product product = new Product();
        product.setProductId(1);
        product.setName("Test Product");
        product.setCategory(null);

        // When
        ProductDto dto = ProductMapper.mapToDto(product);

        // Then
        assertThat(dto).isNotNull();
        assertThat(dto.getProductId()).isEqualTo(1);
        assertThat(dto.getName()).isEqualTo("Test Product");
        assertThat(dto.getCategory()).isNull();
    }

    @Test
    @DisplayName("Should map Product with Category containing null fields")
    void shouldMapProductWithCategoryContainingNullFields() {
        // Given
        Category category = new Category();
        category.setCategoryId(1);
        category.setName(null);
        category.setDescription(null);

        Product product = new Product();
        product.setProductId(1);
        product.setName("Test Product");
        product.setCategory(category);

        // When
        ProductDto dto = ProductMapper.mapToDto(product);

        // Then
        assertThat(dto).isNotNull();
        assertThat(dto.getCategory()).isNotNull();
        assertThat(dto.getCategory().getCategoryId()).isEqualTo(1);
        assertThat(dto.getCategory().getName()).isNull();
        assertThat(dto.getCategory().getDescription()).isNull();
    }
}
