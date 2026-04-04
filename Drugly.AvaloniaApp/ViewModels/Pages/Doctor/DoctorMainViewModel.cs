using CommunityToolkit.Mvvm.ComponentModel;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

public partial class DoctorMainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial ViewModelBase ContentViewModel { get; private set; }

    public DoctorMainViewModel()
    {

    }
}