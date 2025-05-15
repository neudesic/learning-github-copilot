# Database Setup

1. Install MS SQL Server (Developer or Express) on localhost.  
   Download: <https://www.microsoft.com/en-us/sql-server/sql-server-downloads>
2. Create a database named **Inventory**.
3. Run the following DDL statements to create the necessary tables:

```sql
-- Inventory Database schema

-- Categories Table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    ParentCategoryID INT NULL,
    FOREIGN KEY (ParentCategoryID) REFERENCES Categories(CategoryID)
);

-- Products Table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    SKU NVARCHAR(100) UNIQUE NOT NULL,
    CategoryID INT NOT NULL,
    Brand NVARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Pricing Table
CREATE TABLE ProductPrices (
    PriceID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    CurrencyCode CHAR(3) DEFAULT 'USD',
    EffectiveFrom DATETIME DEFAULT GETDATE(),
    EffectiveTill DATETIME NULL,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Inventory Table
CREATE TABLE Inventory (
    InventoryID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    LastUpdated DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Product Attributes
CREATE TABLE ProductAttributes (
    AttributeID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    AttributeName NVARCHAR(100),
    AttributeValue NVARCHAR(255),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Product Reviews
CREATE TABLE ProductReviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    ReviewerName NVARCHAR(100),
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(MAX),
    ReviewDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
```

4. Run the following DML statements to insert sample data:

```sql
-- Insert Categories
INSERT INTO Categories (Name, Description, ParentCategoryID) VALUES
('Electronics', 'Electronic gadgets and devices', NULL),
('Laptops', 'Portable computers', 1),
('Smartphones', 'Mobile phones and accessories', 1),
('Accessories', 'Electronics accessories', 1);

-- Insert Products
INSERT INTO Products (Name, Description, SKU, CategoryID, Brand) VALUES
('UltraBook X1', 'Lightweight business laptop with high performance', 'SKU-UBX1', 2, 'TechBrand'),
('Gaming Beast Z9', 'High-end gaming laptop with RGB lighting', 'SKU-GBZ9', 2, 'GamePro'),
('Galaxy X10', 'Latest smartphone with AI-powered camera', 'SKU-GX10', 3, 'SmartTech'),
('EarPods Pro', 'Wireless earphones with noise cancellation', 'SKU-EPP', 4, 'SoundWave');

-- Insert Product Prices
INSERT INTO ProductPrices (ProductID, Price, CurrencyCode, EffectiveFrom, EffectiveTill) VALUES
(1, 1299.99, 'USD', '2025-01-01', NULL),
(1, 1199.99, 'USD', '2024-10-01', '2024-12-31'),
(2, 1999.00, 'USD', '2025-01-15', NULL),
(3, 899.50, 'USD', '2025-03-15', NULL),
(4, 199.99, 'USD', '2025-02-01', NULL);

-- Insert Inventory
INSERT INTO Inventory (ProductID, Quantity) VALUES
(1, 25),
(2, 10),
(3, 50),
(4, 100);

-- Insert Product Attributes
INSERT INTO ProductAttributes (ProductID, AttributeName, AttributeValue) VALUES
(1, 'Processor', 'Intel Core i7'),
(1, 'RAM', '16GB'),
(1, 'Storage', '512GB SSD'),
(2, 'Processor', 'AMD Ryzen 9'),
(2, 'RAM', '32GB'),
(2, 'Graphics Card', 'NVIDIA RTX 4080'),
(3, 'Display', '6.5-inch OLED'),
(3, 'Battery', '4000mAh'),
(3, 'Camera', '108MP'),
(4, 'Connectivity', 'Bluetooth 5.2'),
(4, 'Noise Cancellation', 'Active'),
(4, 'Battery Life', '8 hours');

-- Insert Product Reviews
INSERT INTO ProductReviews (ProductID, ReviewerName, Rating, Comment) VALUES
(1, 'Alice Johnson', 5, 'Absolutely love this laptop! Fast and sleek.'),
(1, 'Bob Smith', 4, 'Good performance but gets a bit warm.'),
(2, 'Tommy Lee', 5, 'A beast for gaming. Smooth experience!'),
(3, 'Carlos Vega', 5, 'Amazing camera and display! Worth every penny.'),
(3, 'Diana Lee', 3, 'Battery life could be better.'),
(4, 'Emma Stone', 4, 'Very comfortable fit and great sound.'),
(4, 'John Doe', 2, 'Connection drops sometimes.');

-- More Products
INSERT INTO Products (Name, Description, SKU, CategoryID, Brand) VALUES
('ThinkMate Pro 14', 'Durable business laptop with excellent battery life', 'SKU-TMP14', 2, 'ThinkCorp'),
('PixelCam A2', 'Compact smartphone with high-resolution camera', 'SKU-PXA2', 3, 'PixelTech'),
('PowerCharge 10000', 'Portable power bank with fast charging support', 'SKU-PC10000', 4, 'ChargeX');

-- More Product Prices
INSERT INTO ProductPrices (ProductID, Price, CurrencyCode, EffectiveFrom, EffectiveTill) VALUES
(5, 999.99, 'USD', '2025-04-01', NULL),
(6, 649.00, 'USD', '2025-04-15', NULL),
(7, 49.99, 'USD', '2025-03-01', NULL);

-- More Inventory
INSERT INTO Inventory (ProductID, Quantity) VALUES
(5, 30),
(6, 45),
(7, 200);

-- More Product Attributes
INSERT INTO ProductAttributes (ProductID, AttributeName, AttributeValue) VALUES
(5, 'Processor', 'Intel i5'),
(5, 'RAM', '8GB'),
(5, 'Weight', '1.3kg'),
(6, 'Camera', '64MP'),
(6, 'Storage', '128GB'),
(6, 'Display', '6.1-inch AMOLED'),
(7, 'Capacity', '10000mAh'),
(7, 'USB Ports', '2'),
(7, 'Fast Charging', 'Yes');

-- More Product Reviews
INSERT INTO ProductReviews (ProductID, ReviewerName, Rating, Comment) VALUES
(5, 'Harvey Dent', 4, 'Reliable laptop, solid build and decent performance.'),
(5, 'Rachel Green', 3, 'Good for light work, but a bit slow for multitasking.'),
(6, 'Bruce Banner', 5, 'Compact yet powerful. Great value for the price.'),
(6, 'Natasha Romanoff', 4, 'Excellent camera quality and battery.'),
(7, 'Steve Rogers', 5, 'Lasts all day. Perfect for travel.'),
(7, 'Tony Stark', 3, 'Charges fast, but gets warm during use.');
```
