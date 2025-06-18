### Lab: Exploring GitHub Copilot Edit

## Overview

**Goal:**  
Learn how to use GitHub Copilot's inline chat and autocomplete features to make quick, incremental changes directly within the context of your code editor. You’ll practice renaming, generating, and editing methods using inline prompts.

**Estimated Duration:**  
15-20 minutes

**Audience:**  
Developers looking to boost their productivity through fast, context-aware code editing within their IDE.

**Prerequisites:**

- Visual Studio 2022 installed
- GitHub Copilot extension enabled
- Access to GitHub Copilot (with inline chat feature)
- Familiarity with C# or a similar object-oriented language

## Lab Steps

### 1. Open Target File

- Navigate to the `samples\SimpleFullStack\DotnetCoreApi\copilot-sample.Api\Services\CategoryService.cs` folder in Visual Studio 2022.
- Open the `CategoryService.cs` file for editing.

### 2. Use code comments to guide Copilot to assist you

- In the `CategoryService.cs` file, add a comment inside the `AddCategoryAsync` method:

  ```csharp
    //add logic to check if a category with same name already exists with same parentCategory, check for case insesnsitive match and  if it exists, throw an exception with message "Category with same name already exists"
  ```

  Press Enter and wait for few seconds, to see the copilot suggestions. Review the suggestions and apply them.

### 3. Inline Chat with Copilot

To open inline chat, select a code block and press `Alt + /`.

- With the same file still open, open a new inline Copilot prompt.
- Enter the following prompt:

  ```plaintext
  Add or modify a public member named DeleteCategoryAsync that deletes a category by id. Also add logic to see if the category exists before deleting it.
  ```

  Accept the generated code suggestion by clicking `Tab` or discard it by clicking `Alt + Del` .

### 4. explain the code

- In the `CategoryService.cs` file, select a method you want to understand better, such as `AddCategoryAsync`.
- Open an inline chat prompt by pressing `Alt + /`.
- Enter the following prompt:

```plaintext
 /explain
  ```

## Best Practices

- **Use Inline for Small Tasks:** Inline prompts are perfect for renaming, simple refactoring, or method generation.
- **Combine with Ghost Text:** Let Copilot suggest code as you type for even faster iteration.
- **Be Specific:** Clear, concise prompts yield better suggestions.
- **Stay Contextual:** Inline prompts rely on the local context, so place your cursor near relevant code.

## Summary

By completing this lab, you’ve learned to:

- Use GitHub Copilot's inline chat to refactor and generate methods.
- Accept ghost text to streamline minor edits and logic insertions.
- Chain inline edits for a more interactive and focused development experience.

GitHub Copilot Inline is a fast, intuitive way to stay in the flow while writing or improving code directly in your editor.

© Copyright Neudeisc 2025
