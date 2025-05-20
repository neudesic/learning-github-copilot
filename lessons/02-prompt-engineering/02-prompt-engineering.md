## Lesson 02 - Prompt Engineering with GitHub Copilot

Learn effective prompt engineering techniques to maximize your productivity with GitHub Copilot. This lesson covers essential concepts, best practices, and strategies to communicate effectively with AI coding assistants.

## Overview

Prompt engineering is the art and science of crafting effective instructions for AI systems like GitHub Copilot. By understanding how to structure your requests, you can significantly improve the quality, relevance, and accuracy of AI-generated code and responses.

### 1. Understanding Tokens and Context Windows

Explore how Copilot processes text through tokens and how context window limitations affect interactions. Understanding these fundamental concepts helps you optimize your prompts for better results.

**Key concepts:**

- What tokens are and how AI models process them
- Context window limitations and their impact on responses
- Strategies for working within token constraints
- How to prioritize information within limited context

**When to focus on tokens and context:**

- When working with large codebases or complex projects
- When you need to provide extensive background information
- When Copilot's responses seem incomplete or miss important context
- When you're experiencing truncated or irrelevant responses

**Tips and tricks:**

- Start with the most relevant information first since models pay more attention to the beginning and end of prompts
- Use clear section headers to help Copilot organize information (`### Background`, `### Requirements`, etc.)
- For large projects, focus on sharing only the most relevant files or code snippets
- When faced with token limitations, break down complex requests into sequential, smaller prompts
- Remove unnecessary comments, documentation, or boilerplate when providing code as context

- **Lab:** [Working with Tokens and Context Windows](#) Coming Soon

### 2. Crafting Effective Prompts

Learn the elements that make a good prompt and how to structure your requests to get optimal responses from Copilot. Well-crafted prompts lead to more accurate, relevant, and useful code suggestions.

**Key techniques:**

- Being specific and clear in your instructions
- Including relevant details while avoiding unnecessary information
- Using appropriate technical terminology
- Structuring multi-part requests effectively

**When to refine your prompting approach:**

- When generating complex algorithms or patterns
- When implementing specific design patterns or architectural styles
- When Copilot generates code that's close but not exactly what you need
- When you need code that follows particular conventions or standards

**Tips and tricks:**

- Start with a clear task description: "Create a function that..."
- Specify the programming language explicitly: "In TypeScript, write a..."
- Include expected inputs and outputs: "The function should accept an array of strings and return a filtered array"
- Mention error handling requirements: "Include proper error handling for invalid inputs"
- Reference coding standards when applicable: "Follow the Angular style guide for component structure"
- Use formatting to emphasize important aspects of your request (bullet points, numbering)
- For complex requests, provide an example of similar code or pseudocode

- **Lab:** [Crafting Effective Prompts for Code Generation](#) Coming Soon

### 3. Adding Context in VS Code

Master techniques for providing additional context to Copilot within VS Code, including file references and specialized annotations. These approaches help Copilot understand your project structure and requirements better.

**Methods explored:**

- Using the `@` symbol to reference files and directories
- Importing relevant code snippets as context
- Working with multiple files simultaneously
- Leveraging workspace information effectively

**When to provide additional context:**

- When implementing features that interact with existing code
- When maintaining consistency across a codebase
- When Copilot needs to understand project-specific patterns or conventions
- When working on complex systems with interdependencies

**Tips and tricks:**

- Use `@filename` syntax to reference specific files in your workspace
- Create a temporary file with key context from multiple files when needed
- When referencing a file, specify which parts are most relevant: "Focus on the authentication logic in @auth.service.ts"
- For complex projects, create a short description of the architecture and reference it
- When implementing interfaces or extending classes, always provide the original definitions
- Use tabs in VS Code to keep related files open, improving Copilot's awareness of context

- **Lab:** [Providing Context in VS Code](#) Coming Soon

### 4. Prompt Engineering Best Practices

Discover industry-proven best practices for prompt engineering that consistently yield superior results across different coding scenarios and tasks.

**Best practices include:**

- Iterative refinement of prompts
- Providing examples when appropriate
- Using clear formatting and structure
- Setting explicit constraints and requirements

**When to apply these best practices:**

- When working on mission-critical features
- When code quality and accuracy are paramount
- When you need to ensure consistent coding style
- When implementing complex business logic or algorithms

**Tips and tricks:**

- Start general, then get specific: begin with a broad request, then refine based on initial responses
- Use the "few-shot" approach: provide 2-3 examples of similar code to establish patterns
- Explicitly state constraints: "The solution must be O(n) time complexity"
- For complex tasks, break down the problem into steps and prompt for each step
- Provide feedback to Copilot about what worked and didn't work in previous responses
- Use consistent terminology within your prompts and across your project
- Explicitly mention testing requirements: "Include unit tests for edge cases"

- **Lab:** [Implementing Prompt Engineering Best Practices](#) Coming Soon

### 5. Advanced Prompting Techniques

Explore sophisticated prompting patterns and techniques that leverage Copilot's capabilities for complex coding tasks and specialized scenarios.

**Advanced techniques:**

- Chain-of-thought prompting for complex problems
- Role-based prompting for specialized code generation
- Using formal specifications in prompts
- Breaking down complex tasks into manageable prompts

**When to use advanced techniques:**

- When implementing complex algorithms or design patterns
- When generating specialized code (security, performance-critical)
- When standard prompting approaches yield suboptimal results
- When working on novel or unique programming challenges

**Tips and tricks:**

- Use chain-of-thought prompting: "Let's think through this step by step..." to guide Copilot's reasoning
- Assign roles to Copilot: "As a security expert, review this authentication code..."
- For complex systems, create a mental model: "This system follows the CQRS pattern with these components..."
- Use specification-driven prompting: provide formal requirements and acceptance criteria
- Implement conversational programming: build on previous interactions to refine solutions
- For debugging, use hypothesis testing: "Could the issue be related to X? If so, how would we fix it?"
- Use comparative prompting: "Generate two solutions to this problem using different approaches"

- **Lab:** [Advanced Prompting Techniques](#) Coming Soon

### 6. Prompting with GitHub Copilot Agent

Master specialized prompting strategies for GitHub Copilot Agent to automate tasks, manage workflows, and enhance productivity through conversational interactions.

**Agent-specific techniques:**

- Crafting prompts for task automation
- Guiding multi-step processes effectively
- Providing feedback to refine Agent responses
- Managing complex development workflows

**When to leverage Copilot Agent:**

- When automating repetitive development tasks
- When working on large-scale refactoring projects
- When navigating and understanding unfamiliar codebases
- When implementing complex, multi-file features

**Tips and tricks:**

- Provide clear overall objectives: "Help me implement a new authentication system using JWT"
- Set expectations for the interaction: "Let's work on this feature step by step"
- Reference specific tools that Agent can use: "Use the terminal to install the required packages"
- For complex tasks, create a plan first: "Let's outline the steps before implementation"
- Guide the Agent with specific follow-up questions when responses aren't ideal
- Use the Agent to explore alternatives: "Can you suggest three different approaches to this problem?"
- Combine Agent with other Copilot tools: "Use Copilot Edit to implement the changes we discussed"
- Establish clear scope boundaries: "Focus only on the backend API for now, we'll address the frontend later"

- **Lab:** [Effective Prompting with GitHub Copilot Agent](#) Coming Soon

---

## Practical Applications

The following examples demonstrate how effective prompt engineering can significantly improve your development workflow across various common scenarios:

### Example 1: Generating a Complex Data Structure

**Basic Prompt:**

```
Create a user class
```

**Improved Prompt:**

```
In TypeScript, create a User class with the following requirements:
- Properties for id (UUID), name, email, role (enum: admin, user, guest), and lastLogin (Date)
- Constructor that validates email format
- Method to check if user has admin privileges
- Static factory method to create a guest user
- Follow the singleton pattern for an admin user
Include appropriate TypeScript typing and JSDoc comments.
```

### Example 2: Implementing an Algorithm

**Basic Prompt:**

```
Write code to find duplicates
```

**Improved Prompt:**

```
In Python, implement an efficient algorithm to find all duplicates in an array of integers where each integer is between 1 and n (array length).
The algorithm should:
- Run in O(n) time
- Use O(1) extra space
- Not modify the original array
Include comments explaining the approach and handle edge cases like empty arrays.
```

### Example 3: Using Copilot Agent for Project Setup

**Basic Prompt:**

```
Help me set up a React project
```

**Improved Prompt:**

```
I need to set up a new React project with the following requirements:
1. Use Vite as the build tool
2. Configure for TypeScript support
3. Add React Router for navigation
4. Set up ESLint with Airbnb style guide
5. Configure Jest and React Testing Library for testing
6. Add TailwindCSS for styling

Guide me through this setup step by step, explaining each command and configuration file. After setup is complete, help me create a basic project structure with components, pages, and services folders.
```

---
