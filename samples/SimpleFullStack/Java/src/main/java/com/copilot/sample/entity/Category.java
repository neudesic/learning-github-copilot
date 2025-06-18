package com.copilot.sample.entity;

import jakarta.persistence.*;
import java.util.List;

@Entity
@Table(name = "categories")
public class Category {    @Id
    @Column(name = "category_id")
    private Integer categoryId;

    @Column(name = "name", nullable = false, length = 100)
    private String name;

    @Column(name = "description", length = 500)
    private String description;

    @Column(name = "parent_category_id")
    private Integer parentCategoryId;    @OneToMany(mappedBy = "category", fetch = FetchType.LAZY)
    private List<Product> products;

    @OneToMany(fetch = FetchType.LAZY)
    @JoinColumn(name = "parent_category_id")
    private List<Category> subCategories;

    // Default constructor
    public Category() {}

    // Constructor with parameters
    public Category(String name, String description, Integer parentCategoryId) {
        this.name = name;
        this.description = description;
        this.parentCategoryId = parentCategoryId;
    }

    // Getters and Setters
    public Integer getCategoryId() {
        return categoryId;
    }

    public void setCategoryId(Integer categoryId) {
        this.categoryId = categoryId;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public Integer getParentCategoryId() {
        return parentCategoryId;
    }

    public void setParentCategoryId(Integer parentCategoryId) {
        this.parentCategoryId = parentCategoryId;
    }

    public List<Product> getProducts() {
        return products;
    }    public void setProducts(List<Product> products) {
        this.products = products;
    }

    public List<Category> getSubCategories() {
        return subCategories;
    }

    public void setSubCategories(List<Category> subCategories) {
        this.subCategories = subCategories;
    }
}
