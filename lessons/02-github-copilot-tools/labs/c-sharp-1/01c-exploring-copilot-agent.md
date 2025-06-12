# Accelerating .NET Core Web API Development with GitHub Copilot Agent

## Overview

**Goal:**  
Learn how to leverage GitHub Copilot Agent to autonomously implement complex features and architectural changes in a .NET Core Web API project using Visual Studio 2022.

**Estimated Duration:**  
25-30 minutes

**Audience:**  
.NET Core Web API developers, backend engineers.

**Prerequisites:**

- Visual Studio 2022 installed (17.6 or later)
- GitHub Copilot subscription with Agent capabilities enabled
- Access to the DotnetCoreApi sample project
- Basic familiarity with ASP.NET Core Web API concepts

## Lab Description

GitHub Copilot Agent represents the most powerful mode in Copilot Chat, enabling autonomous planning and execution across your entire project. With Agent mode, you provide a high-level prompt and Copilot independently selects the right files, runs necessary tools or terminal commands, and applies code edits until the task is complete. Unlike Edit mode, Agent analyzes related code and identifies additional changes needed across the project to maintain consistency.

What distinguishes Agent mode is its ability to work autonomously - applying edits automatically rather than waiting for explicit approval at each step, while still surfacing potentially risky commands for review. This creates a continuous-edit "driver" model where you define the goal and Copilot executes updates without interruption.

For maximum effectiveness, Agent mode works best with custom instructions that define your project structure, coding standards, and other guidelines. These instructions provide a stronger foundation for Copilot to work from, resulting in more consistent and aligned outcomes across multiple sessions.

## Lab Steps

### 1. Launch Copilot Agent and Enable Agent Mode

- Open Visual Studio 2022 with the copilot-sample.sln solution.
- Navigate to the Copilot Chat panel by clicking the Copilot icon in the activity bar. Click on `Copilot icon > Settings > Options` to open settings
- Check the "Enable Agent mode in the chat pane" checkbox.
[![Open VS settings](../images/VS-copilot-options.png)]
[![Enable Agent Mode](../images/VS-enable-agent-mode.png)]

### 2. Implement a RESTful API Controller and Service

In the Copilot Chat panel, enter the following prompt:

```text
@workspace, Create a ProductReviews API controller with standard CRUD operations (GET, POST, PUT, DELETE). 
Add a service named ProductReviewService that implements IProductReviewService interface.
The service should handle DB operations with Entity Framework Core.
Add necessary DTO models with proper validation attributes.
Register everything in the dependency injection container.
```

What you'll observe:

- Copilot Agent will analyze the existing .NET Core Web API structure
- It will create controller, service interface, and implementation files
- It will generate appropriate DTO models with validation attributes
- It will update Startup.cs or Program.cs to register the service
- It will follow existing project patterns for consistent implementation

After completion, build the solution (Ctrl+Shift+B) to verify there are no compilation errors.

### 3. Generate xUnit Tests for Your API

Enter this prompt in Copilot Chat:

```text
@workspace, Create comprehensive xUnit tests for the ProductAttributeService class.
Include tests for all public methods.
Use Moq for mocking dependencies.
Follow AAA (Arrange-Act-Assert) pattern in test methods.
```

What you'll observe:

- Copilot Agent will analyze the ProductAttributeService implementation
- It will create a test project if one doesn't exist
- It will generate well-structured test methods with appropriate mocks and assertions
- Tests will cover normal operation, edge cases, and error conditions

### 4. Generate Containerization Configuration

Enter this prompt in Copilot Chat:

```text
I need to containerize this .NET Core Web API for Kubernetes deployment.
Please help me generate:
1. A multi-stage Dockerfile optimized for .NET Core API
2. A docker-compose.yml file for local development
3. Basic kubernetes deployment YAML
Please explain each configuration element.
```

What you'll observe:

- Copilot will analyze project structure to create appropriate Docker configurations
- You'll receive a detailed explanation of each Dockerfile instruction
- You'll get complementary configuration files for container orchestration

### 5. Implement Structured Logging with Serilog

Enter this prompt in Copilot Chat:

```text
@workspace, implement structured logging with Serilog in this .NET Core Web API:
1. Add necessary NuGet packages
2. Configure Serilog in Program.cs with console and file sinks
3. Create a middleware to log HTTP requests and responses
4. Add appropriate log statements in the ProductReviews controller
5. Configure log enrichment with correlation IDs
```

## Best Practices for Using Copilot Agent with .NET Core Web APIs

When leveraging GitHub Copilot Agent for .NET Core Web API development, consider these best practices:

- **Provide Architecture Context:** Mention key architectural patterns like Clean Architecture, CQRS, or Repository Pattern.
- **Break Large Tasks into Steps:** For complex APIs, guide Copilot with a numbered list of implementation steps.
- **Specify Technology Preferences:** Be explicit about technologies like Entity Framework Core, AutoMapper, FluentValidation, or Swashbuckle.
- **Review Changes Incrementally:** Pause between major feature additions to review and test endpoints.
- **Ask for Documentation:** Request that Copilot add XML documentation comments and OpenAPI annotations for your Web API.
- **Guide Code Style:** Specify ASP.NET Core coding conventions, especially for controller routes and action methods.

## Summary

By completing this lab, you've learned to:

- Use GitHub Copilot Agent to implement complete RESTful APIs autonomously
- Generate controllers, services, and DTOs with proper validation and dependency injection
- Create comprehensive unit tests for your .NET Core Web API components
- Set up containerization and orchestration for your Web API
- Implement structured logging for HTTP request/response tracking
- Maintain architectural consistency throughout your Web API project

These capabilities demonstrate how GitHub Copilot Agent can dramatically accelerate .NET Core Web API development by handling complex, multi-file changes that would typically require significant manual effort.

---

© Copyright Neudeisc 2025
