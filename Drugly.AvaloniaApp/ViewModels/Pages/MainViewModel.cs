using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels.Pages.Doctor;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Drugly.AvaloniaApp.ViewModels.Pages;

/// <summary>VM for the main view of the application.</summary>
public partial class MainViewModel : ViewModelBase
{
    private readonly IPageRouter _pageRouter;
    private readonly ILogger _logger;

    [ObservableProperty]
    public partial ViewModelBase? ContentViewModel { get; set; }

    public MainViewModel(
        IServiceProvider serviceProvider,
        IPageRouter pageRouter,
        IAccountSessionService accountSessionService,
        ILogger logger
    )
    {
        _pageRouter = pageRouter;
        _logger = logger;

        _pageRouter.ResetPageHistory();
        _pageRouter.PageNavigate += PageRouter_OnPageNavigate;

        // var accountType = accountSessionService.AccountType;
        var accountType = AccountType.Doctor;
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

        _pageRouter.RootPage = vm;
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

    ~MainViewModel()
    {
        _pageRouter.PageNavigate -= PageRouter_OnPageNavigate;
    }
}