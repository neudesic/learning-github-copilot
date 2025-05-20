# Copilot Best Practices

## 1. Use It to Boost, Not Replace, Your Thinking

## 2. Security & Ethics – Avoid Leaking Sensitive Data

- **Do not** include secrets, API keys, or passwords in prompts.
- **Review licenses** for generated code that looks like it came from real projects.
- **Avoid copying** without understanding.

## 3. Design Your Workflows Around Prompt Quality, Context Control, and Code Validation

### a. Prompt Quality

- Write clear and intentional prompts.
- Remove unused or misleading comments in code.
- Rewrite your prompts to generate different responses. If Copilot is not providing a helpful response, try rephrasing.

### b. Control the Context

- Use meaningful variable and function names.
- The quality of results from Copilot Chat may be degraded if very large files, or a large number of files, are used as context for a question.
  - Break down your tasks.
  - Keep functions short and readable.
  - Comment clearly above complex logic.

- **Ways to provide necessary Context:**
  - Open relevant files and close irrelevant files to provide accurate context.
  - Provide folder or workspace as context.
  - Provide context from images, or by attaching files.
  - In Copilot Chat, if a particular request is no longer helpful context, delete that request from the conversation. Alternatively, if none of the context of a particular conversation is helpful, start a new conversation.
  - Provide context by writing clean and readable code.
    - Always use well-descriptive naming conventions that help humans read code and AI to have more context.
    - Use comments to provide context.

- **Custom Instructions to GitHub Copilot:**

  1. Different models at different times can generate code that does not take every detail in your solution into account, depending on the current context and model capacity.
     - For example, open the file `src/Basket.API/Grpc/BasketService.cs` and press `Ctrl-Alt-I` to open Chat view and ask to "create unit tests for this class" and observe the generated code.
     - In most cases, for .NET solutions, it will generate code using the `Moq` library for mocking dependencies.

  2. You can create a custom instructions file to automatically add information to all questions you ask Copilot.
     - Create a file `.github/copilot-instructions.md` and add instructions like "When mocking dependencies in tests, use NSubstitute." Close the file and retry the question.
     - Observe two things:
       - GitHub Copilot used two references: the file in focus and the custom instructions file.
       - Dependencies are mocked using the `NSubstitute` library.

  3. Since custom instructions consume model tokens, you should try to write [effective instructions](https://docs.github.com/en/copilot/customizing-copilot/adding-custom-instructions-for-github-copilot#writing-effective-custom-instructions).

  4. In VS Code, you can temporarily switch custom instructions on and off by using the Settings editor (shortcut `Ctrl + ,` (Linux/Windows) / `Command + ,` (Mac)).
     - Type "instruction file" in the search box and select or clear the **Use Instruction Files** checkbox.

### c. Code Validation

Never blindly accept code, especially for security-critical or production systems.

- **Ask for variants when needed.**
- **Review before you commit.**
  - **Checklist:**
    - Read line by line
    - Check logic and edge cases
    - Add docstrings/comments
    - Run tests or static analysis
    - Make it match your team’s standards

- Use Copilot for repetitive, boilerplate tasks:
  - Writing model classes (e.g., C# classes, TS interfaces)
  - Setting up routes or endpoints
  - Writing unit test scaffolds
  - Generating repetitive validation or serialization logic
