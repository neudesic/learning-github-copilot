package com.copilot.sample.config;

import com.copilot.sample.entity.Category;
import com.copilot.sample.entity.Product;
import com.copilot.sample.repository.CategoryRepository;
import com.copilot.sample.repository.ProductRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.stereotype.Component;

@Component
public class DataInitializer implements CommandLineRunner {

    private final CategoryRepository categoryRepository;
    private final ProductRepository productRepository;

    @Autowired
    public DataInitializer(CategoryRepository categoryRepository, ProductRepository productRepository) {
        this.categoryRepository = categoryRepository;
        this.productRepository = productRepository;
    }

    @Override
    public void run(String... args) throws Exception {
        // Only initialize if database is empty
        if (categoryRepository.count() == 0) {
            initializeData();
        }
    }    private void initializeData() {
        // Create sample categories with manual IDs to work around SQLite limitation
        Category electronics = new Category("Electronics", "Electronic devices and accessories", null);
        electronics.setCategoryId(1);
        Category clothing = new Category("Clothing", "Apparel and fashion items", null);
        clothing.setCategoryId(2);
        Category books = new Category("Books", "Books and literature", null);
        books.setCategoryId(3);
        
        categoryRepository.save(electronics);
        categoryRepository.save(clothing);
        categoryRepository.save(books);

        // Create subcategories
        Category smartphones = new Category("Smartphones", "Mobile phones and accessories", 1);
        smartphones.setCategoryId(4);
        Category laptops = new Category("Laptops", "Portable computers", 1);
        laptops.setCategoryId(5);
        
        categoryRepository.save(smartphones);
        categoryRepository.save(laptops);

        // Create sample products with manual IDs
        Product iphone = new Product("iPhone 15", "Latest iPhone model", "IPHONE15-001", 4, "Apple", new java.math.BigDecimal("999.99"));
        iphone.setProductId(1);
        Product macbook = new Product("MacBook Pro", "Professional laptop", "MBP-M3-001", 5, "Apple", new java.math.BigDecimal("1999.99"));
        macbook.setProductId(2);
        Product tshirt = new Product("Cotton T-Shirt", "Comfortable cotton t-shirt", "TSHIRT-001", 2, "Generic", new java.math.BigDecimal("19.99"));
        tshirt.setProductId(3);
        Product novel = new Product("Programming Book", "Learn Java programming", "BOOK-JAVA-001", 3, "TechBooks", new java.math.BigDecimal("39.99"));
        novel.setProductId(4);

        productRepository.save(iphone);
        productRepository.save(macbook);
        productRepository.save(tshirt);
        productRepository.save(novel);
    }
}
