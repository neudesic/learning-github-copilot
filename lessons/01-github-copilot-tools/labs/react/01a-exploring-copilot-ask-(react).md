### Lab: Exploring GitHub Copilot Ask with React

## Overview

**Goal:**  
Learn how to use GitHub Copilot Ask to explore, understand, and improve your React codebase by asking natural language questions directly within Visual Studio Code.

**Estimated Duration:**  
15-20 minutes

**Audience:**  
React developers, UI/UX engineers, and front-end specialists.

**Prerequisites:**

- Visual Studio Code installed
- GitHub Copilot extension enabled
- Access to GitHub Copilot Chat (requires a Copilot subscription)
- Access to the SimpleFullStack React project

## Lab Steps

### 1. Launch Copilot Chat

- Open Visual Studio Code.
- Navigate to the Copilot Chat icon in the activity bar or use the shortcut `Ctrl + Alt + I` (Windows/Linux) or `Cmd + Option + I` (Mac).
- Ensure the model is set to **GPT-4o** in the settings for best results.

### 2. Ask Contextual Questions About the React Component

- Browse to `samples/SimpleFullStack/Web/src/ui/components/product/ProductCard.tsx` and open the file.
- In the Copilot Chat prompt, type:

```text
How does this ProductCard component work and what props does it accept?
```

Observe the GitHub Copilot response explaining the component structure and props.

Continue the conversation by asking a follow-up question:

```text
How can I improve the performance of this component?
```

Observe and review the suggested improvements, which might include:

- Memoizing the component with React.memo
- Using useMemo for computed values like imageUrl
- Avoiding unnecessary re-renders

### 3. Understand the Application Architecture

- Navigate to `samples/SimpleFullStack/Web/src/App.tsx`
- Ask Copilot:

```text
Explain the structure of this React application and the libraries it uses.
```

Observe how Copilot explains the overall architecture, routing setup, and integration with Material UI and React Query.

### 4. Get Help with API Integration

- Open `samples/SimpleFullStack/Web/src/services/axiosClient.ts`
- Ask Copilot:

```text
How can I add authentication headers to all API requests in this service?
```

Review Copilot's suggestions for implementing:

- Authorization header setup
- Request/response interceptors for handling tokens
- Error handling for authentication failures

### 5. Ask for New Code

Type the following prompt in Copilot Chat:

```text
Generate a new React hook called useProductSearch that filters products based on a search term.
```

Observe how Copilot suggests a complete custom hook implementation. You can:

- Copy the code
- Insert at cursor
- Apply in editor

### 6. Ask for Component Improvements

Try a prompt such as:

```text
Add a price comparison feature to the ProductCard component that shows if the current price is lower than the original price.
```

Review the code Copilot suggests to implement this feature.

### 7. Request Testing Code

Ask Copilot:

```text
Write unit tests for the ProductCard component using React Testing Library.
```

Observe how Copilot generates comprehensive test cases for the component.

## Best Practices

- **Be Specific About React Concepts:** Mention specific React hooks, patterns, or libraries when asking questions.
- **Ask About Component Relationships:** Understanding how components interact is crucial in React applications.
- **Request Performance Optimizations:** React rendering optimization is a strength of Copilot.
- **Break Down Complex Features:** For larger feature requests, break them into smaller, focused prompts.
- **Try / Commands:** Use `/explain`, `/generate`, `/tests`, etc., to streamline specific actions.

## Summary

By completing this lab, you've learned to:

- Ask Copilot natural language questions about React components.
- Generate custom hooks and functional components.
- Request performance optimizations and test code.
- Understand how to leverage Copilot for React-specific tasks.

These techniques will help you incorporate GitHub Copilot Chat into your daily React development workflow effectively.
