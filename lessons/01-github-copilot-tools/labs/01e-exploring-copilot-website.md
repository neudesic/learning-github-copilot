### Lab: Exploring Github Copilot on GitHub.com

## Overview

**Goal:**  
Learn how to interact with GitHub Copilot directly on the GitHub.com website to ask questions about code, understand repositories, and receive helpful suggestions without needing an IDE.

**Estimated Duration:**  
15-20 minutes

**Audience:**  
Developers, technical writers, and students looking to explore GitHub Copilot through the web experience without setting up a local development environment.

**Prerequisites:**

- A GitHub account with an active GitHub Copilot subscription
- A public or private repository with some source code (JavaScript, Python, C#, etc.)
- A modern web browser

## Lab Steps

### 1. Navigate to a Repository

- Open your browser and go to [https://github.com](https://github.com).
- Open a repository that contains some code (e.g., a JavaScript or Python project).
- Make sure you are signed in to your GitHub account with Copilot enabled.

### 2. Open Copilot Chat on GitHub.com

- Click the **Copilot Chat** button in the top-right corner of the repository page (only visible to users with an active Copilot subscription).
- This will open a side panel where you can interact with Copilot.

### 3. Ask a More Interesting Question

- In the Copilot Chat panel, type the following question:

  ```text
  How would I convert this app to use a serverless architecture with AWS Lambda?
  ```

  Press Enter.

  Copilot will provide a conceptual answer based on the code it sees in the repository, such as identifying entry points, explaining how to refactor handlers, and suggesting deployment strategies.

### 4. Ask About Specific Files

- Open a specific file in the repository (e.g., `main.py`, `app.js`, or `OrderService.cs`).
- With the file open, go back to the Copilot Chat panel and ask:

  ```text
  Explain what this file does.
  ```

  - Observe how Copilot provides a contextual explanation.

### 5. Ask for Improvements

- Now try asking:

  ```text
  What improvements can I make to this file's error handling?
  ```

  - Review the suggestions and consider how they could apply to your code.

## Best Practices

- **Be Specific:** Reference files or lines for more precise answers.
- **Ask Iteratively:** Break your questions down if Copilot gives vague results.
- **Explore Freely:** Ask about repo structure, functions, or how to refactor code.

## Summary

By completing this lab, you’ve learned to:

- Access and use GitHub Copilot directly on GitHub.com
- Ask questions about a repository or individual files
- Get actionable feedback and code suggestions without needing to open an IDE

GitHub Copilot on the web is perfect for quick insights, documentation reviews, and exploring unfamiliar codebases in the browser.
