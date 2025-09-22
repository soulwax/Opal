# Setting up MonoGame for VS Code

This guide will help you set up MonoGame for development using Visual Studio Code (VS Code). MonoGame is a popular framework for building cross-platform games, and VS Code is a lightweight, versatile code editor.

## Prerequisites

Before you begin, ensure you have the following installed on your system:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- [MonoGame SDK](https://www.monogame.net/downloads/)
- [Visual Studio Code](https://code.visualstudio.com/)

## Step 1: Install MonoGame

1. Open the terminal in VS Code (View > Terminal, alternatively ``` Ctrl + ` ```).

2. Paste

   ```bash
   dotnet new install MonoGame.Templates.CSharp
   ```

## VS Code Extensions

Install the following VS Code extensions:

- C# Dev Kit (mandatory)
- MonoGame for VSCode (optional)
- .NET MAUI (optional, for additional .NET support)
  
## Step 2: Create a New MonoGame Project

1. In the terminal, navigate to the folder where you want to create your project.
2. Run the following command to create a new MonoGame project:

   ```bash
   dotnet new mgdesktopgl
   ```

3. Open the newly created project folder in VS Code:

   ```bash
    code .
    ```

4. Restore the project dependencies by running:

    ```bash
    dotnet restore
    ```

5. If prompted, add the required assets to your project.
6. Build the project to ensure everything is set up correctly:

    ```bash
    dotnet build
    ```

7. Run the project to see the default MonoGame window:

    ```bash
    dotnet run
    ```

## Step 3: Configure Debugging

1. Open the Debug view in VS Code (View > Run or press `Ctrl + Shift + D`).
2. Click on "create a launch.json file" and select ".NET Core".
3. Modify the generated `launch.json` file to include the following configuration:

   ```json
   {
       "version": "0.2.0",
       "configurations": [
           {
               "name": ".NET Core Launch (console)",
               "type": "coreclr",
               "request": "launch",
               "preLaunchTask": "build",
               "program": "${workspaceFolder}/bin/Debug/net6.0/YourProjectName.dll",
               "args": [],
               "cwd": "${workspaceFolder}",
               "stopAtEntry": false,
               "console": "internalConsole"
           }
       ]
   }
   ```

   Replace `YourProjectName` with the actual name of your project.
4. Save the `launch.json` file.
5. You can now start debugging your MonoGame project by pressing `F5` or clicking the green play button in the Debug view.
6. Set breakpoints in your code to inspect variables and control the execution flow.
7. Use the Debug Console to evaluate expressions and execute commands during debugging sessions.
8. Monitor the Call Stack and Variables panels to get insights into the current state of your application.
9. Utilize the Watch panel to keep an eye on specific variables or expressions as you step through your code.
10. Leverage the integrated terminal to run additional commands or scripts without leaving VS Code.
11. Take advantage of the rich extension ecosystem in VS Code to enhance your development experience with additional tools and features.
12. Regularly update your extensions and VS Code to benefit from the latest features and improvements.
13. Explore the VS Code documentation and community resources for tips and best practices on using the editor effectively.
