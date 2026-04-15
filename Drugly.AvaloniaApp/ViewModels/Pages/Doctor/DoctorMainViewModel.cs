using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

/// <summary>VM for the main page for doctors.</summary>
public partial class DoctorMainViewModel : ViewModelBase, IPageViewModel
{
    private readonly ISukiDialogManager _dialogManager;
    private readonly IPageRouter _pageRouter;
    private readonly IServiceProvider _serviceProvider;

    public string? PageTitle => "Hello, Doctor!";

    public DoctorMainViewModel(
        ISukiDialogManager dialogManager,
        IPageRouter pageRouter,
        IServiceProvider serviceProvider
    )
    {
        _dialogManager = dialogManager;
        _pageRouter = pageRouter;
        _serviceProvider = serviceProvider;
    }

    /// <summary>Requests a page navigation to the doctor's patient list page.</summary>
    [RelayCommand]
    private void ViewPatients()
    {
        var vm = _serviceProvider.GetRequiredService<DoctorPatientListViewModel>();
        _pageRouter.PushPage(vm);
    }

    /// <summary>Requests a page navigation to the doctor's medication list page.</summary>
    [RelayCommand]
    private void PrescribeMedications()
    {
        var vm = _serviceProvider.GetRequiredService<DoctorMedicationListViewModel>();
        _pageRouter.PushPage(vm);
    }
}