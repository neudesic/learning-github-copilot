## Lesson 01 - GitHub Copilot Tools

Explore the different tools available within GitHub Copilot to enhance your development workflow. Below are sections highlighting each tool, including detailed descriptions, use cases, and links to dedicated labs for hands-on learning.

## Overview

GitHub Copilot provides developers with powerful AI-assisted coding tools designed to streamline coding, debugging, optimization, and automation tasks. These tools integrate seamlessly into various development environments, improving productivity and efficiency across multiple stages of the software development lifecycle.

### 1. GitHub Copilot Ask

Engage interactively with Copilot to ask questions, request code suggestions, and optimize your code directly within a conversational interface. Copilot Ask is ideal for understanding existing code, exploring implementation strategies, and generating complex code snippets through conversational prompts.

**Example use cases:**

- Clarifying the logic of unfamiliar or complex code.
- Requesting code examples or implementations of specific features.
- Improving existing code with optimization suggestions.

- **Lab:** [Exploring GitHub Copilot Chat](labs/01a-exploring-copilot-ask.md)

### 2. GitHub Copilot Edit

Use Copilot Edit to perform extensive refactoring and optimizations across larger codebases by providing comprehensive context-aware edits. Copilot Edit analyzes the entire file or multiple files to suggest impactful, cohesive changes, significantly enhancing code maintainability and readability.

**Example use cases:**

- Refactoring legacy code to adhere to modern coding standards.
- Updating methods or classes across multiple files to ensure consistency.
- Optimizing large code blocks or implementing significant logic changes.

- **Lab:** [Exploring Github Copilot Edit](labs/01b-exploring-copilot-edit.md)

### 3. GitHub Copilot Agent

Leverage Copilot Agent to automate repetitive tasks and workflows, integrating seamlessly within your development environment. This tool acts as an intelligent assistant, automating tasks such as generating boilerplate code, managing dependencies, and routine maintenance.

**Example use cases:**

- Automatically generating boilerplate for new projects or components.
- Managing dependency updates and routine environment configurations.
- Automating code reviews and routine coding checks.

- **Lab:** [Automating Tasks with Copilot Agent](labs/01c-exploring-copilot-agent.md)

### 4. GitHub Copilot Inline

Enhance productivity by using inline prompts directly within your editor, enabling quick code changes, method generation, and small incremental improvements without leaving the coding context. This approach is ideal for quick, iterative coding and minor adjustments that don't require extensive context switching.

**When to use:**

- When making focused changes to a specific section of code
- During active development sessions where flow state is important
- For quick prototyping or iterative improvements to existing code
- When you need immediate suggestions without breaking your concentration

**Example use cases:**

- Quickly renaming methods or variables across a single file
- Generating and inserting simple code blocks or functions
- Implementing standard design patterns (e.g., Factory, Singleton, Observer)
- Writing unit tests for existing methods
- Converting between code formats (e.g., converting a for loop to LINQ in C#)
- Generating regular expressions for specific validation requirements
- Implementing interface methods or abstract class implementations
- Creating data models or DTOs based on existing patterns in your codebase

**Azure-specific examples:**

- Quickly adding Azure SDK authentication code snippets directly in your editor
- Generating Azure Storage or Cosmos DB access code inline where needed
- Converting local configuration to Azure App Configuration or Key Vault references
- Adding Azure Monitor Application Insights telemetry to existing methods

- **Lab:** [Exploring Github Copilot Inline](labs/01d-exploring-copilot-inline.md)

### 5. GitHub Copilot Website

Utilize the Copilot Web interface to explore suggestions, manage preferences, and gain insights into your coding habits and productivity. This centralized interface provides a comprehensive dashboard for reviewing usage analytics, setting global preferences, accessing educational resources, and managing your Copilot subscription.

**When to use:**

- For managing subscription details and billing information
- To review personal usage metrics and productivity insights
- When setting up global preferences that apply across all development environments
- To access educational materials and latest updates about Copilot capabilities
- For providing feedback on Copilot's performance and suggestions

**Example use cases:**

- Reviewing coding suggestions outside of your IDE
- Analyzing your coding patterns to identify improvement areas
- Managing subscription details and preferences
- Customizing Copilot's behavior across different programming languages
- Setting up global ignore patterns for sensitive or proprietary code
- Accessing learning resources and official documentation
- Reviewing historical usage statistics to optimize your development workflow
- Providing feedback to the GitHub team on Copilot's suggestions

**Azure-specific examples:**

- Accessing tailored Azure best practices resources through your Copilot dashboard
- Reviewing Azure-related code quality metrics in your Copilot analytics
- Managing Azure-specific Copilot custom instructions through the web interface
- Exploring additional Azure development resources linked from the Copilot dashboard

- **Lab:** [Navigating Copilot's Web Interface](labs/01e-github-copilot-website.md)

### 6. GitHub Copilot CLI

Streamline command-line operations by generating shell commands and automating tasks directly from your terminal. Copilot CLI transforms natural language descriptions into powerful command-line instructions, making complex operations accessible without requiring memorization of syntax or extensive documentation lookups.

**When to use:**

- For executing complex or infrequently used commands
- When working in terminal-heavy workflows
- For automating repetitive command-line tasks
- When you need to construct complex pipelines or data transformations
- If you're unfamiliar with command syntax for specific tools or platforms

**Example use cases:**

- Generating complex git commands for repository management (e.g., interactive rebasing, complex merges)
- Creating automation scripts quickly through natural language prompts
- Executing system-level operations without extensive command-line expertise
- Constructing advanced grep, sed, or awk commands for text processing
- Building deployment pipelines and CI/CD scripts
- Generating database queries or migration scripts
- Creating Docker and Kubernetes management commands
- Formulating complex data transformation pipelines using tools like jq or yq

**Azure-specific examples:**

- Generating Azure CLI (az) commands for resource management
- Creating complex Azure Resource Manager (ARM) deployment commands
- Building Azure DevOps pipeline definitions and commands
- Automating Azure resource monitoring and maintenance tasks
- Generating secure connection strings and authentication commands
- Creating Azure Kubernetes Service (AKS) management commands
- Developing multi-stage deployment scripts for Azure environments

- **Lab:** [Mastering GitHub Copilot CLI](#) Coming Soon

---
