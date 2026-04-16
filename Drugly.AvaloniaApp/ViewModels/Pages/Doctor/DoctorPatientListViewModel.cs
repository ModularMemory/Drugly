using Avalonia.Collections;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Extensions;
using Drugly.AvaloniaApp.Services;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels.Pages.Patient;
using Drugly.DTO;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

/// <summary>VM for the patient list page for doctors.</summary>
public partial class DoctorPatientListViewModel : ViewModelBase, IPageViewModel
{
    private readonly IPageRouter _pageRouter;
    private readonly IAccountDetailsService _accountDetailsService;
    private readonly ISukiDialogManager _dialogManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public string? PageTitle => "Choose a Patient to View";

    public AvaloniaList<AccountDetails> Patients { get; } = [];

    public DoctorPatientListViewModel(
        IPageRouter pageRouter,
        IAccountDetailsService accountDetailsService,
        ISukiDialogManager dialogManager,
        IServiceProvider serviceProvider,
        ILogger logger
    )
    {
        _pageRouter = pageRouter;
        _accountDetailsService = accountDetailsService;
        _dialogManager = dialogManager;
        _serviceProvider = serviceProvider;
        _logger = logger;

        Dispatcher.UIThread.InvokeAsync(LoadPatients);
    }

    private async Task LoadPatients()
    {
        try
        {
            var patients = await _accountDetailsService.GetPatients();
            Patients.AddRange(patients);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error while loading patients");

            await _dialogManager.CreateDialog()
                .OfType(NotificationType.Error)
                .WithTitle("Error while loading patients")
                .WithContent(ex.Message)
                .WithOkResult("Ok")
                .Dismiss().ByClickingBackground()
                .TryShowAsync();
        }
    }

    /// <summary>Requests a page navigation to the patient details page.</summary>
    [RelayCommand]
    private async Task ViewPatient(object? dataContext)
    {
        var patient = dataContext as AccountDetails;
        if (patient is null)
        {
            _logger.Warning("Tried to view prescriptions for a null patient");

            await _dialogManager.CreateDialog()
                .OfType(NotificationType.Error)
                .WithTitle("Internal Error")
                .WithContent("Tried to viewing prescriptions for an unknown patient")
                .WithOkResult("Ok")
                .Dismiss().ByClickingBackground()
                .TryShowAsync();

            return;
        }

        _logger.Debug("Opening prescriptions list modal for {PatientFirstName} {PatientLastName}", patient.FirstName, patient.LastName);

        await _dialogManager.CreateDialog()
            .WithViewModel(dialog => new DoctorPatientPrescriptionListModalViewModel(dialog, patient, _serviceProvider))
            .WithoutResult()
            .Dismiss().ByClickingBackground()
            .TryShowAsync();
    }
}