using Avalonia.Controls;

namespace Drugly.AvaloniaApp.Views.Windows;

/// <summary>The primary window of the application.</summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
#pragma warning disable CA1826
        var primaryScreen = Screens.Primary ?? Screens.All.FirstOrDefault();
#pragma warning restore CA1826

        if (primaryScreen != null)
        {
            Width = Math.Min(Width, primaryScreen.WorkingArea.Width);
            Height = Math.Min(Height, primaryScreen.WorkingArea.Height);
        }

        InitializeComponent();
    }
}