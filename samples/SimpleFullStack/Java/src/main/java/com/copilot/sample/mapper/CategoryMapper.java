package com.copilot.sample.mapper;

import com.copilot.sample.dto.CategoryDto;
import com.copilot.sample.entity.Category;
import org.springframework.stereotype.Component;

import java.util.List;
import java.util.stream.Collectors;

@Component
public class CategoryMapper {
    
    public static CategoryDto mapToDto(Category category) {
        if (category == null) {
            return null;
        }
        
        CategoryDto dto = new CategoryDto();
        dto.setCategoryId(category.getCategoryId());
        dto.setName(category.getName());
        dto.setDescription(category.getDescription());
        dto.setParentCategoryId(category.getParentCategoryId());
        
        return dto;
    }
    
    public static CategoryDto mapToDtoWithSubCategories(Category category) {
        if (category == null) {
            return null;
        }
        
        CategoryDto dto = mapToDto(category);
        
        if (category.getSubCategories() != null && !category.getSubCategories().isEmpty()) {
            List<CategoryDto> subCategoryDtos = category.getSubCategories().stream()
                    .map(CategoryMapper::mapToDto)
                    .collect(Collectors.toList());
            dto.setSubCategories(subCategoryDtos);
        }
        
        return dto;
    }
    
    public static Category mapToEntity(CategoryDto dto) {
        if (dto == null) {
            return null;
        }
        
        Category category = new Category();
        category.setCategoryId(dto.getCategoryId());
        category.setName(dto.getName());
        category.setDescription(dto.getDescription());
        category.setParentCategoryId(dto.getParentCategoryId());
        
        return category;
    }
    
    public static List<CategoryDto> mapToDtoList(List<Category> categories) {
        if (categories == null) {
            return null;
        }
        
        return categories.stream()
                .map(CategoryMapper::mapToDtoWithSubCategories)
                .collect(Collectors.toList());
    }
}
