using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

/// <summary>VM for the medication list page for doctors.</summary>
public partial class DoctorMedicationListViewModel : ViewModelBase
{
    private readonly IPageRouter _pageRouter;
    private readonly IServiceProvider _serviceProvider;

    public AvaloniaList<MedicationModel> Medications { get; } = [];

    public DoctorMedicationListViewModel(
        IPageRouter pageRouter,
        IServiceProvider serviceProvider
    )
    {
        _pageRouter = pageRouter;
        _serviceProvider = serviceProvider;

        Medications.AddRange(Drugly.AvaloniaApp.Design.DesignData.ExampleMedications);
    }

    /// <summary>Requests a page navigation back to the previous page.</summary>
    [RelayCommand]
    private void NavigateBack()
    {
        _pageRouter.PopPage();
    }

    /// <summary>Requests a page navigation to the medication details page.</summary>
    [RelayCommand]
    private void ViewMedication(object? dataContext)
    {
        var vm = _serviceProvider.GetRequiredService<DoctorMedicationDetailsPageViewModel>();
        vm.Medication = dataContext as MedicationModel;
        _pageRouter.PushPage(vm);
    }
}