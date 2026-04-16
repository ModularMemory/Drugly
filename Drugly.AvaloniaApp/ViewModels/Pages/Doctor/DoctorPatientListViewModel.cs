using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Design;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

/// <summary>VM for the patient list page for doctors.</summary>
public partial class DoctorPatientListViewModel : ViewModelBase, IPageViewModel
{
    private readonly IPageRouter _pageRouter;
    private readonly IServiceProvider _serviceProvider;

    public string? PageTitle => "Choose a Patient to View";

    public AvaloniaList<AccountDetails> Patients { get; } = [];

    public DoctorPatientListViewModel(
        IPageRouter pageRouter,
        IServiceProvider serviceProvider
    )
    {
        _pageRouter = pageRouter;
        _serviceProvider = serviceProvider;

        Patients.AddRange(DesignData.ExamplePatients);
    }

    /// <summary>Requests a page navigation to the patient details page.</summary>
    [RelayCommand]
    private void ViewPatient(object? dataContext)
    {
        var vm = _serviceProvider.GetRequiredService<Patient.PatientPrescriptionDetailsViewModel>();
        vm.Patient = dataContext as AccountDetails;
        _pageRouter.PushPage(vm);
    }
}