package com.copilot.sample.repository;

import com.copilot.sample.entity.Category;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface CategoryRepository extends JpaRepository<Category, Integer> {
    
    @Query("SELECT c FROM Category c LEFT JOIN FETCH c.subCategories")
    List<Category> findAllWithSubCategories();
    
    @Query("SELECT c FROM Category c LEFT JOIN FETCH c.subCategories WHERE c.categoryId = :id")
    Optional<Category> findByIdWithSubCategories(@Param("id") Integer id);
    
    Optional<Category> findByName(String name);
    
    List<Category> findByParentCategoryId(Integer parentCategoryId);
    
    boolean existsByName(String name);
}
