package com.copilot.sample.entity;

import jakarta.persistence.*;

@Entity
@Table(name = "inventory")
public class Inventory {    @Id
    @Column(name = "inventory_id")
    private Integer inventoryId;

    @Column(name = "product_id", nullable = false, unique = true)
    private Integer productId;

    @Column(name = "quantity_in_stock", nullable = false)
    private Integer quantityInStock = 0;

    @Column(name = "reserved_quantity")
    private Integer reservedQuantity = 0;

    @Column(name = "reorder_level")
    private Integer reorderLevel = 10;

    @Column(name = "max_stock_level")
    private Integer maxStockLevel = 1000;

    @OneToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "product_id", insertable = false, updatable = false)
    private Product product;

    // Default constructor
    public Inventory() {}

    // Constructor with parameters
    public Inventory(Integer productId, Integer quantityInStock) {
        this.productId = productId;
        this.quantityInStock = quantityInStock;
        this.reservedQuantity = 0;
        this.reorderLevel = 10;
        this.maxStockLevel = 1000;
    }

    // Getters and Setters
    public Integer getInventoryId() {
        return inventoryId;
    }

    public void setInventoryId(Integer inventoryId) {
        this.inventoryId = inventoryId;
    }

    public Integer getProductId() {
        return productId;
    }

    public void setProductId(Integer productId) {
        this.productId = productId;
    }

    public Integer getQuantityInStock() {
        return quantityInStock;
    }

    public void setQuantityInStock(Integer quantityInStock) {
        this.quantityInStock = quantityInStock;
    }

    public Integer getReservedQuantity() {
        return reservedQuantity;
    }

    public void setReservedQuantity(Integer reservedQuantity) {
        this.reservedQuantity = reservedQuantity;
    }

    public Integer getReorderLevel() {
        return reorderLevel;
    }

    public void setReorderLevel(Integer reorderLevel) {
        this.reorderLevel = reorderLevel;
    }

    public Integer getMaxStockLevel() {
        return maxStockLevel;
    }

    public void setMaxStockLevel(Integer maxStockLevel) {
        this.maxStockLevel = maxStockLevel;
    }

    public Product getProduct() {
        return product;
    }

    public void setProduct(Product product) {
        this.product = product;
    }

    // Utility methods
    public Integer getAvailableQuantity() {
        return quantityInStock - reservedQuantity;
    }

    public boolean isLowStock() {
        return quantityInStock <= reorderLevel;
    }

    public boolean isOverStock() {
        return quantityInStock >= maxStockLevel;
    }
}
