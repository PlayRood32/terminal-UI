using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Layout;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using ManagerCommands.Models;
using ManagerCommands.Systems;
using Color = System.Drawing.Color;

namespace ManagerCommands.Views;

public partial class CommendWindow : Window
{
    private User _user;
    private Commend _commend;
    private DataSystem _dataSystem = new DataSystem();
    private TerminalSysytem _terminal = new TerminalSysytem();

    public CommendWindow(User user, Commend commend)
    {
        InitializeComponent();

        this._user = user;
        this._commend = commend;

        SetupEventListeners();
        InitTerminal();
        UIForTerminal();

        RefreshUI();
    }

    public void UIForTerminal()
    {
        if (_user == null)
        {
            return;
        }

        Terminal.Background = MyColors.GetColorByName(_user.ColorTerminal);
        Terminal.Foreground = MyColors.GetColorByName(_user.ColorText);
        Terminal.FontFamily = _user.fontFamily;
        Terminal.FontSize = _user.sizeFont;
    }


    private void SetupEventListeners()
    {
        Commmends.ItemsSource = _user.Commends;
        Commmends.DisplayMemberBinding = new Avalonia.Data.Binding("name");

        Commmends.SelectionChanged += (s, e) =>
        {
            if (Commmends.SelectedItem is Commend selected && selected != _commend)
            {
                _commend = selected;
                RefreshUI();
            }
        };

        ContentCommend.TextChanged += (s, e) => ParseVariables();
        RunCommand.Click += OnRunCommandClick;
        SaveDate.Click += OnSaveDateClick;

        BackMain.Click += (s, e) =>
        {
            MainWindow projectWindow = new MainWindow
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                WindowState = this.WindowState,
                Width = this.Bounds.Width,
                Height = this.Bounds.Height,
                Position = this.Position
            };

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = projectWindow;
            }

            projectWindow.Show();
            this.Close();
        };

        TerminalInput.KeyDown += (s, e) =>
        {
            if (e.Key == Avalonia.Input.Key.Enter)
            {
                string input = TerminalInput.Text ?? "";
                _terminal.SendCommand(input);
                Terminal.Text += $">> Sent: {input}\n";
                TerminalInput.Text = "";
                e.Handled = true;
            }
        };
    }

    private void InitTerminal()
    {
        _terminal.StartTerminal();
        _terminal.OutputReceived += (output) => { Dispatcher.UIThread.InvokeAsync(() => UpdateTerminalText(output)); };
    }

    private void RefreshUI()
    {
        if (_commend == null) return;

        if (_terminal != null)
        {
            _terminal.StopTerminal();
        }

        UIForTerminal();
        _terminal = new TerminalSysytem();

        _terminal.OutputReceived += (output) => { Dispatcher.UIThread.InvokeAsync(() => UpdateTerminalText(output)); };

        _terminal.StartTerminal();

        Terminal.Text = "";
        TerminalInput.Text = "";
        ContentCommend.Text = _commend.content;
        Location.Text = _commend.location;
        Description.Text = _commend.description;

        if (Commmends.SelectedItem != _commend)
        {
            Commmends.SelectedItem = _commend;
        }

        ParseVariables();
    }

    
    private void UpdateTerminalText(string output)
    {
        if (string.IsNullOrEmpty(output)) return;

        string clean = Regex.Replace(output, @"\x1B(?:[@-Z\\-_]|\[[0-?]*[ -/]*[@-~])", "");

        clean = Regex.Replace(clean, @"\x1B\].*?(\x07|\x1B\\)", "");

        clean = Regex.Replace(clean, @"3008;[a-zA-Z0-9\-;=_\./]+", "");

        clean = clean.Replace("\x1B]0;", "").Replace("\x07", "");
    
        if (string.IsNullOrWhiteSpace(clean)) return;

        Terminal.Text += clean + "\n"; 

        if (Terminal.Text.Length > 0)
        {
            Terminal.CaretIndex = Terminal.Text.Length;
        }
    }

    private void ParseVariables()
    {
        if (VariablesPanel == null) return;

        string text = ContentCommend.Text ?? "";
        var matches = Regex.Matches(text, @"\?([^?]+)\?");

        var currentValues = VariablesPanel.Children
            .OfType<TextBox>()
            .Select(tb => tb.Text ?? "")
            .ToList();

        VariablesPanel.Children.Clear();

        for (int i = 0; i < matches.Count; i++)
        {
            string defaultValue = matches[i].Groups[1].Value;

            var tb = new TextBox
            {
                Watermark = $"Variable {i + 1}",
                Text = defaultValue,
                UseFloatingWatermark = true,
                Margin = new Thickness(0, 5),
                Foreground = Avalonia.Media.Brushes.White
            };


            VariablesPanel.Children.Add(tb);
        }
    }


    private async void OnRunCommandClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ContentCommend.Text)) return;

        string targetLocation = Location.Text;
        if (!string.IsNullOrEmpty(targetLocation))
        {
            _terminal.SendCommand($"cd \"{targetLocation}\"");
            await System.Threading.Tasks.Task.Delay(500); 
        }
        string commandToExecute = ContentCommend.Text;
        var textBoxes = VariablesPanel.Children.OfType<TextBox>().ToList();
        var regex = new Regex(@"\?([^?]+)\?");
        int matchIndex = 0;

        string finalCommand = regex.Replace(commandToExecute, m =>
        {
            if (matchIndex < textBoxes.Count)
            {
                string userValue = textBoxes[matchIndex].Text ?? "";
                matchIndex++;
                return userValue;
            }

            return m.Value;
        });

        Terminal.Text += $"\n--- Executing ---\n{finalCommand}\n";

        string[] lines = finalCommand.Replace("\\n", "\n")
            .Split(new[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            string trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine)) continue;
            _terminal.SendCommand(trimmedLine);
            if (trimmedLine.StartsWith("distrobox enter"))
            {
                await System.Threading.Tasks.Task.Delay(_user.TimeWait);
            }
            else
            {
                await System.Threading.Tasks.Task.Delay(_user.TimeWait);
            }
        }
    }


    private async void OnSaveDateClick(object? sender, RoutedEventArgs e)
    {
        _commend.content = ContentCommend.Text;
        _commend.location = Location.Text;
        _commend.description = Description.Text;
        await _dataSystem.SaveUser(_user);
        Terminal.Text += "[System] Saved.\n";
    }

    private async void ExplainWin_OnClick(object? sender, RoutedEventArgs e)
    {
        var titleText = new TextBlock
        {
            Text = "App Command Guide",
            FontSize = 22,
            FontWeight = FontWeight.Bold,
            Foreground = Brushes.SkyBlue,
            Margin = new Thickness(0, 0, 0, 20),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };

        var content = new StackPanel { Spacing = 15 };

        content.Children.Add(CreateInstructionBlock(
            "A. Execution Flow",
            "Every line is a separate command. They are executed sequentially."));

        content.Children.Add(CreateInstructionBlock(
            "B. Timing Issues",
            "If commands fail, try increasing the 'Wait Time' delay between them."));

        var varBlock = CreateInstructionBlock(
            "C. Dynamic Variables",
            "Variables are marked using ?variable?.");

        var exampleBox = new Border
        {
            Background = Brushes.DarkSlateGray,
            Padding = new Thickness(10),
            CornerRadius = new CornerRadius(5),
            Child = new TextBlock
            {
                Text = "Example: find ~ -name \"?Core Keeper?\" -type d",
                Foreground = Brushes.Khaki,
                FontSize = 12,
                FontFamily = new FontFamily("Consolas, Monospace")
            }
        };
        varBlock.Children.Add(exampleBox);
        content.Children.Add(varBlock);

        content.Children.Add(CreateInstructionBlock(
            "D. Terminal Interaction",
            "Interactive commands (sudo, input prompts) are not supported."));

        var closeBtn = new Button
        {
            Content = "Dismiss",
            Background = Brushes.Gray,
            Foreground = Brushes.White,
            Margin = new Thickness(0, 20, 0, 0),
            Padding = new Thickness(25, 8),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };

        var helpWin = new Window
        {
            Title = "Usage Instructions",
            Width = 550,
            Height = 500,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Background = Brushes.Black,
            Content = new ScrollViewer
            {
                Content = new StackPanel
                {
                    Margin = new Thickness(30),
                    Children = { titleText, content, closeBtn }
                }
            }
        };

        closeBtn.Click += (s, ev) => helpWin.Close();

        var parent = TopLevel.GetTopLevel(this) as Window;
        if (parent != null)
        {
            await helpWin.ShowDialog(parent);
        }
        else
        {
            helpWin.Show();
        }
    }

    private StackPanel CreateInstructionBlock(string header, string text)
    {
        return new StackPanel
        {
            Children =
            {
                new TextBlock
                    { Text = header, FontWeight = FontWeight.Bold, Foreground = Brushes.LimeGreen, FontSize = 15 },
                new TextBlock
                {
                    Text = text, Foreground = Brushes.LightGray, TextWrapping = TextWrapping.Wrap, FontSize = 13,
                    Margin = new Thickness(5, 2, 0, 0)
                }
            }
        };
    }
}
