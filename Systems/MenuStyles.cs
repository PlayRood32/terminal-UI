using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ManagerCommands.Models;

namespace ManagerCommands.Systems;

public class MenuStyles
{
    public static ContextMenu CreateContextMenuBook(Action<Commend, string> cmmend, Commend CommendItem)
    {
        var myMenu = new ContextMenu
        {
            Background = Brush.Parse("#18191c"),
            Padding = new Thickness(4),
            CornerRadius = new CornerRadius(4)
        };

        MenuItem CreateStyledItem(string header, IBrush foreground = null)
        {
            return new MenuItem
            {
                Header = header,
                FontSize = 20,
                FontFamily = "gg sans",
                FontWeight = FontWeight.Medium,
                Foreground = foreground ?? Brush.Parse("#b9bbbe"),
                Padding = new Thickness(10, 8),
                MinWidth = 180
            };
        }


        var delectItem = CreateStyledItem("delete Commend");
        var editItem = CreateStyledItem("Edit Commend name");
        delectItem.Click += (sender, args) => { cmmend?.Invoke(CommendItem, "delete"); };
        editItem.Click += (sender, args) => { cmmend?.Invoke(CommendItem, "edit"); };

        myMenu.ItemsSource = new List<object> { editItem,delectItem };

        return myMenu;
    }

    public static async Task<bool> ShowConfirmDialog(string message, string title, string texButton1, string texButton2,
        Visual visual)
    {
        var tcs = new TaskCompletionSource<bool>();

        var btn1 = new Button
            { Content = texButton1, Background = Brushes.Red, Foreground = Brushes.White, Margin = new Thickness(5) };
        var btn2 = new Button
            { Content = texButton2, Background = Brushes.Gray, Foreground = Brushes.White, Margin = new Thickness(5) };

        var confirmWin = new Window
        {
            Title = title,
            Width = 350,
            Height = 150,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Background = Brushes.Black,
            Content = new StackPanel
            {
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Children =
                {
                    new TextBlock
                    {
                        Text = message, Foreground = Brushes.White, Margin = new Thickness(10),
                        TextWrapping = TextWrapping.Wrap
                    },
                    new StackPanel
                    {
                        Orientation = Avalonia.Layout.Orientation.Horizontal,
                        Children = { btn1, btn2 }
                    }
                }
            }
        };

        btn1.Click += (s, e) =>
        {
            tcs.TrySetResult(true);
            confirmWin.Close();
        };
        btn2.Click += (s, e) =>
        {
            tcs.TrySetResult(false);
            confirmWin.Close();
        };

        confirmWin.Closed += (s, e) => tcs.TrySetResult(false);

        var parent = TopLevel.GetTopLevel(visual) as Window;
        if (parent != null)
        {
            await confirmWin.ShowDialog(parent);
        }
        else
        {
            confirmWin.Show();
        }

        return await tcs.Task;
    }

    public static async Task<string> ShowConfirmDialogEditName(string currentName, string message, string title,
        string texButton1, string texButton2, Visual visual)
    {
        var tcs = new TaskCompletionSource<string>();

        var nameBox = new TextBox
        {
            Text = currentName,
            Foreground = Brushes.White,
            Background = Brushes.DarkSlateGray,
            Margin = new Thickness(20, 10),
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
        };

        var btn1 = new Button
            { Content = texButton1, Background = Brushes.Red, Foreground = Brushes.White, Margin = new Thickness(5) };
        var btn2 = new Button
            { Content = texButton2, Background = Brushes.Gray, Foreground = Brushes.White, Margin = new Thickness(5) };

        var confirmWin = new Window
        {
            Title = title,
            Width = 400,
            Height = 220,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Background = Brushes.Black,
            Content = new StackPanel
            {
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Children =
                {
                    new TextBlock
                    {
                        Text = message,
                        Foreground = Brushes.White,
                        Margin = new Thickness(10),
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                    },
                    nameBox,
                    new StackPanel
                    {
                        Orientation = Avalonia.Layout.Orientation.Horizontal,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        Children = { btn1, btn2 }
                    }
                }
            }
        };

        btn1.Click += (s, e) =>
        {
            tcs.TrySetResult(nameBox.Text);
            confirmWin.Close();
        };
        btn2.Click += (s, e) =>
        {
            tcs.TrySetResult(null);
            confirmWin.Close();
        };

        confirmWin.Closed += (s, e) => tcs.TrySetResult(null);

        var parent = TopLevel.GetTopLevel(visual) as Window;
        if (parent != null)
        {
            await confirmWin.ShowDialog(parent);
        }
        else
        {
            confirmWin.Show();
        }

        return await tcs.Task;
    }
}
