using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Serilog;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages;

public partial class PatientDetailsPageViewModel : ViewModelBase
{
    private readonly ISukiDialogManager _dialogManager;
    private readonly IPageRouter _pageRouter;
    private readonly ILogger _logger;

    [ObservableProperty]
    public partial PatientModel? Patient { get; set; }

    public PatientDetailsPageViewModel(
        ISukiDialogManager dialogManager,
        IPageRouter pageRouter,
        ILogger logger
    )
    {
        _dialogManager = dialogManager;
        _pageRouter = pageRouter;
        _logger = logger;
    }

    [RelayCommand]
    private void NavigateBack()
    {
        _pageRouter.PopPage();
    }
}