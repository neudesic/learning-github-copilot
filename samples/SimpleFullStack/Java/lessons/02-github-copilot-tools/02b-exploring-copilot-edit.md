# Lab: Exploring GitHub Copilot Edit with Java Spring Boot

## Overview

**Goal:**  
Learn how to use GitHub Copilot Edit mode to make direct code modifications, refactoring, and improvements to your Java Spring Boot project through conversational editing.

**Estimated Duration:**  
20-25 minutes

**Prerequisites:**

- Completion of the "Exploring GitHub Copilot Ask" lab
- VS Code with GitHub Copilot extension
- Java Spring Boot sample project open

## Lab Steps

### 1. Access Copilot Edit Mode

- Press `Ctrl + Shift + I` to open Copilot Chat
- Select the Edit mode in the Copilot Chat window to enter edit mode
- Open any Java file in the project (e.g., `ProductController.java`)
- Type `#codebase` to ensure workspace context

### 2. Refactor Service Methods

Select the `ProductService` class and ask:

```text
#codebase /edit Refactor this Category service class to use proper exception handling and add logging. Also implement input validation for all public methods.
```

Review the proposed changes and apply them selectively.


### 3. Add New Code Implementation

Type the following prompt in Copilot Chat:

```text
#codebase In the ProductService class, add a method called findProductsByCategory that takes categoryId as a parameter and return list of products.
```

Review the proposed changes and apply them selectively.

Follow up with:

```text
Now add the corresponding REST endpoint in ProductController for the new service method findProductsByCategory with proper validation and response handling.
```

### 4. Unit Testing

Open any test file and ask:

```text
#codebase Add unit tests for the productService class and ensure all methods are covered. Include edge cases and exception scenarios.
```

### 5. Generating documentation

```text
#codebase Add OpenAPI 3.0 annotations to Product REST controller. Include detailed descriptions, example values, and error response documentation. Also update the Swagger configuration class to include these annotations.
```

### 6. Generate Docekr file

```text
#codebase Generate a Dockerfile for the Spring Boot application. Ensure it includes multi-stage builds for production and development environments. Also, include instructions for building and running the Docker container.
```

## Best Practices for Edit Mode

- **Review Changes Carefully:** Always examine suggestions before applying
- **Use Incremental Edits:** Make small, focused changes rather than large refactoring
- **Test After Changes:** Run tests after applying edits
- **Version Control:** Commit working code before major edits

## Summary

Copilot Edit mode provides powerful direct code modification capabilities that can significantly speed up refactoring and feature development while maintaining code quality.

## Additional Resources

[Learn more about Copilot Edit mode](https://code.visualstudio.com/docs/copilot/chat/copilot-edits)

© Copyright Neudesic 2025
