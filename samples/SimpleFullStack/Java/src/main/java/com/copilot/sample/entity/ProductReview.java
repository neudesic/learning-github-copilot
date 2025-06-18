package com.copilot.sample.entity;

import jakarta.persistence.*;
import java.time.LocalDateTime;

@Entity
@Table(name = "ProductReviews")
public class ProductReview {
      @Id
    @Column(name = "ReviewID")
    private Integer reviewId;
    
    @Column(name = "ProductID", nullable = false)
    private Integer productId;
    
    @Column(name = "ReviewerName")
    private String reviewerName;
    
    @Column(name = "Rating", nullable = false)
    private Integer rating;
    
    @Column(name = "Comment", columnDefinition = "TEXT")
    private String comment;
    
    @Column(name = "ReviewDate", nullable = false)
    private LocalDateTime reviewDate;
    
    // Navigation property
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "ProductID", insertable = false, updatable = false)
    private Product product;
    
    // Constructors
    public ProductReview() {}
    
    public ProductReview(Integer productId, String reviewerName, Integer rating, String comment) {
        this.productId = productId;
        this.reviewerName = reviewerName;
        this.rating = rating;
        this.comment = comment;
        this.reviewDate = LocalDateTime.now();
    }
    
    @PrePersist
    protected void onCreate() {
        reviewDate = LocalDateTime.now();
    }
    
    // Getters and Setters
    public Integer getReviewId() {
        return reviewId;
    }
    
    public void setReviewId(Integer reviewId) {
        this.reviewId = reviewId;
    }
    
    public Integer getProductId() {
        return productId;
    }
    
    public void setProductId(Integer productId) {
        this.productId = productId;
    }
    
    public String getReviewerName() {
        return reviewerName;
    }
    
    public void setReviewerName(String reviewerName) {
        this.reviewerName = reviewerName;
    }
    
    public Integer getRating() {
        return rating;
    }
    
    public void setRating(Integer rating) {
        this.rating = rating;
    }
    
    public String getComment() {
        return comment;
    }
    
    public void setComment(String comment) {
        this.comment = comment;
    }
    
    public LocalDateTime getReviewDate() {
        return reviewDate;
    }
    
    public void setReviewDate(LocalDateTime reviewDate) {
        this.reviewDate = reviewDate;
    }
    
    public Product getProduct() {
        return product;
    }
    
    public void setProduct(Product product) {
        this.product = product;
    }
}
