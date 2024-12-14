# Hands-On Lab: Creating Documentation with GitHub Copilot

## Overview

**Goal:**  
By the end of this session, you will learn basics of how GitHub Copilot can help with generating inline comments and project documentation.

**Estimated Duration:**  
[Time in minutes/hours]

**Audience:**  
[developers, ops, testers, etc.]

**Prerequisites:**

- Completed [Lesson 1: Installing and Configuring GitHub Copilot](docs/lesson1.md), or
- Access to GitHub Copilot (subscription),
- Visual Studio Code,
- GitHub Copilot extension to VS Code.

---

## Table of Contents

1. [Lab Objectives](#lab-objectives)
2. [Environment Setup](#environment-setup)
3. [Walkthrough](#walkthrough)
   - [Step 1: Generate Inline Comments](#step-1-generate-inline-comments)
   - [Step 2: Generate Project Documentation](#step-2-generate-project-documentation)
4. [Troubleshooting](#troubleshooting)
5. [Conclusion](#conclusion)

---

## Lab Objectives

- Understand how to leverage GitHub Copilot to add code comments and document your projects.

---

## Environment Setup

### Tools and Resources

- GitHub Copilot enabled on your GitHub account.
- Visual Studio Code with GitHub Copilot extension installed.
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

### Step 1: Generate Inline Comments

**Description:**  
In this step, we will use o1-mini model and Copilot Edits to generate inline comments.

**Instructions:**

1. In VS Code, open file **src/Basket.API/Grps/BasketService.cs**
2. Open Copilot Edits by pressing Ctrl-Shift-I.
3. Observe that BasketService.cs file is already in the Working Set. (1 file).  
   You can manage the Working Set by adding and removing files.
4. In the command bar area at the bottom, click model selection at the right corner and select **o1-mini (Preview)**.  
   We will use faster o1-mini model.
5. In the prompt field, enter "add comments" and press Enter.  
   GitHub Copilot confirms the plan to add simple inline comments to the code and proceeds with applying the edits to the file.
6. Click **Discard** button.  
   All edits will be discarded. We wanted XML comments!
7. In the prompt field, enter "add xml comments" and press Enter.  
   GitHub Copilot confirms the plan to add XML Comments to the class and public methods and proceeds with applying the edits to the file.
   responds with suggested XML comments for each function in the class.
8. (Optional) In the prompt field, enter "add comments to private methods as well" and press Enter.
   GitHub Copilot confirms the plan to add XML Comments to the private methods as well and proceeds with applying the edits to the file.
9. Click **Accept** button and save the file.

**Expected Outcome:**  
GitHub Copilot generates comments based on the code you provide and your prompt. While the generated comments **can** be accurate and useful responses, it's important to review and correct the generated output to ensure it has desired qualities.

---

### Step 2: Generate Project Documentation

**Description:**  
In this step, we will use o1-preview model and Copilot Edits to generate project documentation.

**Instructions:**

1. Open Copilot Edits by pressing Ctrl-Shift-I.
2. In the command bar area at the bottom, ensure that model **o1-mini (Preview)** is selected.  
   We will use faster o1-mini model.
3. In the prompt field, enter "Generate solution documentation that can be used at an executive briefing. Save it as TLDR.md file" and press Enter.  
   GitHub Copilot confirms the plan and creates requested documentation to the TLDR.ms file.
4. Observe the content of the file. You can open the file preview by pressing Ctrl-Shift-V.
5. In the prompt field, enter "add dependencies, please" and press Enter.  
   GitHub Copilot confirms the plan and adds dependencies under Technical Overview.
6. Click **Accept** button and save the file.

**Expected Outcome:**  
GitHub Copilot generated requested executive briefing and you were able to modify it an iterative manner using natural language prompts.  
While the generated comments **can** be accurate and useful responses, it's important to review and and ensure it is accurate and made according to the responsible AI principles, e.g. [Responsible AI with GitHub Copilot](https://learn.microsoft.com/en-us/training/modules/responsible-ai-with-github-copilot/).

---

## Troubleshooting

- **Generated response is truncated or looks incomplete:** LLM models have finite set of tokens to use which includes current scope and previous prompts.  
  **Solution:** You may try selecting a different model, or try clearing previous context (if it is irrelevant to the current prompt). Use /clear command.

---

## Conclusion

**Summary:**  
In this lab you have learned how you can work with GitHub Copilot to better understand and document your solutions.

**Next Steps:**

- [Lesson 5: DevOps with GitHub Copilot](docs/lesson5.md)
