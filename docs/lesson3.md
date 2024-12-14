
# Hands-On Lab: Testing with GitHub Copilot

## Overview

**Goal:**  
By the end of this session, you will learn basics of how GitHub Copilot can help with generating and executing test cases.

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
    - [Step 1: Generating Unit Tests](#step-1-generating-unit-tests)
    - [Step 2: Generating Integration Tests](#step-2-generating-integration-tests)
4. [Troubleshooting](#troubleshooting)
5. [Conclusion](#conclusion)

---

## Lab Objectives
  
- Understand how to leverage GitHub Copilot to generate and execute unit and integration tests.  
- Learn how incremental test improvements can be achieved.  

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

### Step 1: Generating Unit Tests
**Description:**  
Provide a short description of the task or feature being demonstrated.  

**Instructions:**  
1. In VS Code, open file **src/Basket.API/Model/BasketItem.cs**
2. Open Chat View by pressing Ctrl-Alt-I.
3. Enter following request "generate comprehensive suite of unit test for this class. Include edge cases." and press Enter.  In a few moments, a code for new class *BasketItemTests* will show in the Chat View. 
4. Copy the generated code and save it in a new file named *BasketItemTests.cs* in the **tests/Basket.UnitTests/** folder.
5. Inspect the code in the file. Notice that GitHub Copilot recognized test frameworks used in the solution.
6. In the Chat View prompt, enter "how do I run these tests?".
7. The response will include instructions to open the terminal in the test project folder and execute tests. Follow the instructions or executed the following (assuming current directory is **/eShop/):
   ```bash
   cd tests/Basket.UnitTests
   dotnet test
   ```

**Expected Outcome:**  
The existing and new unit tests should pass. If some test cases fail, you can use GitHub Copilot to fix the code or the failing unit tests.  

**NOTE**:  Generated code may not be correct as GitHub Copilot context grasp is limited and may use incorrect assumptions.  In general, it will correctly identify the testing framework for the project.  
Use your skills and GitHub Copilot to resolve the issue by varying your prompts and focus in an iterative manner.  

---

### Step 2: Generating Integration Tests
**Description:**  
In this step, you will use GitHub Copilot to create an new integration tests project for a service and execute it.  

**Instructions:**  
1. In VS Code, open file **src/Basket.API/Grpc/BasketService.cs**
2. Open Chat View by pressing Ctrl-Alt-I.
3. Ask GitHub Copilot help with creating a new project by entering the prompt "how di I create Basket.FunctionalTests test project for testing this service?". In a few moments, GitHub Copilot will respond with a full set of instructions to create the new project. Observe that mocks for the dependencies are generated as well!
4. Follow the instructions, usually grouped in the 4 steps:  
&emsp;- Create a new test project (change current directory to **tests/** before executing the commands)  
&emsp;- Update the project file to include packages without specifying specific versions (remove coverlet.collector if referenced!)  
&emsp;- Create a test class for the service inside the project folder  
&emsp;- Run the tests
   ```bash
   dotnet test
   ```

**Expected Outcome:**  
Newly created tests should pass. If some test cases fail, you can use GitHub Copilot to fix the code or the failing tests.  

**NOTE**:  Generated code may not be correct as GitHub Copilot context grasp is limited and may use incorrect assumptions.  In general, it will correctly identify the testing framework and mocking library for the project.  
Use your skills and GitHub Copilot to resolve the issue by varying your prompts and focus in an iterative manner.  

---

## Troubleshooting

- **Suggested command/script fails:** GitHub Copilot is great, but is not perfect.  
  **Solution:**  Use your skills and GitHub Copilot to help resolve the issue in an iterative manner. This is extra challenge and learning opportunity!   

---

## Conclusion

**Summary:**  
In this lab you should have learned some basic means you can use GitHub Copilot for unit and integration testing.  

**Next Steps:**  
- [Lesson 4: Creating Documentation with GitHub Copilot](docs/lesson4.md)
--- 
