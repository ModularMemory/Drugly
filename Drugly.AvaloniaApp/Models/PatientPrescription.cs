using CommunityToolkit.Mvvm.ComponentModel;
using Drugly.DTO;

namespace Drugly.AvaloniaApp.Models;

/// <summary>Combines a Prescription and medication.</summary>
public partial class PatientPrescription : ObservableObject
{
    public PatientPrescription(Prescription prescription, Medication medication)
    {
        Medication = medication;
        Prescription = prescription;
    }

    /// <summary>The prescription.</summary>
    [ObservableProperty]
    public partial Prescription Prescription { get; set; }

    /// <summary>The medication.</summary>
    [ObservableProperty]
    public partial Medication Medication { get; set; }
}