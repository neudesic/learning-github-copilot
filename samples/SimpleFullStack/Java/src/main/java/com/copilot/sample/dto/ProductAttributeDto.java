package com.copilot.sample.dto;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

public class ProductAttributeDto {
    private Integer attributeId;
    
    @NotNull(message = "Product ID is required")
    private Integer productId;
    
    @NotBlank(message = "Attribute name is required")
    private String attributeName;
    
    @NotBlank(message = "Attribute value is required")
    private String attributeValue;
    
    // Constructors
    public ProductAttributeDto() {}
    
    public ProductAttributeDto(Integer attributeId, Integer productId, String attributeName, String attributeValue) {
        this.attributeId = attributeId;
        this.productId = productId;
        this.attributeName = attributeName;
        this.attributeValue = attributeValue;
    }
    
    // Getters and Setters
    public Integer getAttributeId() {
        return attributeId;
    }
    
    public void setAttributeId(Integer attributeId) {
        this.attributeId = attributeId;
    }
    
    public Integer getProductId() {
        return productId;
    }
    
    public void setProductId(Integer productId) {
        this.productId = productId;
    }
    
    public String getAttributeName() {
        return attributeName;
    }
    
    public void setAttributeName(String attributeName) {
        this.attributeName = attributeName;
    }
    
    public String getAttributeValue() {
        return attributeValue;
    }
    
    public void setAttributeValue(String attributeValue) {
        this.attributeValue = attributeValue;
    }
}