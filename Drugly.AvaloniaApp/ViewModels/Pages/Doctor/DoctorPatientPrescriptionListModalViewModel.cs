using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels.Pages.Patient;
using Drugly.DTO;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

/// <summary>VM for the doctor's view of patient prescriptions.</summary>
public partial class DoctorPatientPrescriptionListModalViewModel : ViewModelBase
{
    public AccountDetails Patient { get; }
    private readonly ISukiDialog _dialog;
    private readonly IServiceProvider _serviceProvider;
    private readonly IPrescriptionDetailsService _prescriptionDetailsService;
    private readonly IMedicationDetailsService _medicationDetailsService;
    private readonly IPageRouter _pageRouter;

    public AvaloniaList<PatientPrescription> Prescriptions { get; } = [];

    public DoctorPatientPrescriptionListModalViewModel(
        ISukiDialog dialog,
        AccountDetails patient,
        IServiceProvider serviceProvider
    )
    {
        Patient = patient;
        _dialog = dialog;
        _serviceProvider = serviceProvider;
        _prescriptionDetailsService = _serviceProvider.GetRequiredService<IPrescriptionDetailsService>();
        _medicationDetailsService = _serviceProvider.GetRequiredService<IMedicationDetailsService>();
        _pageRouter = serviceProvider.GetRequiredService<IPageRouter>();

        Dispatcher.UIThread.InvokeAsync(GetPrescriptions);
    }

    private async Task GetPrescriptions()
    {
        var prescriptions = await _prescriptionDetailsService.GetPrescriptionsByAccountId(Patient.UserId);
        List<Medication> medications = [];

        foreach (var prescription in prescriptions)
        {
            medications.Add(await _medicationDetailsService.GetMedication(prescription.MedicationId));
        }

        Prescriptions.AddRange(medications.Select((m, i) => new PatientPrescription(prescriptions[i], m)));
    }

    /// <summary>Closes the dialog.</summary>
    [RelayCommand]
    private void Cancel()
    {
        _dialog.Dismiss();
    }

    [RelayCommand]
    private void ViewPrescription(object? dataContext)
    {
        var vm = _serviceProvider.GetRequiredService<PatientPrescriptionDetailsViewModel>();
        vm.Patient = Patient;
        vm.Prescription = dataContext as PatientPrescription;
        _pageRouter.PushPage(vm);
        _dialog.Dismiss();
    }
}