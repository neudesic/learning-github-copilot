### Lab: Exploring GitHub Copilot Edit

## Overview

**Goal:**  
Learn how to use GitHub Copilot's inline chat and autocomplete features to make quick, incremental changes directly within the context of your code editor. You’ll practice renaming, generating, and editing methods using inline prompts.

**Estimated Duration:**  
15-20 minutes

**Audience:**  
Developers looking to boost their productivity through fast, context-aware code editing within their IDE.

**Prerequisites:**

- Visual Studio Code installed
- GitHub Copilot extension enabled
- Access to GitHub Copilot (with inline chat feature)
- Familiarity with C# or a similar object-oriented language

## Lab Steps

### 1. Open Target File

- Navigate to the `samples/eshop/src/Ordering.Domain/AggregatesModel/OrderAggregate` folder in Visual Studio Code.
- Open the `Order.cs` file for editing.

### 2. Rename the Method

- Locate the existing `GetTotal` public method at the bottom of the `Order` class.
- Place your cursor on `GetTotal`.
- Press `Ctrl+I` (or use right-click > Copilot: Ask) to open an inline Copilot prompt.
- Enter the following prompt:

  ```plaintext
  Rename symbol `GetTotal` to `GetSubTotal`
  ```

  Press Enter. Review the rename suggestions and apply them.

### 3. Add a New Total Method

- With the same file still open, open a new inline Copilot prompt.
- Enter the following prompt:

  ```plaintext
  Add a new public member named GetTotal that is calculated as sum of order items and a new public property of type decimal named taxes and subtracts the sum of items in a public property of type List<decimal> named Discounts
  ```

  Accept the generated code suggestion.

### 4. Create a Discount Helper Method

- In a new inline prompt, enter:

  ```plaintext
  Add an empty public member named GetOrderDiscounts that returns a decimal
  ```

  - Accept the method stub and close the prompt.
  - Delete the return statement.
  - Observe the "ghost text" Copilot suggests in gray text.
  - Press `Tab` to accept the suggestion, or `Ctrl + Arrow` keys to accept piece-by-piece.

### 5. Update Discount Logic

- Open a new inline Copilot prompt (`Ctrl+I`) inside the `GetOrderDiscounts` method.
- Enter the following:

  ```plaintext
  Update `GetOrderDiscounts` method to use the `Discount` property in OrderItem
  ```

  Accept the Copilot suggestion to calculate the total discounts from all order items.

### 6. Integrate Discount Logic

- Return to the `GetTotal` method.
- Delete the value assigned to the `totalDiscounts` variable.
- Observe Copilot’s ghost text suggesting a call to `GetOrderDiscounts()`.
- Press `Tab` to accept the suggestion and complete the integration.

## Best Practices

- **Use Inline for Small Tasks:** Inline prompts are perfect for renaming, simple refactoring, or method generation.
- **Combine with Ghost Text:** Let Copilot suggest code as you type for even faster iteration.
- **Be Specific:** Clear, concise prompts yield better suggestions.
- **Stay Contextual:** Inline prompts rely on the local context, so place your cursor near relevant code.

## Summary

By completing this lab, you’ve learned to:

- Use GitHub Copilot's inline chat to rename and generate methods.
- Accept ghost text to streamline minor edits and logic insertions.
- Chain inline edits for a more interactive and focused development experience.

GitHub Copilot Inline is a fast, intuitive way to stay in the flow while writing or improving code directly in your editor.
