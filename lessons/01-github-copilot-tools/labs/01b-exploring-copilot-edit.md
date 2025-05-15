### Lab: Exploring GitHub Copilot Edit

## Overview

**Goal:**  
Use GitHub Copilot Edit to refactor, enhance, and optimize code with large-scope changes using natural language prompts across multiple files or code blocks.

**Estimated Duration:**  
15-20 minutes

**Audience:**  
Developers, QA testers, DevOps engineers, and Technical Writers working with medium to large codebases.

**Prerequisites:**

- Visual Studio Code installed
- GitHub Copilot extension enabled
- Access to GitHub Copilot Edit (in Preview as part of GitHub Copilot Chat)
- Familiarity with a sample project (C# preferred for this example)

## Lab Steps

### 1. Reset Your Code

- Undo or discard any previous changes made to the `Order` class (if following from a previous lab).
- Ensure you have a clean version of `Order.cs` open for editing.

### 2. Launch Copilot Edit

- Open the Copilot Chat interface by clicking the Copilot icon in the bottom-right corner of Visual Studio Code.
- In the Copilot Chat window, click the **Copilot Edits** tab in the top-left corner.

![copilot edit](./images/copilot-edits.png)

### 3. Add the File to Context

- Click `+ Add Files` and select the `Order.cs` file from your project to add it to the edit context.

### 4. Issue a Refactoring Prompt

In the Copilot Edit prompt, enter the following natural language instruction:

```plaintext
Refactor the GetTotal method to include taxes and discounts. Get the taxes from a new public property named Taxes and calculate the discounts as the sum of the Discount property for each OrderItem in the order.
```

Copilot Edit will now scan the full file and apply necessary updates across the class.

Review the proposed changes directly in the chat interface.

### 5. Apply the Edits

- Click **Apply in Editor** to confirm the changes and commit them to your working file.
- Test and verify the updated functionality.

## Best Practices

- **Edit in Context:** Use Copilot Edit for large-scale or multi-line changes where standard inline prompts are insufficient.
- **Preview Before Apply:** Always review the full diff before applying edits.
- **Use Descriptive Prompts:** Give specific names, logic, or intended outcomes in your prompt.
- **Iterate Prompting:** If the result is not ideal, revise your prompt and re-run the edit.

## Summary

By completing this lab, you’ve learned to:

- Use Copilot Edit to modify an entire class file based on a natural language instruction.
- Apply and verify complex changes like tax and discount logic refactoring.
- Leverage full-file editing to streamline development and maintenance in real-world scenarios.

This lab demonstrates how GitHub Copilot Edit can boost productivity for developers managing and evolving mid-to-large sized codebases.
