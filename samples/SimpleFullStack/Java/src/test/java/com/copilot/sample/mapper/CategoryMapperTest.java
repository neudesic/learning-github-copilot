package com.copilot.sample.mapper;

import com.copilot.sample.dto.CategoryDto;
import com.copilot.sample.entity.Category;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.DisplayName;

import java.util.ArrayList;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

@DisplayName("CategoryMapper Tests")
class CategoryMapperTest {

    @Test
    @DisplayName("Should map Category entity to CategoryDto")
    void shouldMapCategoryEntityToDto() {
        // Given
        Category category = new Category();
        category.setCategoryId(1);
        category.setName("Electronics");
        category.setDescription("Electronic devices and gadgets");
        category.setParentCategoryId(null);

        // When
        CategoryDto dto = CategoryMapper.mapToDto(category);

        // Then
        assertThat(dto).isNotNull();
        assertThat(dto.getCategoryId()).isEqualTo(1);
        assertThat(dto.getName()).isEqualTo("Electronics");
        assertThat(dto.getDescription()).isEqualTo("Electronic devices and gadgets");
        assertThat(dto.getParentCategoryId()).isNull();
    }

    @Test
    @DisplayName("Should return null when mapping null Category entity to DTO")
    void shouldReturnNullWhenMappingNullCategoryEntityToDto() {
        // When
        CategoryDto dto = CategoryMapper.mapToDto(null);

        // Then
        assertThat(dto).isNull();
    }

    @Test
    @DisplayName("Should map CategoryDto to Category entity")
    void shouldMapCategoryDtoToEntity() {
        // Given
        CategoryDto dto = new CategoryDto();
        dto.setCategoryId(2);
        dto.setName("Mobile Phones");
        dto.setDescription("Smartphones and accessories");
        dto.setParentCategoryId(1);

        // When
        Category entity = CategoryMapper.mapToEntity(dto);

        // Then
        assertThat(entity).isNotNull();
        assertThat(entity.getCategoryId()).isEqualTo(2);
        assertThat(entity.getName()).isEqualTo("Mobile Phones");
        assertThat(entity.getDescription()).isEqualTo("Smartphones and accessories");
        assertThat(entity.getParentCategoryId()).isEqualTo(1);
    }

    @Test
    @DisplayName("Should return null when mapping null CategoryDto to entity")
    void shouldReturnNullWhenMappingNullCategoryDtoToEntity() {
        // When
        Category entity = CategoryMapper.mapToEntity(null);

        // Then
        assertThat(entity).isNull();
    }

    @Test
    @DisplayName("Should map Category with sub-categories to DTO")
    void shouldMapCategoryWithSubCategoriesToDto() {
        // Given
        Category parentCategory = new Category();
        parentCategory.setCategoryId(1);
        parentCategory.setName("Electronics");
        parentCategory.setDescription("Electronic devices");

        Category subCategory1 = new Category();
        subCategory1.setCategoryId(2);
        subCategory1.setName("Mobile Phones");
        subCategory1.setParentCategoryId(1);

        Category subCategory2 = new Category();
        subCategory2.setCategoryId(3);
        subCategory2.setName("Laptops");
        subCategory2.setParentCategoryId(1);

        List<Category> subCategories = List.of(subCategory1, subCategory2);
        parentCategory.setSubCategories(subCategories);

        // When
        CategoryDto dto = CategoryMapper.mapToDtoWithSubCategories(parentCategory);

        // Then
        assertThat(dto).isNotNull();
        assertThat(dto.getCategoryId()).isEqualTo(1);
        assertThat(dto.getName()).isEqualTo("Electronics");
        assertThat(dto.getSubCategories()).hasSize(2);
        assertThat(dto.getSubCategories().get(0).getName()).isEqualTo("Mobile Phones");
        assertThat(dto.getSubCategories().get(1).getName()).isEqualTo("Laptops");
    }

    @Test
    @DisplayName("Should map Category without sub-categories to DTO")
    void shouldMapCategoryWithoutSubCategoriesToDto() {
        // Given
        Category category = new Category();
        category.setCategoryId(1);
        category.setName("Electronics");
        category.setSubCategories(null);

        // When
        CategoryDto dto = CategoryMapper.mapToDtoWithSubCategories(category);

        // Then
        assertThat(dto).isNotNull();
        assertThat(dto.getCategoryId()).isEqualTo(1);
        assertThat(dto.getName()).isEqualTo("Electronics");
        assertThat(dto.getSubCategories()).isNull();
    }

    @Test
    @DisplayName("Should map Category with empty sub-categories to DTO")
    void shouldMapCategoryWithEmptySubCategoriesToDto() {
        // Given
        Category category = new Category();
        category.setCategoryId(1);
        category.setName("Electronics");
        category.setSubCategories(new ArrayList<>());

        // When
        CategoryDto dto = CategoryMapper.mapToDtoWithSubCategories(category);

        // Then
        assertThat(dto).isNotNull();
        assertThat(dto.getCategoryId()).isEqualTo(1);
        assertThat(dto.getName()).isEqualTo("Electronics");
        assertThat(dto.getSubCategories()).isNull(); // Empty list doesn't get mapped
    }

    @Test
    @DisplayName("Should return null when mapping null Category with sub-categories to DTO")
    void shouldReturnNullWhenMappingNullCategoryWithSubCategoriesToDto() {
        // When
        CategoryDto dto = CategoryMapper.mapToDtoWithSubCategories(null);

        // Then
        assertThat(dto).isNull();
    }

    @Test
    @DisplayName("Should map list of Categories to list of CategoryDtos")
    void shouldMapListOfCategoriesToListOfDtos() {
        // Given
        Category category1 = new Category();
        category1.setCategoryId(1);
        category1.setName("Electronics");

        Category category2 = new Category();
        category2.setCategoryId(2);
        category2.setName("Clothing");

        List<Category> categories = List.of(category1, category2);

        // When
        List<CategoryDto> dtos = CategoryMapper.mapToDtoList(categories);

        // Then
        assertThat(dtos).isNotNull();
        assertThat(dtos).hasSize(2);
        assertThat(dtos.get(0).getCategoryId()).isEqualTo(1);
        assertThat(dtos.get(0).getName()).isEqualTo("Electronics");
        assertThat(dtos.get(1).getCategoryId()).isEqualTo(2);
        assertThat(dtos.get(1).getName()).isEqualTo("Clothing");
    }

    @Test
    @DisplayName("Should return null when mapping null list of Categories to DTOs")
    void shouldReturnNullWhenMappingNullListOfCategoriesToDtos() {
        // When
        List<CategoryDto> dtos = CategoryMapper.mapToDtoList(null);

        // Then
        assertThat(dtos).isNull();
    }

    @Test
    @DisplayName("Should map empty list of Categories to empty list of DTOs")
    void shouldMapEmptyListOfCategoriesToEmptyListOfDtos() {
        // Given
        List<Category> categories = new ArrayList<>();

        // When
        List<CategoryDto> dtos = CategoryMapper.mapToDtoList(categories);

        // Then
        assertThat(dtos).isNotNull();
        assertThat(dtos).isEmpty();
    }

    @Test
    @DisplayName("Should handle Category with null fields when mapping to DTO")
    void shouldHandleCategoryWithNullFieldsWhenMappingToDto() {
        // Given
        Category category = new Category();
        category.setCategoryId(null);
        category.setName(null);
        category.setDescription(null);
        category.setParentCategoryId(null);

        // When
        CategoryDto dto = CategoryMapper.mapToDto(category);

        // Then
        assertThat(dto).isNotNull();
        assertThat(dto.getCategoryId()).isNull();
        assertThat(dto.getName()).isNull();
        assertThat(dto.getDescription()).isNull();
        assertThat(dto.getParentCategoryId()).isNull();
    }

    @Test
    @DisplayName("Should handle CategoryDto with null fields when mapping to entity")
    void shouldHandleCategoryDtoWithNullFieldsWhenMappingToEntity() {
        // Given
        CategoryDto dto = new CategoryDto();
        dto.setCategoryId(null);
        dto.setName(null);
        dto.setDescription(null);
        dto.setParentCategoryId(null);

        // When
        Category entity = CategoryMapper.mapToEntity(dto);

        // Then
        assertThat(entity).isNotNull();
        assertThat(entity.getCategoryId()).isNull();
        assertThat(entity.getName()).isNull();
        assertThat(entity.getDescription()).isNull();
        assertThat(entity.getParentCategoryId()).isNull();
    }
}
