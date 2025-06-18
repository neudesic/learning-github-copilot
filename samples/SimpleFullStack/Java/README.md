# Copilot Sample Java API

This is a Java Spring Boot migration of the original DotnetCoreApi solution. It provides a REST API for managing an inventory system with categories, products, and product attributes.

## Project Structure

The project follows a standard Spring Boot layered architecture:

```
src/main/java/com/copilot/sample/
├── CopilotSampleApiApplication.java    # Main application class
├── config/                             # Configuration classes
│   ├── DataInitializer.java           # Sample data initialization
│   └── OpenApiConfig.java             # Swagger/OpenAPI configuration
├── controller/                         # REST controllers
│   ├── CategoryController.java
│   ├── ProductController.java
│   └── ProductAttributeController.java
├── dto/                               # Data Transfer Objects
│   ├── AddCategoryDto.java
│   ├── AddProductDto.java
│   ├── CategoryDto.java
│   ├── ProductAttributeDto.java
│   ├── ProductDto.java
│   ├── UpdateCategoryDescriptionDto.java
│   └── UpdateProductDto.java
├── entity/                            # JPA entities
│   ├── Category.java
│   ├── Inventory.java
│   ├── Product.java
│   ├── ProductAttribute.java
│   ├── ProductPrice.java
│   └── ProductReview.java
├── exception/                         # Exception handling
│   └── GlobalExceptionHandler.java
├── mapper/                           # Entity-DTO mappers
│   ├── CategoryMapper.java
│   └── ProductMapper.java
├── repository/                       # Data access layer
│   ├── CategoryRepository.java
│   ├── ProductAttributeRepository.java
│   └── ProductRepository.java
└── service/                         # Business logic layer
    ├── CategoryService.java
    ├── ProductAttributeService.java
    ├── ProductService.java
    └── impl/
        ├── CategoryServiceImpl.java
        ├── ProductAttributeServiceImpl.java
        └── ProductServiceImpl.java
```

## Features

- **Category Management**: CRUD operations for product categories with hierarchical support
- **Product Management**: Full product lifecycle management with category relationships
- **Product Attributes**: Key-value attributes for products
- **Data Validation**: Input validation using Bean Validation
- **API Documentation**: Swagger/OpenAPI integration
- **Exception Handling**: Global exception handling with proper error responses
- **Database**: SQLite database with JPA/Hibernate

## Technology Stack

- **Java 17**
- **Spring Boot 3.2.0**
- **Spring Data JPA**
- **SQLite Database**
- **Hibernate**
- **Maven**
- **Swagger/OpenAPI 3**
- **Bean Validation**

## Prerequisites

- Java 17 or higher
- Maven 3.6 or higher

## Getting Started

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Java
   ```

2. **Build the project**
   ```bash
   mvn clean compile
   ```

3. **Run the application**
   ```bash
   mvn spring-boot:run
   ```

4. **Access the API**
   - Application: http://localhost:5000
   - Swagger UI: http://localhost:5000/swagger-ui.html
   - API Documentation: http://localhost:5000/api-docs

## API Endpoints

### Categories
- `GET /api/category` - Get all categories
- `GET /api/category/{id}` - Get category by ID
- `POST /api/category` - Create new category
- `PATCH /api/category/{id}/description` - Update category description
- `DELETE /api/category/{id}` - Delete category

### Products
- `GET /api/product` - Get all products
- `GET /api/product/{id}` - Get product by ID
- `POST /api/product` - Create new product
- `PUT /api/product/{id}` - Update product
- `DELETE /api/product/{id}` - Delete product
- `GET /api/product/category/{categoryId}` - Get products by category

### Product Attributes
- `GET /api/productattribute/product/{productId}` - Get product attributes
- `POST /api/productattribute` - Create product attribute
- `PUT /api/productattribute/{attributeId}` - Update product attribute
- `DELETE /api/productattribute/{attributeId}` - Delete product attribute
- `DELETE /api/productattribute/product/{productId}` - Delete all product attributes

## Database

The application uses SQLite as the database, which will be created automatically when the application starts. The database file `inventory.db` will be created in the project root directory.

Sample data is automatically initialized when the application starts with an empty database.

## Configuration

Application configuration can be found in `src/main/resources/application.properties`:

- Database configuration
- Server port (default: 5000)
- JPA/Hibernate settings
- API documentation paths

## Testing

Run the tests using Maven:

```bash
mvn test
```

## Migration from .NET Core API

This Java application is a faithful migration of the original .NET Core API with the following key mappings:

| .NET Core | Java Spring Boot |
|-----------|------------------|
| Controllers | @RestController |
| Services | @Service |
| Entity Framework | JPA/Hibernate |
| DTOs | DTOs with Bean Validation |
| Dependency Injection | @Autowired |
| Swagger | SpringDoc OpenAPI |
| SQLite | SQLite with Hibernate dialect |

## Original .NET Core API

The original .NET Core API can be found in the `DotnetCoreApi` folder. This Java implementation maintains the same:
- API endpoints and response formats
- Business logic and validation rules
- Database schema and relationships
- Error handling patterns

## Development

To continue development:

1. **Add new features**: Follow the existing layered architecture
2. **Database changes**: Update entities and let Hibernate handle schema updates
3. **API changes**: Update controllers and DTOs, regenerate API documentation
4. **Testing**: Add unit and integration tests in the `src/test` directory

## License

This project maintains the same license as the original .NET Core API.
