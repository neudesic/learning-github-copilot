package com.copilot.sample.dto;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

public class AddProductDto {
    
    @NotBlank(message = "Name is required")
    private String name;
    
    private String description;
    
    @NotBlank(message = "SKU is required")
    private String sku;
    
    @NotNull(message = "Category ID is required")
    private Integer categoryId;
    
    private String brand;
    private Boolean isActive = true;
    
    // Constructors
    public AddProductDto() {}
    
    public AddProductDto(String name, String description, String sku, Integer categoryId, String brand, Boolean isActive) {
        this.name = name;
        this.description = description;
        this.sku = sku;
        this.categoryId = categoryId;
        this.brand = brand;
        this.isActive = isActive;
    }
    
    // Getters and Setters
    public String getName() {
        return name;
    }
    
    public void setName(String name) {
        this.name = name;
    }
    
    public String getDescription() {
        return description;
    }
    
    public void setDescription(String description) {
        this.description = description;
    }
    
    public String getSku() {
        return sku;
    }
    
    public void setSku(String sku) {
        this.sku = sku;
    }
    
    public Integer getCategoryId() {
        return categoryId;
    }
    
    public void setCategoryId(Integer categoryId) {
        this.categoryId = categoryId;
    }
    
    public String getBrand() {
        return brand;
    }
    
    public void setBrand(String brand) {
        this.brand = brand;
    }
    
    public Boolean getIsActive() {
        return isActive;
    }
    
    public void setIsActive(Boolean isActive) {
        this.isActive = isActive;
    }
}