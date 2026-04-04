using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Extensions;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

public partial class DoctorMedicationPageViewModel : ViewModelBase
{
    private readonly ISukiDialogManager _dialogManager;

    [ObservableProperty]
    public partial PrescriptionViewModel? Prescription { get; set; }

    public DoctorMedicationPageViewModel(
        ISukiDialogManager dialogManager
    )
    {
        _dialogManager = dialogManager;
    }

    [RelayCommand]
    public async Task PrescribeToPatient()
    {
        await _dialogManager.CreateDialog()
            .WithViewModel(dialog => null)
            .WithoutResult()
            .TryShowAsync();
    }
}