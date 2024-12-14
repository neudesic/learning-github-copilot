
# Hands-On Lab: Development with GitHub Copilot

## Overview

**Goal:**  
In this lab, you will learn how to use GitHub Copilot features such as autocomplete, inline chat and chat view to generate code, understand code and fix errors.
In addition, you will learn how to use GitHub Copilot CLI to generate shell commands.

**Estimated Duration:**  
[Time in minutes/hours]

**Audience:**  
[developers, ops, testers, etc.]

**Prerequisites:**  

- Completed [Lesson 1: Installing and Configuring GitHub Copilot](docs/lesson1.md), or
- Access to GitHub Copilot (subscription),
- Visual Studio Code,
- GitHub Copilot extension to VS Code,
- GitHub CLI with GitHub Copilot extension installed and connected.  

---

## Table of Contents

1. [Lab Objectives](#lab-objectives)
2. [Environment Setup](#environment-setup)
3. [Walkthrough](#walkthrough)
    - [Step 1: Autocomplete​](#step-1-autocomplete)
    - [Step 2: Inline Chat](#step-2-inline-chat)
    - [Step 3: Chat View](#step-3-chat-view)
    - [Step 4: Using GitHub Copilot CLI](#step-4-using-github-copilot-cli)
4. [Conclusion](#conclusion)

---

## Lab Objectives

- Understand how to leverage GitHub Copilot to autocomplete functions.  
- Explore Copilot’s suggestions in specific scenarios, such as refactoring.  
- Learn basics of GitHub CLI and Copilot extension.

---

## Environment Setup

### Tools and Resources

- GitHub Copilot enabled on your GitHub account.
- Visual Studio Code with GitHub Copilot extension installed.
- GitHub CLI with GitHub Copilot extension installed and connected.  
- Code repository for the lab: [GitHub Copilot](https://github.com/neudesic/learning-github-copilot).

### Steps to Prepare

1. Clone the repository:  

   ```bash
   git clone https://github.com/neudesic/learning-github-copilot.git
   cd learning-github-copilot/samples/eShop
   ```

2. Open the project in your IDE:  

   ```bash
   code .
   ```

---

## Walkthrough

### Step 1: Autocomplete

**Description:**  
GitHub Copilot autocomplete feature in action.  

**Instructions:**  

1. In VS Code, open file **src/Basket.API/Model/BasketItem.cs**
2. In the *Validate()* function, select the line

   ```csharp
   throw new NotImplementedException();
   ```

   and delete it.  
3. Observe "ghost text" suggested by GitHub Copilot.  
4. You can accept by pressing Tab key, or accept part word by word by using Ctrl-Arrow keys. Press Tab to accept the code.  
5. (Optional) Observe the following line:  

   ```csharp
   if (Quantity <= 0)
   ```

   Select and replace "= 0" with space " " and observe the new suggestion!  
   Press Tab to accept, or Ctrl-Z to undo your change.

**Expected Outcome:**  
Implemented *Validate()* function.

---

### Step 2: Inline Chat

**Description:**  
In this step, we will demonstrate some basic GitHub Copilot natural language capabilities within the Inline Chat.  

**Instructions:**  

1. In the *Validate()* method, select the following line:  

   ```csharp
   results.Add(new ValidationResult("Invalid quantity", new[] { "Quantity" }));
   ```

   and click Ctrl-I.
2. In the prompt, enter "make the message more specific, please" and press Enter.
3. Observe the proposed change and click **Accept** or **Discard**
6. (Optional) Save the change.  

**Expected Outcome:**  
You have seen some of the basic natural language capabilities of Inline Chat.  
In the subsequent lessons you will learn more.

---

### Step 3: Chat View

**Description:**  
Chat View opens up more space and deeper ways to interact with GitHub Copilot.  

**Instructions:**  

1. In VS Code, open file **src/Basket.API/Grps/BasketService.cs**
2. Open Chat View by pressing Ctrl-Alt-I.  
3. In the Chat View prompt, you can use natural language to ask about current context (files open in the editor).
&emsp;For example, enter "how does this service work?" and press Enter.
4. Observe the GitHub Copilot response.
5. Alternatively, you can just enter one of the / commands, like /explain to get similar result.
6. You can continue dialog within the same context.  
&emsp;For example, enter "how can I optimize logging in this code?" and press Enter.
6. Observe the suggested code changes.

7. (Optional) You can copy parts of or the whole suggested code and paste in the editor manually, or click appropriate icon above the code view to Insert at Cursor, Apply in Editor, or just copy to clipboard.
8. (Optional) Save the file.

**Expected Outcome:**  
GitHub Copilot uses current context and responds to natural language queries as well as built in commands.  
For more details on these, see [Getting started with prompts for Copilot Chat](https://docs.github.com/en/copilot/using-github-copilot/guides-on-using-github-copilot/getting-started-with-prompts-for-copilot-chat)

---

### Step 4: Using GitHub Copilot CLI

**Description:**  
Provide a short description of the task or feature being demonstrated.  

**Instructions:**  

1. In VS Code, press Ctrl-` to open a new terminal if necessary.  
2. In the terminal window enter the following command:  

   ```bash
   gh copilot --help
   ```

3. Observe the command usage help and try some of the commands, e.g.:

   ```bash
   gh copilot suggest "list commits in the last 3 days"
   ```

   When prompted:  
   ? What kind of command can I help you with?  
   &gt; generic shell command  
   &emsp;gh command  
   &emsp;git command  
   press Enter to select generic shell command

   When prompted:  
   ? Select an option  [Use arrows to move, type to filter]  
   &gt; Copy command to clipboard  
   &emsp;Explain command  
   &emsp;Execute command  
   &emsp;Revise command  
   &emsp;Rate response  
   &emsp;Exit  

   you can select any of the listed options using up-down arrow keys and pressing Enter.
   Try few different options and select Exit to end.

**Expected Outcome:**  
You have seen how you can use GitHub Copilot CLI to generate, understand and execute shell commands and scripts.

---

## Conclusion

**Summary:**  
You have learned basics of several major features of GitHub Copilot, including:  

- Autocomplete
- Inline Chat
- Chat View
- how to use GitHub Copilot CLI while working with shell commands and scripts.

**Next Steps:**  

- [Lesson 3: Testing with GitHub Copilot](docs/lesson3.md)
