using CommunityToolkit.Mvvm.ComponentModel;

namespace Drugly.AvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Title { get; } = "Drugly";

    [ObservableProperty]
    public partial MainViewModel ContentViewModel { get; set; }

    public MainWindowViewModel(MainViewModel mainViewModel)
    {
        ContentViewModel = mainViewModel;
    }
}