using Avalonia.Controls;
using JetBrains.Annotations;
using SukiUI.Controls;

namespace Drugly.AvaloniaApp.Views.Pages;

/// <summary>The main view of the application.</summary>
public partial class MainView : UserControl
{
    [UsedImplicitly(Reason = "Used at design time")]
    public MainView()
    {
        InitializeComponent();
    }

    public MainView(SukiDialogHost sukiDialogHost) : this()
    {
        MainHost.Hosts = [sukiDialogHost];
    }
}