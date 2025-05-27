## Lesson 01 - GitHub Copilot Tools

Explore the different tools available within GitHub Copilot to enhance your development workflow. Below are sections highlighting each tool, including detailed descriptions, use cases, and links to dedicated labs for hands-on learning.

## Overview

GitHub Copilot provides developers with powerful AI-assisted coding tools designed to streamline coding, debugging, optimization, and automation tasks. These tools integrate seamlessly into various development environments, improving productivity and efficiency across multiple stages of the software development lifecycle.

### 1. GitHub Copilot Ask

GitHub Copilot Ask provides a simple yet powerful way to get quick answers to your programming questions without interrupting your workflow. Simply highlight code, ask a question in the Chat window, and receive immediate guidance. Copilot Ask operates as a "quick gut check" that helps you understand code, solve problems, or learn new concepts without making any changes to your codebase.

This mode leverages your current editor context to provide highly relevant answers - explaining what code does, suggesting testing approaches, providing code snippets, or addressing edge cases. Think of it as having an expert programmer quietly whispering helpful advice in your ear.

![Github Copilot Ask](images/copilot-ask.png 'GitHub Copilot Ask')

**Example use cases:**

- Understanding unfamiliar or complex code functionality
- Getting guidance on how to test specific code blocks
- Learning how to use specific libraries or frameworks
- Optimizing queries or algorithms for better performance
- Refreshing your knowledge on programming concepts (like closures in JavaScript)
- Finding the right syntax for specific programming tasks
- Getting unstuck on problems without committing to architectural changes

**Labs:**

- [Exploring GitHub Copilot Chat](labs/01a-exploring-copilot-ask.md) (C#/.NET)
- [Exploring GitHub Copilot Chat with React](<labs/react/01a-exploring-copilot-ask-(react).md>) (React)

### 2. GitHub Copilot Edit

GitHub Copilot Edit enables you to apply precise modifications to specific files or sections within your codebase using natural language instructions. This tool excels when you need targeted changes to a well-defined set of files rather than extensive modifications across your entire project. Simply highlight the code you want to change, provide instructions like "add error handling" or "refactor using async/await," and Copilot rewrites the code for you – while always showing you the diff for review before any changes are saved.

What makes Edit mode powerful is that you maintain full control. Copilot does the work, but you get the final say. You can also enhance its effectiveness by providing custom instructions that teach Copilot your team's coding standards, style preferences, and documentation requirements.

![Github Copilot Edit](images/copilot-edit.png 'GitHub Copilot Edit')

**Example use cases:**

- Making controlled changes to a specific subset of your codebase
- Refactoring code with modern patterns while preserving the rest of the system
- Implementing targeted optimizations in performance-critical sections
- Adding error handling or logging to existing implementations
- Applying consistent patterns to related files without affecting other components
- Working in brownfield applications where you need surgical precision

**Labs:**

- [Exploring GitHub Copilot Edit](labs/01b-exploring-copilot-edit.md) (C#/.NET)
- [Exploring GitHub Copilot Edit with React](<labs/react/01b-exploring-copilot-edit-(react).md>) (React)

### 3. GitHub Copilot Agent

GitHub Copilot Agent represents the most powerful mode in Copilot Chat, enabling autonomous planning and execution across your entire project. With Agent mode, you provide a high-level prompt and Copilot independently selects the right files, runs necessary tools or terminal commands, and applies code edits until the task is complete. Unlike Edit mode, Agent analyzes related code and identifies additional changes needed across the project to maintain consistency.

What distinguishes Agent mode is its ability to work autonomously - applying edits automatically rather than waiting for explicit approval at each step, while still surfacing potentially risky commands for review. This creates a continuous-edit "driver" model where you define the goal and Copilot executes updates without interruption.

For maximum effectiveness, Agent mode works best with custom instructions that define your project structure, coding standards, and other guidelines. These instructions provide a stronger foundation for Copilot to work from, resulting in more consistent and aligned outcomes across multiple sessions.

![
GitHub Copilot Agent
](images/copilot-agent.png 'GitHub Copilot Agent')
**Example use cases:**

- Building complete features from high-level descriptions
- Fixing complex bugs that require changes across multiple files
- Creating new files and scaffolding entire sections of an application
- Implementing architectural changes that affect multiple components
- Setting up new projects based on README specifications or requirements documents
- Refactoring code while maintaining consistent patterns throughout the codebase

**Labs:**

- [Automating Tasks with Copilot Agent](labs/01c-exploring-copilot-agent.md) (C#/.NET)
- [Automating Tasks with Copilot Agent in eShop](<labs/01c-exploring-copilot-agent-(eshop).md>) (eShop microservices)
- [Automating Tasks with Copilot Agent in React](<labs/react/01c-exploring-copilot-agent-(react).md>) (React)

### 4. GitHub Copilot Inline

Enhance productivity by using inline prompts directly within your editor, enabling quick code changes, method generation, and small incremental improvements without leaving the coding context. This approach is ideal for quick, iterative coding and minor adjustments that don't require extensive context switching.

**Example use cases:**

- Quickly renaming methods or variables across a single file
- Generating and inserting simple code blocks or functions
- Implementing standard design patterns (e.g., Factory, Singleton, Observer)
- Writing unit tests for existing methods
- Converting between code formats (e.g., converting a for loop to LINQ in C#)
- Generating regular expressions for specific validation requirements
- Implementing interface methods or abstract class implementations
- Creating data models or DTOs based on existing patterns in your codebase

**Labs:**

- [Exploring GitHub Copilot Inline](labs/01d-exploring-copilot-inline.md) (C#/.NET)
- [Exploring GitHub Copilot Inline with React](<labs/react/01d-exploring-copilot-inline-(react).md>) (React)

### 5. GitHub Copilot Website

Utilize the Copilot Web interface to explore suggestions, manage preferences, and gain insights into your coding habits and productivity. This centralized interface provides a comprehensive dashboard for reviewing usage analytics, setting global preferences, accessing educational resources, and managing your Copilot subscription.

**Example use cases:**

- Reviewing coding suggestions outside of your IDE
- Analyzing your coding patterns to identify improvement areas
- Managing subscription details and preferences
- Customizing Copilot's behavior across different programming languages
- Setting up global ignore patterns for sensitive or proprietary code
- Accessing learning resources and official documentation
- Reviewing historical usage statistics to optimize your development workflow
- Providing feedback to the GitHub team on Copilot's suggestions

**Labs:**

- [Navigating Copilot's Web Interface](labs/01e-github-copilot-website.md)

### 6. GitHub Copilot CLI

Streamline command-line operations by generating shell commands and automating tasks directly from your terminal. Copilot CLI transforms natural language descriptions into powerful command-line instructions, making complex operations accessible without requiring memorization of syntax or extensive documentation lookups.

**Example use cases:**

- Generating complex git commands for repository management (e.g., interactive rebasing, complex merges)
- Creating automation scripts quickly through natural language prompts
- Executing system-level operations without extensive command-line expertise
- Constructing advanced grep, sed, or awk commands for text processing
- Building deployment pipelines and CI/CD scripts
- Generating database queries or migration scripts
- Creating Docker and Kubernetes management commands
- Formulating complex data transformation pipelines using tools like jq or yq

**Labs:**

- [Mastering GitHub Copilot CLI](#) (Coming Soon)

---

© Copyright Neudeisc 2025
