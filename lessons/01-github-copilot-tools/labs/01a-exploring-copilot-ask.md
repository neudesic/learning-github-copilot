### Lab: Exploring GitHub Copilot Ask

## Overview

**Goal:**  
Learn how to use GitHub Copilot Ask to explore, understand, and improve your codebase by asking natural language questions directly within Visual Studio Code.

**Estimated Duration:**  
15-20 minutes

**Audience:**  
Developers, QA testers, DevOps engineers, and Technical Writers.

**Prerequisites:**

- Visual Studio Code installed
- GitHub Copilot extension enabled
- Access to GitHub Copilot Chat (requires a Copilot subscription)
- Familiarity with a sample project (C#, JavaScript, or Python)

## Lab Steps

### 1. Launch Copilot Chat

- Open Visual Studio Code.
- Navigate to the Copilot Chat icon in the activity bar or use the shortcut `Ctrl + Alt + I` (Windows/Linux) or `Cmd + Option + I` (Mac).
- Ensure the model is set to **GPT-4o** in the settings for best results.

### 2. Ask Contextual Questions

- Browse to `src/Ordering.API/Apis` folder and open the `OrdersApi.cs` file.
- In the Copilot Chat prompt, type:

```text
how does this service work?
```

Observe the GitHub Copilot response explaining the code.

Alternatively, you can use a command like:

```text
/explain
```

Continue the conversation in the same context by asking follow-up questions:

```text
how can I optimize logging in this code?
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
Generate a method that calculates the total with tax and applies a discount.
```

Observe how Copilot suggests a complete function and copy or insert it into your editor.

### 4. Ask for Documentation

Try a prompt such as:

```text
Write documentation comments for this class.
```

Accept and format the response into your codebase.

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
