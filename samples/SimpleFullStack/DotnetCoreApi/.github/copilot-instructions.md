# Copilot Instructions for .NET 8 Full Stack Inventory API

## Project Overview
This is a .NET 8 Web API solution for inventory and product management with a comprehensive 3-layer architecture. The solution demonstrates modern .NET development practices with Entity Framework Core, SQLite database, clean architecture, and comprehensive testing.

## Project Structure

### Solution Architecture
```
copilot-sample.sln (Solution file)
├── copilot-sample.Api/ (Web API Layer)
├── copilot-sample.DataAccess/ (Data Access Layer)
├── copilot-sample.Test/ (Unit Testing)
└── lessons/ (Learning materials)
```

### 1. **copilot-sample.Api** - Web API Layer
- **Framework**: ASP.NET Core 8.0 Web API (`Microsoft.NET.Sdk.Web`)
- **Purpose**: Handles HTTP requests, API controllers, and business logic services
- **Key Dependencies**:
  - `Microsoft.EntityFrameworkCore.Design` (9.0.5) - For EF migrations
  - `Swashbuckle.AspNetCore` (6.6.2) - For Swagger/OpenAPI documentation
- **Project Reference**: `copilot-sample.DataAccess`
- **Structure**:
  - `Controllers/` - API controllers (ProductController, CategoryController, ProductAttributeController)
  - `Services/` - Business logic services (ProductService, CategoryService, ProductAttributeService)
  - `Contracts/` - Service interfaces (IProductService, ICategoryService)
  - `Models/Dtos/` - Data Transfer Objects for API responses
  - `Program.cs` - Application configuration and dependency injection
  - `appsettings.json` - Configuration including connection strings

### 2. **copilot-sample.DataAccess** - Data Access Layer
- **Framework**: .NET 8 Class Library (`Microsoft.NET.Sdk`)
- **Purpose**: Entity Framework Core data layer with entities, configurations, and migrations
- **Key Dependencies**:
  - `Microsoft.EntityFrameworkCore` (9.0.5) - Core EF functionality
  - `Microsoft.EntityFrameworkCore.Sqlite` (9.0.5) - SQLite provider
  - `Microsoft.EntityFrameworkCore.Tools` (9.0.5) - Migration tools
- **Structure**:
  - `Entities/` - Database entity models (Product, Category, Inventory, ProductPrice, ProductAttribute, ProductReview)
  - `EntityConfiguration/` - EF Core fluent configurations for each entity
  - `Extensions/` - Extension methods for database setup
  - `Migrations/` - EF Core migration files
  - `SeedData/` - Database seed data classes
  - `AppDbContext.cs` - Main Entity Framework DbContext

### 3. **copilot-sample.Test** - Unit Testing Layer
- **Framework**: .NET 8 Test Project (`Microsoft.NET.Sdk`)
- **Purpose**: Unit tests for API controllers and services
- **Key Dependencies**:
  - `Microsoft.NET.Test.Sdk` (17.8.0) - Test framework
  - `xunit` (2.9.3) - Testing framework
  - `xunit.runner.visualstudio` (2.5.3) - Visual Studio test runner
  - `Moq` (4.20.72) - Mocking framework
  - `FluentAssertions` (8.2.0) - Assertion library
  - `Microsoft.EntityFrameworkCore.InMemory` (9.0.5) - In-memory database for testing
  - `coverlet.collector` (6.0.0) - Code coverage
- **Project Reference**: `copilot-sample.Api`

## Database Schema & Entities

### Core Entities
1. **Categories** - Hierarchical product categories with parent-child relationships
2. **Products** - Core product information with SKU, brand, and lifecycle tracking
3. **ProductPrices** - Time-based pricing with currency support and price history
4. **Inventory** - Real-time stock quantity tracking
5. **ProductAttributes** - Flexible key-value product specifications
6. **ProductReviews** - Customer review and rating system (1-5 stars)

### Entity Relationships
- **Categories**: Self-referencing hierarchy (Parent → Children)
- **Products**: Belongs to Category, has multiple Prices/Attributes/Reviews, and one Inventory
- **One-to-One**: Product ↔ Inventory
- **One-to-Many**: Category → Products, Product → (Prices, Attributes, Reviews)

### Database Configuration
- **Primary Database**: SQLite (`inventory.db`)
- **Connection String**: Configured in `appsettings.json`
- **ORM**: Entity Framework Core 9.0.5
- **Migration Support**: Full EF Core migrations with seed data
- **Alternative**: SQL Server support available (see `dbschema.sql`)

## Development Guidelines

### 1. **API Development**
- Follow RESTful API conventions
- Use proper HTTP status codes (200, 201, 400, 404, etc.)
- Implement comprehensive error handling with descriptive messages
- Use DTOs for API contracts (separate from entities)
- Apply dependency injection for services and repositories
- Document APIs with Swagger/OpenAPI annotations

### 2. **Entity Framework Patterns**
- Use Entity Configurations (IEntityTypeConfiguration<T>) for database mapping
- Implement proper navigation properties for relationships
- Use async/await for all database operations
- Apply appropriate EF Core conventions (HasKey, HasIndex, HasMaxLength)
- Leverage fluent API for complex configurations
- Use centralized seed data in `SeedData/` folder

### 3. **Service Layer Design**
- Implement service interfaces in `Contracts/` folder
- Register services with appropriate lifetime (Scoped for database services)
- Use repository pattern through EF Core DbContext
- Implement proper exception handling and logging
- Return DTOs from services, not entities
- Apply business logic in service layer, not controllers

### 4. **Testing Standards**
- Write unit tests for all controllers and services
- Use in-memory database for integration tests
- Mock external dependencies with Moq
- Use FluentAssertions for readable test assertions
- Follow AAA pattern (Arrange, Act, Assert)
- Test both success and failure scenarios

### 5. **Code Organization**
- Use namespace conventions: `copilot_sample.{Layer}.{Feature}`
- Follow C# naming conventions (PascalCase for public members)
- Use nullable reference types (`string?` for optional properties)
- Implement proper using statements and imports
- Use record types for DTOs when appropriate

## Configuration & Deployment

### Environment Setup
- **Target Framework**: .NET 8.0
- **Nullable**: Enabled
- **Implicit Usings**: Enabled
- **Default Ports**: HTTPS (7111), HTTP (5111)

### Key Configuration Files
- `appsettings.json` - Connection strings and app settings
- `appsettings.Development.json` - Development-specific settings
- `launchSettings.json` - Development server configuration

### Database Commands
```bash
# Add migration
dotnet ef migrations add MigrationName --startup-project ../copilot-sample.Api

# Update database
dotnet ef database update --startup-project ../copilot-sample.Api

# Remove last migration
dotnet ef migrations remove --startup-project ../copilot-sample.Api
```

### Running the Application
```bash
# Build solution
dotnet build

# Run API (from Api directory)
cd copilot-sample.Api
dotnet run

# Run tests
dotnet test
```

## API Endpoints Overview

### Products API (`/api/Product`)
- `GET /api/Product` - Get all products
- `GET /api/Product/{id}` - Get product by ID
- `POST /api/Product` - Create new product
- `PUT /api/Product/{id}` - Update existing product
- `DELETE /api/Product/{id}` - Delete product

### Categories API (`/api/Category`)
- `GET /api/Category` - Get all categories
- `GET /api/Category/{id}` - Get category by ID
- `POST /api/Category` - Create new category
- `PUT /api/Category/{id}` - Update existing category
- `DELETE /api/Category/{id}` - Delete category

### Product Attributes API (`/api/ProductAttribute`)
- `GET /api/ProductAttribute/product/{productId}` - Get attributes for product
- `POST /api/ProductAttribute` - Add product attribute
- `PUT /api/ProductAttribute/{id}` - Update product attribute
- `DELETE /api/ProductAttribute/{id}` - Delete product attribute

## Best Practices for GitHub Copilot

### 1. **Context Awareness**
- Always reference the correct project layer when suggesting code
- Consider the existing entity relationships and constraints
- Respect the established naming conventions and patterns
- Use the correct namespace based on the file location

### 2. **Entity Framework Guidelines**
- Suggest proper async/await patterns for database operations
- Use Include() for loading related data when needed
- Recommend appropriate EF Core methods (Find, First, Single, etc.)
- Consider performance implications of queries

### 3. **API Design**
- Follow existing controller patterns and response formats
- Suggest proper validation and error handling
- Use consistent DTO mapping patterns
- Apply appropriate HTTP methods and status codes

### 4. **Testing Recommendations**
- Create comprehensive test coverage for new features
- Use the established testing patterns and dependencies
- Mock database context appropriately for unit tests
- Test edge cases and error scenarios

This project serves as a comprehensive example of modern .NET 8 API development with clean architecture, proper separation of concerns, and industry best practices for enterprise applications.
