
# Hands-On Lab: [Installing and Configuring GitHub Copilot]

## Overview

**Goal:**  
In this lab, you will learn how to install:
- GitHub Copilot extension in Visual Studio Code
- GitHub Copilot CLI

**Estimated Duration:**  
[10-15 minutes]

**Audience:**  
[developers]

**Prerequisites:**  
- Installed Visual Studio Code, 
- GitHub Copilot subscription.

---

## Table of Contents
1. [Lab Objectives](#lab-objectives)
2. [Environment Setup](#environment-setup)
3. [Walkthrough](#walkthrough)
    - [Step 1: Install GitHub Copilot in VS Code](#step-1-install-github-copilot-in-vs-code)
    - [Step 2: Install GitHub CLI](#step-2-install-github-cli)
    - [Step 3: Install GitHub CLI Copilot Extension](#step-3-install-github-cli-copilot-extension)
4. [Verification](#verification)
5. [Troubleshooting](#troubleshooting)
6. [Conclusion](#conclusion)

---

## Lab Objectives

- Install GitHub Copilot in Visual Studio Code.  
- Install GitHub Copilot CLI.  

---

## Environment Setup

### Tools and Resources
- GitHub Copilot enabled on your GitHub account.
- Visual Studio Code installed.
- Sample code repository: [GitHub Copilot](https://github.com/neudesic/learning-github-copilot).

---

## Walkthrough

### Step 1: Install GitHub Copilot in VS Code
**Description:**  
You will install GitHub Copilot and GitHub Copilot Chat Visual Studio Code extensions.  

**Instructions:**  
1. Open Visual Studio Code.
2. Open Extensions view by clicking the Extensions icon in the Activity Bar.
3. Enter 'GitHub Copilot' in the Search Extensions in Marketplace field
4. Click Install button on the GitHub Copilot extension by GitHub
4. Press Enter

**Expected Outcome:**  
Both GitHub Copilot and GitHub Copilot Chat extensions are installed in Visual Studio Code.

---

### Step 2: Install GitHub CLI
**Description:**  
You will install GitHub CLI (*gh*) in Windows using *winget*.  
For Windows *gh* is available via [WinGet](https://github.com/microsoft/winget-cli), [scoop](https://scoop.sh/), [Chocolatey](https://chocolatey.org/), [Conda](https://github.com/cli/cli?tab=readme-ov-file#conda), [Webi](https://github.com/cli/cli?tab=readme-ov-file#webi), and as downloadable MSI.  
For other platforms, see the instructions [here](https://github.com/cli/cli?tab=readme-ov-file#installation).

**Instructions:**  
1. Open Visual Studio Code shell (Ctrl-`).
2. In the Visual Studio Code terminal window, enter the following command:
   ```bash
   winget install --id GitHub.cli
   ```
3. Once GitHub CLI installation completes, login to GitHub:
   ```bash
   gh auth login
   ```
   When prompted, select the following options by using the arrows keys and pressing Enter and follow the instructions:  
   &emsp;? Where do you use GitHub? **GitHub.com**  
   &emsp;? What is your preferred protocol for Git operations on this host? **HTTPS**  
   &emsp;? Authenticate Git with your GitHub credentials? **Yes**  
   &emsp;? How would you like to authenticate GitHub CLI? **Login with a web browser**  
   &emsp;! First copy your one-time code: *XXXX-XXXX*  
   &emsp;Press Enter to open https://github.com/login/device in your browser...  
   &emsp;(this will navigate the default browser to the GitHub Device Activation page to authenticate and give necessary authorizations) 

**Expected Outcome:**  
GitHub CLI is installed and connected to your GitHub account.

---

### Step 3: Install GitHub CLI Copilot Extension
**Description:**  
You will install GitHub Copilot extension to GitHub CLI (*gh*).  

**Instructions:**  
1. In the Visual Studio Code terminal window, enter the following command:
   ```bash
   gh extension install github/gh-copilot

   ```

**Expected Outcome:**  
GitHub Copilot extension to GitHub CLI is installed.

---

## Verification

1. Test the installed Copilot extension to GitHub CLI.  
   ```bash
   gh copilot --help
   ```
2. Read the response and try suggested examples.  

---

## Troubleshooting

- **Issue 1:** Unable to authenticate with GitHub.  
  **Solution:** Ensure you have GitHub account and you have entered correct credentials.

---

## Conclusion

**Summary:**  
You have learned how to install GitHub Copilot in Visual Studio Code and GitHub CLI and have it installed on your computer.  

**Next Steps:**  
- [Lesson 2: Development with GitHub Copilot](docs/lesson2.md)

--- 
