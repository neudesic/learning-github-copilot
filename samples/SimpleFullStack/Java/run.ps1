# Build and Run Java Spring Boot Application
# PowerShell script for building and running the Java application with Swagger documentation

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Java Spring Boot Application - Build and Run" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Function to check command availability
function Test-Command($CommandName) {
    return [bool](Get-Command $CommandName -ErrorAction SilentlyContinue)
}

# Check Java installation
Write-Host "[1/4] Checking Java installation..." -ForegroundColor Yellow
if (Test-Command "java") {
    $javaVersion = java -version 2>&1 | Select-String "version" | Select-Object -First 1
    Write-Host "✓ Java is available: $javaVersion" -ForegroundColor Green
} else {
    Write-Host "✗ ERROR: Java not found in PATH" -ForegroundColor Red
    Write-Host "Please ensure Java 21+ is installed and added to PATH" -ForegroundColor Red
    Write-Host "Or set JAVA_HOME environment variable" -ForegroundColor Red
    Write-Host "Current JAVA_HOME: $env:JAVA_HOME" -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host ""

# Clean previous build
Write-Host "[2/4] Cleaning previous build..." -ForegroundColor Yellow
try {
    if (Test-Path "target") {
        Remove-Item -Path "target" -Recurse -Force
        Write-Host "✓ Clean completed" -ForegroundColor Green
    } else {
        Write-Host "✓ No previous build to clean" -ForegroundColor Green
    }
} catch {
    Write-Host "⚠ WARNING: Clean failed, continuing..." -ForegroundColor Yellow
}

Write-Host ""

# Build application
Write-Host "[3/4] Building application..." -ForegroundColor Yellow
try {
    $buildResult = & .\mvnw.cmd clean package -DskipTests 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Build successful!" -ForegroundColor Green
    } else {
        Write-Host "✗ ERROR: Build failed!" -ForegroundColor Red
        Write-Host $buildResult -ForegroundColor Red
        Read-Host "Press Enter to exit"
        exit 1
    }
} catch {
    Write-Host "✗ ERROR: Build command failed!" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host ""

# Start application
Write-Host "[4/4] Starting application..." -ForegroundColor Yellow
Write-Host ""
Write-Host "Application will start on: " -NoNewline -ForegroundColor White
Write-Host "http://localhost:5000" -ForegroundColor Cyan
Write-Host "Swagger UI available at: " -NoNewline -ForegroundColor White  
Write-Host "http://localhost:5000/swagger-ui/index.html" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Run the application
try {
    java -jar "target\copilot-sample-api-1.0.0.jar"
} catch {
    Write-Host "✗ ERROR: Failed to start application!" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}
