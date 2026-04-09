using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.Validation;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

public partial class DoctorPrescribeModalViewModel : ViewModelBase
{
    private readonly ISukiDialog _dialog;

    public PrescriptionViewModel Prescription { get; }

    public bool PrescriptionConfirmed { get; private set; }

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
    [NumberValidator.Minimum(0)]
    public partial decimal? DaysBetweenDosage { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [NumberValidator.Minimum(1)]
    public partial decimal? DaysPrescribed { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MaxLength(500)]
    public partial string? PrescriptionNotes { get; set; }

    public DoctorPrescribeModalViewModel(
        ISukiDialog dialog,
        PrescriptionViewModel prescription
    )
    {
        Prescription = prescription;
        _dialog = dialog;

        ValidateAllProperties();
    }

    [RelayCommand]
    private void Cancel()
    {
        _dialog.Dismiss();
    }

    [RelayCommand]
    private async Task CreatePrescription()
    {
        ErrorText = null;
        IsLoading = true;

        try
        {
            if (HasErrors)
            {
                await Task.Delay(500);
                var errors = GetErrors()
                    .Where(x => x.ErrorMessage is not null)
                    .Select(x => x.ErrorMessage!.TrimEnd('.'));

                ErrorText = string.Join(Environment.NewLine, errors);
                return;
            }

            // TODO: HTTP request
            await Task.Delay(1_000);

            PrescriptionConfirmed = true;
            _dialog.Dismiss();
        }
        finally
        {
            IsLoading = false;
        }
    }
}