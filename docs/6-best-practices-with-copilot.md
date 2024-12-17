# Best Practices for Using GitHub Copilot

## Provide the Right Context

When you use GitHub Copilot, it's important to provide the right context for the code you want to generate.

1. Provide a clear description of the problem you are trying to solve in the prompt.

2. Include samples of the input and output data when possible. This is called [Few-Shot Learning](https://en.wikipedia.org/wiki/Few-shot_learning).

3. Open relevant files in your workspace to give GitHub Copilot more context. Copilot uses the open files in your workspace to generate suggestions.

4. Use top-level file comments to provide additional context for the code you want to generate.

   > Note: here are two examples of top-level comments that you can use file. This was very common in C++ and older languages, but is less common in modern languages. What is old is new again!

   ```csharp
   //########################################################################
   // <filename>.cs
   //
   // Description: This file contains the implementation of the `Foo` class.
   //########################################################################
   ```

   ```javascript
   //########################################################################
   // <filename>.js
   //
   // Description: Handles user authentication and session management
   //########################################################################
   ```

5. Include the appropriate imports and references in your code. This helps GitHub Copilot understand the dependencies of your code.

6. Follow good coding practices. This includes writing clean, readable code with descriptive variable and class names. GitHub Copilot tries to generate code that follows the same style as the code you provide- so the better your code, the better the suggestions.

## Using GitHub Copilot Chat and Edits

1. Make heavy use of the chat participants and slash commands to guide Copilot in the right direction.

## Custom Instructions to GitHub Copilot

1. Different models at different times can generate code that does not take every details in your solution, depending on the current context and model capacity.

   For example, open the file **src/Basket.API/Grpc/BasketService.cs** and press Ctrl-Alt-I to open Chat view and ask to `create unit tests for this class` and observe the generated code.  
   In most cases, for .NET solutions, it will generate code using `Moq` library for mocking dependencies.  

2. You can create a custom instructions file to automatically add information to all questions you ask Copilot.  
   To this end, create a file **.github/copilot-instructions.md** and add and instruction like `When mocking dependencies in tests, we use NSubstitute.`.  
   Close the file and retry the question. Observe, two things:  
   - GitHub Copilot used two references, the file in focus and the custom instructions file.  
   - Dependencies are mocked using `NSubstitute` library.  

3. Since custom instructions consume model tokens, you should try to write [effective instructions](https://docs.github.com/en/copilot/customizing-copilot/adding-custom-instructions-for-github-copilot#writing-effective-custom-instructions).  

4. in VS Code, you can temporary switch custom instructions on and off by opening the Setting editor (by using shortcut `Ctrl + ,`, (Linux/Windows) / `Command + ,` (Mac).  

   Type `instruction file` in the search box and select or clear the **Use Instruction Files** checkbox.
