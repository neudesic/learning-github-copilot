--Categories Table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    ParentCategoryID INT NULL,
    FOREIGN KEY (ParentCategoryID) REFERENCES Categories(CategoryID)
);

--Products Table
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

--Pricing Table
CREATE TABLE ProductPrices (
    PriceID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    CurrencyCode CHAR(3) DEFAULT 'USD',
    EffectiveFrom DATETIME DEFAULT GETDATE(),
    EffectiveTill DATETIME NULL,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

--Inventory Table
CREATE TABLE Inventory (
    InventoryID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    LastUpdated DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

--Product Attributes
CREATE TABLE ProductAttributes (
    AttributeID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    AttributeName NVARCHAR(100),
    AttributeValue NVARCHAR(255),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

--Product Reviews
CREATE TABLE ProductReviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    ReviewerName NVARCHAR(100),
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(MAX),
    ReviewDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
