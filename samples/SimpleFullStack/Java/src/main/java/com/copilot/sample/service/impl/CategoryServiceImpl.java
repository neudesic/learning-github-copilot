package com.copilot.sample.service.impl;

import com.copilot.sample.dto.AddCategoryDto;
import com.copilot.sample.dto.CategoryDto;
import com.copilot.sample.entity.Category;
import com.copilot.sample.mapper.CategoryMapper;
import com.copilot.sample.repository.CategoryRepository;
import com.copilot.sample.service.CategoryService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Optional;

/**
 * Service for managing categories in the application.
 * Provides methods for CRUD operations on categories.
 */
@Service
@Transactional
public class CategoryServiceImpl implements CategoryService {
    
    private final CategoryRepository categoryRepository;
    
    @Autowired
    public CategoryServiceImpl(CategoryRepository categoryRepository) {
        this.categoryRepository = categoryRepository;
    }
    
    @Override
    @Transactional(readOnly = true)
    public List<CategoryDto> getCategoriesAsync() {
        List<Category> categories = categoryRepository.findAllWithSubCategories();
        return CategoryMapper.mapToDtoList(categories);
    }
    
    @Override
    @Transactional(readOnly = true)
    public CategoryDto getCategoryByIdAsync(Integer id) {
        Optional<Category> category = categoryRepository.findByIdWithSubCategories(id);
        return category.map(CategoryMapper::mapToDtoWithSubCategories).orElse(null);
    }
    
    @Override
    public CategoryDto addCategoryAsync(AddCategoryDto addCategoryDto) {
        // Check if category with the same name already exists
        if (categoryRepository.existsByName(addCategoryDto.getName())) {
            return null; // Return null to indicate conflict
        }
        
        Category category = new Category();
        category.setName(addCategoryDto.getName());
        category.setDescription(addCategoryDto.getDescription());
        category.setParentCategoryId(addCategoryDto.getParentCategoryId());
        
        Category savedCategory = categoryRepository.save(category);
        return CategoryMapper.mapToDto(savedCategory);
    }    
    @Override
    public boolean deleteCategoryAsync(Integer id) {
        if (!categoryRepository.existsById(id)) {
            return false;
        }

        categoryRepository.deleteById(id);
        return true;
    }
    
    // Controller method implementations
    @Override
    public List<CategoryDto> getAllCategories() {
        return getCategoriesAsync();
    }
    
    @Override
    public CategoryDto getCategoryById(Integer id) {
        return getCategoryByIdAsync(id);
    }
    
    @Override
    public CategoryDto createCategory(AddCategoryDto addCategoryDto) {
        return addCategoryAsync(addCategoryDto);
    }
    
    @Override
    public CategoryDto updateCategory(Integer id, AddCategoryDto addCategoryDto) {
        Optional<Category> optionalCategory = categoryRepository.findById(id);
        
        if (optionalCategory.isEmpty()) {
            return null;
        }
        
        Category category = optionalCategory.get();
        category.setName(addCategoryDto.getName());
        category.setDescription(addCategoryDto.getDescription());
        category.setParentCategoryId(addCategoryDto.getParentCategoryId());
        
        Category savedCategory = categoryRepository.save(category);
        return CategoryMapper.mapToDto(savedCategory);
    }
    
    @Override
    public boolean deleteCategory(Integer id) {
        return deleteCategoryAsync(id);
    }
}
