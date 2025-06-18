package com.copilot.sample.service.impl;

import com.copilot.sample.dto.ProductAttributeDto;
import com.copilot.sample.entity.ProductAttribute;
import com.copilot.sample.repository.ProductAttributeRepository;
import com.copilot.sample.service.ProductAttributeService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

@Service
@Transactional
public class ProductAttributeServiceImpl implements ProductAttributeService {
    
    private final ProductAttributeRepository productAttributeRepository;
    
    @Autowired
    public ProductAttributeServiceImpl(ProductAttributeRepository productAttributeRepository) {
        this.productAttributeRepository = productAttributeRepository;
    }
    
    @Override
    @Transactional(readOnly = true)
    public List<ProductAttributeDto> getProductAttributesAsync(Integer productId) {
        List<ProductAttribute> attributes = productAttributeRepository.findByProductId(productId);
        return attributes.stream()
                .map(this::mapToDto)
                .collect(Collectors.toList());
    }
    
    @Override
    public ProductAttributeDto addProductAttributeAsync(ProductAttributeDto productAttributeDto) {
        ProductAttribute attribute = new ProductAttribute();
        attribute.setProductId(productAttributeDto.getProductId());
        attribute.setAttributeName(productAttributeDto.getAttributeName());
        attribute.setAttributeValue(productAttributeDto.getAttributeValue());
        
        ProductAttribute savedAttribute = productAttributeRepository.save(attribute);
        return mapToDto(savedAttribute);
    }
    
    @Override
    public boolean updateProductAttributeAsync(Integer attributeId, ProductAttributeDto productAttributeDto) {
        Optional<ProductAttribute> optionalAttribute = productAttributeRepository.findById(attributeId);
        
        if (optionalAttribute.isEmpty()) {
            return false;
        }
        
        ProductAttribute attribute = optionalAttribute.get();
        attribute.setAttributeName(productAttributeDto.getAttributeName());
        attribute.setAttributeValue(productAttributeDto.getAttributeValue());
        
        productAttributeRepository.save(attribute);
        return true;
    }
    
    @Override
    public boolean deleteProductAttributeAsync(Integer attributeId) {
        if (!productAttributeRepository.existsById(attributeId)) {
            return false;
        }
        
        productAttributeRepository.deleteById(attributeId);
        return true;
    }
      @Override
    public void deleteAllProductAttributesAsync(Integer productId) {
        productAttributeRepository.deleteByProductId(productId);
    }
    
    // Controller method implementations
    @Override
    public List<ProductAttributeDto> getAttributesByProductId(Integer productId) {
        return getProductAttributesAsync(productId);
    }
    
    @Override
    public ProductAttributeDto getAttributeById(Integer id) {
        Optional<ProductAttribute> attribute = productAttributeRepository.findById(id);
        return attribute.map(this::mapToDto).orElse(null);
    }
    
    @Override
    public ProductAttributeDto createAttribute(ProductAttributeDto productAttributeDto) {
        return addProductAttributeAsync(productAttributeDto);
    }
    
    @Override
    public ProductAttributeDto updateAttribute(Integer id, ProductAttributeDto productAttributeDto) {
        Optional<ProductAttribute> optionalAttribute = productAttributeRepository.findById(id);
        
        if (optionalAttribute.isEmpty()) {
            return null;
        }
        
        ProductAttribute attribute = optionalAttribute.get();
        attribute.setAttributeName(productAttributeDto.getAttributeName());
        attribute.setAttributeValue(productAttributeDto.getAttributeValue());
        
        ProductAttribute savedAttribute = productAttributeRepository.save(attribute);
        return mapToDto(savedAttribute);
    }
    
    @Override
    public boolean deleteAttribute(Integer id) {
        return deleteProductAttributeAsync(id);
    }

    private ProductAttributeDto mapToDto(ProductAttribute attribute) {
        ProductAttributeDto dto = new ProductAttributeDto();
        dto.setAttributeId(attribute.getAttributeId());
        dto.setProductId(attribute.getProductId());
        dto.setAttributeName(attribute.getAttributeName());
        dto.setAttributeValue(attribute.getAttributeValue());
        return dto;
    }
}
