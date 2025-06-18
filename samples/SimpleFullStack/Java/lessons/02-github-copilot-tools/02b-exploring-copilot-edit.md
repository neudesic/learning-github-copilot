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

- Open any Java file in the project (e.g., `ProductController.java`)
- Press `Ctrl + Shift + I` to open Copilot Chat
- Type `@workspace` to ensure workspace context
- Select the Edit mode in the Copilot Chat window to enter edit mode

### 2. Refactor Service Methods

Select the `ProductService` class and ask:

```text
@workspace /edit Refactor this service class to use proper exception handling and add logging. Also implement input validation for all public methods.
```

Review the proposed changes and apply them selectively.

### 3. Add New Features

In the `CategoryController`, in the Copilot Chat, type:

```text
@workspace /edit Add a new endpoint to get category hierarchy with all subcategories in CategoryController and implement the corresponding service method.
```

### 4. Add New Code Implementation

Type the following prompt in Copilot Chat:

```text
@workspace In the ProductService class, add a method called `findProductsByCategory` that takes categoryId as a parameter. Include proper error handling and return list of products.
```

Review the proposed changes and apply them selectively.

Follow up with:

```text
Now add the corresponding REST endpoint in ProductController for the new service method findProductsByCategory with proper validation and response handling.
```

### 5. Unit Testing

Open any test file and ask:

```text
@workspace Add unit tests for the productService class and ensure all methods are covered. Include edge cases and exception scenarios.
```

### 6. Generating documentation

```text
@workspace Add OpenAPI 3.0 annotations to all REST controllers. Include detailed descriptions, example values, and error response documentation. Also update the Swagger configuration class to include these annotations.
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
