using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels.Pages.Doctor;
using Drugly.AvaloniaApp.ViewModels.Pages.Patient;
using Drugly.DTO;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Drugly.AvaloniaApp.ViewModels.Pages;

/// <summary>VM for the main view of the application.</summary>
public partial class MainViewModel : ViewModelBase
{
    private readonly ILogger _logger;

    public IPageRouter PageRouter { get; }

    [ObservableProperty]
    public partial ViewModelBase? ContentViewModel { get; set; }

    [ObservableProperty]
    public partial ViewModelBase SettingsViewModel { get; set; }

    public MainViewModel(
        IServiceProvider serviceProvider,
        IPageRouter pageRouter,
        IAccountSessionService accountSessionService,
        SettingsViewModel settingsViewModel,
        ILogger logger
    )
    {
        PageRouter = pageRouter;
        SettingsViewModel = settingsViewModel;
        _logger = logger;

        PageRouter.ResetPageHistory();
        PageRouter.PageNavigate += PageRouter_OnPageNavigate;

        var accountType = accountSessionService.AccountType;
        ViewModelBase? vm = accountType switch
        {
            AccountType.Patient => serviceProvider.GetRequiredService<PatientMainViewModel>(),
            AccountType.Doctor => serviceProvider.GetRequiredService<DoctorMainViewModel>(),
            _ => null
        };

        if (vm is null)
        {
            _logger.Error("Failed to find valid content ViewModel. Account type was {AccountType}", accountType);
        }

        PageRouter.RootPage = vm;
    }

    /// <summary>Invoked when a page navigation is requested.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The requested page.</param>
    private void PageRouter_OnPageNavigate(object? sender, ViewModelBase? e)
    {
        if (e is null)
        {
            var stack = new StackTrace();
            _logger.Warning("A page navigation to null was requested. Stack trace {Stack}", stack);
        }

        ContentViewModel = e;
    }

    /// <summary>Requests a page navigation back to the previous page.</summary>
    [RelayCommand]
    private void NavigateBack()
    {
        PageRouter.PopPage();
    }

    ~MainViewModel()
    {
        PageRouter.PageNavigate -= PageRouter_OnPageNavigate;
    }
}