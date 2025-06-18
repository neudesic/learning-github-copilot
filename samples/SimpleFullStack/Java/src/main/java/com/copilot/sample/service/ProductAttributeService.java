package com.copilot.sample.service;

import com.copilot.sample.dto.ProductAttributeDto;

import java.util.List;

public interface ProductAttributeService {
    
    List<ProductAttributeDto> getProductAttributesAsync(Integer productId);
    
    ProductAttributeDto addProductAttributeAsync(ProductAttributeDto productAttributeDto);
    
    boolean updateProductAttributeAsync(Integer attributeId, ProductAttributeDto productAttributeDto);
    
    boolean deleteProductAttributeAsync(Integer attributeId);
    
    void deleteAllProductAttributesAsync(Integer productId);
    
    // Methods called by controllers
    List<ProductAttributeDto> getAttributesByProductId(Integer productId);
    ProductAttributeDto getAttributeById(Integer id);
    ProductAttributeDto createAttribute(ProductAttributeDto productAttributeDto);
    ProductAttributeDto updateAttribute(Integer id, ProductAttributeDto productAttributeDto);
    boolean deleteAttribute(Integer id);
}
