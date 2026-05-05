<div align="center">

# ManagerCommands

**A desktop command manager for saving, editing, and running reusable terminal workflows.**

Built with **C#**, **.NET 10**, and **Avalonia UI**.

</div>

## Preview

<table>
  <tr>
    <td width="50%">
      <img width="100%" alt="ManagerCommands main screen" src="https://github.com/user-attachments/assets/5369a22e-e6d1-473a-a6a9-7dce646f2f7a" />
    </td>
    <td width="50%">
      <img width="100%" alt="ManagerCommands command editor" src="https://github.com/user-attachments/assets/7d94711f-5658-45b1-a067-1fe171498f5e" />
    </td>
  </tr>
  <tr>
    <td width="50%">
      <img width="100%" alt="ManagerCommands terminal view" src="https://github.com/user-attachments/assets/f395639a-031d-4318-9361-11f3e27bc635" />
    </td>
    <td width="50%">
      <img width="100%" alt="ManagerCommands settings view" src="https://github.com/user-attachments/assets/b7df773d-a8bf-4fba-b396-297e666954e7" />
    </td>
  </tr>
</table>

## About

ManagerCommands is a GUI tool for people who run the same terminal commands again and again.

Instead of keeping long commands in notes, scripts, or shell history, you can save them as command templates, fill in dynamic values, choose the working directory, and run them from one place.

## Features

- **Command library** - save, search, edit, import, and delete commands.
- **Dynamic variables** - use `?variable?` placeholders and fill them before running.
- **Embedded terminal** - run commands and view output inside the app.
- **Working directory per command** - choose where each command should run.
- **Multi-line execution** - run a full command flow line by line.
- **Configurable delay** - set the wait time between commands.
- **Terminal customization** - change font, text color, and background color.
- **Local JSON storage** - keep all user data on the local machine.

## Workflow

Commands are saved as reusable templates. A template can contain one command or several commands, with each line executed in order.

Use `?variable?` placeholders for values that change between runs. ManagerCommands detects those placeholders and creates input fields for them before execution.

Example template:

```bash
git status
dotnet build "?ProjectFile?"
```

Here, `?ProjectFile?` becomes a field in the UI. After you enter a value, the final command is executed in the configured working directory and the output appears in the embedded terminal.

## Install And Run

### Requirements

- **.NET 10 SDK** installed on your machine.
- A desktop OS that can run Avalonia apps, such as **Windows** or **Linux**.
- `git` for cloning the repository.

Check that .NET is installed:

```bash
dotnet --version
```

The version should start with `10`. If the command is not found, install the .NET 10 SDK first.

### Clone

```bash
git clone https://github.com/GOIOOD1234/terminal-UI.git
cd terminal-UI
```

### Run From Source

```bash
dotnet restore
dotnet run --project ManagerCommands.csproj
```

On Linux, the app opens as a desktop window and uses `/bin/bash` for the embedded terminal. On Windows, it uses `cmd.exe`.

### Build A Standalone App

Linux:

```bash
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

Windows:

```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

Published files are generated under `bin/Release/`. The `bin/` and `obj/` folders are build output and should not be committed to the repository.

## Data Storage

User data is stored locally at:

```text
<LocalApplicationData>/MyCommend/Users/current_user.json
```

This file contains saved commands, terminal appearance settings, font settings, and the delay between commands.

## Project Structure

```text
├── 📁 Assets
│   ├── 🖼️ avalonia-logo.ico
│   ├── 🖼️ ICON.ico
│   └── 🖼️ ICON.png
├── 📁 Models
│   ├── 🟪 Commend.cs
│   └── 🟪 User.cs
├── 📁 Styles
│   └── 📄 AppDefaultStyles.axaml
├── 📁 Systems
│   ├── 🟪 DataSystem.cs
│   ├── 🟪 MenuStyles.cs
│   ├── 🟪 MyColors.cs
│   └── 🟪 TerminalSysytem.cs
├── 📁 ViewModels
│   ├── 🟪 MainWindowViewModel.cs
│   └── 🟪 ViewModelBase.cs
├── 📁 Views
│   ├── 📄 CommendWindow.axaml
│   ├── 🟪 CommendWindow.cs
│   ├── 📄 MainWindow.axaml
│   └── 🟪 MainWindow.axaml.cs
├── 📄 App.axaml
├── 🟪 App.axaml.cs
├── 📄 app.manifest
├── 🟪 ManagerCommands.csproj
├── 🟪 Program.cs
├── 📖 README.md
├── 🟦 terminal-UI.sln
└── 🟪 ViewLocator.cs
```

## Tech Stack

- C#
- .NET 10
- Avalonia UI 12
- CommunityToolkit.Mvvm
