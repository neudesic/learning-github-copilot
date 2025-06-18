package com.copilot.sample.service.impl;

import com.copilot.sample.dto.AddProductDto;
import com.copilot.sample.dto.ProductDto;
import com.copilot.sample.entity.Product;
import com.copilot.sample.mapper.ProductMapper;
import com.copilot.sample.repository.CategoryRepository;
import com.copilot.sample.repository.ProductRepository;
import com.copilot.sample.service.ProductService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Optional;

@Service
@Transactional
public class ProductServiceImpl implements ProductService {
    
    private final ProductRepository productRepository;
    private final CategoryRepository categoryRepository;
    
    @Autowired
    public ProductServiceImpl(ProductRepository productRepository, CategoryRepository categoryRepository) {
        this.productRepository = productRepository;
        this.categoryRepository = categoryRepository;
    }
    
    @Override
    @Transactional(readOnly = true)
    public List<ProductDto> getProductsAsync() {
        List<Product> products = productRepository.findAllWithCategory();
        return ProductMapper.mapToDtoList(products);
    }
    
    @Override
    @Transactional(readOnly = true)
    public ProductDto getProductByIdAsync(Integer id) {
        Optional<Product> product = productRepository.findByIdWithCategory(id);
        return product.map(ProductMapper::mapToDto).orElse(null);
    }
    
    @Override
    public ProductDto addProductAsync(AddProductDto addProductDto) {
        // Validate that category exists
        if (!categoryRepository.existsById(addProductDto.getCategoryId())) {
            throw new IllegalArgumentException("Category with ID " + addProductDto.getCategoryId() + " does not exist.");
        }
        
        // Check if SKU already exists
        if (productRepository.existsBySku(addProductDto.getSku())) {
            throw new IllegalArgumentException("A product with SKU " + addProductDto.getSku() + " already exists.");
        }
        
        Product product = new Product();
        product.setName(addProductDto.getName());
        product.setDescription(addProductDto.getDescription());
        product.setSku(addProductDto.getSku());
        product.setCategoryId(addProductDto.getCategoryId());
        product.setBrand(addProductDto.getBrand());
        product.setIsActive(addProductDto.getIsActive() != null ? addProductDto.getIsActive() : true);
        
        Product savedProduct = productRepository.save(product);        return ProductMapper.mapToDto(savedProduct);
    }
    
    @Override
    public boolean deleteProductAsync(Integer id) {
        if (!productRepository.existsById(id)) {
            return false;
        }
        
        productRepository.deleteById(id);
        return true;
    }
      @Override
    @Transactional(readOnly = true)
    public List<ProductDto> getProductsByCategoryAsync(Integer categoryId) {
        List<Product> products = productRepository.findByCategoryId(categoryId);
        return ProductMapper.mapToDtoList(products);
    }
    
    // Controller method implementations
    @Override
    public List<ProductDto> getAllProducts() {
        return getProductsAsync();
    }
    
    @Override
    public ProductDto getProductById(Integer id) {
        return getProductByIdAsync(id);
    }
    
    @Override
    public List<ProductDto> getProductsByCategory(Integer categoryId) {
        return getProductsByCategoryAsync(categoryId);
    }
      @Override
    public List<ProductDto> searchProductsByName(String name) {
        List<Product> products = productRepository.findByNameContaining(name);
        return ProductMapper.mapToDtoList(products);
    }
    
    @Override
    public ProductDto createProduct(AddProductDto addProductDto) {
        return addProductAsync(addProductDto);
    }
    
    @Override
    public ProductDto updateProduct(Integer id, AddProductDto addProductDto) {
        // Update product fields from AddProductDto
        Optional<Product> optionalProduct = productRepository.findById(id);
        
        if (optionalProduct.isEmpty()) {
            return null;
        }
        
        Product product = optionalProduct.get();
        product.setName(addProductDto.getName());
        product.setDescription(addProductDto.getDescription());
        product.setSku(addProductDto.getSku());
        product.setCategoryId(addProductDto.getCategoryId());
        product.setBrand(addProductDto.getBrand());
        product.setIsActive(addProductDto.getIsActive() != null ? addProductDto.getIsActive() : true);
        
        Product savedProduct = productRepository.save(product);
        return ProductMapper.mapToDto(savedProduct);
    }
    
    @Override
    public boolean deleteProduct(Integer id) {
        return deleteProductAsync(id);
    }
}
