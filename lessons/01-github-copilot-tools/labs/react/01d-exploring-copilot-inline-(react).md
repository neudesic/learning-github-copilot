### Lab: Exploring GitHub Copilot Inline with React

## Overview

**Goal:**  
Learn how to use GitHub Copilot's inline chat and autocomplete features to make quick, incremental changes directly within your React components. You'll practice renaming, generating functions, and enhancing components using inline prompts.

**Estimated Duration:**  
15-20 minutes

**Audience:**  
React developers looking to boost their productivity through fast, context-aware code editing within their IDE.

**Prerequisites:**

- Visual Studio Code installed
- GitHub Copilot extension enabled
- Access to GitHub Copilot (with inline chat feature)
- Familiarity with React and TypeScript

## Lab Steps

### 1. Open Target File

- Navigate to the `samples/SimpleFullStack/Web/src/ui/components/global_snackbar` folder in Visual Studio Code.
- Open the `GlobalSnackbar.tsx` file for editing.

### 2. Rename Component Method

- Locate the existing `handleClose` method in the `GlobalSnackbar` component.
- Place your cursor on `handleClose`.
- Press `Ctrl+I` (Windows/Linux) or `Cmd+I` (Mac) to open an inline Copilot prompt.
- Enter the following prompt:

  ```plaintext
  Rename symbol `handleClose` to `dismissSnackbar`
  ```

  Press Enter. Review the rename suggestions and apply them.

### 3. Add a Custom Animation Method

- With the same file still open, place your cursor after the `TransitionUp` function and before the `GlobalSnackbar` component.
- Open a new inline Copilot prompt.
- Enter the following prompt:

  ```plaintext
  Add a new transition function for sliding from the left
  ```

  Accept the generated code suggestion, which should look similar to this:

  ```typescript
  function TransitionLeft(props: SlideProps) {
  	return <Slide {...props} direction='left' />;
  }
  ```

### 4. Create a Duration Helper

- Place your cursor inside the `GlobalSnackbar` component, after the destructured hook variables and before the `dismissSnackbar` method.
- In a new inline prompt, enter:

  ```plaintext
  Add a function to determine duration based on severity
  ```

  - Accept the method stub and close the prompt.
  - Delete the return statement if you want to see Copilot's ghost text suggestions.
  - Observe the "ghost text" Copilot suggests in gray text.
  - Press `Tab` to accept the suggestion, or arrow keys to navigate through the suggestion.

### 5. Enhance the Snackbar Styling

- Locate the `<Alert>` component in the return statement.
- Place your cursor inside the `sx` prop of the Alert component.
- Open a new inline Copilot prompt (`Ctrl+I`) and enter:

  ```plaintext
  Enhance styling with dynamic colors based on severity
  ```

  Accept the Copilot suggestion to add dynamic styling based on the severity level.

### 6. Add Accessibility Features

- Locate the `<Snackbar>` component in the return statement.
- Place your cursor at the end of the props, before the closing `>`.
- Open an inline prompt and enter:

  ```plaintext
  Add accessibility attributes
  ```

  Review and accept Copilot's suggested accessibility enhancements.

### 7. Generate a New Hook

- Open a new file called `useSnackbarTimer.ts` in the same directory.
- With your cursor at the beginning of the empty file, open an inline prompt and enter:

  ```plaintext
  Create a custom hook that manages snackbar display timing
  ```

  Accept the hook implementation that Copilot suggests, which should include imports, type definitions, and the hook logic.

### 8. Integrate the Hook

- Return to `GlobalSnackbar.tsx`.
- Add an import for the new hook at the top of the file.
- Place your cursor inside the component, after the useGlobalSnackbar hook.
- Open an inline prompt and enter:

  ```plaintext
  Use the useSnackbarTimer hook
  ```

  Accept the suggestion to integrate the hook into the component.

## Best Practices

- **Use Inline for Quick Enhancements:** Inline prompts are perfect for adding small methods, JSX elements, or styling.
- **Leverage TypeScript Hints:** Copilot uses TypeScript types to provide better suggestions, so maintain good type definitions.
- **Chain Small Changes:** Build complex features through a series of small, focused inline prompts.
- **Position Your Cursor Strategically:** Place your cursor where you want the code to be inserted for more accurate context.
- **Review Component Logic:** Always verify that added methods work with your component's state and props.

## Summary

By completing this lab, you've learned to:

- Use GitHub Copilot's inline chat to rename methods and generate new functions in React components.
- Create and implement custom React hooks using inline prompts.
- Enhance component UI with inline-suggested JSX elements and styling.
- Improve accessibility and user experience with minimal effort.
- Accept ghost text to streamline minor edits and logic insertions.

GitHub Copilot Inline provides an efficient way to iterate rapidly on your React components without breaking your development flow.
