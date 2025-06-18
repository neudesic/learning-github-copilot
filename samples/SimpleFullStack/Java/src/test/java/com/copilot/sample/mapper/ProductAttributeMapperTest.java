package com.copilot.sample.mapper;

import com.copilot.sample.entity.Product;
import com.copilot.sample.entity.ProductAttribute;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.DisplayName;

import java.util.ArrayList;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

@DisplayName("ProductAttributeMapper Tests")
class ProductAttributeMapperTest {

    @Test
    @DisplayName("Should validate ProductAttribute entity relationships")
    void shouldValidateProductAttributeEntityRelationships() {
        // Given
        Product product = new Product();
        product.setProductId(1);
        product.setName("Test Product");

        ProductAttribute attribute1 = new ProductAttribute(1, "Color", "Red");
        attribute1.setAttributeId(1);
        attribute1.setProduct(product);

        ProductAttribute attribute2 = new ProductAttribute(1, "Size", "Large");
        attribute2.setAttributeId(2);
        attribute2.setProduct(product);

        List<ProductAttribute> attributes = List.of(attribute1, attribute2);
        product.setAttributes(attributes);

        // When & Then
        assertThat(attribute1.getProduct()).isEqualTo(product);
        assertThat(attribute2.getProduct()).isEqualTo(product);
        assertThat(product.getAttributes()).hasSize(2);
        assertThat(product.getAttributes()).contains(attribute1, attribute2);
    }

    @Test
    @DisplayName("Should handle bidirectional relationships correctly")
    void shouldHandleBidirectionalRelationshipsCorrectly() {
        // Given
        Product product = new Product();
        product.setProductId(1);
        product.setName("Test Product");

        ProductAttribute attribute = new ProductAttribute();
        attribute.setAttributeId(1);
        attribute.setProductId(1);
        attribute.setAttributeName("Weight");
        attribute.setAttributeValue("2.5 kg");
        attribute.setProduct(product);

        List<ProductAttribute> attributes = new ArrayList<>();
        attributes.add(attribute);
        product.setAttributes(attributes);

        // When & Then
        // Verify the bidirectional relationship
        assertThat(attribute.getProduct()).isSameAs(product);
        assertThat(product.getAttributes().get(0)).isSameAs(attribute);
        assertThat(attribute.getProductId()).isEqualTo(product.getProductId());
    }

    @Test
    @DisplayName("Should validate attribute constraints and data types")
    void shouldValidateAttributeConstraintsAndDataTypes() {
        // Given
        ProductAttribute attribute = new ProductAttribute();

        // Test various data types as string values
        attribute.setAttributeValue("123"); // Numeric as string
        assertThat(attribute.getAttributeValue()).isEqualTo("123");

        attribute.setAttributeValue("true"); // Boolean as string
        assertThat(attribute.getAttributeValue()).isEqualTo("true");

        attribute.setAttributeValue("2024-01-01"); // Date as string
        assertThat(attribute.getAttributeValue()).isEqualTo("2024-01-01");

        attribute.setAttributeValue("JSON:{\"key\":\"value\"}"); // JSON as string
        assertThat(attribute.getAttributeValue()).isEqualTo("JSON:{\"key\":\"value\"}");
    }
}
