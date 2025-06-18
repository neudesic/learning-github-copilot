# Unit Tests for Entities and Mappers

This document provides an overview of the comprehensive unit tests created for the Java Spring Boot application's entities and mappers.

## Test Coverage Summary

### Entity Tests

#### 1. CategoryTest
- **File**: `CategoryTest.java`
- **Coverage**: 10 test methods
- **Tests Include**:
  - Default and parameterized constructor validation
  - Getter/setter functionality for all fields
  - Null value handling
  - Empty collection handling
  - List operations for products and sub-categories

#### 2. ProductTest
- **File**: `ProductTest.java`
- **Coverage**: 17 test methods
- **Tests Include**:
  - Default and parameterized constructor validation
  - All field getter/setter operations
  - BigDecimal price handling
  - LocalDateTime timestamp handling
  - Category relationship testing
  - Product attributes relationship testing
  - Null value handling
  - Default value validation (isActive = true)

#### 3. ProductAttributeTest
- **File**: `ProductAttributeTest.java`
- **Coverage**: 11 test methods
- **Tests Include**:
  - Constructor validation
  - All field operations
  - Product relationship handling
  - String value constraints
  - Special character handling
  - Long text value handling

### Mapper Tests

#### 1. CategoryMapperTest
- **File**: `CategoryMapperTest.java`
- **Coverage**: 13 test methods
- **Tests Include**:
  - Entity to DTO mapping
  - DTO to Entity mapping
  - Sub-categories mapping with `mapToDtoWithSubCategories()`
  - List mapping operations
  - Null handling for all scenarios
  - Empty collection handling

#### 2. ProductMapperTest
- **File**: `ProductMapperTest.java`
- **Coverage**: 14 test methods
- **Tests Include**:
  - Product entity to DTO mapping
  - DTO to entity mapping
  - Category relationship mapping
  - List operations
  - Field exclusion validation (price, timestamps not mapped to DTO)
  - Null value handling
  - Edge cases for category mapping

#### 3. ProductAttributeMapperTest
- **File**: `ProductAttributeMapperTest.java`
- **Coverage**: 3 test methods
- **Tests Include**:
  - Bidirectional relationship validation
  - Data type flexibility testing
  - Constraint validation

## Testing Framework

- **Framework**: JUnit 5
- **Assertion Library**: AssertJ
- **Total Tests**: 68 tests
- **Test Result**: All tests passing ✅

## Key Testing Patterns Used

### 1. Boundary Testing
- Null values
- Empty collections
- Empty strings
- Large text values

### 2. Relationship Testing
- Entity relationships (@OneToMany, @ManyToOne)
- Bidirectional mappings
- Foreign key consistency

### 3. Data Type Testing
- BigDecimal precision
- LocalDateTime handling
- Boolean default values
- String constraints

### 4. Mapper Validation
- Field mapping accuracy
- Null safety
- Collection handling
- Excluded field verification

## Running the Tests

```bash
# Using Maven wrapper
./mvnw test

# Or with regular Maven
mvn test
```

## Test Structure

All tests follow a consistent structure:
1. **@DisplayName** annotations for clear test descriptions
2. **Given-When-Then** pattern with clear comments
3. **BeforeEach** setup methods where needed
4. **AssertJ** assertions for readable test validation

## Coverage Areas

### Entity Coverage
- ✅ All constructors
- ✅ All getters and setters
- ✅ Relationship mappings
- ✅ Edge cases and null handling
- ✅ Data type validation

### Mapper Coverage
- ✅ Entity to DTO conversion
- ✅ DTO to Entity conversion
- ✅ List operations
- ✅ Null safety
- ✅ Field mapping accuracy
- ✅ Relationship preservation

This comprehensive test suite ensures the reliability and robustness of the application's core data layer components.
