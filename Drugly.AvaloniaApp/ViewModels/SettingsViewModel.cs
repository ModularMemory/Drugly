using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Services.Interfaces;

namespace Drugly.AvaloniaApp.ViewModels;

/// <summary>VM for the settings flyout of the application.</summary>
public partial class SettingsViewModel : ViewModelBase
{
    private readonly ILoginService _loginService;
    private readonly IFontSizeService _fontSizeService;

    public IAccountSessionService AccountSessionService { get; }

    [ObservableProperty]
    public partial decimal FontSize { get; set; }

    [ObservableProperty]
    public partial decimal MinFontSize { get; set; }

    [ObservableProperty]
    public partial decimal MaxFontSize { get; set; }

    public AvaloniaList<double> FontSizeTicks { get; } = [10, 12, 14, 16, 18, 20, 24, 28, 36, 42];

    partial void OnFontSizeChanged(decimal value)
    {
        _fontSizeService.FontSize = (int)value;
    }

    public SettingsViewModel(
        ILoginService loginService,
        IAccountSessionService accountSessionService,
        IFontSizeService fontSizeService
    )
    {
        _loginService = loginService;
        AccountSessionService = accountSessionService;
        _fontSizeService = fontSizeService;
        FontSize = _fontSizeService.FontSize;
        MinFontSize = (decimal)FontSizeTicks.Min();
        MaxFontSize = (decimal)FontSizeTicks.Max();
    }

    [RelayCommand]
    private async Task Logout()
    {
        await _loginService.LogoutAsync();
    }
}