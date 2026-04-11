using Avalonia.Controls;
using Avalonia.Input;
using JetBrains.Annotations;
using SukiUI.Controls;

namespace Drugly.AvaloniaApp.Views.Windows;

/// <summary>The window shown at startup.</summary>
public partial class StartupWindow : Window
{
    [UsedImplicitly(Reason = "Used at design time")]
    public StartupWindow()
    {
        InitializeComponent();
    }

    public StartupWindow(SukiDialogHost sukiDialogHost) : this()
    {
        MainHost.Hosts = [sukiDialogHost];
    }

    /// <summary>Allows moving the window by clicking and dragging anywhere.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The pointer even args.</param>
    private void SukiMainHost_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
}