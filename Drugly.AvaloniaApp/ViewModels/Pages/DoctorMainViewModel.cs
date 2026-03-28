using CommunityToolkit.Mvvm.ComponentModel;

namespace Drugly.AvaloniaApp.ViewModels.Pages;

public partial class DoctorMainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial ViewModelBase ContentViewModel { get; private set; }
}