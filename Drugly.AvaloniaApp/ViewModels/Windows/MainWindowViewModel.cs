using Drugly.AvaloniaApp.ViewModels.Pages;

namespace Drugly.AvaloniaApp.ViewModels.Windows;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Title { get; } = $"{nameof(Drugly)}";

    public ViewModelBase ContentViewModel { get; }

    public MainWindowViewModel(
        MainViewModel mainViewModel
    )
    {
        ContentViewModel = mainViewModel;
    }
}