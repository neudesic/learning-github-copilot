package com.copilot.sample.controller;

import com.copilot.sample.dto.CategoryDto;
import com.copilot.sample.dto.AddCategoryDto;
import com.copilot.sample.service.CategoryService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/categories")
@CrossOrigin(origins = "*")
@Tag(name = "Categories", description = "Category management APIs")
public class CategoryController {    @Autowired
    private CategoryService categoryService;

    @Operation(summary = "Get all categories", description = "Retrieve a list of all categories in the system")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Successfully retrieved categories",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = CategoryDto.class)))
    })
    @GetMapping
    public ResponseEntity<List<CategoryDto>> getAllCategories() {
        List<CategoryDto> categories = categoryService.getAllCategories();
        return ResponseEntity.ok(categories);
    }

    @Operation(summary = "Get category by ID", description = "Retrieve a specific category by its ID")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Category found",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = CategoryDto.class))),
            @ApiResponse(responseCode = "404", description = "Category not found", content = @Content)
    })
    @GetMapping("/{id}")
    public ResponseEntity<CategoryDto> getCategoryById(
            @Parameter(description = "ID of the category to retrieve", required = true)
            @PathVariable Integer id) {
        CategoryDto category = categoryService.getCategoryById(id);
        if (category != null) {
            return ResponseEntity.ok(category);
        }
        return ResponseEntity.notFound().build();
    }

    @Operation(summary = "Create a new category", description = "Create a new category in the system")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "201", description = "Category created successfully",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = CategoryDto.class))),
            @ApiResponse(responseCode = "400", description = "Invalid input data", content = @Content)
    })
    @PostMapping
    public ResponseEntity<CategoryDto> createCategory(
            @Parameter(description = "Category data to create", required = true)
            @Valid @RequestBody AddCategoryDto addCategoryDto) {
        CategoryDto createdCategory = categoryService.createCategory(addCategoryDto);
        return ResponseEntity.status(HttpStatus.CREATED).body(createdCategory);
    }

    @Operation(summary = "Update a category", description = "Update an existing category by its ID")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Category updated successfully",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = CategoryDto.class))),
            @ApiResponse(responseCode = "404", description = "Category not found", content = @Content),
            @ApiResponse(responseCode = "400", description = "Invalid input data", content = @Content)
    })
    @PutMapping("/{id}")
    public ResponseEntity<CategoryDto> updateCategory(
            @Parameter(description = "ID of the category to update", required = true)
            @PathVariable Integer id,
            @Parameter(description = "Updated category data", required = true)
            @Valid @RequestBody AddCategoryDto addCategoryDto) {
        CategoryDto updatedCategory = categoryService.updateCategory(id, addCategoryDto);
        if (updatedCategory != null) {
            return ResponseEntity.ok(updatedCategory);
        }
        return ResponseEntity.notFound().build();
    }    @Operation(summary = "Delete a category", description = "Delete a category by its ID")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "204", description = "Category deleted successfully", content = @Content),
            @ApiResponse(responseCode = "404", description = "Category not found", content = @Content)
    })
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteCategory(
            @Parameter(description = "ID of the category to delete", required = true)
            @PathVariable Integer id) {
        boolean deleted = categoryService.deleteCategory(id);
        if (deleted) {
            return ResponseEntity.noContent().build();
        }
        return ResponseEntity.notFound().build();
    }
}
