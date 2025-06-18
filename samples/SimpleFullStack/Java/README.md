# Java Spring Boot API with Swagger Documentation

A comprehensive Spring Boot REST API demonstrating CRUD operations with interactive Swagger documentation, built for GitHub Copilot learning scenarios.

## 🚀 Quick Start (3 Steps)

### Prerequisites
- **Java 21+** installed
- Internet connection (for Maven dependencies)

### Get Running Fast

#### 1. Build the Application
```powershell
.\mvnw.cmd clean package -DskipTests
```

#### 2. Run the Application
```powershell
java -jar target\copilot-sample-api-1.0.0.jar
```

#### 3. Access Swagger UI
Open your browser to: **http://localhost:5000/swagger-ui/index.html**

---

## 📋 Table of Contents

- [Project Overview](#-project-overview)
- [Prerequisites](#-prerequisites)
- [Project Structure](#-project-structure)
- [Build Instructions](#-build-instructions)
- [Run Instructions](#-run-instructions)
- [API Documentation](#-api-documentation)
- [Testing the Application](#-testing-the-application)
- [Troubleshooting](#-troubleshooting)
- [Development Tips](#-development-tips)

---

## 🎯 Project Overview

This Spring Boot application provides a RESTful API for managing:
- **Categories** - Product category management
- **Products** - Product catalog with relationships
- **Product Attributes** - Extended product properties

### Key Features
- **Spring Boot 3.2.0** with Java 21
- **SQLite Database** (file-based, no setup required)
- **Interactive Swagger UI** for API testing
- **Comprehensive CRUD Operations**
- **Search and Filter Capabilities**
- **Input Validation** with Jakarta Bean Validation
- **Auto-generated Documentation**

---

## ✅ Prerequisites

### Required Software
- **Java Development Kit (JDK) 21 or higher**
- **Maven 3.6+** (or use the included Maven wrapper)
- **Git** (for version control)

### Java Installation Verification
```powershell
java -version
```
Expected output should show Java 21 or higher.

If Java is not found, ensure `JAVA_HOME` is set:
```powershell
$env:JAVA_HOME
```

---

## 📁 Project Structure

```
Java/
├── src/
│   ├── main/
│   │   ├── java/
│   │   │   └── com/copilot/sample/
│   │   │       ├── CopilotSampleApiApplication.java    # Main application class
│   │   │       ├── config/
│   │   │       │   └── OpenApiConfig.java              # Swagger configuration
│   │   │       ├── controller/                         # REST controllers
│   │   │       │   ├── CategoryController.java
│   │   │       │   ├── ProductController.java
│   │   │       │   └── ProductAttributeController.java
│   │   │       ├── dto/                               # Data Transfer Objects
│   │   │       ├── entity/                            # JPA entities
│   │   │       ├── repository/                        # Data repositories
│   │   │       ├── service/                           # Business logic
│   │   │       └── mapper/                            # Entity-DTO mappers
│   │   └── resources/
│   │       └── application.properties                 # Configuration
├── src/test/                                          # Test classes
├── target/                                            # Build output (generated)
├── pom.xml                                            # Maven dependencies
├── mvnw / mvnw.cmd                                    # Maven wrapper
├── run.bat / run.ps1                                  # Convenience scripts
└── README.md                                          # This file
```

---

## 🔨 Build Instructions

### Option 1: Using Maven Wrapper (Recommended)

1. **Navigate to project directory**:
   ```powershell
   cd ".\samples\SimpleFullStack\Java"
   ```

2. **Clean and build**:
   ```powershell
   .\mvnw.cmd clean package -DskipTests
   ```

### Option 2: Using System Maven

If Maven is installed globally:
```powershell
mvn clean package -DskipTests
```

### Build Output
After successful build:
- ✅ `BUILD SUCCESS` message
- 📦 Generated JAR: `target/copilot-sample-api-1.0.0.jar`

---

## ▶️ Run Instructions

### Method 1: JAR File (Recommended)
```powershell
java -jar target\copilot-sample-api-1.0.0.jar
```

### Method 2: Maven Spring Boot Plugin
```powershell
.\mvnw.cmd spring-boot:run
```

### Method 3: Convenience Scripts

**Windows Batch:**
```cmd
run.bat
```

**PowerShell:**
```powershell
.\run.ps1
```

### Method 4: With Specific Java Path

If you encounter Java path issues:

```powershell
& "C:\Program Files\JetBrains\IntelliJ IDEA 2025.1.2\jbr\bin\java.exe" -jar target/copilot-sample-api-1.0.0.jar
```

---

## 🌐 Application Access

### Startup Confirmation

Look for this in the console:

```
  .   ____          _            __ _ _
 /\\ / ___'_ __ _ _(_)_ __  __ _ \ \ \ \
( ( )\___ | '_ | '_| | '_ \/ _` | \ \ \ \
 \\/  ___)| |_)| | | | | || (_| |  ) ) ) )
  '  |____| .__|_| |_|_| |_\__, | / / / /
 =========|_|==============|___/=/_/_/_/
 :: Spring Boot ::                (v3.2.0)

Tomcat started on port 5000 (http) with context path ''
Started CopilotSampleApiApplication in X.XXX seconds
```

### Base URLs

- **Application Root**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger-ui/index.html
- **OpenAPI JSON**: http://localhost:5000/v3/api-docs
- **Health Check**: http://localhost:5000/actuator/health

---

## 📚 API Documentation

### 🎯 API Endpoints Summary

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/categories` | GET | List all categories |
| `/api/categories/{id}` | GET | Get category by ID |
| `/api/categories` | POST | Create new category |
| `/api/categories/{id}` | PUT | Update category |
| `/api/categories/{id}` | DELETE | Delete category |
| `/api/products` | GET | List all products |
| `/api/products/{id}` | GET | Get product by ID |
| `/api/products/category/{categoryId}` | GET | Get products by category |
| `/api/products/search?name={name}` | GET | Search products by name |
| `/api/products` | POST | Create new product |
| `/api/products/{id}` | PUT | Update product |
| `/api/products/{id}` | DELETE | Delete product |
| `/api/product-attributes/product/{productId}` | GET | Get attributes by product |
| `/api/product-attributes/{id}` | GET | Get attribute by ID |
| `/api/product-attributes` | POST | Create new attribute |
| `/api/product-attributes/{id}` | PUT | Update attribute |
| `/api/product-attributes/{id}` | DELETE | Delete attribute |

### 🔧 Interactive Swagger UI

Access the Swagger UI at: **http://localhost:5000/swagger-ui/index.html**

**Features:**

- 🧪 **Interactive Testing** - Try APIs directly in browser
- 📖 **Auto-generated Documentation** - Complete API specs
- 🔍 **Schema Explorer** - View request/response models
- 📝 **Request Examples** - Copy-paste ready examples
- ✅ **Response Validation** - See expected responses

### 📄 OpenAPI Specification

- **JSON Format**: http://localhost:5000/v3/api-docs
- **YAML Format**: http://localhost:5000/v3/api-docs.yaml

---

## 🧪 Testing the Application

### Quick API Test

```powershell
# Test if application is running
curl http://localhost:5000/api/categories

# Or open in browser
start http://localhost:5000/api/categories
```

### Using Swagger UI (Recommended)

1. 🌐 Open: http://localhost:5000/swagger-ui/index.html
2. 📂 Explore API groups:
   - **Categories** - Category management
   - **Products** - Product catalog
   - **Product Attributes** - Extended properties
3. 🧪 Use "Try it out" for interactive testing

### Sample API Calls

**Get all products:**

```bash
GET http://localhost:5000/api/products
```

**Search products:**

```bash
GET http://localhost:5000/api/products/search?name=laptop
```

**Create a category:**

```bash
POST http://localhost:5000/api/categories
Content-Type: application/json

{
  "name": "Electronics",
  "description": "Electronic devices and accessories"
}
```

---

## ❗ Troubleshooting

### Common Issues & Solutions

#### 🔴 Java Not Found

**Error**: `java: The term 'java' is not recognized`

**Solutions**:

```powershell
# Check Java installation
java -version

# Check JAVA_HOME
echo $env:JAVA_HOME

# Set JAVA_HOME if needed
$env:JAVA_HOME = "C:\Program Files\Java\jdk-21"

# Add to PATH
$env:PATH += ";$env:JAVA_HOME\bin"
```

#### 🔴 Maven Wrapper Permission Issues

**Error**: Permission denied on `mvnw`

**Solution**:

```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

#### 🔴 Port Already in Use

**Error**: `Port 5000 is already in use`

**Solutions**:

```powershell
# Find process using port 5000
netstat -ano | findstr :5000

# Kill specific process (replace PID)
taskkill /PID <PID> /F

# Or change port in application.properties
server.port=8080
```

#### 🔴 Database Lock Issues

**Error**: Database locked

**Solutions**:

- Stop application properly (Ctrl+C)
- Delete SQLite database file if corrupted
- Restart the application

#### 🔴 Build Failures

**Error**: Compilation errors

**Solutions**:

```powershell
# Clean build
.\mvnw.cmd clean compile

# Verbose build
.\mvnw.cmd clean package -X

# Skip tests temporarily
.\mvnw.cmd clean package -DskipTests
```

### 🔧 Enable Debug Logging

Add to `application.properties`:

```properties
logging.level.com.copilot.sample=DEBUG
logging.level.org.springframework.web=DEBUG
logging.level.org.springframework.data=DEBUG
```

---

## 💡 Development Tips

### 🔄 Run the applicaiton

```powershell
.\mvnw.cmd spring-boot:run
```

### 🏗️ Production Build

```powershell
.\mvnw.cmd clean package -Pproduction
```

### 🧪 Running Tests

```powershell
# All tests
.\mvnw.cmd test

# Specific test class
.\mvnw.cmd test -Dtest=CategoryTest

```

### 📊 Database Management

- **Type**: SQLite (file-based)
- **Location**: Auto-created in project directory
- **Reset**: Delete `.db` file and restart
- **View**: Use SQLite browser tools

---

## 🛠️ Technology Stack

### Core Framework

- **Spring Boot 3.2.0** - Main application framework
- **Spring Data JPA** - Database abstraction layer
- **Spring Web** - REST API framework
- **Spring Validation** - Input validation

### Database & ORM

- **SQLite** - Lightweight file-based database
- **Hibernate** - JPA implementation
- **H2** - Alternative in-memory database option

### Documentation & Testing

- **springdoc-openapi** - Swagger/OpenAPI integration
- **JUnit 5** - Testing framework
- **Mockito** - Mocking framework
- **Spring Boot Test** - Integration testing

### Build & Deployment

- **Maven** - Dependency management and build
- **Maven Wrapper** - Version-locked Maven
- **Spring Boot DevTools** - Development utilities

---

## 📖 Additional Resources

### Configuration Files

- **`pom.xml`** - Maven dependencies and build configuration
- **`application.properties`** - Application configuration
- **`OpenApiConfig.java`** - Swagger documentation setup

### Learning Resources

- [Spring Boot Documentation](https://docs.spring.io/spring-boot/docs/current/reference/htmlsingle/)
- [Spring Data JPA Reference](https://docs.spring.io/spring-data/jpa/docs/current/reference/html/)
- [OpenAPI 3 Specification](https://swagger.io/specification/)

---

## 🎓 GitHub Copilot Learning Labs

This project includes comprehensive GitHub Copilot learning materials:

- **Lab 02a**: Copilot Ask (15-20 min) - Basic questioning and exploration
- **Lab 02b**: Copilot Edit (20-25 min) - Targeted code modifications  
- **Lab 02c**: Copilot Agent (25-30 min) - Autonomous development
- **Lab 02d**: Copilot Inline (25-30 min) - Inline code generation and suggestions

Find all labs in the `lessons/` directory.

---

## 📞 Support

If you encounter issues:

1. 📖 Check this README troubleshooting section
2. 🔍 Review application logs in console
3. 🌐 Test with Swagger UI for API validation
4. 🧪 Verify with simple curl commands
5. 🔄 Try clean build: `.\mvnw.cmd clean package`

---

**Happy Coding! 🚀**
*Built with ❤️ using Java, Spring Boot and GitHub Copilot*

## 📝 License

copyright © Neudesic 2025

