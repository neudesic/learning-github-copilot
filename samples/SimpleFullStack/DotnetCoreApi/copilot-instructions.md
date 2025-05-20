# Copilot Instructions for .NET Core API

## Project Overview
This project is a backend API built with ASP.NET Core (.NET 8). It provides RESTful endpoints for business logic and data access.

## Using GitHub Copilot Effectively
- Use Copilot to generate controller actions, service methods, and data access code.
- Ask Copilot to write unit tests for your controllers and services.
- Use Copilot to generate documentation comments for methods and classes.

## Coding Conventions
- Follow C# naming conventions (PascalCase for classes/methods, camelCase for variables).
- Use dependency injection for services.
- Keep controllers thin; put business logic in services.
- Write XML documentation for public APIs.

## Example Copilot Prompts
- "Generate a controller for managing orders with CRUD endpoints."
- "Write a unit test for the OrderService class."
- "Add XML documentation to the ProductController methods."
- "Suggest a LINQ query to filter orders by status."

## Running and Testing
- Build: `dotnet build`
- Run: `dotnet run`
- Test: `dotnet test`

## Additional Tips
- Use Copilot Chat for code explanations and refactoring suggestions.
- Use inline Copilot suggestions for quick code completions.
