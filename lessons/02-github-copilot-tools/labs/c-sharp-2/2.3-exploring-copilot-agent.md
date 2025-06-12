### Lab: Automating Tasks with GitHub Copilot Agent in eShop

## Overview

**Goal:**  
Learn how to leverage GitHub Copilot Agent to autonomously implement complex features and architectural changes across the eShop microservices application.

**Estimated Duration:**  
25-30 minutes

**Audience:**  
.NET developers, backend engineers, and architects working with microservice-based applications.

**Prerequisites:**

- Visual Studio Code installed
- GitHub Copilot extension enabled
- Access to GitHub Copilot Chat with Agent capabilities
- Access to the eShop sample project

## Lab Description

GitHub Copilot Agent represents the most powerful mode in Copilot Chat, enabling autonomous planning and execution across your entire project. With Agent mode, you provide a high-level prompt and Copilot independently selects the right files, runs necessary tools or terminal commands, and applies code edits until the task is complete. Unlike Edit mode, Agent analyzes related code and identifies additional changes needed across the project to maintain consistency.

What distinguishes Agent mode is its ability to work autonomously - applying edits automatically rather than waiting for explicit approval at each step, while still surfacing potentially risky commands for review. This creates a continuous-edit "driver" model where you define the goal and Copilot executes updates without interruption.

For maximum effectiveness, Agent mode works best with custom instructions that define your project structure, coding standards, and other guidelines. These instructions provide a stronger foundation for Copilot to work from, resulting in more consistent and aligned outcomes across multiple sessions.

## Lab Steps

### 1. Launch Copilot Agent

- Open Visual Studio Code with the eShop project.
- Navigate to the Copilot Chat panel by clicking the Copilot icon in the activity bar or using the keyboard shortcut `Ctrl+Shift+I` (Windows/Linux) or `Cmd+Shift+I` (Mac).
- Select the "Agent" tab from the top of the chat interface.

### 2. Implement a Complete Feature: Order Tracking

Ask Copilot Agent to implement a complete order tracking feature for the eShop application with this prompt:

```
Implement an order tracking feature for the eShop application. The feature should:
1. Add a new endpoint to the Ordering.API to get tracking information
2. Create a new TrackingInfo entity in the Ordering.Domain
3. Update the Order aggregate to include tracking information
4. Add the necessary repository methods and service logic
5. Ensure the feature works with the existing architecture and patterns
```

Observe as Copilot Agent:

- Analyzes the existing code architecture in the project
- Identifies all necessary files to modify across multiple projects
- Creates new entities and updates existing ones
- Adds new endpoints and services
- Ensures consistency with the existing domain-driven design patterns

Review the changes and test the new order tracking functionality.

### 3. Enhance Error Handling Across Microservices

Ask Copilot Agent to implement comprehensive error handling with the following prompt:

```
Enhance error handling across the eShop microservices by:
1. Implementing a consistent exception handling middleware in all API projects
2. Creating custom exception types for domain-specific errors
3. Adding proper logging with correlation IDs across service boundaries
4. Ensuring all API endpoints return standardized error responses
5. Implementing retry policies for transient failures in service-to-service communication
```

Observe as Copilot Agent:

- Identifies common patterns across microservices
- Creates shared exception types and middleware
- Implements consistent logging
- Adds retry policies using Polly or similar libraries
- Updates API endpoints to use standardized error responses

### 4. Implement Performance Monitoring

Prompt Copilot Agent to add performance monitoring to the application:

```
Add performance monitoring to the eShop application by:
1. Implementing Application Insights integration across all microservices
2. Adding custom metrics for key business operations
3. Creating performance counters for database operations
4. Implementing distributed tracing across service boundaries
5. Adding health checks with appropriate degradation responses
```

Watch as Copilot Agent:

- Installs necessary NuGet packages
- Configures Application Insights
- Implements custom metrics and telemetry
- Sets up distributed tracing
- Adds health check endpoints with appropriate logic

### 5. Upgrade to .NET 8 Features

Ask Copilot Agent to modernize the codebase with this prompt:

```
Upgrade the eShop application to take advantage of .NET 8 features by:
1. Updating project files and dependencies
2. Implementing minimal API improvements where applicable
3. Utilizing the new rate limiting features
4. Adding AOT compilation support where beneficial
5. Implementing the new identity features for better security
```

Observe as Copilot Agent:

- Updates project files and NuGet packages
- Refactors code to use new .NET 8 features
- Implements rate limiting
- Configures AOT compilation settings
- Enhances identity and security features

### 6. Implement a New Payment Gateway

Challenge Copilot Agent with a complex integration task:

```
Implement a new payment gateway integration for the eShop application:
1. Create a new payment service provider interface
2. Implement a concrete provider for Stripe payments
3. Update the ordering process to support multiple payment providers
4. Add appropriate unit and integration tests
5. Ensure the implementation follows clean architecture principles
```

Watch as Copilot Agent:

- Creates the necessary interfaces and implementations
- Integrates with the existing ordering process
- Adds unit and integration tests
- Ensures proper separation of concerns
- Maintains clean architecture principles

## Best Practices for Using Copilot Agent with .NET Microservices

- **Provide Architecture Context:** Mention key architectural patterns like DDD, CQRS, or event sourcing.
- **Break Large Tasks into Steps:** For complex features, guide Copilot with a numbered list of steps.
- **Specify Technology Preferences:** Mention specific technologies like Entity Framework, MediatR, or FluentValidation.
- **Review Changes Incrementally:** Pause between major feature additions to review and test.
- **Ask for Documentation:** Request that Copilot add XML documentation comments for new APIs.
- **Guide Code Style:** Mention preferences for naming conventions, exception handling, or logging approaches.

## Summary

By completing this lab, you've learned to:

- Use GitHub Copilot Agent to implement complete features autonomously
- Enhance error handling and performance monitoring across microservices
- Upgrade applications to use new framework features
- Implement complex integrations with external services
- Maintain architectural consistency during significant changes

These capabilities demonstrate how GitHub Copilot Agent can dramatically accelerate development by handling complex, multi-file changes that would typically require significant manual effort and coordination across multiple microservices.

---

© Copyright Neudeisc 2025
