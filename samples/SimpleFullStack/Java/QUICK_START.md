# Quick Start Guide

## 🚀 Fast Track - Get Running in 3 Steps

### Prerequisites
- Java 21+ installed
- Internet connection (for Maven dependencies)

### Steps

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

## 🛠️ Alternative Methods

### Method 1: Use the Batch Script (Windows)
```cmd
run.bat
```

### Method 2: Use the PowerShell Script
```powershell
.\run.ps1
```

### Method 3: Maven Direct Run
```powershell
.\mvnw.cmd spring-boot:run
```

---

## 📊 API Endpoints Summary

| Endpoint | Description |
|----------|-------------|
| `GET /api/categories` | List all categories |
| `GET /api/products` | List all products |
| `GET /api/products/search?name=laptop` | Search products |
| `GET /api/product-attributes/product/1` | Get product attributes |

---

## 🔧 Troubleshooting

**Problem**: Java not found
**Solution**: Set JAVA_HOME or add Java to PATH

**Problem**: Port 5000 in use  
**Solution**: Kill process or change port in application.properties

**Problem**: Build fails
**Solution**: Check internet connection and Java version

---

## 📖 Full Documentation
See [BUILD_AND_RUN.md](BUILD_AND_RUN.md) for complete instructions.
