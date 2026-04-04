using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

public partial class DoctorPrescribeModalViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial PrescriptionViewModel? Prescription { get; set; }

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

    public DoctorPrescribeModalViewModel() { }

    [RelayCommand]
    public void Cancel() { }

    [RelayCommand]
    public async Task CreatePrescription() { }
}