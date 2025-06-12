## Lesson 03 - Prompt Engineering with GitHub Copilot

Learn effective prompt engineering techniques to maximize your productivity with GitHub Copilot. This lesson covers essential concepts, best practices, and strategies to communicate effectively with AI coding assistants.

---

## Overview

Prompt engineering is the art and science of crafting effective instructions for AI systems like GitHub Copilot. By understanding how to structure your requests, you can significantly improve the quality, relevance, and accuracy of AI-generated code and responses.

---

### 1. Understanding Tokens and the Context Window

Before you can prompt effectively, it helps to know how Copilot “reads” your input.  
**Tokens** are the building blocks—words or parts of words—that the AI uses to process information. The **context window** is the amount of recent text (code, comments, prompts) that Copilot can “see” and consider at once.

- **Token:** A chunk of text (word, symbol, or punctuation) used by AI models to understand and generate responses.
- **Context Window:** The limit on how many tokens the model can process at once. This affects how much code, comments, or instructions Copilot can reference in a single response.

> _Why it matters:_ If your prompt, code, and context are too large, Copilot may “forget” or ignore earlier parts.

<!-- Add diagrams/screenshots explaining tokens and context windows -->

---

### 2. Models and When to Use Them

GitHub Copilot offers several AI models, each with different strengths. Choosing the right model helps you get the best results for your scenario.

| Model Name       | Max Tokens | Best For                    | Example Use Case          |
| ---------------- | ---------- | --------------------------- | ------------------------- |
| Copilot-Classic  | 4,096      | Everyday code completion    | Writing functions         |
| Copilot-Advanced | 8,192+     | Larger context, multi-file  | Refactoring a large class |
| GPT-4            | 32,000+    | Complex logic, big projects | Explaining a full repo    |

> _Tip:_ Use simpler models for speed, larger models for deep context.

<!-- Add table or graphic showing models and features. Add example prompts for each model. -->

---

### 3. Adding Context in VS Code

Help Copilot help you! Supplying relevant files, folders, and comments can dramatically improve the quality of suggestions.

#### How to Provide Additional Context

- **Referencing Files and Folders:**  
  Add references to related files or folders in your prompt or comments.

- **Including Tools/Libraries:**  
  Mention frameworks, libraries, or tools you’re using so Copilot can tailor its suggestions.

- **Special Syntax:**  
  Use `@` to reference files, and `#` to indicate sections or tags.

> _Example:_
>
> ```plaintext
> // @utils/helpers.js
> #validation
> Write a function that checks if an email is valid.
> ```

#### Problem:

Without context, Copilot may hallucinate or generate irrelevant code.

<!-- Add screenshots and more concrete examples of context. List commonly used VS Code tools for context. -->

---

### 4. Crafting Effective Prompts

Getting great results from Copilot depends on how you ask. Here are some best practices for writing clear, concise, and goal-focused prompts.

#### How to Construct a Good Prompt

- Be specific: Mention the language, framework, and desired output.
- Give context: Describe what the code is for and any constraints.
- Break down complex tasks: One step at a time is better than one huge request.

#### Key Information to Include

- Input/output examples
- Edge cases or known bugs
- Performance requirements

> _Example Prompt:_  
> “In Python, write a function that parses a CSV file and returns a list of dictionaries. Handle missing values.”

<!-- Add before/after prompt examples and screenshots if possible -->

---

### 5. Prompting with GitHub Copilot Agent

Copilot Agent lets you automate multi-step tasks with conversational prompts.  
It works best when you guide it with clear, incremental instructions.

#### How is Copilot Agent Prompting Different?

- Can operate across multiple files or the whole repo
- Can execute terminal commands and apply project-wide changes
- Works best with step-by-step or staged instructions

#### Best Practices

- Tackle one workflow or task at a time
- Review intermediate results before proceeding
- Use conversational, actionable instructions

> _Example:_  
> “Generate CRUD endpoints for the User model, then add logging to each route.”

---

<!-- End of Lesson 2 template. Add copyright and any additional info -->

© Copyright Neudeisc 2025
