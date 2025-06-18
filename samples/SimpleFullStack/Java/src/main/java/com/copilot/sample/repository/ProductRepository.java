package com.copilot.sample.repository;

import com.copilot.sample.entity.Product;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface ProductRepository extends JpaRepository<Product, Integer> {
    
    @Query("SELECT p FROM Product p LEFT JOIN FETCH p.category")
    List<Product> findAllWithCategory();
    
    @Query("SELECT p FROM Product p LEFT JOIN FETCH p.category WHERE p.productId = :id")
    Optional<Product> findByIdWithCategory(@Param("id") Integer id);
    
    List<Product> findByCategoryId(Integer categoryId);
    
    List<Product> findByIsActive(Boolean isActive);
    
    Optional<Product> findBySku(String sku);
    
    boolean existsBySku(String sku);
    
    @Query("SELECT p FROM Product p WHERE p.name LIKE %:name%")
    List<Product> findByNameContaining(@Param("name") String name);
}
