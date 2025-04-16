# Lesson 2: Development with GitHub Copilot

## Notes (@skyarkitekten - fix this @mikeholdorf)

- fork
- clone
- add your lab partner as collaborator
- can watch and do later, can drive with me

## Overview

**Goal:**  
In this lab, you will explore and utilize various GitHub Copilot features, including autocomplete, inline chat, chat view, Copilot CLI, and Copilot Edits (preview), to enhance the **Ordering.Domain**. You will add a method to calculate order totals, incorporating discounts and taxes. Additionally, you will learn how to use Copilot to:

- Generate new code and improve existing code.
- Understand code efficiently using chat-based interactions.
- Streamline shell command creation with Copilot CLI.
- Refactor and optimize code using Copilot Edit.

By the end of this lab, you will have enhanced the `OrderAggregate` with a comprehensive `GetTotal` method and gained practical experience with Copilot's full suite of features.

**Estimated Duration:**  
30-45 minutes

**Audience:**  
 Developers, QA testers, DevOps engineers, and Technical Writers.

**Prerequisites:**  
To successfully complete this lab, ensure you have the following:

- **Access to GitHub Copilot:** You must have an active GitHub Copilot subscription.
- **Visual Studio Code (VS Code):** Installed and set up for development.
- **GitHub Copilot Extension for VS Code:** Installed and properly configured in VS Code.
- **GitHub CLI (gh):** Installed on your local machine.
- **GitHub Copilot CLI Extension:** Installed and connected to your GitHub account via GitHub CLI.

> **Note:** If you have not completed any of the above steps, please refer to - [Lesson 1: Installing and Configuring GitHub Copilot](docs/1-installing-copilot.md) for detailed instructions.

## Autocomplete with Code Completions (Ghost Text mode)

GitHub Copilot's autocomplete feature provides intelligent code suggestions as you type, helping you write code faster and with fewer errors. In this step, we will explore how to use autocomplete effectively.

1. Open the file `samples/Orders/Orders.Domain/Order.cs` in Visual Studio Code.
2. Hit enter a few times and see the autocomplete suggestion. It likely suggests a method signature similar to the following:

   ```csharp
   public void RemoveOrderItem(OrderItem item)
   ```

3. Press the Tab key to accept the suggestion.
4. Chat completions are nice for rapid prototyping, but they can be a bit of a distraction for most developers. You can disable it by going to status bar, clicking on the GitHub Copilot icon, and unchecking code completions.

## Inline Chat

Now, let’s start by enhancing the OrderingService by leveraging inline chat feature. Our task is to add a method for calculating order totals, considering discounts and taxes.

1. In Visual Studio Code, navigate to the `samples/eshop/src/Ordering.Domain/AggregatesModel/OrderAggregate` folder. Open the `Order` class file for modification.
2. Before we add a method named CalculatedTotal that includes taxes, we need to rename existing GetTotal method to GetSubTotal  
   Use GitHub Copilot to rename the `GetTotal` method to `GetSubTotal`.

   - Scroll to the bottom of the Order class and select `GetTotal` public member
   - Open inline Co-pilot Prompt with (`Ctrl+I`), and enter the following prompt:

     ```plaintext
     Rename symbol `GetTotal` to `GetSubTotal`
     ```

3. In the inline chat, enter the following prompt:

   ```plaintext
   Add a new public member named GetTotal that is calculated as sum of order items and a new public property of type decimal named taxes and subtracts the sum of items in a public property of type List<decimal> named Discounts
   ```

4. Now lets update the logic for discount to use the Discount property of `OrderItem` class.

   - In the inline chat, enter the following prompt:

   ```plaintext
     Add an empty public member named GetOrderDiscounts that returns a decimal
   ```

   - Click Accept of the inline chat to close the window.
   - delete the return statement
   - Observe "ghost text" suggested by GitHub Copilot.
   - You can accept by pressing Tab key, or accept part word by word by using Ctrl-Arrow keys. Press Tab to accept the code.
   - Open Inline chat again (`Ctrl+I`), enter the following prompt

   ```plaintext
      Update `GetOrderDiscounts` method to use use `Discount` property in OrderItem
   ```

   - This would replace the logic to calculate the order discount by adding up the discounts on each line item
   - Finally, in the `GetTotal` method, if you delete the value assigned to `totalDiscounts` variable, you should see the new `GetOrderDiscounts` method. Enter Tab to accept changes

## Copilot Chat - Ask

GitHub Copilot Chat provides an interactive way to engage with Copilot, offering more space and deeper ways to interact with your code. With the Chat View, you can ask natural language questions about the current context in your editor, request code suggestions, and optimize your code with Copilot's help.  

1. Open the Copilot Chat pane in Visual Studio Code by clicking the Copilot icon in the menu bar or using the keyboard shortcut (`Ctrl+Alt+I`). In the chat settings, ensure the model is set to GPT 4o. If it’s not already selected, switch the model to GPT 4o in the Copilot settings.  
   >**Note**: We will use this model for this lesson. In later lessons we will experiment with utilizing other models for various tasks such as testing and documentation.  
2. In the Chat View prompt, you can use natural language to ask about current context (files open in the editor)  

   - For example, browse to `src/Ordering.API/Apis` folder and open 'OrdersApi.cs' file.
   - Enter "how does this service work?" and press Enter.
   - Observe the GitHub Copilot response.
   - Alternatively, you can just enter one of the / commands, like /explain to get similar result.
   - You can continue dialog within the same context.  
      &emsp;For example, enter "how can I optimize logging in this code?" and press Enter.
   - Observe the suggested code changes.

3. (Optional) You can copy parts of or the whole suggested code and paste in the editor manually, or click appropriate icon above the code view to Insert at Cursor, Apply in Editor, or just copy to clipboard.
4. (Optional) Save the file.

**Additional Notes**
For more details on these, see [Getting started with prompts for Copilot Chat](https://docs.github.com/en/copilot/using-github-copilot/guides-on-using-github-copilot/getting-started-with-prompts-for-copilot-chat)

## Copilot Chat - Edit

Using inline chat is very helpful in make quick updates on the go as you're working thru a user story or resolving a bug. However, you might've encountered some challenges with context especially when changes involve large code changes.

In this section we will repeat the previous steps using Copilot Edits to showcase how we could accomplish the same results more efficiently.

1. Undo all changes made to `Order` class
2. Open chat by clicking on the Copilot button at the bottom right hand side corner, this will open up a Copilot menu, select GitHub Copilot chat
3. Select the Copilot Edits button (see below) at the top left corner of the chat window:  
   ![copilot edit](./images/copilot-edits.png)
4. Click `+Add Files` and select Order.cs to add the `Order` class to the context
5. Now add the following prompt to refactor the `GetTotal` public member to include taxes and discounts in calculation of order total

```plaintext
Refactor the GetTotal method to include taxes and discounts. get the taxes from a new public property named Taxes and the discounts is sum of discount property for each `OrderItem` in the order
```

You will notice that instead of generating code snippets like `Inline Chat`, copilot edits parses the entire Order class and modifies the logic accordingly.

## Copilot Chat - Agent

(This section needs to be updated to reflect the latest changes in Copilot Agent)

## Copilot CLI

GitHub Copilot CLI offers an efficient way to interact with Copilot directly from the command line. With Copilot CLI, you can generate shell commands and automate tasks with ease, making it a powerful tool for developers who prefer working in a terminal environment.

1. In VS Code, press Ctrl+\` to open a new terminal if necessary.
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
   ? Select an option (Use arrows to move, type to filter)
   &gt; Copy command to clipboard  
   &emsp;Explain command  
   &emsp;Execute command  
   &emsp;Revise command  
   &emsp;Rate response  
   &emsp;Exit

   You can select any of the listed options using up-down arrow keys and pressing Enter.
   Try few different options and select Exit to end.

## Summary

You have learned basics of several major features of GitHub Copilot, including:

- Autocomplete
- Inline Chat
- Copilot Chat
- Copilot Edits
- Copilot CLI

**Next Steps:**

- [Lesson 3: Testing with GitHub Copilot](3-testing-with-copilot.md)
