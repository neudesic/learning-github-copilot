# Accelerating Java Spring Boot API Development with GitHub Copilot Agent

## Overview

**Goal:**  
Learn how to leverage GitHub Copilot Agent to autonomously implement complex features and architectural changes in a Java Spring Boot Web API project using VS Code.

**Estimated Duration:**  
25-30 minutes

**Audience:**  
Java Spring Boot developers, backend engineers.

**Prerequisites:**

- VS Code installed (latest version)
- GitHub Copilot subscription with Agent capabilities enabled
- GitHub Copilot Chat extension
- Access to the Java Spring Boot sample project
- Basic familiarity with Spring Boot, JPA, and REST API concepts
- Java 21 and Maven installed

## Lab Description

GitHub Copilot Agent represents the most powerful mode in Copilot Chat, enabling autonomous planning and execution across your entire Java project. With Agent mode, you provide a high-level prompt and Copilot independently selects the right files, runs necessary Maven commands, and applies code edits until the task is complete. Unlike Edit mode, Agent analyzes related Spring Boot components and identifies additional changes needed across the project to maintain consistency.

What distinguishes Agent mode is its ability to work autonomously - applying edits automatically rather than waiting for explicit approval at each step, while still surfacing potentially risky commands for review. This creates a continuous-edit "driver" model where you define the goal and Copilot executes updates without interruption.

For maximum effectiveness, Agent mode works best with custom instructions that define your project structure, Spring Boot conventions, and coding standards. These instructions provide a stronger foundation for Copilot to work from, resulting in more consistent and aligned outcomes across multiple sessions.

## Lab Steps

### 1. Launch Copilot Agent and Enable Agent Mode

- Open VS Code with the Java Spring Boot project.
- Navigate to the Copilot Chat panel by pressing `Ctrl + Shift + I` or clicking the Copilot Chat icon.
- Click on the settings icon in the chat panel and enable "Agent mode" if available.
- Alternatively, use the `@agent` prefix in your prompts to invoke agent capabilities.

### 2. Implement a RESTful API Controller and Service

In the Copilot Chat panel, enter the following prompt:

```text
@workspace @agent Create a ProductReviews API controller with standard CRUD operations (GET, POST, PUT, DELETE). 
Add a service named ProductReviewService that implements a ProductReviewService interface.
The service should handle database operations with JPA repositories.
Add necessary DTO models with proper Bean Validation annotations.
Register everything in the Spring Boot dependency injection container.
Create the ProductReview entity with proper JPA annotations and relationships.
Build the project after completion.
```

What you'll observe:

- Copilot Agent will analyze the existing Spring Boot project structure
- It will create controller, service interface, and implementation files
- It will generate appropriate DTO models with validation annotations (@Valid, @NotNull, etc.)
- It will create the ProductReview entity with JPA annotations
- It will create a JPA repository interface
- It will follow existing project patterns for consistent implementation
- It will use proper Spring Boot annotations (@RestController, @Service, @Repository)
- It will automatically build the project after completing the changes

After completion, build the project to verify there are no compilation errors:

```bash
./mvnw compile
```

```text
@workspace @agent Add a search endpoint to ProductController that allows searching products by name, category, and price range. Use Spring Data JPA Specifications for dynamic query building.
Create a ProductSearchCriteria DTO with proper validation annotations for the search parameters.
Add unit tests for the functionality. Build the project after completion and test the endpoint.
```

What you'll observe:

- Copilot Agent will create a new search endpoint in the ProductController
- It will implement the search logic using Spring Data JPA Specifications
- It will generate a ProductSearchCriteria DTO with validation annotations
- It will add unit tests for the new search functionality
- It will automatically build the project after completing the changes

### 3. Generate JUnit Tests for Your API

Enter this prompt in Copilot Chat:

```text
@workspace @agent Create comprehensive JUnit 5 tests for the ProductService in Service folder and ProductController in Controller folder.
Create integration tests for the ProductController that test the full request-response cycle including database interactions. Use @SpringBootTest and TestContainers for real database testing.
Create test data builders and factories for entities using the Builder pattern for clean test setup.
Include tests for all public methods with various scenarios.
Use Mockito for mocking dependencies including the JPA repository.
Follow AAA (Arrange-Act-Assert) pattern in test methods.
Include test data builders for clean test setup.
```

What you'll observe:

- Copilot Agent will analyze the Service implementation
- It will create comprehensive unit tests with Mockito mocks
- It will generate integration tests for the controller layer
- It will create test data builders using the builder pattern
- Tests will cover normal operation, edge cases, and error conditions
- It will use proper Spring Boot testing annotations (@MockBean, @WebMvcTest, etc.)

### 4. Generate Containerization Configuration

Enter this prompt in Copilot Chat:

```text
@workspace @agent I need to containerize this Java Spring Boot API for Kubernetes deployment.
Please help me generate:
1. A multi-stage Dockerfile optimized for Spring Boot with Maven
2. A .dockerignore file to optimize build context
Please explain each configuration element and include health checks.
```

What you'll observe:

- Copilot will analyze the Maven project structure to create appropriate Docker configurations
- You'll receive a detailed explanation of each Dockerfile instruction
- You'll get complementary configuration files for container orchestration
- Health check endpoints will be configured using Spring Boot Actuator

### 6. Add API Documentation with OpenAPI

Enter this prompt in Copilot Chat:

```text
@workspace @agent Add comprehensive API documentation to this Spring Boot project:
1. Configure Springdoc OpenAPI for automatic documentation generation
2. Add detailed @Operation, @ApiResponse, and @Schema annotations to controllers
3. Create API documentation examples and descriptions
4. Configure Swagger UI with custom styling and information
5. Add security scheme documentation for future authentication
6. Generate example request/response payloads for all endpoints
```

## Best Practices for Using Copilot Agent with Java Spring Boot APIs

When leveraging GitHub Copilot Agent for Spring Boot development, consider these best practices:

- **Provide application and technology speicific Context:** Mention key Spring features like Spring Data JPA, Spring Security, Spring Cache, or Spring Boot Actuator.
- **Break Large Tasks into Steps:** For complex APIs, guide Copilot with a numbered list of implementation steps.
- **Specify Technology Preferences:** Be explicit about technologies like Hibernate, MapStruct, Bean Validation, or Testcontainers.
- **Review Changes Incrementally:** Pause between major feature additions to review and test endpoints.
- **Ask for Documentation:** Request that Copilot add JavaDoc comments and OpenAPI annotations for your REST API.
- **Guide Code Style:** Specify Java coding conventions, especially for package structure and naming conventions.
- **Include Testing Strategy:** Always request corresponding tests when implementing new features.

## Advanced Agent Scenarios

## Verification Commands

After each major implementation, build and test the changes with these commands:

```bash
# Compile the project
./mvnw compile

# Run tests
./mvnw test

# Run the application
./mvnw spring-boot:run

```

## Summary

By completing this lab, you've learned to:

- Use GitHub Copilot Agent to implement complete RESTful APIs autonomously in Spring Boot
- Generate controllers, services, entities, and DTOs with proper annotations and dependency injection
- Create comprehensive JUnit tests for your Spring Boot components
- Set up containerization and orchestration for your Spring Boot API
- Implement structured logging for HTTP request/response tracking
- Add API documentation with OpenAPI and Swagger UI
- Implement caching strategies for performance optimization
- Maintain Spring Boot architectural consistency throughout your project

These capabilities demonstrate how GitHub Copilot Agent can dramatically accelerate Spring Boot development by handling complex, multi-file changes that would typically require significant manual effort while following Java and Spring Boot best practices.

## Additional Resources

- [Spring Boot Documentation](https://spring.io/projects/spring-boot)
- [GitHub Copilot Agent Documentation](https://docs.github.com/en/copilot)
- [Spring Boot Testing Guide](https://spring.io/guides/gs/testing-web/)
- [Docker Best Practices for Java](https://docs.docker.com/language/java/build-images/)

---

© Copyright Neudesic 2025
