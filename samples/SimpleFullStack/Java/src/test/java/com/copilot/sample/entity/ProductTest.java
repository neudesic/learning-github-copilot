package com.copilot.sample.entity;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.DisplayName;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

@DisplayName("Product Entity Tests")
class ProductTest {

    private Product product;

    @BeforeEach
    void setUp() {
        product = new Product();
    }

    @Test
    @DisplayName("Should create Product with default constructor")
    void shouldCreateProductWithDefaultConstructor() {
        // When
        Product newProduct = new Product();

        // Then
        assertThat(newProduct).isNotNull();
        assertThat(newProduct.getProductId()).isNull();
        assertThat(newProduct.getName()).isNull();
        assertThat(newProduct.getDescription()).isNull();
        assertThat(newProduct.getSku()).isNull();
        assertThat(newProduct.getCategoryId()).isNull();
        assertThat(newProduct.getBrand()).isNull();
        assertThat(newProduct.getPrice()).isNull();
        assertThat(newProduct.getIsActive()).isTrue(); // Default value
        assertThat(newProduct.getCreatedAt()).isNull();
        assertThat(newProduct.getUpdatedAt()).isNull();
    }

    @Test
    @DisplayName("Should create Product with parameterized constructor")
    void shouldCreateProductWithParameterizedConstructor() {
        // Given
        String name = "iPhone 15";
        String description = "Latest Apple smartphone";
        String sku = "IPHONE15-128GB";
        Integer categoryId = 1;
        String brand = "Apple";
        BigDecimal price = new BigDecimal("999.99");

        // When
        Product newProduct = new Product(name, description, sku, categoryId, brand, price);

        // Then
        assertThat(newProduct).isNotNull();
        assertThat(newProduct.getName()).isEqualTo(name);
        assertThat(newProduct.getDescription()).isEqualTo(description);
        assertThat(newProduct.getSku()).isEqualTo(sku);
        assertThat(newProduct.getCategoryId()).isEqualTo(categoryId);
        assertThat(newProduct.getBrand()).isEqualTo(brand);
        assertThat(newProduct.getPrice()).isEqualTo(price);
    }

    @Test
    @DisplayName("Should set and get productId correctly")
    void shouldSetAndGetProductId() {
        // Given
        Integer productId = 123;

        // When
        product.setProductId(productId);

        // Then
        assertThat(product.getProductId()).isEqualTo(productId);
    }

    @Test
    @DisplayName("Should set and get name correctly")
    void shouldSetAndGetName() {
        // Given
        String name = "Samsung Galaxy S24";

        // When
        product.setName(name);

        // Then
        assertThat(product.getName()).isEqualTo(name);
    }

    @Test
    @DisplayName("Should set and get description correctly")
    void shouldSetAndGetDescription() {
        // Given
        String description = "Premium Android smartphone with advanced camera";

        // When
        product.setDescription(description);

        // Then
        assertThat(product.getDescription()).isEqualTo(description);
    }

    @Test
    @DisplayName("Should set and get SKU correctly")
    void shouldSetAndGetSku() {
        // Given
        String sku = "GALAXY-S24-256GB";

        // When
        product.setSku(sku);

        // Then
        assertThat(product.getSku()).isEqualTo(sku);
    }

    @Test
    @DisplayName("Should set and get categoryId correctly")
    void shouldSetAndGetCategoryId() {
        // Given
        Integer categoryId = 456;

        // When
        product.setCategoryId(categoryId);

        // Then
        assertThat(product.getCategoryId()).isEqualTo(categoryId);
    }

    @Test
    @DisplayName("Should set and get brand correctly")
    void shouldSetAndGetBrand() {
        // Given
        String brand = "Samsung";

        // When
        product.setBrand(brand);

        // Then
        assertThat(product.getBrand()).isEqualTo(brand);
    }

    @Test
    @DisplayName("Should set and get price correctly")
    void shouldSetAndGetPrice() {
        // Given
        BigDecimal price = new BigDecimal("1199.99");

        // When
        product.setPrice(price);

        // Then
        assertThat(product.getPrice()).isEqualTo(price);
    }

    @Test
    @DisplayName("Should set and get isActive correctly")
    void shouldSetAndGetIsActive() {
        // Given
        Boolean isActive = false;

        // When
        product.setIsActive(isActive);

        // Then
        assertThat(product.getIsActive()).isEqualTo(isActive);
    }

    @Test
    @DisplayName("Should set and get createdAt correctly")
    void shouldSetAndGetCreatedAt() {
        // Given
        LocalDateTime createdAt = LocalDateTime.now();

        // When
        product.setCreatedAt(createdAt);

        // Then
        assertThat(product.getCreatedAt()).isEqualTo(createdAt);
    }

    @Test
    @DisplayName("Should set and get updatedAt correctly")
    void shouldSetAndGetUpdatedAt() {
        // Given
        LocalDateTime updatedAt = LocalDateTime.now();

        // When
        product.setUpdatedAt(updatedAt);

        // Then
        assertThat(product.getUpdatedAt()).isEqualTo(updatedAt);
    }

    @Test
    @DisplayName("Should set and get category correctly")
    void shouldSetAndGetCategory() {
        // Given
        Category category = new Category("Electronics", "Electronic devices", null);
        category.setCategoryId(1);

        // When
        product.setCategory(category);

        // Then
        assertThat(product.getCategory()).isEqualTo(category);
        assertThat(product.getCategory().getName()).isEqualTo("Electronics");
    }

    @Test
    @DisplayName("Should set and get attributes correctly")
    void shouldSetAndGetAttributes() {
        // Given
        List<ProductAttribute> attributes = new ArrayList<>();
        ProductAttribute attribute = new ProductAttribute(1, "Color", "Black");
        attributes.add(attribute);

        // When
        product.setAttributes(attributes);

        // Then
        assertThat(product.getAttributes()).isEqualTo(attributes);
        assertThat(product.getAttributes()).hasSize(1);
        assertThat(product.getAttributes().get(0).getAttributeName()).isEqualTo("Color");
    }

    @Test
    @DisplayName("Should handle null values gracefully")
    void shouldHandleNullValues() {
        // When
        product.setProductId(null);
        product.setName(null);
        product.setDescription(null);
        product.setSku(null);
        product.setCategoryId(null);
        product.setBrand(null);
        product.setPrice(null);
        product.setIsActive(null);
        product.setCreatedAt(null);
        product.setUpdatedAt(null);
        product.setCategory(null);
        product.setAttributes(null);

        // Then
        assertThat(product.getProductId()).isNull();
        assertThat(product.getName()).isNull();
        assertThat(product.getDescription()).isNull();
        assertThat(product.getSku()).isNull();
        assertThat(product.getCategoryId()).isNull();
        assertThat(product.getBrand()).isNull();
        assertThat(product.getPrice()).isNull();
        assertThat(product.getIsActive()).isNull();
        assertThat(product.getCreatedAt()).isNull();
        assertThat(product.getUpdatedAt()).isNull();
        assertThat(product.getCategory()).isNull();
        assertThat(product.getAttributes()).isNull();
    }

    @Test
    @DisplayName("Should handle empty collections")
    void shouldHandleEmptyCollections() {
        // Given
        List<ProductAttribute> emptyAttributes = new ArrayList<>();

        // When
        product.setAttributes(emptyAttributes);

        // Then
        assertThat(product.getAttributes()).isEmpty();
    }

    @Test
    @DisplayName("Should work with decimal prices")
    void shouldWorkWithDecimalPrices() {
        // Given
        BigDecimal price1 = new BigDecimal("0.99");
        BigDecimal price2 = new BigDecimal("999999.99");

        // When & Then
        product.setPrice(price1);
        assertThat(product.getPrice()).isEqualTo(price1);

        product.setPrice(price2);
        assertThat(product.getPrice()).isEqualTo(price2);
    }
}
