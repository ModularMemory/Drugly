using System.ComponentModel.DataAnnotations;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Humanizer;
using Serilog;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Patient;

/// <summary>VM for any prescription details for patients.</summary>
public partial class PatientPrescriptionDetailsViewModel : ViewModelBase, IPageViewModel
{
    private readonly ISukiDialogManager _dialogManager;
    private readonly IPageRouter _pageRouter;
    private readonly ILogger _logger;
    private readonly IPrescriptionDetailsService _prescriptionDetailsService;

    public string? PageTitle => $"Viewing Prescription for {Prescription?.Medication.Name}";

    [ObservableProperty]
    public partial AccountDetails? Patient { get; set; }

    [ObservableProperty]
    public partial PatientPrescription? Prescription { get; set; }

    [ObservableProperty]
    public partial string DoctorPrescriptionStateText { get; set; } = "Approve";

    [ObservableProperty]
    public partial int StepIndex { get; set; }

    public IEnumerable<string> Steps { get; }
    public IAccountSessionService AccountSessionService { get; }

public PatientPrescriptionDetailsViewModel(
        ISukiDialogManager dialogManager,
        IPageRouter pageRouter,
        ILogger logger,
        IAccountSessionService accountSessionService,
        IPrescriptionDetailsService prescriptionDetailsService
    )
    {
        AccountSessionService = accountSessionService;
        _dialogManager = dialogManager;
        _pageRouter = pageRouter;
        _logger = logger;
        _prescriptionDetailsService = prescriptionDetailsService;

        Steps = Enum.GetValues<PrescriptionState>()
            .Where(x => x is not PrescriptionState.Unknown and not PrescriptionState.Cancelled)
            .Select(x => x.Humanize(LetterCasing.Title));

        SetDoctorPrescriptionStateText();
        ValidateAllProperties();
    }

    [RelayCommand]
    private async Task SubmitBillingInfo()
    {
        ValidateAllProperties();

        if (HasErrors)
        {
            var errors = string.Join(Environment.NewLine, GetErrors().Where(x => !string.IsNullOrWhiteSpace(x.ErrorMessage)));
            await _dialogManager.CreateDialog()
                .OfType(NotificationType.Error)
                .WithTitle("Invalid Billing Information")
                .WithContent(errors)
                .WithOkResult("Ok")
                .Dismiss().ByClickingBackground()
                .TryShowAsync();

            return;
        }

        await ProgressStepper();
        _pageRouter.ReshowPage();
    }

    [RelayCommand]
    private async Task ProgressStepper()
    {
        if (Prescription is null)
        {
            _logger.Warning("Tried to advance state of null prescription");
            return;
        }

        var prescription = Prescription.Prescription;
        prescription.State++;
        prescription = await _prescriptionDetailsService.AdvanceState(prescription, prescription.State);

        Prescription = new PatientPrescription(prescription, Prescription.Medication);
        SetDoctorPrescriptionStateText();
    }

    private void SetDoctorPrescriptionStateText()
    {
        DoctorPrescriptionStateText = Prescription?.Prescription.State switch
        {
            PrescriptionState.DoctorPrescription => "Approve",
            PrescriptionState.PharmacyProcessing => "Ready at Pharmacy",
            PrescriptionState.Filled => "Confirmed payment",
            PrescriptionState.Billing => "Patient picked up",
            PrescriptionState.PickedUp => "Approve",
            PrescriptionState.Cancelled => "Approve",
            _ => "Unknown",
        };
    }

    [RelayCommand]
    private async Task CancelPrescription()
    {
        if (Prescription is null)
        {
            _logger.Warning("Tried to cancel a null prescription");
            return;
        }

        var prescription = Prescription.Prescription;
        prescription = await _prescriptionDetailsService.AdvanceState(prescription, PrescriptionState.Cancelled);

        Prescription = new PatientPrescription(prescription, Prescription.Medication);
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    public partial string? CardHolder { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    public partial string? CardNumber { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    public partial string? CardExpiry { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    public partial string? CardCCV { get; set; }
}