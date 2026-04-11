using Drugly.AvaloniaApp.ViewModels.Pages;

namespace Drugly.AvaloniaApp.ViewModels.Windows;

/// <summary>VM for the primary window of the application.</summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>The title of the window.</summary>
    public string Title { get; } = $"{nameof(Drugly)}";

    /// <summary>The VM for the content of the window.</summary>
    public ViewModelBase ContentViewModel { get; }

    public MainWindowViewModel(
        MainViewModel mainViewModel
    )
    {
        ContentViewModel = mainViewModel;
    }
}