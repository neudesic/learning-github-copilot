# Lab: Testing with GitHub Copilot in Java Spring Boot

## Overview

**Goal:**  
Master using GitHub Copilot to generate comprehensive tests for Java Spring Boot applications including unit tests, integration tests, and test automation strategies.

**Estimated Duration:**  
25-30 minutes

**Prerequisites:**

- Java Spring Boot project
- GitHub Copilot enabled
- Understanding of JUnit 5 and testing concepts

## Lab Steps

### 1. Generate Unit Tests for Services

Open `ProductService.java` and ask Copilot:

```text
@workspace Generate comprehensive unit tests for this ProductService class. Include tests for all methods, edge cases, exception scenarios, and mock dependencies using Mockito.
```

### 2. Create Integration Tests

Ask Copilot to create integration tests:

```text
@workspace Create integration tests for the ProductController that test the full request-response cycle including database interactions. Use @SpringBootTest and TestContainers for real database testing.
```

### 3. Repository Testing

For repository layer testing:

```text
@workspace Generate @DataJpaTest tests for ProductRepository including custom query methods, pagination, and relationship testing.
```

### 4. Test Data Management

Ask for test data creation strategies:

```text
@workspace Create test data builders and factories for Product, Category, and ProductAttribute entities using the Builder pattern for clean test setup.
```

### 5. Performance Testing

For performance validation:

```text
@workspace Generate performance tests for the ProductService methods that measure execution time and identify potential bottlenecks.
```

### 6. API Contract Testing

For API testing:

```text
@workspace Create REST API tests using MockMvc that validate request/response contracts, status codes, and error handling for all ProductController endpoints.
```

## Test Categories Generated

1. **Unit Tests** - Fast, isolated tests with mocked dependencies
2. **Integration Tests** - End-to-end testing with real database
3. **Repository Tests** - Data layer testing
4. **API Tests** - Controller layer validation
5. **Performance Tests** - Execution time validation

## Running the Tests

Execute different test categories:

```bash
# Run all tests
./mvnw test

# Run only unit tests
./mvnw test -Dtest="*Test"

# Run only integration tests  
./mvnw test -Dtest="*IT"

# Generate test coverage report
./mvnw jacoco:report
```

## Best Practices

- **Test Naming:** Use descriptive test method names
- **Arrange-Act-Assert:** Follow the AAA pattern
- **Mock External Dependencies:** Use Mockito for isolation
- **Test Edge Cases:** Include boundary conditions
- **Parameterized Tests:** Use `@ParameterizedTest` for multiple scenarios

## Summary

GitHub Copilot can significantly accelerate test creation while maintaining quality and coverage. Always review and customize generated tests to match your specific requirements and testing standards.

© Copyright 2025
