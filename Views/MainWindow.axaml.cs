using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using ManagerCommands.Models;
using ManagerCommands.Systems;

namespace ManagerCommands.Views;

public partial class MainWindow : Window
{
    private User _user;
    private DataSystem _dataSystem = new DataSystem();

    public MainWindow()
    {
        InitializeComponent();
        LoadData();
    }


    public async void LoadData()
    {
        FontFamilyInput.ItemsSource = MyColors.AllFonts;
        FontFamilyInput.SelectedIndex = 0;
        User getUser = await _dataSystem.LoadUser();
        if (getUser != null && getUser.Commends != null && getUser.Commends.Count > 0)
        {
            Console.WriteLine("Loaded user");
            PathSave.Text = "File Save in: "+_dataSystem.GetUserFilePath();
            _user = getUser;
            Timer.Value = _user.TimeWait;

            printToUI();
            printCommands();
        }
        else
        {
            Console.WriteLine("Create user");


            User newUser = new User
            {
                Commends = new List<Commend>(),
                TimeWait = 400,
                ColorTerminal = "000000",
                ColorText = "FFFFFF",
                sizeFont = 14,
                fontFamily = FontFamilyInput.SelectedItem.ToString() ?? "Arial"
            };
            await _dataSystem.SaveUser(newUser);
            _user = newUser;
            printToUI();
            printCommands();
        }
    }

    public void printToUI()
    {
        if (_user == null)
        {
            return;
        }

        HexTerminalTextInput.Text = _user.ColorTerminal;
        HexColorTextInput.Text = _user.ColorText;
        sizeFont.Value = _user.sizeFont;
        int index = MyColors.AllFonts.FindIndex(f => f == _user.fontFamily);
        FontFamilyInput.SelectedIndex = index;
        
        Test();
    }

    public void Test()
    {
        var color = MyColors.GetColorByName(_user.ColorTerminal);

        LabelHexTerminalTextInput.Background = color;
        LabelHexColorTextInput.Foreground = MyColors.GetColorByName(_user.ColorText);
        LabelFontSize.FontSize = _user.sizeFont;
        LabelFontFamily.FontFamily = _user.fontFamily;
    }


    public void printCommands(List<Commend> SearchCommends = null)
    {
        if (_user != null && _user.Commends != null && _user.Commends.Count > 0)
        {
            List<Commend> printCommends = new List<Commend>(_user.Commends);
            if (SearchCommends != null)
            {
                printCommends = new List<Commend>(SearchCommends);
            }

            ListCommendS.Items.Clear();

            foreach (Commend commend in printCommends)
            {
                StackPanel stackPanel = new StackPanel
                {
                    Cursor = new Cursor(StandardCursorType.Hand),
                    Background = Brushes.Transparent,
                    Orientation = Orientation.Vertical
                };


                stackPanel.ContextMenu = MenuStyles.CreateContextMenuBook((selectedProject, state) =>
                {
                    if (state == "delete") deletCommend(commend);
                    else if (state == "edit")
                    {
                        EditCommend(commend);
                    }
                }, commend);

                Label CommendName = new Label
                {
                    Content = commend.name
                };
                if (commend.isChange)
                {
                    CommendName.Foreground = Brushes.CornflowerBlue;
                }

                stackPanel.PointerPressed += (sender, e) =>
                {
                    var point = e.GetCurrentPoint(CommendName);
                    if (point.Properties.IsLeftButtonPressed)
                    {
                        CommendWindow commendWindow = new CommendWindow(_user, commend);
                        commendWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                        commendWindow.Width = this.Bounds.Width;
                        commendWindow.Height = this.Bounds.Height;
                        commendWindow.Position = this.Position;
                        commendWindow.WindowState = this.WindowState;

                        commendWindow.Show();
                        this.Close();
                    }
                };

                stackPanel.Children.Add(CommendName);
                ListCommendS.Items.Add(stackPanel);
            }
        }
    }


    public async void EditCommend(Commend commend)
    {
        string name = await MenuStyles.ShowConfirmDialogEditName(commend.name,
            "Are you sure you want to change this commend", "name commend ", "yes", "cancel", this);
        if (name != null)
        {
            int index = _user.Commends.IndexOf(commend);
            _user.Commends[index].name = name;
            await _dataSystem.SaveUser(_user);


            printCommands();
        }
    }

    public async void deletCommend(Commend commend)
    {
        bool confirmed = await MenuStyles.ShowConfirmDialog("Are you sure you want to delete this Commend?",
            "Commend", "yes", "cancel", this);
        if (!confirmed) return;

        _user.Commends.Remove(commend);
        await _dataSystem.SaveUser(_user);


        printCommands();
    }


    private async void CreateCommendButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_user == null)
        {
            Console.WriteLine("_user null");
            return;
        }

        if (_user.Commends == null)
        {
            _user.Commends = new List<Commend>();
        }


        string str = NameCommend.Text;
        if (str != null && str != "")
        {
            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Commend commend = new Commend()
            {
                name = str,
                isChange = ChangeCommend.IsChecked ?? false,
                location = homePath,
            };

            ChangeCommend.IsChecked = false;
            _user.Commends.Add(commend);
            await _dataSystem.SaveUser(_user);
            printCommands();
        }
    }

    private async void Timer_OnValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        decimal? newValue = e.NewValue;
        if (newValue.HasValue)
        {
            _user.TimeWait = (int)newValue.Value;
            await _dataSystem.SaveUser(_user);
        }
    }

    private async void ImportCommendsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        string downloadsPath = System.IO.Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile),
            "Downloads");

        var downloadsFolder = await topLevel.StorageProvider.TryGetFolderFromPathAsync(downloadsPath);

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select JSON file to import commands",
            AllowMultiple = false,
            SuggestedStartLocation = downloadsFolder,
            FileTypeFilter = new[] { new FilePickerFileType("JSON Files") { Patterns = new[] { "*.json" } } }
        });

        if (files.Count == 0) return;

        string jsonFilePath = files[0].Path.LocalPath;

        try
        {
            string jsonContent = await File.ReadAllTextAsync(jsonFilePath);
            var importedData = JsonSerializer.Deserialize<User>(jsonContent);

            if (importedData?.Commends == null || importedData.Commends.Count == 0)
            {
                return;
            }

            int addedCount = 0;

            foreach (var importedCmd in importedData.Commends)
            {
                bool exists = _user.Commends.Any(c =>
                    c.name == importedCmd.name &&
                    c.content == importedCmd.content &&
                    c.description == importedCmd.description);

                if (!exists)
                {
                    _user.Commends.Add(importedCmd);
                    addedCount++;
                }
            }

            if (addedCount > 0)
            {
                await _dataSystem.SaveUser(_user);
                Console.WriteLine($"Imported {addedCount} new commands.");
                printCommands();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to import: {ex.Message}");
        }
    }

    private void NameSearch_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        string strSearch = NameSearch.Text;

        if (!string.IsNullOrEmpty(strSearch))
        {
            printCommands(_user.Commends.Where(f => f.name.ToUpper().StartsWith(strSearch.ToUpper()))
                .ToList());
        }
        else
        {
            printCommands();
        }
    }

    private async void HexTerminalTextInput_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_user == null)
        {
            return;
        }

        string color = HexTerminalTextInput.Text;
        if (!string.IsNullOrEmpty(color))
        {
            var getColor = MyColors.GetColorByName(color);
            if (getColor.ToString() != MyColors.GetColorByName(MyColors.colorsForBackgroun[0]).ToString())
            {
                _user.ColorTerminal = color;
                await _dataSystem.SaveUser(_user);
                Test();
            }
        }
    }


    private async void HexColorTextInput_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_user == null)
        {
            return;
        }

        string color = HexColorTextInput.Text;
        if (!string.IsNullOrEmpty(color))
        {
            var getColor = MyColors.GetColorByName(color);
            if (getColor.ToString() != MyColors.GetColorByName(MyColors.colorsForBackgroun[0]).ToString())
            {
                _user.ColorText = color;
                await _dataSystem.SaveUser(_user);
                Test();
            }
        }
    }


    private async void SizeFont_OnValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        if (_user == null || e.NewValue == null)
        {
            return;
        }

        decimal? newValue = e.NewValue;
        if (newValue.HasValue)
        {
            _user.sizeFont = (int)newValue.Value;
            await _dataSystem.SaveUser(_user);
            Test();
        }
    }

    private async void FontFamilyInput_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_user == null || FontFamilyInput.SelectionBoxItem == null)
        {
            return;
        }

        if (FontFamilyInput.SelectedItem is string selectedFont)
        {
            _user.fontFamily = selectedFont;
            await _dataSystem.SaveUser(_user);
            Test();
        }
    }

    private async void ResetSettingsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _user.ColorTerminal = "000000";
        _user.ColorText = "FFFFFF";
        _user.sizeFont = 14;
        FontFamilyInput.ItemsSource = MyColors.AllFonts;
        FontFamilyInput.SelectedIndex = 0;

     
        _user.fontFamily = FontFamilyInput.SelectedItem.ToString() ?? "Arial";
        await _dataSystem.SaveUser(_user);
        Test();
        printToUI();
    }
}
