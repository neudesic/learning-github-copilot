# Lab: Exploring GitHub Copilot Ask with Java Spring Boot

## Overview

**Goal:**  
Learn how to use GitHub Copilot Ask to explore, understand, and enhance your Java Spring Boot Web API project by asking natural language questions directly within VS Code.

**Estimated Duration:**  
15-20 minutes

**Audience:**  
Java developers, QA testers, DevOps engineers, and Technical Writers.

**Prerequisites:**

- VS Code (latest version)
- GitHub Copilot extension enabled in VS Code
- GitHub Copilot Chat extension
- Access to GitHub Copilot Chat (requires a Copilot subscription)
- Access to the Java Spring Boot sample project
- Basic familiarity with Spring Boot, JPA, and REST API concepts
- Java 21 and Maven installed

## Lab Steps

### 1. Launch Copilot Chat

- Open the Java project in VS Code.
- Navigate to the Copilot Chat icon in the activity bar or use the shortcut `Ctrl + Shift + I` (Windows/Linux) or `Cmd + Shift + I` (Mac).
- Alternatively, press `Ctrl + Shift + P` and type "Chat: Open Chat".
- Ensure you're using the latest Copilot model for best results.

### 2. Ask Contextual Questions About Spring Boot Architecture

In the Copilot Chat prompt, type:

```text
@workspace Explain the Spring Boot JPA data access patterns in this project. How are the entities, repositories, services, and controllers structured and how do they interact with each other?
```

Observe the GitHub Copilot response explaining the layered architecture.

Follow up with more specific questions:

```text
@workspace How is the Category entity relationship with Product implemented? Explain the JPA annotations used.
```

Observe and review the suggested improvements.

### 3. Ask About Testing Strategies

Type the following prompt in Copilot Chat:

```text
@workspace What testing strategies should I implement for this Spring Boot application? Show me examples for unit tests, integration tests, and repository tests.
```

Follow up with:

```text
Generate unit tests for the ProductService class including test cases for all CRUD operations and exception handling.
```

### 4. Ask About Data Validation and Error Handling

```text
@workspace How can I implement comprehensive input validation for the Product and Category DTOs? Show me examples using Bean Validation annotations and custom validators.
```

Then ask:

```text
@workspace Show me how to implement request/response logging with Spring Boot.
```

Then ask:

```text
@workspace How do I configure CORS for this API?
```

### 5. Generate Client Integration Code

```text
@workspace Generate a TypeScript/JavaScript client library for consuming this Spring Boot REST API. Include interfaces for all DTOs, proper error handling, and support for authentication.
```

## Advanced Copilot Commands

Use these specialized commands for specific tasks:

```text
/explain ProductController
```

```text
/tests Generate comprehensive tests for CategoryService class including edge cases and exception scenarios.
```

```text
/doc Generate API documentation for all REST endpoints
```

## Best Practices for Java Development with Copilot

- **Be Specific About Frameworks:** Mention "Spring Boot", "JPA", "Hibernate" when relevant.
- **Include Package Context:** Reference specific classes like `com.copilot.sample.entity.Product`.
- **Ask for Best Practices:** Request Spring Boot conventions and Java coding standards.
- **Request Complete Examples:** Ask for full implementation including imports and annotations.
- **Validate Security:** Always review security-related suggestions carefully.
- **Test Suggestions:** Ask for corresponding unit tests with new code.


## Summary

By completing this lab, you've learned to:

- Ask Copilot natural language questions about Java Application
- Generate Java code with proper annotations and best practices
- Request testing strategies and implementations
- Generate integration code for client applications

These techniques will help you incorporate GitHub Copilot Chat into your Java development workflow effectively, making you more productive while maintaining code quality and following Spring Boot best practices.

## Additional Resources

- [GitHub Copilot Ask Documentation](https://docs.github.com/en/copilot/using-github-copilot/copilot-chat/asking-github-copilot-questions-in-your-ide)

© Copyright Neudesic 2025
