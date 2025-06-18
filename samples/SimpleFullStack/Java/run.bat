@echo off
echo Building and running Copilot Sample Java API...

REM Check if Java is installed
java -version >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo Error: Java is not installed or not in PATH.
    echo Please install Java 17 or higher from https://adoptium.net/
    echo.
    echo Quick install with Chocolatey:
    echo   choco install openjdk17
    echo.
    pause
    exit /b 1
)

REM Check if Maven is available, use wrapper if not
mvn -version >nul 2>&1
if %ERRORLEVEL% equ 0 (
    set MVN_CMD=mvn
    echo Using system Maven...
) else (
    set MVN_CMD=mvnw.cmd
    echo Using Maven Wrapper...
)

REM Clean and compile the project
echo Building project...
%MVN_CMD% clean compile

REM Check if build was successful
if %ERRORLEVEL% equ 0 (
    echo Build successful. Starting application...
    echo Application will be available at: http://localhost:5000
    echo Swagger UI will be available at: http://localhost:5000/swagger-ui.html
    echo.
    echo Press Ctrl+C to stop the application
    echo.
    
    REM Run the Spring Boot application
    %MVN_CMD% spring-boot:run
) else (
    echo Build failed. Please check the error messages above.
    pause
    exit /b 1
)
