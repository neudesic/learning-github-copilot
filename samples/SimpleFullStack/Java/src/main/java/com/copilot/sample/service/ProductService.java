package com.copilot.sample.service;

import com.copilot.sample.dto.AddProductDto;
import com.copilot.sample.dto.ProductDto;

import java.util.List;

public interface ProductService {
    
    List<ProductDto> getProductsAsync();
    
    ProductDto getProductByIdAsync(Integer id);
      ProductDto addProductAsync(AddProductDto addProductDto);
    
    boolean deleteProductAsync(Integer id);
    
    List<ProductDto> getProductsByCategoryAsync(Integer categoryId);
    
    // Methods called by controllers
    List<ProductDto> getAllProducts();
    ProductDto getProductById(Integer id);
    List<ProductDto> getProductsByCategory(Integer categoryId);
    List<ProductDto> searchProductsByName(String name);
    ProductDto createProduct(AddProductDto addProductDto);
    ProductDto updateProduct(Integer id, AddProductDto addProductDto);
    boolean deleteProduct(Integer id);
}
