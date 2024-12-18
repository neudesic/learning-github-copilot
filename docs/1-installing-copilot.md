# Lesson 1: Installing and Configuring GitHub Copilot

## Overview

**Goal:**  
In this lab, you will learn how to install:

- GitHub Copilot extensions in Visual Studio Code
- GitHub Copilot CLI

**Estimated Duration:**  
10-15 minutes

**Audience:**  
 Developers, QA testers, DevOps engineers, and Technical Writers.

**Prerequisites:**

- Installed Visual Studio Code,
- GitHub Copilot subscription.

## Install GitHub Copilot in VS Code

First, we will install GitHub Copilot and its corresponding Visual Studio Code extensions.

1. Open Visual Studio Code.
2. Open Extensions view by clicking the Extensions icon in the Activity Bar
3. Enter 'GitHub Copilot' in the Search Extensions in Marketplace.
4. Click Install button on the **GitHub Copilot** extension by GitHub.
   Wait until GitHub Copilot is installed.  
   It will also install **GitHub Copilot Chat** by GitHub extension.
5. Next, we will install the following Copilot Extensions that are needed for the remainder of the training:
      - **Install GitHub Copilot for Azure**  
         - While in the Extensions view, search for **GitHub Copilot for Azure** by Microsoft.  
         - Click the **Install** button.  
         - With this extension, you can ask **@azure** questions about Azure services and receive assistance with Azure-specific tasks.  
      - **Install VS Code Speech (Optional)**  
         - In the **Search Extensions in Marketplace** field, type **Voice**.  
         - Locate the **VS Code Speech** extension by Microsoft and click the **Install** button.  
         - This extension adds speech-to-text and text-to-speech functionality, allowing you to interact with GitHub Copilot using natural language speech.
      - **Install Vision for Copilot Preview (Optional)**  
         - In the **Search Extensions in Marketplace** field, type **Vision**.  
         - Locate the **Vision for Copilot Preview** extension by Microsoft and click the **Install** button.  
         - This extension enables advanced vision capabilities, allowing you to include images in your prompts for enhanced interactions with GitHub Copilot.
         - >**Note**: This feature requires an Open AI Service and access credentials.
         - After installing the extension, update your Visual Studio Code settings:  
            - Open the Command Palette (`Ctrl+Shift+P`) and type **Preferences: Open Settings (JSON)**.  
            - Add the following parameters to your `settings.json` file:  

                  ```json
                  "copilot.vision.azureEndpoint": "<your instructor will provide an endpoint>",  
                  "copilot.vision.provider": "AzureOpenAI",  
                  "copilot.vision.model": "gpt-4o"
                  ```  

**Expected Outcome:**  
Both GitHub Copilot and GitHub Copilot Chat extensions are installed in Visual Studio Code.

---

### Step 2: Install GitHub CLI

**Description:**  
You will install GitHub CLI (_gh_) in Windows using _winget_.  
For Windows _gh_ is available via [WinGet](https://github.com/microsoft/winget-cli), [scoop](https://scoop.sh/), [Chocolatey](https://chocolatey.org/), [Conda](https://github.com/cli/cli?tab=readme-ov-file#conda), [Webi](https://github.com/cli/cli?tab=readme-ov-file#webi), and as downloadable MSI.  
For other platforms, see the instructions [here](https://github.com/cli/cli?tab=readme-ov-file#installation).

**Instructions:**

1. Open Visual Studio Code shell (Ctrl-`).
2. In the Visual Studio Code terminal window, enter the following command:

      winget install --id GitHub.cli  

3. Once GitHub CLI installation completes, login to GitHub:

      gh auth login

4. When prompted, select the following options by using the arrows keys and pressing Enter and follow the instructions:

   - Where do you use GitHub? **GitHub.com**
   - What is your preferred protocol for Git operations on this host? **HTTPS**
   - Authenticate Git with your GitHub credentials? **Yes**
   - How would you like to authenticate GitHub CLI? **Login with a web browser**
   - First copy your one-time code: _XXXX-XXXX_
   - Press Enter to open <https://github.com/login/device> in your browser...
   - (this will navigate the default browser to the GitHub Device Activation page to authenticate and give necessary authorizations)

**Expected Outcome:**  
GitHub CLI is installed and connected to your GitHub account.

---

### Step 3: Install GitHub CLI Copilot Extension

**Description:**  
You will install GitHub Copilot extension to GitHub CLI (_gh_).

**Instructions:**

1. In the Visual Studio Code terminal window, enter the following command:

      gh extension install github/gh-copilot

**Expected Outcome:**  
GitHub Copilot extension to GitHub CLI is installed.

---

## Verification

1. Test the installed Copilot extension to GitHub CLI.

      gh copilot --help

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

- [Lesson 2: Development with GitHub Copilot](2-development-with-copilot.md)

---
