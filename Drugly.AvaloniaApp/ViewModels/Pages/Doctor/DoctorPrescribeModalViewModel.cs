using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Controls;
using Drugly.AvaloniaApp.Services;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Drugly.Validation;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

/// <summary>VM for the medication prescription modal.</summary>
public partial class DoctorPrescribeModalViewModel : ViewModelBase
{
    private readonly ISukiDialog _dialog;
    private readonly IAccountDetailsService _accountDetailsService;
    private readonly IPrescriptionDetailsService _prescriptionDetailsService;
    private readonly IImageDetailsService _imageDetailsService;

    /// <summary>The medication associated with the dialog.</summary>
    public Medication Medication { get; }

    public Prescription? CreatedPrescription { get; private set; }

    [ObservableProperty]
    public partial string? ErrorText { get; private set; }

    [ObservableProperty]
    public partial bool IsLoading { get; private set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    public partial string? PatientFirstName { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    public partial string? PatientLastName { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [EmailAddress]
    [Required]
    public partial string? PatientEmail { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    public partial string? DosagePerDay { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [Minimum(0)]
    public partial decimal? DaysBetweenDosage { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [Minimum(1)]
    public partial decimal? DaysPrescribed { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MaxLength(500)]
    public partial string? PrescriptionNotes { get; set; }

    public DoctorPrescribeModalViewModel(
        ISukiDialog dialog,
        Medication medication,
        IServiceProvider serviceProvider
    )
    {
        Medication = medication;
        _dialog = dialog;
        _accountDetailsService = serviceProvider.GetRequiredService<IAccountDetailsService>();
        _prescriptionDetailsService = serviceProvider.GetRequiredService<IPrescriptionDetailsService>();
        _imageDetailsService = serviceProvider.GetRequiredService<IImageDetailsService>();

        ValidateAllProperties();
    }

    /// <summary>Closes the dialog.</summary>
    [RelayCommand]
    private void Cancel()
    {
        _dialog.Dismiss();
    }

    /// <summary>Validates all fields, submits details to the server, and closes the dialog on success.</summary>
    /// <param name="signature">The doctor's signature bitmap.</param>
    [RelayCommand]
    private async Task CreatePrescription(DrawableBitmap signature)
    {
        Debug.Assert(signature != null);

        ErrorText = null;
        IsLoading = true;

        try
        {
            if (HasErrors)
            {
                await DelayService.FakeDelay(500);

                ErrorText = string.Join(Environment.NewLine, GetErrors().Where(x => !string.IsNullOrWhiteSpace(x.ErrorMessage)));
                return;
            }

            var signatureStream = new MemoryStream();
            if (!signature.Save(signatureStream))
            {
                ErrorText = "Failed to save signature.";
                return;
            }

            if (await SubmitPrescription(signatureStream))
            {
                _dialog.Dismiss();
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task<bool> SubmitPrescription(MemoryStream signatureStream)
    {
        try
        {
            var patient = await _accountDetailsService.GetAccountByEmail(PatientEmail!);
            var uri = await _imageDetailsService.UploadImage(signatureStream);
            var prescription = new Prescription(
                Medication.Id,
                patient.UserId,
                DosagePerDay!,
                (ulong)DaysBetweenDosage!,
                (ulong)DaysPrescribed!,
                PrescriptionNotes!,
                uri.AbsoluteUri
            );

            CreatedPrescription = await _prescriptionDetailsService.CreatePrescription(prescription);
        }
        catch (Exception ex)
        {
            ErrorText = ex.Message;
            return false;
        }

        return true;
    }
}