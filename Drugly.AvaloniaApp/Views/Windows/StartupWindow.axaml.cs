using Avalonia.Controls;
using Avalonia.Input;
using JetBrains.Annotations;
using SukiUI.Controls;

namespace Drugly.AvaloniaApp.Views.Windows;

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

    private void SukiMainHost_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
}