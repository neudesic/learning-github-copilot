package com.copilot.sample.mapper;

import com.copilot.sample.dto.ProductDto;
import com.copilot.sample.entity.Product;
import org.springframework.stereotype.Component;

import java.util.List;
import java.util.stream.Collectors;

@Component
public class ProductMapper {
    
    public static ProductDto mapToDto(Product product) {
        if (product == null) {
            return null;
        }
        
        ProductDto dto = new ProductDto();
        dto.setProductId(product.getProductId());
        dto.setName(product.getName());
        dto.setDescription(product.getDescription());
        dto.setSku(product.getSku());
        dto.setCategoryId(product.getCategoryId());
        dto.setBrand(product.getBrand());
        dto.setIsActive(product.getIsActive());
        
        // Map category if available
        if (product.getCategory() != null) {
            dto.setCategory(CategoryMapper.mapToDto(product.getCategory()));
        }
        
        return dto;
    }
    
    public static Product mapToEntity(ProductDto dto) {
        if (dto == null) {
            return null;
        }
        
        Product product = new Product();
        product.setProductId(dto.getProductId());
        product.setName(dto.getName());
        product.setDescription(dto.getDescription());
        product.setSku(dto.getSku());
        product.setCategoryId(dto.getCategoryId());
        product.setBrand(dto.getBrand());
        product.setIsActive(dto.getIsActive());
        
        return product;
    }
    
    public static List<ProductDto> mapToDtoList(List<Product> products) {
        if (products == null) {
            return null;
        }
        
        return products.stream()
                .map(ProductMapper::mapToDto)
                .collect(Collectors.toList());
    }
}
