using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Extensions;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Serilog;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

/// <summary>VM for the medication details page for doctors.</summary>
public partial class DoctorMedicationDetailsPageViewModel : ViewModelBase
{
    private readonly ISukiDialogManager _dialogManager;
    private readonly IPageRouter _pageRouter;
    private readonly ILogger _logger;

    [ObservableProperty]
    public partial MedicationModel? Medication { get; set; }

    public DoctorMedicationDetailsPageViewModel(
        ISukiDialogManager dialogManager,
        IPageRouter pageRouter,
        ILogger logger
    )
    {
        _dialogManager = dialogManager;
        _pageRouter = pageRouter;
        _logger = logger;
    }

    /// <summary>Requests a page navigation back to the previous page.</summary>
    [RelayCommand]
    private void NavigateBack()
    {
        _pageRouter.PopPage();
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
            .WithViewModel(dialog => vm = new DoctorPrescribeModalViewModel(dialog, Medication))
            .WithoutResult()
            // .Dismiss().ByClickingBackground() // Explicitly do not allow dismissing by clicking out to prevent accidental closes
            .TryShowAsync();

        if (vm is not { PrescriptionConfirmed: true })
        {
            _logger.Debug("Canceled new prescription");
            return;
        }

        // TODO:
    }
}