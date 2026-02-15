using Avalonia.Controls;
using JetBrains.Annotations;
using SukiUI.Controls;

namespace Drugly.AvaloniaApp.Views;

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