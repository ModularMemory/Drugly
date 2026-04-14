using Avalonia.Controls;
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
}