# Copilot Instructions for .NET Core API

## Project Overview
This project is a backend API built with ASP.NET Core (.NET 8). It provides RESTful endpoints for business logic and data access.
Has 3 projects
1. copilot-sample.Api - ASP.NET Core Web API project.
2. copilot-sample.DataAccess - C# class library project to handle database context, repositories, and data models.
3. copilot-sample.Test - C# unit test project.

## Technologies and libraries used
- ASP.NET Core 8
- Entity Framework Core 8
- Swagger / Swashbuckle (for API documentation)
- Microsoft.Extensions.DependencyInjection (for DI)
- C#
- xUnit (for testing)
- Moq and FluentAssertions (for mocking dependencies and assertions)
- Microsoft.EntityFrameworkCore.InMemory library for EF entity unit tests

## Coding Conventions
- Follow C# naming conventions (PascalCase for classes/methods, camelCase for variables).
- Use dependency injection for services.
- Keep controllers thin; put business logic in services.

## Example Copilot Prompts
- "Generate a controller for managing categories with CRUD endpoints."
- "Write a unit test for the CategoryService class."
- "Add XML documentation to the ProductController methods."
- "Suggest a LINQ query to filter products by category."

## Running and Testing
- Build: `dotnet build`
- Run: `dotnet run`
- Test: `dotnet test`

## Additional Tips
- Use Copilot Chat for code explanations and refactoring suggestions.
- Use inline Copilot suggestions for quick code completions.
