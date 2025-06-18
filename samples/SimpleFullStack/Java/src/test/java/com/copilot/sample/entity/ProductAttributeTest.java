package com.copilot.sample.entity;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.DisplayName;

import static org.assertj.core.api.Assertions.assertThat;

@DisplayName("ProductAttribute Entity Tests")
class ProductAttributeTest {

    private ProductAttribute productAttribute;

    @BeforeEach
    void setUp() {
        productAttribute = new ProductAttribute();
    }

    @Test
    @DisplayName("Should create ProductAttribute with default constructor")
    void shouldCreateProductAttributeWithDefaultConstructor() {
        // When
        ProductAttribute newAttribute = new ProductAttribute();

        // Then
        assertThat(newAttribute).isNotNull();
        assertThat(newAttribute.getAttributeId()).isNull();
        assertThat(newAttribute.getProductId()).isNull();
        assertThat(newAttribute.getAttributeName()).isNull();
        assertThat(newAttribute.getAttributeValue()).isNull();
        assertThat(newAttribute.getProduct()).isNull();
    }

    @Test
    @DisplayName("Should create ProductAttribute with parameterized constructor")
    void shouldCreateProductAttributeWithParameterizedConstructor() {
        // Given
        Integer productId = 1;
        String attributeName = "Color";
        String attributeValue = "Blue";

        // When
        ProductAttribute newAttribute = new ProductAttribute(productId, attributeName, attributeValue);

        // Then
        assertThat(newAttribute).isNotNull();
        assertThat(newAttribute.getProductId()).isEqualTo(productId);
        assertThat(newAttribute.getAttributeName()).isEqualTo(attributeName);
        assertThat(newAttribute.getAttributeValue()).isEqualTo(attributeValue);
        assertThat(newAttribute.getAttributeId()).isNull(); // Not set in constructor
    }

    @Test
    @DisplayName("Should set and get attributeId correctly")
    void shouldSetAndGetAttributeId() {
        // Given
        Integer attributeId = 123;

        // When
        productAttribute.setAttributeId(attributeId);

        // Then
        assertThat(productAttribute.getAttributeId()).isEqualTo(attributeId);
    }

    @Test
    @DisplayName("Should set and get productId correctly")
    void shouldSetAndGetProductId() {
        // Given
        Integer productId = 456;

        // When
        productAttribute.setProductId(productId);

        // Then
        assertThat(productAttribute.getProductId()).isEqualTo(productId);
    }

    @Test
    @DisplayName("Should set and get attributeName correctly")
    void shouldSetAndGetAttributeName() {
        // Given
        String attributeName = "Size";

        // When
        productAttribute.setAttributeName(attributeName);

        // Then
        assertThat(productAttribute.getAttributeName()).isEqualTo(attributeName);
    }

    @Test
    @DisplayName("Should set and get attributeValue correctly")
    void shouldSetAndGetAttributeValue() {
        // Given
        String attributeValue = "Large";

        // When
        productAttribute.setAttributeValue(attributeValue);

        // Then
        assertThat(productAttribute.getAttributeValue()).isEqualTo(attributeValue);
    }

    @Test
    @DisplayName("Should set and get product correctly")
    void shouldSetAndGetProduct() {
        // Given
        Product product = new Product();
        product.setProductId(1);
        product.setName("Test Product");

        // When
        productAttribute.setProduct(product);

        // Then
        assertThat(productAttribute.getProduct()).isEqualTo(product);
        assertThat(productAttribute.getProduct().getName()).isEqualTo("Test Product");
    }

    @Test
    @DisplayName("Should handle null values gracefully")
    void shouldHandleNullValues() {
        // When
        productAttribute.setAttributeId(null);
        productAttribute.setProductId(null);
        productAttribute.setAttributeName(null);
        productAttribute.setAttributeValue(null);
        productAttribute.setProduct(null);

        // Then
        assertThat(productAttribute.getAttributeId()).isNull();
        assertThat(productAttribute.getProductId()).isNull();
        assertThat(productAttribute.getAttributeName()).isNull();
        assertThat(productAttribute.getAttributeValue()).isNull();
        assertThat(productAttribute.getProduct()).isNull();
    }

    @Test
    @DisplayName("Should handle empty string values")
    void shouldHandleEmptyStringValues() {
        // Given
        String emptyAttributeName = "";
        String emptyAttributeValue = "";

        // When
        productAttribute.setAttributeName(emptyAttributeName);
        productAttribute.setAttributeValue(emptyAttributeValue);

        // Then
        assertThat(productAttribute.getAttributeName()).isEmpty();
        assertThat(productAttribute.getAttributeValue()).isEmpty();
    }

    @Test
    @DisplayName("Should handle long attribute values")
    void shouldHandleLongAttributeValues() {
        // Given
        String longAttributeName = "Very Long Attribute Name That Exceeds Normal Length";
        String longAttributeValue = "This is a very long attribute value that contains a lot of text and should be handled properly by the entity even though it might be quite lengthy and detailed";

        // When
        productAttribute.setAttributeName(longAttributeName);
        productAttribute.setAttributeValue(longAttributeValue);

        // Then
        assertThat(productAttribute.getAttributeName()).isEqualTo(longAttributeName);
        assertThat(productAttribute.getAttributeValue()).isEqualTo(longAttributeValue);
    }

    @Test
    @DisplayName("Should handle special characters in attribute values")
    void shouldHandleSpecialCharactersInAttributeValues() {
        // Given
        String specialAttributeName = "Special-Chars_Name!@#";
        String specialAttributeValue = "Value with special chars: !@#$%^&*()_+-=[]{}|;:'\",.<>?/";

        // When
        productAttribute.setAttributeName(specialAttributeName);
        productAttribute.setAttributeValue(specialAttributeValue);

        // Then
        assertThat(productAttribute.getAttributeName()).isEqualTo(specialAttributeName);
        assertThat(productAttribute.getAttributeValue()).isEqualTo(specialAttributeValue);
    }
}
