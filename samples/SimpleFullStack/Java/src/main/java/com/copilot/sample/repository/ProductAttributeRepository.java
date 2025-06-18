package com.copilot.sample.repository;

import com.copilot.sample.entity.ProductAttribute;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ProductAttributeRepository extends JpaRepository<ProductAttribute, Integer> {
    
    List<ProductAttribute> findByProductId(Integer productId);
    
    List<ProductAttribute> findByAttributeName(String attributeName);
    
    List<ProductAttribute> findByProductIdAndAttributeName(Integer productId, String attributeName);
    
    void deleteByProductId(Integer productId);
}
