using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Design;
using Drugly.AvaloniaApp.Extensions;
using Drugly.AvaloniaApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

public partial class DoctorMainViewModel : ViewModelBase
{
    private readonly ISukiDialogManager _dialogManager;
    private readonly IPageRouter _pageRouter;
    private readonly IServiceProvider _serviceProvider;

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

    [RelayCommand]
    private void ViewPatients()
    {
        // TODO: DoctorPatientListViewModel
        var vm = _serviceProvider.GetRequiredService<DoctorMedicationListViewModel>();
        _pageRouter.PushPage(vm);
    }

    [RelayCommand]
    private void PrescribeMedications()
    {
        var vm = _serviceProvider.GetRequiredService<DoctorMedicationListViewModel>();
        _pageRouter.PushPage(vm);
    }

    [RelayCommand]
    private async Task DialogClick()
    {
        await _dialogManager.CreateDialog()
            .WithViewModel(dialog => new DoctorPrescribeModalViewModel(dialog, DesignData.ExamplePrescription))
            .WithoutResult()
            .Dismiss().ByClickingBackground()
            .TryShowAsync();
    }
}