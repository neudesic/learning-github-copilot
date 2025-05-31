# Lab: Exploring GitHub Copilot Ask

## Overview

**Goal:**  
Learn how to use GitHub Copilot Ask to explore, understand, and improve your codebase by asking natural language questions directly within Visual Studio.

**Estimated Duration:**  
15-20 minutes

**Audience:**  
Developers, QA testers, DevOps engineers, and Technical Writers.

**Prerequisites:**

- Visual Studio installed
- GitHub Copilot extension enabled
- Access to GitHub Copilot Chat (requires a Copilot subscription)
- Familiarity with a sample project (C# .NET Core API project is used in this lab)

## Lab Steps

### 1. Launch Copilot Chat

- Open the solution in Visual Studio.
- Navigate to the Copilot Chat icon in the activity bar or use the shortcut `Ctrl + \, C` (Windows/Linux).
- Ensure the model is set to **GPT-4o** in the settings for best results or choose the model of your choice.

### 2. Ask Contextual Questions

- In the Copilot Chat prompt, type:

```text
 @workspace, Explain the DataAccess project in the solution. Also explain how it is being used and triggered.
```

Observe the GitHub Copilot response explaining the code.

Alternatively, you can use a command like:

```text
/explain DataAccess project
```

Continue the conversation in the same context by asking follow-up questions:

```text
how can I add loggoing to the DataAccess project?
```

Observe and review the suggested improvements.

(Optional)

- You can copy the suggested code manually into your file.
- Or click the icons above the suggestion pane to:
  - Insert at Cursor
  - Apply in Editor
  - Copy to Clipboard
- (Optional) Save the file to retain changes.

### 3. Ask for New Code

Type the following prompt in Copilot Chat:

```text
@workspace, In ProductService class, AddProductAsync method, add a check to see if the product already exists in the database. If it does, throw a custom exception named ProductAlreadyExistsException.
```

Observe how Copilot suggests a complete function and copy or insert it into your editor.

### 4. Ask Copilot about an exception

```text
Run GetProducts endpoint and check if there are any exceptions, and explore "Analyze with Copilot".
```

### 5. Generate integration code

```text
@workspace, I am trying to consume CategoryController endpoint in UI TypeScript project. It uses axios for HTTP calls, please generate TS code with interfaces.
```

## Best Practices

- **Be Specific:** Include relevant class, method, or file names when asking questions.
- **Break Down Requests:** Use step-by-step prompts if Copilot struggles with more complex tasks.
- **Use Follow-ups:** Build on previous answers to refine results.
- **Insert Carefully:** Always review suggestions before inserting into production code.
- **Try / Commands:** Use `/explain`, `/generate`, `/tests`, etc., to streamline specific actions.

## Summary

By completing this lab, you’ve learned to:

- Ask Copilot natural language questions about your code.
- Generate new code using descriptive prompts.
- Request documentation or refactoring suggestions.

These techniques will help you incorporate GitHub Copilot Chat into your daily development workflow effectively.
