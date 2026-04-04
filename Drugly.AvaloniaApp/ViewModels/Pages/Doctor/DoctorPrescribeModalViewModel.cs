using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

public partial class DoctorPrescribeModalViewModel : ViewModelBase
{
    private readonly ISukiDialog _dialog;

    public PrescriptionViewModel Prescription { get; }

    public bool PrescriptionConfirmed { get; private set; }

    [ObservableProperty]
    public partial string PatientFirstName { get; set; } = "";

    [ObservableProperty]
    public partial string PatientLastName { get; set; } = "";

    [ObservableProperty]
    public partial string PatientEmail { get; set; } = "";

    [ObservableProperty]
    public partial double DosagePerDay { get; set; } = 0;

    [ObservableProperty]
    public partial double DaysBetweenDosage { get; set; } = 0;

    [ObservableProperty]
    public partial double DaysPrescribed { get; set; } = 0;

    [ObservableProperty]
    public partial string PrescriptionNotes { get; set; } = "";

    public DoctorPrescribeModalViewModel(
        ISukiDialog dialog,
        PrescriptionViewModel prescription
    )
    {
        Prescription = prescription;
        _dialog = dialog;
    }

    [RelayCommand]
    private void Cancel()
    {
        _dialog.Dismiss();
    }

    [RelayCommand]
    private void CreatePrescription()
    {
        PrescriptionConfirmed = true;
        _dialog.Dismiss();
    }
}