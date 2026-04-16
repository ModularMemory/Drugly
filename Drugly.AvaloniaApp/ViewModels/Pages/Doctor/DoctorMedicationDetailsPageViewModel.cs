using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Extensions;
using Drugly.AvaloniaApp.Services;
using Drugly.DTO;
using Serilog;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

/// <summary>VM for the medication details page for doctors.</summary>
public partial class DoctorMedicationDetailsPageViewModel : ViewModelBase, IPageViewModel
{
    private readonly ISukiDialogManager _dialogManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public string? PageTitle => $"Viewing {Medication?.Name}";

    [ObservableProperty]
    public partial Medication? Medication { get; set; }

    public DoctorMedicationDetailsPageViewModel(
        ISukiDialogManager dialogManager,
        IServiceProvider serviceProvider,
        ILogger logger
    )
    {
        _dialogManager = dialogManager;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>Opens the modal for prescribing the <see cref="Medication"/> to a patient.</summary>
    [RelayCommand]
    private async Task PrescribeToPatient()
    {
        if (Medication is null)
        {
            _logger.Warning("Tried to prescribe a null prescription");

            await _dialogManager.CreateDialog()
                .OfType(NotificationType.Error)
                .WithTitle("Internal Error")
                .WithContent("Tried to prescribe an unknown prescription")
                .WithOkResult("Ok")
                .Dismiss().ByClickingBackground()
                .TryShowAsync();

            return;
        }

        _logger.Debug("Opening prescription modal for {PrescriptionName}", Medication.Name);

        DoctorPrescribeModalViewModel? vm = null;
        await _dialogManager.CreateDialog()
            .WithViewModel(dialog => vm = new DoctorPrescribeModalViewModel(dialog, Medication, _serviceProvider))
            .WithoutResult()
            // .Dismiss().ByClickingBackground() // Explicitly do not allow dismissing by clicking out to prevent accidental closes
            .TryShowAsync();

        if (vm is not { CreatedPrescription: { } prescription })
        {
            _logger.Debug("Canceled new prescription");
            return;
        }

        await DelayService.FakeDelay();

        await _dialogManager.CreateDialog()
            .OfType(NotificationType.Success)
            .WithTitle("Success")
            .WithContent("Prescription created successfully!")
            .WithOkResult("Ok")
            .Dismiss().ByClickingBackground()
            .TryShowAsync();
    }
}