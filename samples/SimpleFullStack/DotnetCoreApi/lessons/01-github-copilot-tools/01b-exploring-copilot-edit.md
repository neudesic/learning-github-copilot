# Lab: Exploring GitHub Copilot Edit in Visual Studio 2022

## Overview

**Goal:**  
Use GitHub Copilot Edit to refactor, enhance, and optimize .NET Core Web API code with large-scope changes using natural language prompts across multiple files or code blocks in Visual Studio 2022.

**Estimated Duration:**  
15-20 minutes

**Audience:**  
Developers, QA testers, DevOps engineers, and Technical Writers working with medium to large codebases.

**Prerequisites:**

- Visual Studio 2022 (version 17.6 or later)
- GitHub Copilot extension enabled in Visual Studio
- Access to GitHub Copilot Edit (available as part of GitHub Copilot Chat)
- Access to the DotnetCoreApi sample project
- Basic familiarity with ASP.NET Core Web API patterns

## Lab Steps

### 1. Launch Copilot Edit in Visual Studio 2022

- Open the DotnetCoreApi solution in Visual Studio 2022
- Open the Copilot Chat interface by clicking the Copilot icon in the Visual Studio activity bar or using the shortcut `Ctrl + \, C` (Windows/Linux).
- In the Copilot Chat window, click the **Create New Edits thread** icon in the top-right corner.

![copilot edit](../images/VS-open-edits-thread.png)

### 2. Add API Files to Edit Context

- Click `+ Add Files` and select the `Services/ProductAttributeService.cs` file from your `copilot-sample.Api` project to add it to the edit context.

### 3. Document the code

In the Copilot Edit prompt, enter the following natural language instruction:

```plaintext
Add comprehensive XML documentation comments to the ProductAttributeService class and all its methods. 
Include parameter descriptions, return value descriptions, and exception documentation.
Also add input validation with proper exception handling for null or invalid parameters.
Follow ASP.NET Core service layer best practices.
```

Copilot Edit will now scan the full file and apply necessary updates across the class.

Review the proposed changes directly in the chat interface.

### 4. Apply the Edits

- Click **Apply** to confirm the changes and commit them to your working file.
- Test and verify the updated functionality.

## Best Practices

- **Edit in Context:** Use Copilot Edit for large-scale or multi-line changes where standard inline prompts are insufficient.
- **Preview Before Apply:** Always review the full diff before applying edits.
- **Use Descriptive Prompts:** Give specific names, logic, or intended outcomes in your prompt.
- **Iterate Prompting:** If the result is not ideal, revise your prompt and re-run the edit.

## Summary

By completing this lab, you've learned to:

- Use Copilot Edit to modify an entire class file based on a natural language instruction.
- Apply and verify complex changes like tax and discount logic refactoring.
- Leverage full-file editing to streamline development and maintenance in real-world scenarios.
- Follow ASP.NET Core best practices through AI-assisted code transformation

This demonstrates how GitHub Copilot Edit can significantly accelerate development and maintenance by handling complex, multi-line changes that would be time-consuming to implement manually.

## Hands-On Exercises

### Exercise 1: Implement a feature

```text
@workspace, Create a controller for ProductReviews CRUD operations and add a service namedProductReviewService that implements IProductReviewService interface to handle DB operationsand add it to DI. Also add necessary DTO models.
```

### 2. Create unit tests

```text
@workspace, Create unit tests for the ProductAttributeService class and its methods. Use xUnit and Moq for mocking dependencies. Ensure to cover all public methods, including edge cases and exception handling.
```

### 3. Build deployments

```text
I am planning to deploy the .net core api as a docker container on kubernetes, please helpme generate the docker file. Also explain what each statement in the dockerfile does
```

© Copyright Neudeisc 2025
