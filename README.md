 Here is a professional and clear description for your GitHub repository:
ManagerCommands

ManagerCommands is a powerful GUI tool built with Avalonia UI, designed for developers and Linux enthusiasts who want to manage and execute complex terminal commands without the headache of manual typing or syntax errors.

It is particularly effective for repetitive, high-stakes tasks like video encoding (FFmpeg), system maintenance, or project deployments where consistency is key.
Key Features

    Command Template Management: Save and organize your most-used commands in a clean, searchable library.

    Dynamic Variables (?variable?): Forget editing long command strings. Define variables within your commands, and the app automatically generates input fields for quick updates.

    Integrated Terminal: Watch your command execution in real-time within the app. It includes built-in cleaning of ANSI escape codes for a polished look.

    Automated Workflow: Execute multiple lines of commands sequentially with customizable wait times between them.

    Context-Aware Execution: Automatically performs a cd to your specified project location before running, ensuring your commands always hit the right target.

    Fansubbing & Media Friendly: Perfect for anime encoding or batch processing—simply swap the file name or folder variable and hit run.

How It Works

    Define a Template: Add a command like this:
    Bash

    ffmpeg -i "?Input_File?" -vf "subtitles='?Subtitle_File?'" -c:v libx264 -crf 18 "output.mp4"

    Fill the Variables: The app detects ?Input_File? and ?Subtitle_File? and provides text boxes for them.

    Execute: Click "Run". The app handles the directory change and executes the command exactly as configured.

<img width="1826" height="997" alt="image" src="https://github.com/user-attachments/assets/5369a22e-e6d1-473a-a6a9-7dce646f2f7a" />
<img width="1891" height="902" alt="image" src="https://github.com/user-attachments/assets/7d94711f-5658-45b1-a067-1fe171498f5e" />
<img width="1891" height="902" alt="image" src="https://github.com/user-attachments/assets/f395639a-031d-4318-9361-11f3e27bc635" />
