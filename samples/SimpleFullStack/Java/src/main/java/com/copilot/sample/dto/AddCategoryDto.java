package com.copilot.sample.dto;

import jakarta.validation.constraints.NotBlank;

public class AddCategoryDto {
    
    @NotBlank(message = "Name is required")
    private String name;
    
    private String description;
    private Integer parentCategoryId;
    
    // Constructors
    public AddCategoryDto() {}
    
    public AddCategoryDto(String name, String description, Integer parentCategoryId) {
        this.name = name;
        this.description = description;
        this.parentCategoryId = parentCategoryId;
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
    
    public Integer getParentCategoryId() {
        return parentCategoryId;
    }
    
    public void setParentCategoryId(Integer parentCategoryId) {
        this.parentCategoryId = parentCategoryId;
    }
}
