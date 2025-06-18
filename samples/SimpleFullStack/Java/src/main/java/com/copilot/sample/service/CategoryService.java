package com.copilot.sample.service;

import com.copilot.sample.dto.AddCategoryDto;
import com.copilot.sample.dto.CategoryDto;

import java.util.List;

public interface CategoryService {
    
    List<CategoryDto> getCategoriesAsync();
    
    CategoryDto getCategoryByIdAsync(Integer id);
      CategoryDto addCategoryAsync(AddCategoryDto addCategoryDto);
    
    boolean deleteCategoryAsync(Integer id);
    
    // Methods called by controllers
    List<CategoryDto> getAllCategories();
    CategoryDto getCategoryById(Integer id);
    CategoryDto createCategory(AddCategoryDto addCategoryDto);
    CategoryDto updateCategory(Integer id, AddCategoryDto addCategoryDto);
    boolean deleteCategory(Integer id);
}
