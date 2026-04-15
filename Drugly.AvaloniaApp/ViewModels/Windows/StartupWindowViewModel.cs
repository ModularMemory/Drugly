using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Extensions;
using Drugly.AvaloniaApp.Services;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.Validation;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Windows;

/// <summary>VM for the window shown at startup. Includes logic for initiating logins.</summary>
public partial class StartupWindowViewModel : ViewModelBase
{
    private readonly ILoginService _loginService;
    private readonly IFontSizeService _fontSizeService;
    private readonly ISukiDialogManager _dialogManager;

    /// <summary>The title of the window.</summary>
    public string Title { get; } = $"{nameof(Drugly)} Login";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [EmailAddress]
    public partial string? EmailText { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [Password]
    public partial string? PasswordText { get; set; }

    [ObservableProperty]
    public partial bool LoggingIn { get; private set; }

    [ObservableProperty]
    public partial ViewModelBase SettingsViewModel { get; set; }

    public StartupWindowViewModel(
        ILoginService loginService,
        IFontSizeService fontSizeService,
        SettingsViewModel settingsViewModel,
        ISukiDialogManager dialogManager
    )
    {
        SettingsViewModel = settingsViewModel;
        _loginService = loginService;
        _fontSizeService = fontSizeService;
        _dialogManager = dialogManager;
        _loginService.LoginError += LoginService_OnLoginError;

        ValidateAllProperties();
    }

    /// <summary>Initiates a login via the <see cref="_loginService"/>.</summary>
    [RelayCommand]
    private async Task Login()
    {
        LoggingIn = true;

        try
        {
            if (HasErrors)
            {
                await DelayService.FakeDelay();
                var errors = GetErrors()
                    .Where(x => !string.IsNullOrWhiteSpace(x.ErrorMessage))
                    .Select(x => x.ErrorMessage!.TrimEnd('.'));

                ShowLoginError($"Bad or invalid login information:{Environment.NewLine}{string.Join(", ", errors)}.");
                return;
            }

            Debug.Assert(EmailText != null);
            Debug.Assert(PasswordText != null);
            await _loginService.TryLoginAsync(EmailText, PasswordText);
        }
        finally
        {
            LoggingIn = false;
        }
    }

    /// <summary>Invoked when the login service reports a failed login.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The login error message.</param>
    private void LoginService_OnLoginError(object? sender, string e) => ShowLoginError(e);

    private void ShowLoginError(string e)
    {
        _dialogManager.CreateDialog()
            .OfType(NotificationType.Error)
            .WithTitle("Login Error")
            .WithGroupedContent(new TextBlock
            {
                Text = e
            })
            .WithOkResult("Ok")
            .Dismiss().ByClickingBackground()
            .TryShow();
    }
}