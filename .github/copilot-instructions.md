# Instructions for GitHub Copilot

## Source Control

- We use GitHub Issues for all work tracking.
- We use Pull Requests to facilitate code reviews.
- We use Trunk-based development and short term feature branches. Feature branches should start with `feat/`, for example a feature called "add login" would be `feat/add-login`.

## C# Development

- Follow Microsoft's C# Coding Conventions
- Use Pascal Case for class names, properties, and methods
- Use Camel Case for local variables and parameters
- Prefer async/await over direct Task manipulation
- Use XML documentation comments for public APIs
- Implement IDisposable for classes that own disposable resources
- Use nullable reference types
- Prefer LINQ queries for collection transformations
- Write unit tests using xUnit
- Follow SOLID principles
- no minimal api, use controllers and services