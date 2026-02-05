using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Services.Interfaces;

namespace Drugly.AvaloniaApp.ViewModels;

public partial class StartupWindowViewModel : ViewModelBase
{
    private readonly ILoginService _loginService;

    public string Title { get; } = "Login";

    [ObservableProperty]
    public partial string? KeyText { get; set; }

    public StartupWindowViewModel(
        ILoginService loginService
    )
    {
        _loginService = loginService;
    }

    [RelayCommand]
    public void ButtonClick()
    {
        throw new NotImplementedException(KeyText);
    }
}