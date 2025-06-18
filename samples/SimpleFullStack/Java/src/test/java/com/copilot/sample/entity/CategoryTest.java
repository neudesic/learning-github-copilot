package com.copilot.sample.entity;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.DisplayName;

import java.util.ArrayList;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

@DisplayName("Category Entity Tests")
class CategoryTest {

    private Category category;

    @BeforeEach
    void setUp() {
        category = new Category();
    }

    @Test
    @DisplayName("Should create Category with default constructor")
    void shouldCreateCategoryWithDefaultConstructor() {
        // When
        Category newCategory = new Category();

        // Then
        assertThat(newCategory).isNotNull();
        assertThat(newCategory.getCategoryId()).isNull();
        assertThat(newCategory.getName()).isNull();
        assertThat(newCategory.getDescription()).isNull();
        assertThat(newCategory.getParentCategoryId()).isNull();
        assertThat(newCategory.getProducts()).isNull();
        assertThat(newCategory.getSubCategories()).isNull();
    }

    @Test
    @DisplayName("Should create Category with parameterized constructor")
    void shouldCreateCategoryWithParameterizedConstructor() {
        // Given
        String name = "Electronics";
        String description = "Electronic products and gadgets";
        Integer parentCategoryId = 1;

        // When
        Category newCategory = new Category(name, description, parentCategoryId);

        // Then
        assertThat(newCategory).isNotNull();
        assertThat(newCategory.getName()).isEqualTo(name);
        assertThat(newCategory.getDescription()).isEqualTo(description);
        assertThat(newCategory.getParentCategoryId()).isEqualTo(parentCategoryId);
        assertThat(newCategory.getCategoryId()).isNull(); // Not set in constructor
    }

    @Test
    @DisplayName("Should set and get categoryId correctly")
    void shouldSetAndGetCategoryId() {
        // Given
        Integer categoryId = 123;

        // When
        category.setCategoryId(categoryId);

        // Then
        assertThat(category.getCategoryId()).isEqualTo(categoryId);
    }

    @Test
    @DisplayName("Should set and get name correctly")
    void shouldSetAndGetName() {
        // Given
        String name = "Mobile Phones";

        // When
        category.setName(name);

        // Then
        assertThat(category.getName()).isEqualTo(name);
    }

    @Test
    @DisplayName("Should set and get description correctly")
    void shouldSetAndGetDescription() {
        // Given
        String description = "Latest smartphones and accessories";

        // When
        category.setDescription(description);

        // Then
        assertThat(category.getDescription()).isEqualTo(description);
    }

    @Test
    @DisplayName("Should set and get parentCategoryId correctly")
    void shouldSetAndGetParentCategoryId() {
        // Given
        Integer parentCategoryId = 456;

        // When
        category.setParentCategoryId(parentCategoryId);

        // Then
        assertThat(category.getParentCategoryId()).isEqualTo(parentCategoryId);
    }

    @Test
    @DisplayName("Should set and get products list correctly")
    void shouldSetAndGetProducts() {
        // Given
        List<Product> products = new ArrayList<>();
        Product product1 = new Product();
        product1.setName("iPhone");
        products.add(product1);

        // When
        category.setProducts(products);

        // Then
        assertThat(category.getProducts()).isEqualTo(products);
        assertThat(category.getProducts()).hasSize(1);
        assertThat(category.getProducts().get(0).getName()).isEqualTo("iPhone");
    }

    @Test
    @DisplayName("Should set and get subCategories list correctly")
    void shouldSetAndGetSubCategories() {
        // Given
        List<Category> subCategories = new ArrayList<>();
        Category subCategory = new Category("Smartphones", "Smart mobile devices", 1);
        subCategories.add(subCategory);

        // When
        category.setSubCategories(subCategories);

        // Then
        assertThat(category.getSubCategories()).isEqualTo(subCategories);
        assertThat(category.getSubCategories()).hasSize(1);
        assertThat(category.getSubCategories().get(0).getName()).isEqualTo("Smartphones");
    }

    @Test
    @DisplayName("Should handle null values gracefully")
    void shouldHandleNullValues() {
        // When
        category.setCategoryId(null);
        category.setName(null);
        category.setDescription(null);
        category.setParentCategoryId(null);
        category.setProducts(null);
        category.setSubCategories(null);

        // Then
        assertThat(category.getCategoryId()).isNull();
        assertThat(category.getName()).isNull();
        assertThat(category.getDescription()).isNull();
        assertThat(category.getParentCategoryId()).isNull();
        assertThat(category.getProducts()).isNull();
        assertThat(category.getSubCategories()).isNull();
    }

    @Test
    @DisplayName("Should handle empty collections")
    void shouldHandleEmptyCollections() {
        // Given
        List<Product> emptyProducts = new ArrayList<>();
        List<Category> emptySubCategories = new ArrayList<>();

        // When
        category.setProducts(emptyProducts);
        category.setSubCategories(emptySubCategories);

        // Then
        assertThat(category.getProducts()).isEmpty();
        assertThat(category.getSubCategories()).isEmpty();
    }
}
