# Java Spring Boot Application - Build and Run Instructions

This document provides step-by-step instructions for building and running the Java Spring Boot application with Swagger documentation.

## Prerequisites

### Required Software
- **Java Development Kit (JDK) 21 or higher**
- **Maven 3.6+ or use the included Maven wrapper**
- **Git** (for version control)

### Java Installation Verification
Check if Java is properly installed:
```powershell
java -version
```
Expected output should show Java 21 or higher.

If Java is not found, ensure `JAVA_HOME` is set:
```powershell
$env:JAVA_HOME
```

## Project Structure

```
Java/
├── src/
│   ├── main/
│   │   ├── java/
│   │   │   └── com/copilot/sample/
│   │   │       ├── CopilotSampleApiApplication.java
│   │   │       ├── config/
│   │   │       │   └── OpenApiConfig.java
│   │   │       ├── controller/
│   │   │       │   ├── CategoryController.java
│   │   │       │   ├── ProductController.java
│   │   │       │   └── ProductAttributeController.java
│   │   │       ├── dto/
│   │   │       ├── entity/
│   │   │       ├── repository/
│   │   │       └── service/
│   │   └── resources/
│   │       └── application.properties
├── target/ (generated after build)
├── pom.xml
├── mvnw (Maven wrapper for Unix)
├── mvnw.cmd (Maven wrapper for Windows)
└── BUILD_AND_RUN.md (this file)
```

## Build Instructions

### Option 1: Using Maven Wrapper (Recommended)

1. **Open PowerShell/Command Prompt** and navigate to the project directory:
   ```powershell
   cd ".\samples\SimpleFullStack\Java"
   ```

2. **Clean and build the application**:
   ```powershell
   .\mvnw clean package -DskipTests
   ```

### Option 2: Using System Maven

If you have Maven installed globally:
```powershell
mvn clean package -DskipTests
```

### Build Output
After successful build, you should see:
- `BUILD SUCCESS` message
- Generated JAR file: `target/copilot-sample-api-1.0.0.jar`

## Run Instructions

### Method 1: Using Maven Wrapper (Development)

```powershell
.\mvnw spring-boot:run
```

### Method 2: Using JAR File (Production-like)

1. **Build the application first** (if not already done):
   ```powershell
   .\mvnw clean package -DskipTests
   ```

2. **Run the JAR file**:
   ```powershell
   java -jar target/copilot-sample-api-1.0.0.jar
   ```

### Method 3: With Specific Java Path (if JAVA_HOME issues)

If you encounter Java path issues:
```powershell
& "C:\Program Files\JetBrains\IntelliJ IDEA 2025.1.2\jbr\bin\java.exe" -jar target/copilot-sample-api-1.0.0.jar
```

## Application Startup

### Expected Startup Logs
You should see output similar to:
```
  .   ____          _            __ _ _
 /\\ / ___'_ __ _ _(_)_ __  __ _ \ \ \ \
( ( )\___ | '_ | '_| | '_ \/ _` | \ \ \ \
 \\/  ___)| |_)| | | | | || (_| |  ) ) ) )
  '  |____| .__|_| |_|_| |_\__, | / / / /
 =========|_|==============|___/=/_/_/_/
 :: Spring Boot ::                (v3.2.0)

...
Tomcat started on port 5000 (http) with context path ''
Started CopilotSampleApiApplication in X.XXX seconds
```

### Application Details
- **Port**: 5000 (configured in application.properties)
- **Context Path**: `/` (root)
- **Database**: SQLite (file-based, created automatically)
- **Framework**: Spring Boot 3.2.0 with Java 21

## Accessing the Application

### Base URLs
- **Application Root**: http://localhost:5000
- **Health Check**: http://localhost:5000/actuator/health (if actuator is enabled)

### API Endpoints

#### Categories API
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get category by ID
- `POST /api/categories` - Create new category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category

#### Products API
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/category/{categoryId}` - Get products by category
- `GET /api/products/search?name={name}` - Search products by name
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

#### Product Attributes API
- `GET /api/product-attributes/product/{productId}` - Get attributes by product
- `GET /api/product-attributes/{id}` - Get attribute by ID
- `POST /api/product-attributes` - Create new attribute
- `PUT /api/product-attributes/{id}` - Update attribute
- `DELETE /api/product-attributes/{id}` - Delete attribute

### Swagger Documentation

#### Swagger UI (Interactive Documentation)
- **URL**: http://localhost:5000/swagger-ui/index.html
- **Features**: 
  - Interactive API testing
  - Request/response examples
  - Schema documentation
  - Try-it-out functionality

#### OpenAPI Specification
- **JSON Format**: http://localhost:5000/v3/api-docs
- **YAML Format**: http://localhost:5000/v3/api-docs.yaml

## Testing the Application

### Quick API Test
Test if the application is running:
```powershell
curl http://localhost:5000/api/categories
```

Or open in browser:
```
http://localhost:5000/api/categories
```

### Using Swagger UI
1. Open browser and navigate to: http://localhost:5000/swagger-ui/index.html
2. Explore the available APIs grouped by:
   - **Categories** - Category management APIs
   - **Products** - Product management APIs  
   - **Product Attributes** - Product attribute management APIs
3. Use the "Try it out" button to test endpoints interactively

## Troubleshooting

### Common Issues

#### 1. Java Not Found
**Error**: `java: The term 'java' is not recognized`

**Solution**:
- Verify Java installation: `java -version`
- Check JAVA_HOME: `echo $env:JAVA_HOME`
- Use full path to java.exe if needed

#### 2. Maven Wrapper Permission Issues
**Error**: Permission denied on `mvnw`

**Solution** (Windows):
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

#### 3. Port Already in Use
**Error**: `Port 5000 is already in use`

**Solutions**:
- Kill process using port 5000: `netstat -ano | findstr :5000`
- Change port in `application.properties`: `server.port=8080`
- Stop other applications using port 5000

#### 4. Database Lock Issues
**Error**: Database locked

**Solution**:
- Stop the application properly (Ctrl+C)
- Delete the SQLite database file if corrupted
- Restart the application

#### 5. Build Failures
**Error**: Compilation errors

**Solutions**:
- Clean build: `.\mvnw clean compile`
- Check Java version compatibility
- Verify all dependencies in `pom.xml`

### Logs and Debugging

#### Enable Debug Logging
Add to `application.properties`:
```properties
logging.level.com.copilot.sample=DEBUG
logging.level.org.springframework.web=DEBUG
```

#### View Detailed Startup Logs
Run with verbose output:
```powershell
.\mvnw spring-boot:run -X
```

## Development Tips

### Hot Reload (Development Mode)
For faster development cycles:
```powershell
.\mvnw spring-boot:run -Dspring-boot.run.jvmArguments="-Dspring.devtools.restart.enabled=true"
```

### Building for Production
Create an optimized build:
```powershell
.\mvnw clean package -Pproduction
```

### Running Tests
Execute all tests:
```powershell
.\mvnw test
```

## Additional Resources

### Project Dependencies
Key libraries used:
- **Spring Boot 3.2.0** - Main framework
- **Spring Data JPA** - Database access
- **Hibernate** - ORM framework
- **SQLite** - Database
- **springdoc-openapi** - Swagger/OpenAPI documentation
- **Jakarta Validation** - Input validation

### Configuration Files
- `pom.xml` - Maven dependencies and build configuration
- `application.properties` - Application configuration
- `OpenApiConfig.java` - Swagger documentation configuration

### Database
- **Type**: SQLite (file-based)
- **Location**: Created automatically in project directory
- **Tables**: Categories, Products, Product Attributes
- **Sample Data**: Automatically loaded on startup

---

## Quick Start Summary

1. **Prerequisites**: Ensure Java 21+ is installed
2. **Build**: `.\mvnw clean package -DskipTests`
3. **Run**: `java -jar target/copilot-sample-api-1.0.0.jar`
4. **Access**: Open http://localhost:5000/swagger-ui/index.html
5. **Test**: Use Swagger UI to interact with APIs

The application will start on port 5000 with full Swagger documentation available for interactive API testing.
