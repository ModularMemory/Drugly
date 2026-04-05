using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Extensions;
using Serilog;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

public partial class DoctorMedicationDetailsPageViewModel : ViewModelBase
{
    private readonly ISukiDialogManager _dialogManager;
    private readonly ILogger _logger;

    [ObservableProperty]
    public partial PrescriptionViewModel? Prescription { get; set; }

    public DoctorMedicationDetailsPageViewModel(
        ISukiDialogManager dialogManager,
        ILogger logger
    )
    {
        _dialogManager = dialogManager;
        _logger = logger;
    }

    [RelayCommand]
    private async Task PrescribeToPatient()
    {
        if (Prescription is null)
        {
            _logger.Warning("Tried to prescribe a null prescription");

            await _dialogManager.CreateDialog()
                .OfType(NotificationType.Error)
                .WithTitle("Internal Error")
                .WithContent("Tried to prescribe an unknown prescription")
                .WithOkResult("Ok")
                .TryShowAsync();

            return;
        }

        _logger.Debug("Opening prescription modal for {PrescriptionName}", Prescription.Name);

        DoctorPrescribeModalViewModel? vm = null;
        await _dialogManager.CreateDialog()
            .WithViewModel(dialog => vm = new DoctorPrescribeModalViewModel(dialog, Prescription))
            .WithoutResult()
            .TryShowAsync();

        if (vm is { PrescriptionConfirmed: true })
        {
            _logger.Debug("Canceled new prescription");
            return;
        }

        // TODO:
    }
}