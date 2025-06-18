package com.copilot.sample.dto;

import jakarta.validation.constraints.NotBlank;
import java.util.List;

public class CategoryDto {
    private Integer categoryId;
    
    @NotBlank(message = "Name is required")
    private String name;
    
    private String description;
    private Integer parentCategoryId;
    private List<CategoryDto> subCategories;
    
    // Constructors
    public CategoryDto() {}
    
    public CategoryDto(Integer categoryId, String name, String description, Integer parentCategoryId) {
        this.categoryId = categoryId;
        this.name = name;
        this.description = description;
        this.parentCategoryId = parentCategoryId;
    }
    
    // Getters and Setters
    public Integer getCategoryId() {
        return categoryId;
    }
    
    public void setCategoryId(Integer categoryId) {
        this.categoryId = categoryId;
    }
    
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
    
    public Integer getParentCategoryId() {
        return parentCategoryId;
    }
    
    public void setParentCategoryId(Integer parentCategoryId) {
        this.parentCategoryId = parentCategoryId;
    }
    
    public List<CategoryDto> getSubCategories() {
        return subCategories;
    }
    
    public void setSubCategories(List<CategoryDto> subCategories) {
        this.subCategories = subCategories;
    }
}