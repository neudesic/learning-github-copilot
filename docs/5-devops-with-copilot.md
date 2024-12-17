# Lesson 5: DevOps with GitHub Copilot

## Overview

**Goal:**  
In this lab, you will learn how to leverage GitHub Copilot for DevOps tasks in the **OrderingService**. Specifically, you will:

- Create a parameterized GitHub Actions build pipeline for CI
- Develop a deployment workflow for Azure App Service
- Set up Docker containerization with:
  - Dockerfile creation
  - Container image build and publish workflow
- Implement Kubernetes deployment with:
  - K8s manifest generation
  - AKS deployment workflow
  - Monitoring and rollback configurations

By the end of this lab, you will have hands-on experience using GitHub Copilot to create end-to-end CI/CD pipelines, containerization, and cloud deployment workflows.

**Estimated Duration:**  
45-60 minutes

**Audience:**  
DevOps Engineers, Platform Engineers, and Developers interested in DevOps practices.

**Prerequisites:**  
To successfully complete this lab, ensure you have the following:  

- **Access to GitHub Copilot:** You must have an active GitHub Copilot subscription.
- **Visual Studio Code (VS Code):** Installed and set up for development.
- **GitHub Copilot Extension for VS Code:** Installed and properly configured in VS Code.
- **GitHub CLI (gh):** Installed on your local machine.
- **GitHub Copilot CLI Extension:** Installed and connected to your GitHub account via GitHub CLI.

> **Note:** If you have not completed any of the above steps, please refer to - [Lesson 1: Installing and Configuring GitHub Copilot](docs/1-installing-copilot.md) for detailed instructions.

## Create a Build Pipeline

In this step we will generate a build pipeline for the `OrdersApi` using GitHub Actions.  

1. In Visual Studio Code, navigate to the `samples/eshop/src/Ordering.API` folder.
2. Open the Copilot Edits pane in Visual Studio Code by clicking the Copilot icon in the sidebar or using the command palette (`Ctrl+Shift+I`). In the chat settings, Lets select `Claude 3.5 Sonnet (Preview)` model for API documentation.
3. Open `Orders.Api.cs` and `Ordering.API.csproj` files.
4. Next we will develop a GitHub action that will create a YAML file to generates a build for Orders API. Enter the following prompt in the chat:

    ```plaintext
    add a github action to generate build for Ordering.API and name it `ordering-api-build.yml` and place it under eshop/.github/workflows folder
    ```

5. Observe the content of the file. You can open the file preview by pressing `Ctrl-Shift-V`.
6. This looks good but we would like to parameterize some of the elements in this pipeline (for example: the artifact location). Use the following prompt to parameterize the GitHub Action:

    ``` plaintext
    parameterize the workflow so it takes inputs such as the branch name to build, the build configuration (e.g., Debug or Release). Use on: workflow_dispatch to accept these inputs and include steps for checking out the code, running the build using a script, and uploading the build artifacts. The workflow should use environment variables derived from the inputs to control the build process
    ```

7. Observe the changes made in YAML.
8. Add a prompt to add any other missing parameters.

## Create a Deployment pipeline

**Description:**  
In this step we will generate a deployment pipeline for the `OrdersApi` using GitHub Actions.

1. In Visual Studio Code, navigate to the `samples/eshop/src/Ordering.API` folder.
2. Open the Copilot Edits pane in Visual Studio Code using the command palette (`Ctrl+Shift+I`). Keep the `Claude 3.5 Sonnet (Preview)` model selected.
3. Open `Orders.Api.cs` and `Ordering.API.csproj` files.
4. Let's create a deployment workflow that will deploy our API to Azure App Service. Enter the following prompt in the chat:

    ```plaintext
    create a github action named ordering-api-deploy.yml that deploys Ordering.API to Azure App Service. Include parameters for environment selection (dev/staging/prod), Azure credentials, and App Service name. Add necessary secrets handling
    ```

5. Observe the content of the workflow file.
6. Let's parameterize the deployment workflow to make it more flexible. Use this prompt:

    ```plaintext
    modify the deployment workflow to include parameters for:
    - Azure region
    - Service plan tier
    - Application settings
    - Deployment slots
    Add environment-specific configurations and approval gates for production
    ```

7. (Optional) Add any additional security measures with this prompt:

    ```plaintext
    enhance the deployment workflow with:
    - Security scanning steps
    - SSL certificate handling
    - IP restrictions
    ```

**Additional Notes:** Always review the generated workflows carefully and ensure all sensitive information is properly handled through GitHub Secrets.

## Create a pipeline to Build a Container Image

In the previous steps, we've developed workflows to create a build and deploy that build to a Azure App Service. In this step we will create a docker image for `Ordering.API` project. In the next step we will deploy it to Azure Kubernetes Services.

1. In Visual Studio Code, navigate to the `samples/eshop/src/Ordering.API` folder.
2. Open the Copilot Edits pane in Visual Studio Code using the command palette (`Ctrl+Shift+I`). Keep the `Claude 3.5 Sonnet (Preview)` model selected.
3. Open `Orders.Api.cs` and `Ordering.API.csproj` files.
4. Next, create a Dockerfile for the project using the following prompt:

    ```plaintext
    Create a dockerfile for Ordering.API.csproj and add docker support for the project. Save the dockerfile in the project folder         
    ```

5. Confirm that co-pilot created a Dockerfile, a .dockerignore file.
6. Next create a build Image pipeline that creates an image using the Dockerfile and publishes the image to Azure container Registry. Check to make sure the project file and the docker files are still in the Co-pilot edits' working set. Enter the following in Co-pilot edits chat:

    ```plaintext
    generate a github action workflow to build the docker image for Ordering.API using the Dockerfile and publish the image to Azure container registry. Parameterize the workflow file for all azure inputs and build configurations. save it in a file named ordering-api-build-image.yml under eshop/.github/workflows folder
    ```

7. Review the generated workflow file.

## Deploy image to an AKS Cluster

In this step we will deploy the `Ordering.API` image to AKS cluster

1. In Visual Studio Code, ensure you're still in the `samples/eshop/src/Ordering.API` folder.
2. We'll need to create Kubernetes deployment manifests first. Enter this prompt in the Copilot chat:

    ```plaintext
    create kubernetes deployment and service manifests for Ordering.API. Include configuration for horizontal pod autoscaling, resource limits, and health probes. Save them in a k8s folder under the project
    ```

3. Review the generated Kubernetes manifests in the `k8s` folder.
4. Now, let's create a deployment workflow. Use this prompt in Copilot:

    ```plaintext
    create a github action workflow named ordering-api-deploy-aks.yml that deploys the Ordering.API container to AKS. Include:
    - Parameters for AKS cluster name, resource group, and namespace
    - Kubernetes manifest application
    - Helm chart support (optional)
    - Configuration for different environments
    - Health checks after deployment
    ```

5. The workflow should now appear in the `.github/workflows` folder. Review it carefully.
6. To add monitoring and rollback capabilities, use this prompt:

    ```plaintext
    enhance the AKS deployment workflow with:
    - Deployment health monitoring
    - Automatic rollback on failure
    - Integration with Azure Monitor
    ```

## Additional Notes

- **Review Generated Code**: Always review Copilot-generated code for accuracy and relevance.
- **Test Locally**: Test workflows and scripts in a local or staging environment before deploying.
- **Validate YAML Syntax**: Use a YAML validator to check for syntax errors in GitHub Actions workflows.
- **Parameterize Configurations**: Use environment variables and input parameters for flexibility.
- **Security Considerations**: Store secrets securely using GitHub Secrets; avoid hardcoding credentials.
- **Continuous Improvement**: Regularly review and update workflows based on feedback and best practices.
- **Error Handling**: Implement error handling to manage failures gracefully.

## Introduction to GitHub Copilot for Azure  

In the previous steps, we've worked directly with GitHub Copilot to create a build and deploy that build to a Azure App Service. In this part of the lab we will explore how GitHub Copilot for Azure Chat Assistant can help.

1. In Visual Studio Code, navigate to the `samples/eshop` folder.
2. Open the Copilot Chat using the command shortcut (`Ctrl+Alt+I`). Keep the `Claude 3.5 Sonnet (Preview)` model selected.
3. In Chat view, type "@azure how can you help with Azure?" or similar prompt and press Enter.  
  GitHub will response with a sample list of the services it can help with.
4. Next, type "@azure which subscriptions I can use?" and press Enter.  
  The assistant will reply with available subscriptions, if any.
5. Next, type "@azure can you create the files I need to deploy this solution?" and press Enter.  
  The Assistant will suggest requested files, such as .bicep file for AKS cluster and manifests for deployments and services, as well as the steps to deploy them.  
6. Review the files and suggested steps.

## Summary

You have learned how to leverage GitHub Copilot for various DevOps tasks, including:

- Creating parameterized CI/CD pipelines with GitHub Actions
  - Build pipeline configuration
  - Deployment workflow setup
  - Environment-specific configurations
- Containerization workflows
  - Dockerfile generation
  - Container image build and publish
  - Container registry integration
- Kubernetes deployment
  - Manifest generation
  - AKS deployment configuration
  - Monitoring and rollback setup

Throughout this lab, you've gained practical experience in using GitHub Copilot to streamline DevOps processes

**Next Steps:**

- [Lesson 6: Best practices with GitHub Copilot](6-best-practices-with-copilot)
