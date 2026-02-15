using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Extensions;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.Validation;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels;

public partial class StartupWindowViewModel : ViewModelBase
{
    private readonly ILoginService _loginService;
    private readonly ISukiDialogManager _dialogManager;

    public string Title { get; } = $"{nameof(Drugly)} Login";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [LoginKey]
    public partial string? KeyText { get; set; }

    [ObservableProperty]
    public partial bool LoggingIn { get; private set; }

    public StartupWindowViewModel(
        ILoginService loginService,
        ISukiDialogManager dialogManager
    )
    {
        _loginService = loginService;
        _dialogManager = dialogManager;
        _loginService.LoginError += LoginService_OnLoginError;
    }

    [RelayCommand]
    private async Task ButtonClick()
    {
        LoggingIn = true;

        try
        {
            await _loginService.TryLoginAsync(KeyText);
        }
        finally
        {
            LoggingIn = false;
        }
    }

    private void LoginService_OnLoginError(object? sender, string e)
    {
        _dialogManager.CreateDialog()
            .OfType(NotificationType.Error)
            .WithTitle("Login Error")
            .WithGroupedContent(new TextBlock
            {
                Text = e
            })
            .WithOkResult("Ok")
            .TryShow();
    }
}